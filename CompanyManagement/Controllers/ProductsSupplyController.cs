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
            List<SuppliedProductViewModel> productsViewModels = _productsSupplyRepository.GetAll().Select(p => new SuppliedProductViewModel()
            {
                IdSupplier = p.IdSupplier,
                IdProduct = p.IdProduct,
                Price = p.Price,
                Date = p.Date
            }).ToList();

            return View();
        }
    }
}