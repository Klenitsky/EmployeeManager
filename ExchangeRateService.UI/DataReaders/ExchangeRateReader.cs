using ExchangeRateService.UI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateService.UI.DataReaders
{
    public class ExchangeRateReader
    {
        private string _connectionString;

        private HttpClient httpClient = new HttpClient();
        public ExchangeRateReader(string connectionString)
        {
            _connectionString = connectionString;
        }


        public IEnumerable<ExchangeRate> GetRatesOnDateRangeInSystem(DateTime startDate, DateTime endDate)
        {
            try
            {
                string url = _connectionString + "/ExchangeRateInSystem/OnDateRange" + "?startDate=" + startDate.ToString("yyyy-MM-dd") + "&endDate=" + endDate.ToString("yyyy - MM - dd");
                List<ExchangeRate> rates = httpClient.GetFromJsonAsync<IEnumerable<ExchangeRate>>(url)
                                                     .Result
                                                     .ToList();
                return rates;
            }
            catch (Exception ex)
            {

                return new List<ExchangeRate>();
            }
        }

        public IEnumerable<ExchangeRate> GetRatesOnDateRangeAndCurrencyInSystem(DateTime startDate, DateTime endDate, string currency)
        {
            try
            {
                string url = _connectionString + "/ExchangeRateInSystem/OnDateRangeAndCurrency" + "?startDate=" + startDate.ToString("yyyy-MM-dd") + "&endDate=" + endDate.ToString("yyyy-MM-dd") + "&currencyAbbreviation=" + currency.ToUpper();
                List<ExchangeRate> rates = httpClient.GetFromJsonAsync<IEnumerable<ExchangeRate>>(url).Result.ToList();
                return rates;
            }
            catch (Exception ex)
            {

                return new List<ExchangeRate>();
            }
        }

        public IEnumerable<ExchangeRate> GetRatesOnDateRangeAndCurrencyOnRequest(DateTime startDate, DateTime endDate, string currency)
        {
            try
            {
                string url = _connectionString + "/ExchangeRate/OnDateRangeAndCurrency" + "?startDate=" + startDate.ToString("yyyy-MM-dd") + "&endDate=" + endDate.ToString("yyyy-MM-dd") + "&currencyAbbreviation=" + currency.ToUpper();
                List<ExchangeRate> rates = httpClient.GetFromJsonAsync<IEnumerable<ExchangeRate>>(url).Result.ToList();
                return rates;
            }
            catch (Exception ex)
            {

                return new List<ExchangeRate>();
            }
        }

        public IEnumerable<ExchangeRate> GetRatesOnDateRangeOnRequest(DateTime startDate, DateTime endDate)
        {
            try
            {
                string url = _connectionString + "/ExchangeRate/OnDateRange" + "?startDate=" + startDate.ToString("yyyy-MM-dd") + "&endDate=" + endDate.ToString("yyyy - MM - dd");
                List<ExchangeRate> rates = httpClient.GetFromJsonAsync<IEnumerable<ExchangeRate>>(url)
                                                     .Result
                                                     .ToList();
                return rates;
            }
            catch (Exception ex)
            {

                return new List<ExchangeRate>();
            }
        }


        public IEnumerable<ActiveCurrency> GetActiveCurrencies()
        {
            string url = _connectionString + "/ExchangeRateInSystem/Currencies";
            List<ActiveCurrency> currencies = httpClient.GetFromJsonAsync<IEnumerable<ActiveCurrency>>(url).Result.ToList();
            return currencies;
        }
    }
}

