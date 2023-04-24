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
        private readonly CompanyMNGEntities _context;
        //public ProductRepository()
        //{
        //    _context = new CompanyMNGEntities();
        //}
        public ProductRepository(CompanyMNGEntities context) : base(context)
        {
            _context = context;
        }
        public void Delete(int IdProduct)
        {
            Product product = _context.Product.Find(IdProduct);
            _context.Product.Remove(product);
        }

        //public IEnumerable<Product> GetAll()
        //{
        //    return _context.Product.ToList();
        //}

        public IEnumerable<Product> GetProductsByIdName(string ProductName, string idProd)
        {
            return _context.Product
                .Where(p =>
                (string.IsNullOrEmpty(idProd) || p.IdProduct.ToString().Contains(idProd))
                && ((string.IsNullOrEmpty(ProductName)) || p.Name.Contains(ProductName))).ToList();
                
        }

        public Product GetById(int IdProduct)
        {
            return _context.Product.Find(IdProduct);
        }

        //public void Insert(Product product)
        //{
        //    _context.Product.Add(product);
        //}

        //public void Save()
        //{
        //    _context.SaveChanges();
        //}

        //public void Update(Product product)
        //{
        //    _context.Entry(product).State = EntityState.Modified;
        //}
    }
}