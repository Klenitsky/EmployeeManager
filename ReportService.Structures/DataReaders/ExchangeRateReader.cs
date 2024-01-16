using ExchangeRateService.DAL.BasicStructures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Structures.DataReaders
{
    public class ExchangeRateReader
    {
        private string _connectionString;
        private HttpClient httpClient= new HttpClient();
        public ExchangeRateReader(string connectionString ) 
        {
            _connectionString = connectionString;
        }

        public IEnumerable<ExchangeRate> GetRatesOnDate( DateTime date )
        {
            try
            {
                List<ExchangeRate> rates = httpClient.GetFromJsonAsync<IEnumerable<ExchangeRate>>(_connectionString+ "api/ExchangeRate/OnDate" + "?Date=" + date.ToString("yyyy-MM-dd")).Result.ToList();
                return rates;
            }
            catch(Exception ex)
            {
                
                return new List<ExchangeRate>();
            }
        }

        public IEnumerable<ExchangeRate> GetRatesOnDateRange(DateTime startDate,DateTime endDate)
        {
            try
            {
                List<ExchangeRate> rates = httpClient.GetFromJsonAsync<IEnumerable<ExchangeRate>>(_connectionString + "api/ExchangeRate/OnDateRange" + "?startDate=" + startDate.ToString("yyyy-MM-dd")+ "&endDate = " + endDate.ToString("yyyy - MM - dd"))
                                                     .Result
                                                     .ToList();
                return rates;
            }
            catch (Exception ex)
            {

                return new List<ExchangeRate>();
            }
        }

        public ExchangeRate GetByDateAndCurrency(DateTime date,string currency)
        {
            try
            {
                ExchangeRate rate = httpClient.GetFromJsonAsync<ExchangeRate>(_connectionString + "api/ExchangeRate/OnDateAndCurrency" + "?date=" + date.ToString("yyyy-MM-dd")+"&currencyAbbreviation="+currency.ToUpper()).Result;
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
                List<ExchangeRate> rates = httpClient.GetFromJsonAsync<IEnumerable<ExchangeRate>>(_connectionString + "/OnDateRangeAndCurrency" + "?startDate=" + startDate.ToString("yyyy-MM-dd") + "&endDate = " + endDate.ToString("yyyy - MM - dd")+ "&currencyAbbreviation=" + currency.ToUpper()).Result.ToList();
                return rates;
            }
            catch (Exception ex)
            {

                return new List<ExchangeRate>();
            }
        }
    }
}
