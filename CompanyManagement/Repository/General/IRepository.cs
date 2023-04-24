using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Repository.General
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> GetAll();

        T Find(object[] keyValues);

        void Add(T entity);

        void Delete(T entity);

        void Attach(T entity);

        void Update(T entity);

        void Save();

    }
}
