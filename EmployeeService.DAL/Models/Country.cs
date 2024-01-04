using Microsoft.EntityFrameworkCore;
namespace EmployeeService.Models
{
    public class Country
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public int? CurrencyId { get; set; }
        public Currency? Currency { get; set; }

        public IEnumerable<Office> Offices { get; set; } = new List<Office>();
    }
}
