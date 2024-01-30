using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateService.UI.Model
{
    public class ActiveCurrency
    {
        public int Id { get; set; }
        [Required, MaxLength(4), MinLength(3)]
        public string? Abbreviation { get; set; }
    }
}
