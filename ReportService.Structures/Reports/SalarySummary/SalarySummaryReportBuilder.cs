using EmployeeService.DAL.Models;
using ExchangeRateService.DAL.BasicStructures.Models;
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
            foreach (var country in countries)
            {
                if (_includedCountries.Where(c => c.Key.Id == country.Id).Count() == 0)
                {
                    _includedCountries.Add(country, new StatisticMetric());
                    HttpClient httpClient = new HttpClient();
                    IEnumerable<Office> officesInCountry = httpClient.GetFromJsonAsync<IEnumerable<Office>>(connectionStringEmployeeService + "/api/EmployeeService/Office/FindByCountry/" + country.Id).Result;
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
            foreach (var office in _includedOffices)
            {
                office.Value.SalarySummary = 0;
                office.Value.AverageSalary = 0;
                office.Value.NumberOfEmployees = 0;
                List<Employee> employees = _includedEmployees.Where(e => (e.OfficeId == office.Key.Id) && (e.EmploymentDate <= _date && (e.DismissalDate == null || e.DismissalDate >= _date))).ToList();
                foreach (var employee in employees)
                {
                    var exchangeRate = _includedExchangeRates.Where(r => r.Abbreviation == _includedCurrencies.Where(c => c.Id == employee.CurrencyId).First().Abbreviation).First();
                    office.Value.SalarySummary += employee.Salary * exchangeRate.Rate / exchangeRate.Scale;
                    office.Value.NumberOfEmployees += 1;
                }
                office.Value.AverageSalary = office.Value.SalarySummary / office.Value.NumberOfEmployees;
            }

            foreach (var country in _includedCountries)
            {
                country.Value.SalarySummary = 0;
                country.Value.AverageSalary = 0;
                country.Value.NumberOfEmployees = 0;
                List<KeyValuePair<Office, StatisticMetric>> officesInCountry = _includedOffices.Where(o => o.Key.CountryId == country.Key.Id).ToList();
                foreach (var office in officesInCountry)
                {
                    country.Value.SalarySummary += office.Value.SalarySummary;
                    country.Value.NumberOfEmployees += office.Value.NumberOfEmployees;
                }
                country.Value.AverageSalary = country.Value.SalarySummary / country.Value.NumberOfEmployees;
            }

            _generalStatistics.SalarySummary = 0;
            _generalStatistics.NumberOfEmployees = 0;
            _generalStatistics.AverageSalary = 0;
            foreach (var country in _includedCountries)
            {
                _generalStatistics.SalarySummary += country.Value.SalarySummary;
                _generalStatistics.NumberOfEmployees += country.Value.NumberOfEmployees;
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
            foreach(var office in _includedOffices)
            {
                HttpClient httpClient = new HttpClient();

                List<Employee> employees = httpClient.GetFromJsonAsync<IEnumerable<Employee>>(connectionStringEmployeeService+ "/api/EmployeeService/Employee/FindByOffice/"+office.Key.Id).Result.ToList();
                _includedEmployees.AddRange(employees);
                foreach(var employee in employees)
                {
                    if (!_includedCurrencies.Contains(employee.CurrencyNavigation))
                    {
                        _includedCurrencies.Add(employee.CurrencyNavigation);
                        ExchangeRate rate = httpClient.GetFromJsonAsync<ExchangeRate>(connectionStringExchangeRateService + "/api/ExchangeRate/OnDateAndCurrency?date=" + _date.ToString("yyyy-MM-dd") + "&currencyAbbreviation=" + employee.CurrencyNavigation.Abbreviation).Result;
                        _includedExchangeRates.Add(rate);
                    }
                }
            }
        }
    }
}
