using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.DAL.Contexts
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder =
                    new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionstring = @"server=.,5433;Database=EmployeeStorage;
                            User Id=sa;Password=kosROR_2002; TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionstring);
            //Console.WriteLine(connectionstring);
            return new ApplicationDbContext(optionsBuilder.Options);

        }
    }
}
