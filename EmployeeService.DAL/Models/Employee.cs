using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeService.Models
{
    public class Employee
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public int OfficeId { get; set; }
        public Office OfficeNavigation { get; set; }


        public int CurrencyId {  get; set; }
        public Currency CurrencyNavigation { get; set; }

        public float Salary {  get; set; }
    }
}
