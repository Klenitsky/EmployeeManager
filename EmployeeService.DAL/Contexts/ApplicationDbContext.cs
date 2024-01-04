using EmployeeService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.DAL.Contexts
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Country>?  Countries { get; set; }
        public DbSet<Currency>? Currencies{ get; set; }
        public DbSet<Employee>? Employees { get; set; }
        public DbSet<Office>? Offices { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }
        


    }
}
