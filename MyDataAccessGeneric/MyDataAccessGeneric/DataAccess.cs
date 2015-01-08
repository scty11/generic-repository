using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataAccessGeneric
{
    class EmployeeDb : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
    }

    //allows us to use a person repos to read a employss repos
    public interface IReadOnlyRepository< out T>
    {
        T FindBy(int id);
        IQueryable<T> FindAll();
    }
    //allows us to use a employee repos as a manager repos
    public interface IWriteOnlyRepository<in T>
    {
        void Add(T newEntity);
        void Delete(T entity);
        int Commit();
    }
    public interface IRepository<T> : IWriteOnlyRepository<T>,IReadOnlyRepository<T>,IDisposable
    {
        
    }

    //the where is constraining the T to be a reference type.
    //and also it must implement the IEntity interface.
    //and a default constructor
    public class SqlRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        DbContext _ctx;
        DbSet<T> _set;

        public SqlRepository(DbContext ctx)
        {
            _ctx = ctx;
            _set = ctx.Set<T>();
        }

        public void Add(T newEntity)
        {
            if (newEntity.IsValid())
            {
                _set.Add(newEntity);
            }          
        }

        public void Delete(T entity)
        {
            _set.Remove(entity);
        }

        public T FindBy(int id)
        {
            return _set.Find(id);
        }

        public IQueryable<T> FindAll()
        {
            return _set;
        }

        public int Commit()
        {
           return  _ctx.SaveChanges();
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}
