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

        public IEnumerable<ExchangeRate> GetRatesOnDate(DateTime date)
        {
            try
            {
                string url = _connectionString + "/OnDate" + "?Date=" + date.ToString("yyyy-MM-dd");
                List<ExchangeRate> rates = httpClient.GetFromJsonAsync<IEnumerable<ExchangeRate>>(url).Result.ToList();
                return rates;
            }
            catch (Exception ex)
            {

                return new List<ExchangeRate>();
            }
        }

        public IEnumerable<ExchangeRate> GetRatesOnDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                string url = _connectionString + "/OnDateRange" + "?startDate=" + startDate.ToString("yyyy-MM-dd") + "&endDate=" + endDate.ToString("yyyy - MM - dd");
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

        public ExchangeRate GetByDateAndCurrency(DateTime date, string currency)
        {
            try
            {
                string url = _connectionString + "/OnDateAndCurrency" + "?date=" + date.ToString("yyyy-MM-dd") + "&currencyAbbreviation=" + currency.ToUpper();
                ExchangeRate rate = httpClient.GetFromJsonAsync<ExchangeRate>(url).Result;
                return rate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public IEnumerable<ExchangeRate> GetRatesOnDateRangeAndCurrency(DateTime startDate, DateTime endDate, string currency)
        {
            try
            {
                string url = _connectionString + "/OnDateRangeAndCurrency" + "?startDate=" + startDate.ToString("yyyy-MM-dd") + "&endDate=" + endDate.ToString("yyyy-MM-dd") + "&currencyAbbreviation=" + currency.ToUpper();
                List<ExchangeRate> rates = httpClient.GetFromJsonAsync<IEnumerable<ExchangeRate>>(url).Result.ToList();
                return rates;
            }
            catch (Exception ex)
            {

                return new List<ExchangeRate>();
            }
        }
    }
}

