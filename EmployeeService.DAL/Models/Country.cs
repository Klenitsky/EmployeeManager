using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeService.Models
{
    public class Country
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public int? CurrencyId { get; set; }
        public Currency? Currency { get; set; }

        public IEnumerable<Office> Offices { get; set; } = new List<Office>();
    }
}
