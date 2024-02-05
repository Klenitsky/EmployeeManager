using EmployeeService.DAL.Models;

namespace ReportService.Structures.ParameterModels
{
    public class ExchangeRateParametersModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<Currency> IncludedCurrencies { get; set; } 
    }
}
