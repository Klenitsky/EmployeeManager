using EmployeeService.DAL.Models;
using ExchangeRateService.DAL.BasicStructures.Models;
using ReportService.Structures.DataReaders;
using ReportService.Structures.Reports.Interfaces;
using ReportService.Structures.Reports.SalarySummary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Structures.Reports.PaymentOnDateRange
{
    public class PaymentOnDateRangeReportBuilder : IReportBuilder<PaymentOnDateRangeReport>
    {
        private DateTime _startDate { get; set; }
        private DateTime _endDate { get; set; }
        private Dictionary<Country, float> _includedCountriesMetrics { get; set; }

        private Dictionary<Office, float> _includedOfficesMetrics { get; set; }

        private Dictionary<Employee, float> _includedEmployees { get; set; }
        private List<Currency> _includedCurrencies = new List<Currency>();
        private List<ExchangeRate> _includedExchangeRates = new List<ExchangeRate>();
        private float _totalPayment { get; set; }



        private string connectionStringEmployeeService;
        private string connectionStringExchangeRateService;

        public PaymentOnDateRangeReportBuilder(string connectionStringEmployeeService, string connectionStringExchangeRateService)
        {
            this.connectionStringEmployeeService = connectionStringEmployeeService;
            this.connectionStringExchangeRateService = connectionStringExchangeRateService;
        }


        public void SetDateRange(DateTime startDate, DateTime endDate)
        {
            _endDate = endDate;
            _startDate = startDate;
        }


        public void IncludeOfficeParams(IEnumerable<Office> offices)
        {
            foreach (var office in offices)
            {
                if (_includedOfficesMetrics.Where(o => o.Key.Id == office.Id).Count() == 0)
                {
                    _includedOfficesMetrics.Add(office, 0);
                    if (_includedCountriesMetrics.Where(c => c.Key.Id == office.CountryId).Count() == 0)
                    {
                        _includedCountriesMetrics.Add(office.CountryNavigation, 0);
                    }
                }
            }

        }

        public void IncludeFullCountriesParams(IEnumerable<Country> countries)
        {
            CompanyInfoReader reader = new CompanyInfoReader(connectionStringEmployeeService);
            foreach (var country in countries)
            {
                if (_includedCountriesMetrics.Where(c => c.Key.Id == country.Id).Count() == 0)
                {
                    _includedCountriesMetrics.Add(country, 0);
                    IEnumerable<Office> officesInCountry = reader.FindOfficesByCountry(country.Id);
                    foreach (var office in officesInCountry)
                    {
                        if (_includedOfficesMetrics.Where(o => o.Key.Id == office.Id).Count() == 0)
                        {
                            _includedOfficesMetrics.Add(office, 0);
                        }
                    }
                }
            }

        }
        public void CountMetrics()
        {
            foreach (var employeeKey in _includedEmployees.Keys)
            {
                DateTime workStart = employeeKey.EmploymentDate > _startDate ? employeeKey.EmploymentDate : _startDate;
                DateTime workEnd = _endDate;
                if(employeeKey.DismissalDate!= null && employeeKey.DismissalDate < _endDate)
                {
                    workEnd = (DateTime)employeeKey.DismissalDate;
                }
                DateTime currentDate = workStart;
                float payment = 0;
                int workdays = 0;
                while(currentDate <= workEnd)
                {
                    if(currentDate.Day == 1 || currentDate == workEnd)
                    {
                        ExchangeRate exchangeRate = _includedExchangeRates.Where(r => (r.Abbreviation == _includedCurrencies.Where(c => c.Id == employeeKey.CurrencyId).First().Abbreviation) && r.Date <= currentDate).First();
                        int currentMonth = currentDate.AddDays(-1).Month;
                        int daysInMonth;
                        switch (currentMonth)
                        {
                            case 1:
                            case 3:
                            case 5:
                            case 7:
                            case 8:
                            case 10:
                            case 12:
                                daysInMonth = 31;
                                break;
                            case 2:
                                daysInMonth = 28;
                                    break;
                            default:
                                daysInMonth = 30;
                                break;
                        };

                        payment += exchangeRate.Rate / exchangeRate.Scale * employeeKey.Salary * workdays / daysInMonth;
                        workdays = 0;
                    }

                    workdays++;
                    currentDate = currentDate.AddDays(1);
                }
                _includedEmployees[employeeKey] = payment;
            }

            foreach (var officeKey in _includedOfficesMetrics.Keys)
            {
                _includedOfficesMetrics[officeKey] = 0;
                var employees = _includedEmployees.Where(e => (e.Key.OfficeId == officeKey.Id) && (e.Key.EmploymentDate <= _endDate && (e.Key.DismissalDate == null || e.Key.DismissalDate >= _startDate))).ToList();
                
                foreach (var employee in employees)
                {
                    _includedOfficesMetrics[officeKey] += employee.Value;
                }
            }

            foreach (var country in _includedCountriesMetrics.Keys)
            {
                _includedCountriesMetrics[country] = 0;
                List<Office> officesInCountryKeys = _includedOfficesMetrics.Keys.Where(o => o.CountryId == country.Id).ToList();
                foreach (var office in officesInCountryKeys)
                {
                    _includedCountriesMetrics[country] += _includedOfficesMetrics[office];
                }
            }

            _totalPayment = 0;
            foreach (var country in _includedCountriesMetrics.Keys)
            {
                _totalPayment += _includedCountriesMetrics[country];
            }

        }

        public void LoadData()
        {
            CompanyInfoReader companyInfoReader = new CompanyInfoReader(connectionStringEmployeeService);
            ExchangeRateReader exchangeRateReader = new ExchangeRateReader(connectionStringExchangeRateService);
            foreach (var office in _includedOfficesMetrics)
            {

                List<Employee> employees = companyInfoReader.FindEmployeesByOffice(office.Key.Id).ToList();
                foreach (var employee in employees)
                {
                    _includedEmployees.Add(employee, 0);
                    if (!_includedCurrencies.Contains(employee.CurrencyNavigation))
                    {
                        _includedCurrencies.Add(employee.CurrencyNavigation);
                        var currentDate = _startDate;
                        while (currentDate <= _endDate)
                        {
                            if (currentDate.Day == 1 || currentDate == _endDate)
                            {
                                ExchangeRate rate = exchangeRateReader.GetByDateAndCurrency(currentDate, employee.CurrencyNavigation.Abbreviation);
                                _includedExchangeRates.Add(rate);
                            }
                            currentDate = currentDate.AddDays(1);
                        }                    
                    }
                }
            }
        }

        public PaymentOnDateRangeReport GetResult()
        {
            return new PaymentOnDateRangeReport()
            {
                StartDate = _startDate,
                EndDate = _endDate,
                IncludedCountriesMetrics = _includedCountriesMetrics,
                IncludedOfficesMetrics = _includedOfficesMetrics,
                IncludedEmployees = _includedEmployees,
                TotalPayment = _totalPayment,
            };
        }
    }
}
