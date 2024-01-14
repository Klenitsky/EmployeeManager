using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateService.DAL.DatabaseStructures.Contexts
{
    public class ExchangeRateDbContextFactory : IDesignTimeDbContextFactory<ExchangeRateDbContext>
    {
        public ExchangeRateDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder =
                    new DbContextOptionsBuilder<ExchangeRateDbContext>();
            var connectionstring = @"server=.,5433;Database=ExchangeRateStorage;
                            User Id=sa;Password=kosROR_2002; TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionstring);
            return new ExchangeRateDbContext(optionsBuilder.Options);

        }
    }
   
}
