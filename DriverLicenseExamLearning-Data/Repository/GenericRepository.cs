using DriverLicenseExamLearning_Data.Entity;
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
        private static DriverLicenseExamLearningContext Context;
        private static DbSet<T> Table { get; set; }
        public GenericRepository(DriverLicenseExamLearningContext context)
        {
            Context = context;
            Table = Context.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await Context.AddAsync(entity);
        }

        public EntityEntry<T> Delete(T entity)
        {
            return Context.Remove(entity);
        }

        public T Find(Func<T, bool> predicate)
        {
            return Table.FirstOrDefault(predicate);
        }

        public IQueryable<T> FindAll(Func<T, bool> predicate)
        {
            return Table.Where(predicate).AsQueryable();
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await Table.SingleOrDefaultAsync(predicate);
        }

        public DbSet<T> GetAll()
        {
            return Table;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await Table.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = Table;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await Table.FindAsync(id);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> filter)
        {
            return await Table.SingleOrDefaultAsync(filter);
        }

        public async Task<List<T>> GetWhere(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = Table;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = Table;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        public async Task Update(T entity, int Id)
        {
            var existEntity = await GetById(Id);
            Context.Entry(existEntity).CurrentValues.SetValues(entity);
            Table.Update(existEntity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return Table.Where(predicate);
        }
    }
}
