using EmployeeService.DAL.Models;

namespace ReportService.Web.DataReaders.EmployeeServiceReaders
{
    public class CountryReader
    {
        private HttpClient _httpClient;
        private string _connectionString;

        public CountryReader(string connectionString)
        {
            _httpClient = new HttpClient();
            _connectionString = connectionString + "/EmployeeService/Country";
        }

        public IEnumerable<Country> Read()
        {
            List<Country> result = _httpClient.GetFromJsonAsync<IEnumerable<Country>>(_connectionString).Result.ToList();
            return result;
        }
    }
}
