using EmployeeService.DAL.Models;

namespace ReportService.DataModels
{
    public class SalaryOnDayParametersModel
    {
        public DateTime Date { get; set; }
        public List<Country> IncludedCountries { get; set; }
        public List<Office> IncludedOffices { get; set; }
    }
}
