using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EmployeeService.DAL.Models;

namespace EmployeeService.Models
{
    public class Country: BaseModel
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MinLength(2),MaxLength(3)]
        public string Abbreviation { get; set; }

        public int? CurrencyId { get; set; }
        public Currency? Currency { get; set; }

        public IEnumerable<Office> Offices { get; set; } = new List<Office>();
    }
}
