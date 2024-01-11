using ExchangeRateService.DAL.BasicStructures.CurrencyLoaders.Interfaces;
using ExchangeRateService.DAL.BasicStructures.Models;
using ExchangeRateService.DAL.BasicStructures.RateReaders;
using ExchangeRateService.DAL.BasicStructures.TypeFormatters;
using ExchangeRateService.DAL.DatabaseStructures.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateService.DAL.DatabaseStructures.Repositories
{
    public interface IExchangeRateRepo
    {
        protected ExchangeRateDbContext DbContext { get;}
        protected ICurrencyLoader CurrencyLoader { get;}

        public IEnumerable<ExchangeRate> GetExchangeRates();
        public IEnumerable<ActiveCurrency> GetActiveCurrencies();

        public IEnumerable<ExchangeRate> GetExchangeRatesByDate(DateTime date);
        public IEnumerable<ExchangeRate> GetExchangeRatesByDateRange(DateTime startDate, DateTime endDate);

        public IEnumerable<ExchangeRate> GetExchangeRatesByCurrency(string currencyAbbreviation);
        public ExchangeRate GetExchangeRateByDateAndCurrency(string currencyAbbreviation, DateTime date);
        public IEnumerable<ExchangeRate> GetExchangeRateByDateRangeAndCurrency(string currencyAbbreviation, DateTime startDate,DateTime endDate);

        public void LoadActiveCurrencies() => CurrencyLoader.LoadCurrency(DbContext);


    }
}
