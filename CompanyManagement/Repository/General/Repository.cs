using CompanyManagement.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CompanyManagement.Repository.General
{
    public class Repository<T> : IRepository<T> where T : class
    {
        //private readonly CompanyMNGEntities _context;
        protected UnitOfWork _uow;

        public Repository(UnitOfWork uow)
        {
            _uow = uow;
        }

        public IEnumerable<T> GetAll()
        {
            return _uow.Context.Set<T>().AsEnumerable();
        }

        public T Find(object[] keyValues)
        {
            return _uow.Context.Set<T>().Find(keyValues);
        }

        public void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentException("Cannot add a null entity");

            _uow.Context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentException("Cannot add a null entity");

            _uow.Context.Entry(entity).State = EntityState.Modified;

        }

        public virtual void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentException("Cannot add a null entity");

            _uow.Context.Set<T>().Remove(entity);
        }

        public void Attach(T entity)
        {
            if (entity == null)
                throw new ArgumentException("Cannot add a null entity");

            _uow.Context.Set<T>().Attach(entity);
        }

        public void Save()
        {
            _uow.Context.SaveChanges();
        }

        public void Dispose()
        {
            _uow.Dispose();
        }
    }
}