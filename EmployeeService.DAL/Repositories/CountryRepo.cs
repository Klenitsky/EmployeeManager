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
    public class CountryRepo : BasicRepo<Country>, ICountryRepo
    {
        public CountryRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public CountryRepo(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public override IEnumerable<Country> GetAll()
        {
            return Table
                    .Include(c => c.Currency);
        }

        public override IEnumerable<Country> GetAllIgnoreQueryFilters()
        {
            return Table
                .IgnoreQueryFilters()
                .Include(c => c.Currency);          
        }

        public override Country? Find(int? id)
        {
            return Table
                    .Where(c=> c.Id == id)
                    .Include(c=> c.Currency)
                    .FirstOrDefault();
        }

        public override Country? FindIgnoreQueryFilters(int id)
        {
            return Table
                    .IgnoreQueryFilters()
                    .Where(c => c.Id == id)
                    .Include(c => c.Currency)
                    .FirstOrDefault();
        }

        

    }
}
