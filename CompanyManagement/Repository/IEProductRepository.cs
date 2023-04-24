using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyManagement.DAL;
using CompanyManagement.Repository.General;

namespace CompanyManagement.Repository
{
    public interface IEProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetProductsByIdName(string ProductName, string idProd);

        Product GetById(int IdProduct);

        void Delete(int IdProduct);

        void Update(Product product);
    }
}
