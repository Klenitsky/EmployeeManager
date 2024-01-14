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

        public IEnumerable<ExchangeRate> GetExchangeRatesByDateOnRequest(DateTime date);
        public IEnumerable<ExchangeRate> GetExchangeRatesByDateRangeOnRequest(DateTime startDate, DateTime endDate);

        public ExchangeRate GetExchangeRateByDateAndCurrencyOnRequest(string currencyAbbreviation, DateTime date);
        public IEnumerable<ExchangeRate> GetExchangeRatesByDateRangeAndCurrencyOnRequest(string currencyAbbreviation, DateTime startDate,DateTime endDate);

        public IEnumerable<ExchangeRate>? GetExchangeRatesByCurrencyInSystem(string currencyAbbreviation);
        public IEnumerable<ExchangeRate>? GetExchangeRatesByDateInSystem(DateTime date);
        public IEnumerable<ExchangeRate>? GetExchangeRatesByDateRangeInSystem(DateTime startDate, DateTime endDate);

        public ExchangeRate? GetExchangeRateByDateAndCurrencyInSystem(string currencyAbbreviation, DateTime date);
        public IEnumerable<ExchangeRate>? GetExchangeRatesByDateRangeAndCurrencyInSystem(string currencyAbbreviation, DateTime startDate, DateTime endDate);

        public void LoadActiveCurrencies() => CurrencyLoader.LoadCurrency(DbContext);


    }
}
