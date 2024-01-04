using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeService.Models
{
    public class Employee
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        public int OfficeId { get; set; }
        public Office OfficeNavigation { get; set; }


        public int CurrencyId {  get; set; }
        public Currency CurrencyNavigation { get; set; }

        [Required,Range(0.0,float.MaxValue)]
        public float Salary {  get; set; }
    }
}
