using EmployeeService.DAL.Models;

namespace ReportService.Structures.ParameterModels
{
    public class SalarySummaryParametersModel
    {
        public DateTime Date { get; set; }
        public List<Country> IncludedCountries { get; set; }
        public List<Office> IncludedOffices { get; set; }
    }
}
