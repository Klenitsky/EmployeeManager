using EmployeeService.DAL.Models;

namespace ReportService.Web.DataReaders.EmployeeServiceReaders
{
    public class CurrencyReader
    {
        private HttpClient _httpClient;
        private string _connectionString;

        public CurrencyReader(string connectionString)
        {
            _httpClient = new HttpClient();
            _connectionString = connectionString + "/EmployeeService/Currency";
        }

        public IEnumerable<Currency> Read()
        {
            List<Currency> result = _httpClient.GetFromJsonAsync<IEnumerable<Currency>>(_connectionString).Result.ToList();
            return result;
        }
    }
}
