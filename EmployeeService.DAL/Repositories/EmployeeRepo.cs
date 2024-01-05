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
                    .OrderBy(e => e.LastName);
        }

        public override IEnumerable<Employee> GetAll()
        {
            return Table
                    .Include(e => e.CurrencyNavigation)
                    .Include(e=>e.OfficeNavigation);
        }

        public override IEnumerable<Employee> GetAllIgnoreQueryFilters()
        {
            return Table
                .IgnoreQueryFilters()
                .Include(e => e.CurrencyNavigation)
                .Include(e => e.OfficeNavigation);
        }

        public override Employee? Find(int? id)
        {
            return Table
                    .Where(e => e.Id == id)
                    .Include(e => e.CurrencyNavigation)
                    .Include(e => e.OfficeNavigation)
                    .FirstOrDefault();
        }

        public override Employee? FindIgnoreQueryFilters(int id)
        {
            return Table
                    .IgnoreQueryFilters()
                    .Where(e => e.Id == id)
                    .Include(e => e.CurrencyNavigation)
                    .Include(e => e.OfficeNavigation)
                    .FirstOrDefault();
        }
    }
}
