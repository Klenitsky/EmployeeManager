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
    public class EmployeeRepo : BasicRepo<Employee>, IEmployeeRepo
    {
        public EmployeeRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public EmployeeRepo(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public IEnumerable<Employee> GetAllByOffice(int officeId)
        {
            return Table
                    .Where(e => e.OfficeId == officeId)
                    .Include(e => e.CurrencyNavigation)
                    .Include(e => e.OfficeNavigation)
                    .ThenInclude(o => o.CountryNavigation)
                    .ThenInclude(c => c.Currency)
                    .OrderBy(e => e.LastName);
        }

        public override IEnumerable<Employee> GetAll()
        {
            return Table
                    .Include(e => e.CurrencyNavigation)
                    .Include(e => e.OfficeNavigation)
                    .ThenInclude(o => o.CountryNavigation)
                    .ThenInclude(c => c.Currency);
        }

        public override IEnumerable<Employee> GetAllIgnoreQueryFilters()
        {
            return Table
                .IgnoreQueryFilters()
                .Include(e => e.CurrencyNavigation)
                .Include(e => e.OfficeNavigation)
                .ThenInclude(o => o.CountryNavigation)
                .ThenInclude(c => c.Currency);
        }

        public override Employee? Find(int? id)
        {
            return Table
                    .Where(e => e.Id == id)
                    .Include(e => e.CurrencyNavigation)
                    .Include(e => e.OfficeNavigation)
                    .ThenInclude(o => o.CountryNavigation)
                    .ThenInclude(c => c.Currency)
                    .FirstOrDefault();
        }

        public override Employee? FindIgnoreQueryFilters(int id)
        {
            return Table
                    .IgnoreQueryFilters()
                    .Where(e => e.Id == id)
                    .Include(e => e.CurrencyNavigation)
                    .Include(e => e.OfficeNavigation)
                    .ThenInclude(o => o.CountryNavigation)
                    .ThenInclude(c => c.Currency)
                    .FirstOrDefault();
        }

        public override int Add(Employee entity, bool persist = true)
        {
            if(entity.DismissalDate != null && entity.DismissalDate < entity.EmploymentDate) 
            {
                throw new ArgumentException("Employment terms violated");
            }
            Table.Add(entity);
            return persist ? SaveChanges() : 0;
        }

        public override int AddRange(IEnumerable<Employee> entities, bool persist = true)
        {
            foreach(var entity in entities)
            {
                if (entity.DismissalDate != null && entity.DismissalDate < entity.EmploymentDate)
                {
                    throw new ArgumentException("Employment terms violated");
                }
            }
            Table.AddRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public override int Update(Employee entity, bool persist = true)
        {
            if (Table.Where(e => e.Id == entity.Id).Count() == 0)
            {
                throw new ArgumentException("Invalid entity to update");
            }
            if (entity.DismissalDate != null && entity.DismissalDate < entity.EmploymentDate)
            {
                throw new ArgumentException("Employment terms violated");
            }
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public override int UpdateRange(IEnumerable<Employee> entities, bool persist = true)
        {
            foreach (var entity in entities)
            {
                if (Table.Where(e => e.Id == entity.Id).Count() == 0)
                {
                   throw new ArgumentException("Invalid entity to update");
                }
                if (entity.DismissalDate != null && entity.DismissalDate < entity.EmploymentDate)
                {
                    throw new ArgumentException("Employment terms violated");
                }
            }
            Table.UpdateRange(entities);
            return persist ? SaveChanges() : 0;
        }
    }
}
