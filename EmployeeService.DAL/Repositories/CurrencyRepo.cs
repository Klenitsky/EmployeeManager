using EmployeeService.DAL.Contexts;
using EmployeeService.DAL.Repositories.Interfaces;
using EmployeeService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.DAL.Repositories
{
    public class CurrencyRepo : BasicRepo<Currency>, ICurrencyRepo
    {
        public CurrencyRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public CurrencyRepo(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
