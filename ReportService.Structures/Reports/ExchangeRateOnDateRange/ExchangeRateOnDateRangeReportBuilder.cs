using EmployeeService.DAL.Models;
using ExchangeRateService.DAL.BasicStructures.Models;
using ReportService.Structures.DataReaders;
using ReportService.Structures.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Structures.Reports.ExchangeRateOnDateRange
{
    public class ExchangeRateOnDateRangeReportBuilder : IReportBuilder<ExchangeRateOnDateRangeReport>
    {
        private DateTime _startDate;
        private DateTime _endDate { get; set; }
        private Dictionary<Currency, float> _includedCurrencies { get; set; }
        private List<ExchangeRate> _exchangeRates { get; set; }

        private string _connectionStringExchangeRateService;

        public ExchangeRateOnDateRangeReportBuilder( string ExchangeRateConnectionString)
        {
            _connectionStringExchangeRateService = ExchangeRateConnectionString;
            _includedCurrencies = new Dictionary<Currency, float>();
            _exchangeRates = new List<ExchangeRate>();
        }

        public void SetProperties(DateTime startDate, DateTime endDate, IEnumerable<Currency> includedCurrencies)
        {
            _startDate = startDate;
            _endDate = endDate;
            foreach(var currency in includedCurrencies)
            {
                _includedCurrencies.Add(currency, 0);
            }
        }
        public void CountMetrics()
        {
            foreach(var currencyKey in _includedCurrencies.Keys)
            {
                _includedCurrencies[currencyKey] = _exchangeRates.Where(r=>r.Abbreviation == currencyKey.Abbreviation).Select(r=>r.Rate).Average();
            }
        }

        public ExchangeRateOnDateRangeReport GetResult()
        {
            return new ExchangeRateOnDateRangeReport()
            {
                StartDate = _startDate,
                EndDate = _endDate,
                IncludedCurrencies = _includedCurrencies,
                ExchangeRates = _exchangeRates,
            };
        }

        public void LoadData()
        {
           ExchangeRateReader reader = new ExchangeRateReader(_connectionStringExchangeRateService);
           foreach(var currency in _includedCurrencies)
            {
                IEnumerable<ExchangeRate> rates = reader.GetRatesOnDateRangeAndCurrency(_startDate, _endDate, currency.Key.Abbreviation);
                _exchangeRates.AddRange(rates);
            }
        }
    }
}
