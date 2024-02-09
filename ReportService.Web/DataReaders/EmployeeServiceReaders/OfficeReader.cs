using EmployeeService.DAL.Models;

namespace ReportService.Web.DataReaders.EmployeeServiceReaders
{
    public class OfficeReader
    {
        private HttpClient _httpClient;
        private string _connectionString;

        public OfficeReader(string connectionString)
        {
            _httpClient = new HttpClient();
            _connectionString = connectionString + "/EmployeeService/Office";
        }

        public IEnumerable<Office> Read()
        {
            List<Office> result = _httpClient.GetFromJsonAsync<IEnumerable<Office>>(_connectionString).Result.ToList();
            return result;
        }
    }
}
