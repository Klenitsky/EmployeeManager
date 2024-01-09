using ExchangeRateService.DAL.BasicStructures.Models;
using ExchangeRateService.DAL.BasicStructures.TypeFormatters;
using ExchangeRateService.DAL.NbrbAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateService.DAL.NbrbAPI.TypeConverters
{
    public class RBRateConverter : ITypeConverter<Rate>
    {
        public ExchangeRate ConvertToExchangeRate(Rate value)
        {
            ExchangeRate exchangeRate = new ExchangeRate()
            {
                CurrencyName = value.Cur_Name,
                Abbreviation= value.Cur_Abbreviation,
                Scale= value.Cur_Scale,
                Rate = (float)value.Cur_OfficialRate               
            };

            return exchangeRate;
        }
    }
}
