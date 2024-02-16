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

        public virtual int Add(T entity, bool persist = true)
        {
            Table.Add(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int AddRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.AddRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Update(T entity, bool persist = true)
        {
            if(Table.Where(e=> e.Id == entity.Id).Count() == 0)
            {
                throw new ArgumentException("Invalid entity to update");
            }
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int UpdateRange(IEnumerable<T> entities, bool persist = true)
        {
            foreach(var entity in entities)
            {
                if (Table.Where(e => e.Id == entity.Id).Count() == 0)
                {
                    throw new ArgumentException("Invalid entity to update");
                }
            }
            Table.UpdateRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual  int Delete(int id, bool persist = true)
        {
            if(Table.Where(o => o.Id == id).Count() == 0)
            {
                throw new ArgumentException("No entity to delete found");
            }
            var entity = Table.Where(o =>  o.Id == id).FirstOrDefault();
            Delete(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Delete(T entity, bool persist = true)
        {
            if (Table.Where(o => o.Id == entity.Id).Count() == 0)
            {
                throw new ArgumentException("No entity to delete found");
            }
            Table.Remove(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int DeleteRange(IEnumerable<T> entities, bool persist = true)
        {
            foreach (var entity in entities)
            {
                if (Table.Where(e => e.Id == entity.Id).Count() == 0)
                {
                    throw new ArgumentException("Invalid entity to delete");
                }
            }
            Table.RemoveRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual  T? Find(int? id)
        {
            return Table.Find(id);
        }

        public virtual T? FindAsNoTracking(int id)
        {
            return  Table.AsNoTrackingWithIdentityResolution().FirstOrDefault(x => x.Id  == id);
        }

        public virtual T? FindIgnoreQueryFilters(int id)
        {
          return Table.IgnoreQueryFilters().FirstOrDefault(x=> x.Id == id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Table;
        }

        public virtual IEnumerable<T> GetAllIgnoreQueryFilters()
        {
            return Table.IgnoreQueryFilters();
        }

        public virtual void ExecuteQuery(string sql, object[] sqlParametersObjects)
        {
            DbContext.Database.ExecuteSqlRaw(sql, sqlParametersObjects);
        }

        public virtual int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        ~BasicRepo()
        {
            Dispose(false);
        }

        
    }
   
}
