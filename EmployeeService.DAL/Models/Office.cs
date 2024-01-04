using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeService.Models
{
    public class Office
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public int CountryId { get; set; }
        public Country CountryNavigation { get; set; }

        public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();

    }
}
