using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CompanyManagement.Repository;
using CompanyManagement.DAL;
using CompanyManagement.Models;
using System.Web.Mvc;
using System.Text;
using System.IO;

namespace CompanyManagement.Controllers
{
    public class ProductController : Controller
    {
        private IEProductRepository _productRepository;
        private IEOrderRepository _orderRepository;
        // GET: Product

        public ProductController()
        {
            _orderRepository = new OrderRepository(new CompanyMNGEntities());
            _productRepository = new ProductRepository(new CompanyMNGEntities());
        }
        public ProductController(IEProductRepository productRepository, IEOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<ProductsViewModel> productsViewModels = _productRepository.GetAll().Select(p => new ProductsViewModel()
            {
                IdProduct = p.IdProduct,
                Description = p.Description,
                Name = p.Name,
                Price = p.Price,
                Type = p.Type,
                AttachmentFileName = p.AttachmentFileName,
            }).ToList();

            return View(productsViewModels);
        }

        [HttpGet]
        public ActionResult SearchProduct()
        {
            List<ProductsViewModel> productsViewModels = _productRepository.GetAll().Select(p => new ProductsViewModel()
            {
                IdProduct = p.IdProduct,
                Description = p.Description,
                Name = p.Name,
                Price = p.Price,
                Type = p.Type
            }).ToList();

            return View(productsViewModels);
        }

        [HttpPost]
        public ActionResult SearchProduct(string ProductName, string idProd)
        {
            IEnumerable<Product> productsList = _productRepository.GetProductsByIdName(ProductName, idProd);
            List<ProductsViewModel> productsViewModel = new List<ProductsViewModel>();

            foreach (var p in productsList)
            {
                productsViewModel.Add(new ProductsViewModel()
                {
                    Description = p.Description,
                    IdProduct = p.IdProduct,
                    Name = p.Name,
                    Price = p.Price,
                    Type = p.Type
                });
            }
            return View(productsViewModel);
        }

        [HttpGet]
        public ActionResult _DeleteProduct(int IdProduct)
        {
            Product productModel = _productRepository.GetById(IdProduct);
            ProductsViewModel productsViewModel = new ProductsViewModel()
            {
                IdProduct = productModel.IdProduct,
                Description = productModel.Description,
                Name = productModel.Name,
                Price = productModel.Price,
                Type = productModel.Type,
                //  Attachment = productModel.Attachment,
            };

            return PartialView(productsViewModel);
        }

        [HttpPost]
        public ActionResult Delete(int IdProduct)
        {
            try
            {
                _productRepository.Delete(IdProduct);
                _productRepository.Save();
            }
            catch (Exception ex)
            {
                TempData["errorMsg"] = ex.Message;
                string test = TempData["errorMsg"].ToString();
            }
            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public ActionResult _EditProduct(int IdProduct)
        {
            Product productModel = _productRepository.GetById(IdProduct);
            ProductsViewModel productsViewModel = new ProductsViewModel()
            {
                IdProduct = productModel.IdProduct,
                Description = productModel.Description,
                Name = productModel.Name,
                Price = productModel.Price,
                Type = productModel.Type,
                AttachmentFileName = productModel.AttachmentFileName,
                AttachmentByteArr = productModel.Attachment,

                //Attachment = productModel.Attachment,
            };

            return PartialView(productsViewModel);
        }

        [HttpPost]
        public ActionResult _EditProduct(ProductsViewModel productsViewModel)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product()
                {
                    IdProduct = productsViewModel.IdProduct,
                    Description = productsViewModel.Description,
                    Name = productsViewModel.Name,
                    Price = productsViewModel.Price,
                };

                if (productsViewModel.Attachment != null)
                {
                    product.Attachment = ConvertToByteArray(productsViewModel.Attachment);
                    product.AttachmentFileName = productsViewModel.Attachment.FileName;
                }

                _productRepository.Update(product);
                _productRepository.Save();
                return RedirectToAction("Index", "Product");
            }
            else
                return PartialView(productsViewModel);
        }

        [HttpGet]
        public ActionResult _AddProduct()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult _AddProduct(ProductsViewModel productsViewModel)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product();

                product.IdProduct = productsViewModel.IdProduct;
                product.Description = productsViewModel.Description;
                product.Name = productsViewModel.Name;
                product.Price = productsViewModel.Price;
                product.Type = "default";

                if (productsViewModel.Attachment != null)
                {
                    product.Attachment = ConvertToByteArray(productsViewModel.Attachment);
                    product.AttachmentFileName = productsViewModel.Attachment.FileName;
                }

                _productRepository.Add(product);
                _productRepository.Save();

                return RedirectToAction("Index", "Product");
            }
            return PartialView();
        }

        [HttpGet]
        public ActionResult GenerateProductPrice()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GenerateProductPrice(ProductPriceViewModel productPriceViewModel)
        {
            try
            {
                productPriceViewModel.Delivery = productPriceViewModel.Delivery / productPriceViewModel.Qty;
                productPriceViewModel.Price = (productPriceViewModel.Materials + productPriceViewModel.Manpower + productPriceViewModel.FixedCosts) + (productPriceViewModel.Materials + productPriceViewModel.Manpower + productPriceViewModel.FixedCosts) * productPriceViewModel.PercentageIncrease / 100 + productPriceViewModel.Delivery;

                Product product = new Product()
                {
                    Name = productPriceViewModel.Name,
                    Description = productPriceViewModel.Description,
                    Price = productPriceViewModel.Price,
                    Type = "generated",
                };
                _productRepository.Add(product);
                _productRepository.Save();
            }
            catch (Exception ex)
            {
                return View(ex);
            }

            return RedirectToAction("Index", "Product");
        }

        public ActionResult _DownloadAttachment(int IdProd)
        {
            Product productModel = new Product();
            productModel = _productRepository.GetById(IdProd);

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=" + productModel.AttachmentFileName);
            Response.BinaryWrite(productModel.Attachment);
            Response.End();

            return RedirectToAction("Index", "Product");
        }


        private byte[] ConvertToByteArray(HttpPostedFileBase attachment)
        {
            if (attachment != null)
            {
                var memoryStream = new MemoryStream();
                attachment.InputStream.CopyTo(memoryStream);
                var file = memoryStream.ToArray();
                return file;
            }
            else
                return null;
        }
    }

}