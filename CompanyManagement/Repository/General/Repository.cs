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
        private readonly CompanyMNGEntities _context;
        public Repository(CompanyMNGEntities context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsEnumerable();
        }

        public T Find(object[] keyValues)
        {
            return _context.Set<T>().Find(keyValues);
        }

        public void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentException("Cannot add a null entity");

            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentException("Cannot add a null entity");

            _context.Entry(entity).State = EntityState.Modified;

        }

        public virtual void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentException("Cannot add a null entity");

            _context.Set<T>().Remove(entity);
        }

        public void Attach(T entity)
        {
            if (entity == null)
                throw new ArgumentException("Cannot add a null entity");

            _context.Set<T>().Attach(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}