using ExchangeRateService.DAL.BasicStructures.CurrencyLoaders;
using ExchangeRateService.DAL.BasicStructures.CurrencyLoaders.Interfaces;
using ExchangeRateService.DAL.BasicStructures.Models;
using ExchangeRateService.DAL.BasicStructures.RateReaders;
using ExchangeRateService.DAL.BasicStructures.TypeFormatters;
using ExchangeRateService.DAL.DatabaseStructures.Contexts;
using ExchangeRateService.DAL.DatabaseStructures.Repositories;
using ExchangeRateService.DAL.NbrbAPI.Models;
using ExchangeRateService.DAL.NbrbAPI.RateReaders;
using ExchangeRateService.DAL.NbrbAPI.TypeConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExchangeRateService.DAL.NbrbAPI.Repositories
{
    public class NbrbExchangeRateRepo : IExchangeRateRepo
    {
        private ExchangeRateDbContext _dbContext = new ExchangeRateDbContextFactory().CreateDbContext(new List<string>().ToArray());
        private  EmployeeServiceCurrencyLoader _currencyLoader = new EmployeeServiceCurrencyLoader();
        private NbrbApiReader _reader = new NbrbApiReader();
        private NbrbRateConverter _rateConverter = new NbrbRateConverter();
        ExchangeRateDbContext IExchangeRateRepo.DbContext { get => _dbContext;}
        ICurrencyLoader IExchangeRateRepo.CurrencyLoader { get => _currencyLoader; }


        public IEnumerable<ActiveCurrency> GetActiveCurrencies()
        {
            return _dbContext.ActiveCurrencies.ToList();
        }

        public ExchangeRate GetExchangeRateByDateAndCurrency(string currencyAbbreviation, DateTime date)
        {
            var exchangeRate = _dbContext.ExchangeRates
                                         .Where(r => (r.Abbreviation.ToUpper() == currencyAbbreviation.ToUpper()) && r.Date == date)
                                         .FirstOrDefault();

            if(exchangeRate != null)
            {
                return exchangeRate;
            }
            else
            {
                ExchangeRate loadedRate = LoadRate(currencyAbbreviation, date);
                return loadedRate;
            }

        }

        public IEnumerable<ExchangeRate> GetExchangeRateByDateRangeAndCurrency(string currencyAbbreviation, DateTime startDate, DateTime endDate)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();
            while(startDate <= endDate)
            {
                var currentDateRate = GetExchangeRateByDateAndCurrency(currencyAbbreviation, startDate);
                rates.Add(currentDateRate);
                startDate.AddDays(1);
            }

            return rates;
        }
    

        public IEnumerable<ExchangeRate> GetExchangeRates()
        {
            return _dbContext.ExchangeRates.ToList();
        }

        public IEnumerable<ExchangeRate> GetExchangeRatesByCurrency(string currencyAbbreviation)
        {
            return _dbContext.ExchangeRates.Where(r => r.Abbreviation == currencyAbbreviation);
        }

        public IEnumerable<ExchangeRate> GetExchangeRatesByDate(DateTime date)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();

            List<string?> currencyAbbreviations = _dbContext.ActiveCurrencies.Select(c=>c.Abbreviation).ToList();
            foreach (var currencyAbbreviation in currencyAbbreviations)
            {
                var exchangeRate = GetExchangeRateByDateAndCurrency(currencyAbbreviation,date);
                rates.Add(exchangeRate);
            }

            return rates;
        }

        public IEnumerable<ExchangeRate> GetExchangeRatesByDateRange(DateTime startDate, DateTime endDate)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();
            while (startDate <= endDate)
            {
                var currentDateRates = GetExchangeRatesByDate(startDate);
                rates.AddRange(currentDateRates);
                startDate.AddDays(1);
            }

            return rates;
        }


        private ExchangeRate LoadRate(string currencyAbbreviation, DateTime date)
        {
            var rate = _reader.ReadRate(date, currencyAbbreviation);
            var exchangeRate = _rateConverter.ConvertToExchangeRate(rate);
            _dbContext.ExchangeRates.Add(exchangeRate);
            return exchangeRate;

        }
    }
}
