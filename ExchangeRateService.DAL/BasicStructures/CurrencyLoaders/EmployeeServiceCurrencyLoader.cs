using ExchangeRateService.DAL.BasicStructures.CurrencyLoaders.Interfaces;
using ExchangeRateService.DAL.DatabaseStructures.Contexts;
using EmployeeService.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using ExchangeRateService.DAL.BasicStructures.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateService.DAL.BasicStructures.CurrencyLoaders
{
    public class EmployeeServiceCurrencyLoader : ICurrencyLoader
    {
        public void LoadCurrency(ExchangeRateDbContext dbContext)
        {
            //TODO: 
            //Move this connection string to the settings file, and configure it to use everything via docker
            string connectionString = "https://localhost:7054/api/EmployeeService/Currency";

            HttpClient httpClient = new HttpClient();
            IEnumerable<Currency> currencies =  httpClient.GetFromJsonAsync<IEnumerable<Currency>>(connectionString).Result;

            List<ActiveCurrency> activeCurrencies = new List<ActiveCurrency>();

            foreach(var currency in currencies)
            {
                var activeCurrency = new ActiveCurrency() { Abbreviation = currency.Abbreviation };
                activeCurrencies.Add(activeCurrency);
            }

            foreach (var activeCurrency in activeCurrencies)
            {
                if(dbContext.ActiveCurrencies.Where(c => c.Abbreviation.ToUpper() == activeCurrency.Abbreviation.ToUpper()).ToList().Count == 0)
                {
                    dbContext.ActiveCurrencies.Add(activeCurrency);
                }
            }
        }
    }
}
