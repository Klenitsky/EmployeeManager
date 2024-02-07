using ReportService.Structures.ParameterModels;
using ReportService.Structures.Reports.ExchangeRateOnDateRange;
using ReportService.Structures.Reports.PaymentOnDateRange;
using System.Text.Json;

namespace ReportService.Web.DataReaders.ReportServiceReaders
{
    public class PaymentOnDateRangeReportReader
    {
        private HttpClient _httpClient;
        private string _connectionString;
        private static PaymentOnDateRangeJsonConverter _converter = new PaymentOnDateRangeJsonConverter();
        public PaymentOnDateRangeReportReader(string connectionString)
        {
            _httpClient = new HttpClient();
            _connectionString = connectionString + "/PaymentOnDateRangeReport";
        }


        public PaymentOnDateRangeReport GetReport(PaymentParametersModel args)
        {
            var postResult = _httpClient.PostAsJsonAsync(_connectionString, args).Result;
            if (postResult.IsSuccessStatusCode)
            {
                JsonSerializerOptions options = new JsonSerializerOptions()
                {
                    Converters =
                    {
                        _converter
                    }
                };
                var result = _httpClient.GetFromJsonAsync<PaymentOnDateRangeReport>(_connectionString, options).Result;
                return result;
            }
            else
            {
                throw new ArgumentException("Response with such args is unsuccessful");
            }

        }
    }
}
