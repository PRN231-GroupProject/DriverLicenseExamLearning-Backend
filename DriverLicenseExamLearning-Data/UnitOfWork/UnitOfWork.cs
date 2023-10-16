using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PRN231_DriverLicenseExamLearningContext _driverLicenseExamLearningContext;
        public UnitOfWork(PRN231_DriverLicenseExamLearningContext driverLicenseExamLearningContext)
        {
            _driverLicenseExamLearningContext = driverLicenseExamLearningContext;
        }
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();
        public int Commit()
        {
            return _driverLicenseExamLearningContext.SaveChanges();
        }
        public Task<int> CommitAsync() => _driverLicenseExamLearningContext.SaveChangesAsync();

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _driverLicenseExamLearningContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            Type type = typeof(T);
            if (!repositories.TryGetValue(type, out object value))
            {
                var genericRepos = new GenericRepository<T>(_driverLicenseExamLearningContext);
                repositories.Add(type, genericRepos);
                return genericRepos;
            }
            return value as GenericRepository<T>;
        }
    }
}
