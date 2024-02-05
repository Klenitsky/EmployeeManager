using EmployeeService.DAL.Models;

namespace ReportService.Structures.ParameterModels
{
    public class PaymentParametersModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Country> IncludedCountries { get; set; }
        public List<Office> IncludedOffices { get; set; }
    }
}
