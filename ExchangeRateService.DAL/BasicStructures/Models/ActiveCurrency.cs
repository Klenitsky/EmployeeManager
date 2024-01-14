using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateService.DAL.BasicStructures.Models
{
    public class ActiveCurrency: BaseModel
    {
        [Required, MaxLength(4), MinLength(3)]
        public string? Abbreviation { get; set; }
    }
}
