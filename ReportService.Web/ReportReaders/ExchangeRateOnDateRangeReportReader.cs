using ReportService.Structures.ParameterModels;
using ReportService.Structures.Reports.ExchangeRateOnDateRange;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReportService.Web.ReportReaders
{
    public class ExchangeRateOnDateRangeReportReader
    {
        private HttpClient _httpClient;
        private string _connectionString;
        private static ExchangeRateOnDateRangeJsonConverter _converter = new ExchangeRateOnDateRangeJsonConverter(); 
        public ExchangeRateOnDateRangeReportReader(string connectionString)
        {
            _httpClient= new HttpClient();
            _connectionString = connectionString+"/ExchangeRateOnDateRangeReport";
        }


        public ExchangeRateOnDateRangeReport GetReport(ExchangeRateParametersModel args)
        {
           var postResult =  _httpClient.PostAsJsonAsync<ExchangeRateParametersModel>(_connectionString,args).Result;
            if (postResult.IsSuccessStatusCode)
            {
                JsonSerializerOptions options = new JsonSerializerOptions()
                {
                    Converters =
                    {
                        _converter
                    }
                };
                var result = _httpClient.GetFromJsonAsync<ExchangeRateOnDateRangeReport>(_connectionString, options).Result;
                return result;
            }
            else
            {
                throw new ArgumentException("Response with such args is unsuccessful");
            }
           
        }

    }
}
