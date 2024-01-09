using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EmployeeService.DAL.Models;

namespace EmployeeService.DAL.Models
{
    public class Currency : BaseModel
    {
      
        [Required, MaxLength(50)]
        public string Name { get; set; }
        public int Code { get; set; }
        [Required, MinLength(2), MaxLength(3)]
        public string Abbreviation { get; set; }
    }
}
