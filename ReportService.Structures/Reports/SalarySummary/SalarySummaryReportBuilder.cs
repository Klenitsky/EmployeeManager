using EmployeeService.DAL.Models;
using ExchangeRateService.DAL.BasicStructures.Models;
using ReportService.Structures.DataReaders;
using ReportService.Structures.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Structures.Reports.SalarySummary
{
    public class SalarySummaryReportBuilder : IReportBuilder<SalarySummaryReport>
    {
        private Dictionary<Office, StatisticMetric> _includedOffices = new Dictionary<Office, StatisticMetric>();
        private Dictionary<Country, StatisticMetric> _includedCountries = new Dictionary<Country, StatisticMetric>();
        private List<Employee> _includedEmployees = new List<Employee>();
        private List<Currency> _includedCurrencies = new List<Currency>();
        private List<ExchangeRate> _includedExchangeRates = new List<ExchangeRate>();

        private string connectionStringEmployeeService;
        private string connectionStringExchangeRateService;

        private StatisticMetric _generalStatistics= new StatisticMetric();
        private DateTime _date;


        public SalarySummaryReportBuilder(string connectionStringEmployeeService, string connectionStringExchangeRateService)
        {
            this.connectionStringEmployeeService = connectionStringEmployeeService;
            this.connectionStringExchangeRateService = connectionStringExchangeRateService;
            _includedOffices = new Dictionary<Office, StatisticMetric>();
            _includedCountries = new Dictionary<Country, StatisticMetric>();
            _includedEmployees = new List<Employee>();
            _includedCurrencies = new List<Currency>();
            _includedExchangeRates = new List<ExchangeRate>();
    }

        public void SetDate(DateTime date)
        {
            _date = date;
        }
        public void IncludeOfficeParams(IEnumerable<Office> offices)
        {
            foreach( var office in offices)
            {
                if (_includedOffices.Where(o => o.Key.Id == office.Id).Count() == 0)
                {
                    _includedOffices.Add(office, new StatisticMetric());
                    if(_includedCountries.Where(c=> c.Key.Id == office.CountryId).Count() == 0)
                    {
                        _includedCountries.Add(office.CountryNavigation, new StatisticMetric());
                    }
                }
            }
          
        }

        public void IncludeFullCountriesParams(IEnumerable<Country> countries)
        {
            CompanyInfoReader reader = new CompanyInfoReader(connectionStringEmployeeService); 
            foreach (var country in countries)
            {
                if (_includedCountries.Where(c => c.Key.Id == country.Id).Count() == 0)
                {
                    _includedCountries.Add(country, new StatisticMetric());
                    IEnumerable<Office> officesInCountry = reader.FindOfficesByCountry(country.Id);
                    foreach(var office in officesInCountry)
                    {
                        if(_includedOffices.Where(o=>o.Key.Id == office.Id).Count() == 0)
                        {
                            _includedOffices.Add(office, new StatisticMetric());
                        }
                    }
                }
            }

        }

        public void CountMetrics()
        {
            foreach (var officeKey in _includedOffices.Keys)
            {
                _includedOffices[officeKey] = new StatisticMetric { AverageSalary = 0, NumberOfEmployees = 0, SalarySummary = 0 };
                List<Employee> employees = _includedEmployees.Where(e => (e.OfficeId == officeKey.Id) && (e.EmploymentDate <= _date && (e.DismissalDate == null || e.DismissalDate >= _date))).ToList();
                foreach (var employee in employees)
                {
                    var exchangeRate = _includedExchangeRates.Where(r => r.Abbreviation == _includedCurrencies.Where(c => c.Id == employee.CurrencyId).First().Abbreviation).First();
                    _includedOffices[officeKey].SalarySummary += employee.Salary * exchangeRate.Rate / exchangeRate.Scale;
                    _includedOffices[officeKey].NumberOfEmployees += 1;
                }
                _includedOffices[officeKey].AverageSalary = _includedOffices[officeKey].SalarySummary / _includedOffices[officeKey].NumberOfEmployees;
            }

            foreach (var country in _includedCountries.Keys)
            {
                _includedCountries[country] = new StatisticMetric { AverageSalary = 0, NumberOfEmployees = 0, SalarySummary = 0 };
                List<Office> officesInCountryKeys = _includedOffices.Keys.Where(o => o.CountryId == country.Id).ToList();
                foreach (var office in officesInCountryKeys)
                {
                    _includedCountries[country].SalarySummary += _includedOffices[office].SalarySummary;
                    _includedCountries[country].NumberOfEmployees += _includedOffices[office].NumberOfEmployees;
                }
                _includedCountries[country].AverageSalary = _includedCountries[country].SalarySummary / _includedCountries[country].NumberOfEmployees;
            }

            _generalStatistics.SalarySummary = 0;
            _generalStatistics.NumberOfEmployees = 0;
            _generalStatistics.AverageSalary = 0;
            foreach (var country in _includedCountries.Keys)
            {
                _generalStatistics.SalarySummary += _includedCountries[country].SalarySummary;
                _generalStatistics.NumberOfEmployees += _includedCountries[country].NumberOfEmployees;
            }
            _generalStatistics.AverageSalary = _generalStatistics.SalarySummary / _generalStatistics.NumberOfEmployees;
        }

        public SalarySummaryReport GetResult()
        {
            return new SalarySummaryReport()
            {
                Date = _date,
                IncludedOfficesMetrics = _includedOffices,
                IncludedCountriesMetrics = _includedCountries,
                IncludedEmployees = _includedEmployees,
                GeneralStatistics = _generalStatistics
            };
        }

        public void LoadData()
        {
            CompanyInfoReader companyInfoReader = new CompanyInfoReader(connectionStringEmployeeService);
            ExchangeRateReader exchangeRateReader = new ExchangeRateReader(connectionStringExchangeRateService);
            foreach(var office in _includedOffices)
            {

                List<Employee> employees = companyInfoReader.FindEmployeesByOffice(office.Key.Id).ToList();
                _includedEmployees.AddRange(employees.Where(e => e.EmploymentDate <= _date && ((e.DismissalDate == null) || (e.DismissalDate >= _date))));
                foreach(var employee in employees)
                {
                    if ((_includedCurrencies.Where(c => c.Id == employee.CurrencyId).Count() == 0))
                    {
                        _includedCurrencies.Add(employee.CurrencyNavigation);
                        ExchangeRate rate = exchangeRateReader.GetByDateAndCurrency(_date, employee.CurrencyNavigation.Abbreviation);
                        _includedExchangeRates.Add(rate);
                    }
                }
            }
        }
    }
}
