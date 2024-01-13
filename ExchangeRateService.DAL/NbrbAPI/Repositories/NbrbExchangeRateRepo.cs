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

        public ExchangeRate GetExchangeRateByDateAndCurrencyOnRequest(string currencyAbbreviation, DateTime date)
        {
            var exchangeRate = GetExchangeRateByDateAndCurrencyInSystem(currencyAbbreviation, date);

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

        public IEnumerable<ExchangeRate> GetExchangeRatesByDateRangeAndCurrencyOnRequest(string currencyAbbreviation, DateTime startDate, DateTime endDate)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();
            while(startDate <= endDate)
            {
                var currentDateRate = GetExchangeRateByDateAndCurrencyOnRequest(currencyAbbreviation, startDate);
                rates.Add(currentDateRate);
                startDate = startDate.AddDays(1);
            }

            return rates.OrderBy(r => r.Date);
        }
    

     

        public IEnumerable<ExchangeRate> GetExchangeRatesByDateOnRequest(DateTime date)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();

            List<string?> currencyAbbreviations = _dbContext.ActiveCurrencies.Select(c=>c.Abbreviation).ToList();
            foreach (var currencyAbbreviation in currencyAbbreviations)
            {
                var exchangeRate = GetExchangeRateByDateAndCurrencyOnRequest(currencyAbbreviation,date);
                rates.Add(exchangeRate);
            }

            return rates;
        }

        public IEnumerable<ExchangeRate> GetExchangeRatesByDateRangeOnRequest(DateTime startDate, DateTime endDate)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();
            while (startDate <= endDate)
            {
                var currentDateRates = GetExchangeRatesByDateOnRequest(startDate);
                rates.AddRange(currentDateRates);
                startDate =  startDate.AddDays(1);
            }

            return rates.OrderBy(r => r.CurrencyName).OrderBy(r => r.Date);
        }
        public IEnumerable<ExchangeRate> GetExchangeRates()
        {
            return _dbContext.ExchangeRates.OrderBy(r => r.CurrencyName).OrderBy(r => r.Date).ToList();
        }

        public IEnumerable<ExchangeRate>? GetExchangeRatesByCurrencyInSystem(string currencyAbbreviation)
        {
            return _dbContext.ExchangeRates.Where(r => r.Abbreviation == currencyAbbreviation);
        }

        public IEnumerable<ExchangeRate>? GetExchangeRatesByDateInSystem(DateTime date)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();

            List<string?> currencyAbbreviations = _dbContext.ActiveCurrencies.Select(c => c.Abbreviation).ToList();
            foreach (var currencyAbbreviation in currencyAbbreviations)
            {
                var exchangeRate = GetExchangeRateByDateAndCurrencyInSystem(currencyAbbreviation, date);
                if (exchangeRate != null)
                {
                    rates.Add(exchangeRate);
                }
            }

            return rates.OrderBy(r => r.Date);
        }

        public IEnumerable<ExchangeRate>? GetExchangeRatesByDateRangeInSystem(DateTime startDate, DateTime endDate)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();
            while (startDate <= endDate)
            {
                var currentDateRates = GetExchangeRatesByDateInSystem(startDate);
                if (currentDateRates != null)
                {
                    rates.AddRange(currentDateRates);
                }
               startDate =  startDate.AddDays(1);
            }

            return rates.OrderBy(r => r.CurrencyName).OrderBy(r => r.Date);
        }

        public ExchangeRate? GetExchangeRateByDateAndCurrencyInSystem(string currencyAbbreviation, DateTime date)
        {
            return _dbContext.ExchangeRates
                                         .Where(r => (r.Abbreviation.ToUpper() == currencyAbbreviation.ToUpper()) && r.Date == date)
                                         .FirstOrDefault();
        }

        public IEnumerable<ExchangeRate>? GetExchangeRatesByDateRangeAndCurrencyInSystem(string currencyAbbreviation, DateTime startDate, DateTime endDate)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();
            while (startDate <= endDate)
            {
                var currentDateRate = GetExchangeRateByDateAndCurrencyInSystem(currencyAbbreviation, startDate);
                if (currentDateRate != null)
                {
                    rates.Add(currentDateRate);
                }
                startDate = startDate.AddDays(1);
            }

            return rates.OrderBy(r => r.CurrencyName).OrderBy(r => r.Date);
        }


        private ExchangeRate LoadRate(string currencyAbbreviation, DateTime date)
        {
            var rate = _reader.ReadRate(date, currencyAbbreviation);
            var exchangeRate = _rateConverter.ConvertToExchangeRate(rate);
            _dbContext.ExchangeRates.Add(exchangeRate);
            _dbContext.SaveChanges();
            return exchangeRate;

        }

   
    }
}
