using ExchangeRateService.DAL.DatabaseStructures.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateService.DAL.BasicStructures.CurrencyLoaders.Interfaces
{
    public interface ICurrencyLoader
    {

        public void LoadCurrency(ExchangeRateDbContext dbContext);
    }
}
