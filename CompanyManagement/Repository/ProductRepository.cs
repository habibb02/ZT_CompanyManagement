using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CompanyManagement.DAL;
using CompanyManagement.Models;
using System.Data.Entity;
using CompanyManagement.Repository.General;

namespace CompanyManagement.Repository
{
    public class ProductRepository : Repository<Product>, IEProductRepository
    {
        public ProductRepository(UnitOfWork uow) : base(uow)
        {
        }
        
        public void Delete(int IdProduct)
        {
            Product product = _uow.Context.Product.Find(IdProduct);
            _uow.Context.Product.Remove(product);
        }

        public IEnumerable<Product> GetProductsByIdName(string ProductName, string idProd)
        {
            return 
                _uow.Context.Product
                    .Where(p =>
                        (string.IsNullOrEmpty(idProd) || p.IdProduct.ToString().Contains(idProd))
                        && ((string.IsNullOrEmpty(ProductName)) || p.Name.Contains(ProductName))
                    ).ToList();
        }

        public Product GetById(int IdProduct)
        {
            return _uow.Context.Product.Find(IdProduct);
        }
    }
}