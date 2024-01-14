using ExchangeRateService.DAL.BasicStructures.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateService.DAL.DatabaseStructures.Contexts
{
    public class ExchangeRateDbContext: DbContext
    {

        public DbSet<ActiveCurrency> ActiveCurrencies { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public ExchangeRateDbContext(DbContextOptions<ExchangeRateDbContext> options) : base(options) { }
    }
}
