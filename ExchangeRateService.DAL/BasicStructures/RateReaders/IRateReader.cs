using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateService.DAL.BasicStructures.RateReaders
{
    public interface IRateReader<T>
    {

        public T ReadRate(DateTime date, string currencyCode);
    }
}
