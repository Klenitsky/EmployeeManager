using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EmployeeService.DAL.Models;

namespace EmployeeService.Models
{
    public class Office : BaseModel
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        public int CountryId { get; set; }
        public Country CountryNavigation { get; set; }

        public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();

    }
}
