using EmployeeService.DAL.Contexts;
using EmployeeService.DAL.Repositories.Interfaces;
using EmployeeService.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.DAL.Repositories
{
    public class OfficeRepo : BasicRepo<Office>, IOfficeRepo
    {
        public OfficeRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public OfficeRepo(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public IEnumerable<Office> GetAllByCountry(int countryId)
        {
            return Table
                    .Where(o => o.CountryId == countryId)
                    .Include(o => o.CountryNavigation)
                    .ThenInclude(c => c.Currency)
                    .OrderBy(o => o.Name);
        }

        public override IEnumerable<Office> GetAll()
        {
            return Table
                    .Include(o => o.CountryNavigation)
                    .ThenInclude(c => c.Currency);
        }

        public override IEnumerable<Office> GetAllIgnoreQueryFilters()
        {
            return Table
                .IgnoreQueryFilters()
                .Include(o => o.CountryNavigation)
                .ThenInclude(c => c.Currency);
        }

        public override Office? Find(int? id)
        {
            return Table
                    .Where(e => e.Id == id)
                    .Include(o => o.CountryNavigation)
                    .ThenInclude(c => c.Currency)
                    .FirstOrDefault();
        }

        public override Office? FindIgnoreQueryFilters(int id)
        {
            return Table
                    .IgnoreQueryFilters()
                    .Where(e => e.Id == id)
                    .Include(o => o.CountryNavigation)
                    .ThenInclude(c => c.Currency)
                    .FirstOrDefault();
        }
    }
}
