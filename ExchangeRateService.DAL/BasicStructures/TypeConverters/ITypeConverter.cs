using ExchangeRateService.DAL.BasicStructures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateService.DAL.BasicStructures.TypeFormatters
{
    public interface ITypeConverter<T>
    {
        public ExchangeRate ConvertToExchangeRate(T value);
    }
}
