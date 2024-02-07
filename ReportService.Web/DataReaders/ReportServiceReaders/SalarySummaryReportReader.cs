using ReportService.Structures.ParameterModels;
using ReportService.Structures.Reports.PaymentOnDateRange;
using ReportService.Structures.Reports.SalarySummary;
using System.Text.Json;

namespace ReportService.Web.DataReaders.ReportServiceReaders
{
    public class SalarySummaryReportReader
    {
        private HttpClient _httpClient;
        private string _connectionString;
        private static SalarySummaryReportJsonConverter _converter = new SalarySummaryReportJsonConverter();
        public SalarySummaryReportReader(string connectionString)
        {
            _httpClient = new HttpClient();
            _connectionString = connectionString + "/SalarySummaryReport";
        }


        public SalarySummaryReport GetReport(SalarySummaryParametersModel args)
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
                var result = _httpClient.GetFromJsonAsync<SalarySummaryReport>(_connectionString, options).Result;
                return result;
            }
            else
            {
                throw new ArgumentException("Response with such args is unsuccessful");
            }

        }
    }
}
