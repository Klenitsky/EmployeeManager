using EmployeeService.DAL.Contexts;
using EmployeeService.DAL.Models;
using EmployeeService.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.DAL.Repositories
{
    public abstract class BasicRepo<T>: IRepo<T> where T: BaseModel, new()
    {
        private readonly bool _disposeContext;
        public DbSet<T> Table { get; }
        public ApplicationDbContext DbContext { get; }
        protected BasicRepo(DbContextOptions<ApplicationDbContext> options)
                : this(new ApplicationDbContext(options))
        {
            _disposeContext = true;
        }

        public BasicRepo(ApplicationDbContext applicationDbContext)
        {
            this.DbContext = applicationDbContext;
            Table = DbContext.Set<T>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool _isDisposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }
            if (disposing)
            {
                if (
                _disposeContext)
                {
                    DbContext.Dispose();
                }
            }
            _isDisposed = true;
        }

        public int Add(T entity, bool persist = true)
        {
            Table.Add(entity);
            return persist ? SaveChanges() : 0;
        }

        public int AddRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.AddRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public int Update(T entity, bool persist = true)
        {
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public int UpdateRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.UpdateRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public int Delete(int id, bool persist = true)
        {
            var entity = new T { Id = id };
            DbContext.Entry(entity).State = EntityState.Deleted;
            return persist ? SaveChanges() : 0;
        }

        public int Delete(T entity, bool persist = true)
        {
            Table.Remove(entity);
            return persist ? SaveChanges() : 0;
        }

        public int DeleteRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.RemoveRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public T? Find(int? id)
        {
            return Table.Find(id);
        }

        public T? FindAsNoTracking(int id)
        {
            return  Table.AsNoTrackingWithIdentityResolution().FirstOrDefault(x => x.Id  == id);
        }

        public T? FindlgnoreQueryFilters(int id)
        {
          return Table.IgnoreQueryFilters().FirstOrDefault(x=> x.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return Table;
        }

        public IEnumerable<T> GetAllIgnoreQueryFilters()
        {
            return Table.IgnoreQueryFilters();
        }

        public void ExecuteQuery(string sql, object[] sqlParametersObjects)
        {
            DbContext.Database.ExecuteSqlRaw(sql, sqlParametersObjects);
        }

        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        ~BasicRepo()
        {
            Dispose(false);
        }

        
    }
   
}
