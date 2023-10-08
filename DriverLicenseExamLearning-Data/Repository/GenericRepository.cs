using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public Task CreateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public EntityEntry<T> Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public T Find(Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> FindAll(Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public DbSet<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(Expression<Func<T, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetSingleAsync(Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetWhere(Expression<Func<T, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task Update(T entity, int Id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
