using CompanyManagement.DAL;
using CompanyManagement.Models;
using CompanyManagement.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompanyManagement.Controllers
{
    public class ProductsSupplyController : Controller
    {
        private ProductsSupplyRepository _productsSupplyRepository;

        public ProductsSupplyController()
        {
            _productsSupplyRepository = new ProductsSupplyRepository(new CompanyMNGEntities());
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<SuppliedProductViewModel> suppliedProductsViewModels = _productsSupplyRepository.GetAll().Select(p => new SuppliedProductViewModel()
            {
                IdSupplier = p.IdSupplier,
                IdProduct = p.IdProduct,
                Price = p.Price,
                Date = p.Date,
                Supplier = p.Supplier,
                Product = p.Product,
            }).ToList();

            return View(suppliedProductsViewModels);
        }

        [HttpGet]
        public ActionResult _AddSuppliedProduct(int IdSupplier, int IdProduct)
        {
            SuppliedProduct suppliedProduct = _productsSupplyRepository.Find(new object[] { IdSupplier, IdProduct });
            SuppliedProductViewModel suppliedProductViewModel = new SuppliedProductViewModel()
            {
                IdSupplier = suppliedProduct.IdSupplier,
                IdProduct = suppliedProduct.IdProduct,
                Price = suppliedProduct.Price,
                Date = suppliedProduct.Date,
                Supplier = suppliedProduct.Supplier,
                Product = suppliedProduct.Product
            };

            return PartialView(suppliedProductViewModel);
        }

        [HttpGet]
        public ActionResult _EditSuppliedProduct(int IdSupplier, int IdProduct)
        {
            SuppliedProduct suppliedProduct = _productsSupplyRepository.Find(new object[] { IdSupplier, IdProduct });
            SuppliedProductViewModel suppliedProductViewModel = new SuppliedProductViewModel()
            {
                IdSupplier = suppliedProduct.IdSupplier,
                IdProduct = suppliedProduct.IdProduct,
                Price = suppliedProduct.Price,
                Date = suppliedProduct.Date,
                Supplier = suppliedProduct.Supplier,
                Product = suppliedProduct.Product
            };

            return PartialView(suppliedProductViewModel);
        }

        [HttpGet]
        public ActionResult _DeleteSuppliedProduct(int IdSupplier, int IdProduct)
        {
            SuppliedProduct suppliedProduct = _productsSupplyRepository.Find(new object[] { IdSupplier, IdProduct });
            SuppliedProductViewModel suppliedProductViewModel = new SuppliedProductViewModel()
            {
                IdSupplier = suppliedProduct.IdSupplier,
                IdProduct = suppliedProduct.IdProduct,
                Price = suppliedProduct.Price,
                Date = suppliedProduct.Date,
                Supplier = suppliedProduct.Supplier,
                Product = suppliedProduct.Product
            };

            return PartialView(suppliedProductViewModel);
        }
    }
}