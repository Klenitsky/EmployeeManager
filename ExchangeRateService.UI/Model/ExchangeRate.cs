using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateService.UI.Model
{
    public class ExchangeRate 
    {
        public int Id {get; set;}

        [Required, MaxLength(50)]
        public string? CurrencyName { get; set; }

        [Required, MaxLength(4), MinLength(3)]
        public string? Abbreviation { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Scale { get; set; }

        [Required, Range(0, float.MaxValue)]
        public float Rate { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
