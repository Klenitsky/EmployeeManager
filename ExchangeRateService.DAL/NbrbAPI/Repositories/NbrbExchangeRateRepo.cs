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
            var currencies = _dbContext.ActiveCurrencies.ToList();
            return currencies;
        }

        public ExchangeRate GetExchangeRateByDateAndCurrencyOnRequest(string currencyAbbreviation, DateTime date)
        {
            if (date < DateTime.Today.AddYears(-5) || date > DateTime.Now)
            {
                throw new ArgumentOutOfRangeException("Wrong date period");
            }
            if (!_dbContext.ActiveCurrencies.Select(c => c.Abbreviation.ToUpper()).Contains(currencyAbbreviation))
            {
                throw new ArgumentException("Impossible currency");
            }
            var exchangeRate = LoadRateByDateRangeAndCurrency(currencyAbbreviation, date, date).First();
            return exchangeRate;

        }

        public IEnumerable<ExchangeRate> GetExchangeRatesByDateRangeAndCurrencyOnRequest(string currencyAbbreviation, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate || startDate < DateTime.Today.AddYears(-5) || endDate > DateTime.Now)
            {
                throw new ArgumentOutOfRangeException("Wrong date period");
            }
            if (!_dbContext.ActiveCurrencies.Select(c => c.Abbreviation.ToUpper()).Contains(currencyAbbreviation))
            {
                throw new ArgumentException("Impossible currency");
            }

            var rates = LoadRateByDateRangeAndCurrency(currencyAbbreviation, startDate, endDate);
            return rates.OrderBy(r => r.Date);
        }
    

     

        public IEnumerable<ExchangeRate> GetExchangeRatesByDateOnRequest(DateTime date)
        {
            if (date < DateTime.Today.AddYears(-5) || date > DateTime.Now)
            {
                throw new ArgumentOutOfRangeException("Wrong date period");
            }

            List<ExchangeRate> rates = new List<ExchangeRate>();

            List<string?> currencyAbbreviations = _dbContext.ActiveCurrencies.Select(c=>c.Abbreviation).ToList();
            foreach (var currencyAbbreviation in currencyAbbreviations)
            {
                var exchangeRate = LoadRateByDateRangeAndCurrency(currencyAbbreviation, date, date).First(); ;
                rates.Add(exchangeRate);
            }

            return rates;
        }

        public IEnumerable<ExchangeRate> GetExchangeRatesByDateRangeOnRequest(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate || startDate < DateTime.Today.AddYears(-5) || endDate > DateTime.Now)
            {
                throw new ArgumentOutOfRangeException("Wrong date period");
            }
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
            if (!_dbContext.ActiveCurrencies.Select(c => c.Abbreviation.ToUpper()).Contains(currencyAbbreviation))
            {
                throw new ArgumentException("Impossible currency");
            }
            return _dbContext.ExchangeRates.Where(r => r.Abbreviation == currencyAbbreviation).OrderBy(r => r.Date);
        }

        public IEnumerable<ExchangeRate>? GetExchangeRatesByDateInSystem(DateTime date)
        {
            if (date < DateTime.Today.AddYears(-5) || date > DateTime.Now)
            {
                throw new ArgumentOutOfRangeException("Wrong date period");
            }
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
            if (startDate > endDate || startDate < DateTime.Today.AddYears(-5) || endDate > DateTime.Now)
            {
                throw new ArgumentOutOfRangeException("Wrong date period");
            }
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
            if (date < DateTime.Today.AddYears(-5) || date > DateTime.Now)
            {
                throw new ArgumentOutOfRangeException("Wrong date period");
            }
            if (!_dbContext.ActiveCurrencies.Select(c => c.Abbreviation.ToUpper()).Contains(currencyAbbreviation))
            {
                throw new ArgumentException("Impossible currency");
            }
            return _dbContext.ExchangeRates
                                         .Where(r => (r.Abbreviation.ToUpper() == currencyAbbreviation.ToUpper()) && r.Date == date)
                                         .FirstOrDefault();
        }

        public IEnumerable<ExchangeRate>? GetExchangeRatesByDateRangeAndCurrencyInSystem(string currencyAbbreviation, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate || startDate < DateTime.Today.AddYears(-5) || endDate > DateTime.Now)
            {
                throw new ArgumentOutOfRangeException("Wrong date period");
            }
            if (!_dbContext.ActiveCurrencies.Select(c => c.Abbreviation.ToUpper()).Contains(currencyAbbreviation))
            {
                throw new ArgumentException("Impossible currency");
            }
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


        private IEnumerable<ExchangeRate> LoadRateByDateRangeAndCurrency(string currencyAbbreviation, DateTime startDate,DateTime endDate)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();
            if(startDate> endDate || startDate< DateTime.Today.AddYears(-5) || endDate > DateTime.Now)
            {
                throw new ArgumentOutOfRangeException("Wrong date period");
            }
            if (!_dbContext.ActiveCurrencies.Select(c => c.Abbreviation.ToUpper()).Contains(currencyAbbreviation))
            {
                throw new ArgumentException("Impossible currency");
            }
            while (startDate <= endDate)
            {
                var rateInSystem = _dbContext.ExchangeRates
                                         .Where(r => (r.Abbreviation.ToUpper() == currencyAbbreviation.ToUpper()) && r.Date == startDate)
                                         .FirstOrDefault();
                if (rateInSystem == null)
                {
                    var rate = _reader.ReadRate(startDate, currencyAbbreviation);
                    var exchangeRate = _rateConverter.ConvertToExchangeRate(rate);
                    _dbContext.ExchangeRates.Add(exchangeRate);
                    rates.Add(exchangeRate);
                }
                else
                {
                    rates.Add(rateInSystem);
                }

                startDate = startDate.AddDays(1);

            }
          
            _dbContext.SaveChanges();
            return rates;

        }

   
    }
}
