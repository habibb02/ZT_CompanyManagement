using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CompanyManagement.DAL;
using CompanyManagement.Models;
using CompanyManagement.Repository;
using CompanyManagement.Repository.General;

namespace CompanyManagement.Controllers
{
    public class SupplierController : Controller
    {
        private IESupplierRepository _supplierRepository;
        private IEOrderRepository _orderRepository;
        private IEOrderDetailRepository _orderProductRepository;

        public SupplierController()
        {
            _supplierRepository = new SupplierRepository(new UnitOfWork());
            _orderRepository = new OrderRepository(new UnitOfWork());
            _orderProductRepository = new OrderDetailRepository(new UnitOfWork());
        }
        public SupplierController(IESupplierRepository supplierRepository, IEOrderRepository orderRepository, IEOrderDetailRepository orderProductRepository)
        {
            _supplierRepository = supplierRepository;
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<SupplierViewModel> suppliersList = _supplierRepository.GetAll().Select(o => new SupplierViewModel()
            {
                IdSupplier = o.IdSupplier,
                Address = o.Address,
                Name = o.Name,
                Email = o.Email,
                Phone = o.Phone,
            }).ToList();

            var model = _supplierRepository.GetAll();
            return View(suppliersList);
        }

        [HttpGet]
        public ActionResult SearchSupplier()
        {
            List<SupplierViewModel> suppliersList = _supplierRepository.GetAll().Select(o => new SupplierViewModel()
            {
                IdSupplier = o.IdSupplier,
                Address = o.Address,
                Name = o.Name,
                Email = o.Email,
                Phone = o.Phone,
            }).ToList();

            return View(suppliersList);
        }

        [HttpPost]
        public ActionResult SearchSupplier(string SupplierName)
        {
            IEnumerable<Supplier> supplierModel = _supplierRepository.GetByName(SupplierName);

            List<SupplierViewModel> suppliersList = new List<SupplierViewModel>();
            foreach (var c in supplierModel)
            {
                suppliersList.Add(new SupplierViewModel()
                {
                    IdSupplier = c.IdSupplier,
                    Address = c.Address,
                    Name = c.Name,
                    Email = c.Email,
                    Phone = c.Phone,
                });
            }

            return View(suppliersList);
        }

        [HttpGet]
        public ActionResult _AddSupplier()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult _AddSupplier(SupplierViewModel model)
        {
            if (ModelState.IsValid)
            {
                Supplier supplier = new Supplier()
                {
                    Name = model.Name,
                    Phone = model.Phone,
                    Email = model.Email,
                    Address = model.Address,
                };

                _supplierRepository.Add(supplier);
                _supplierRepository.Save();
                return RedirectToAction("Index", "Supplier");
            }
            return PartialView();
        }

        [HttpGet]
        public ActionResult _EditSupplier(int IdSupplier)
        {
            Supplier supplierModel = _supplierRepository.GetByID(IdSupplier);
            SupplierViewModel supplierViewModel = new SupplierViewModel()
            {
                IdSupplier = supplierModel.IdSupplier,
                Address = supplierModel.Address,
                Name = supplierModel.Name,
                Email = supplierModel.Email,
                Phone = supplierModel.Phone,
            };
            return PartialView(supplierViewModel);
        }

        [HttpPost]
        public ActionResult _EditSupplier(SupplierViewModel costumersViewModel)
        {
            if (ModelState.IsValid)
            {
                Supplier supplier = new Supplier()
                {
                    IdSupplier = costumersViewModel.IdSupplier,
                    Name = costumersViewModel.Name,
                    Phone = costumersViewModel.Phone,
                    Email = costumersViewModel.Email,
                    Address = costumersViewModel.Address,
                };

                _supplierRepository.Update(supplier);
                _supplierRepository.Save();
                return RedirectToAction("Index", "Supplier");
            }
            else
                return PartialView(costumersViewModel);
        }

        [HttpGet]
        public ActionResult _DeleteSupplier(int IdSupplier)
        {
            Supplier model = _supplierRepository.GetByID(IdSupplier);
            SupplierViewModel supplierViewModel = new SupplierViewModel()
            {
                IdSupplier = model.IdSupplier,
                Name = model.Name,
                Phone = model.Phone,
                Email = model.Email,
                Address = model.Address,
            };

            return PartialView(supplierViewModel);
        }

        [HttpPost]
        public ActionResult Delete(int IdSupplier)
        {
            Supplier model = _supplierRepository.GetByID(IdSupplier);
            SupplierViewModel supplierViewModel = new SupplierViewModel()
            {
                IdSupplier = model.IdSupplier,
                Name = model.Name,
                Phone = model.Phone,
                Email = model.Email,
                Address = model.Address,
            };

            Order OrderModel = new Order();
            IEnumerable<Order> ordersViewModelList = _orderRepository.GetOrdersBySupplierId(supplierViewModel.IdSupplier);

            foreach (var x in ordersViewModelList)
            {
                OrderModel = _orderRepository.GetById(x.IdOrder);
                _orderRepository.Delete(x.IdOrder);
                _orderRepository.Save();
                foreach (var o in OrderModel.OrderDetail)
                {
                    _orderProductRepository.Delete(o.IdOrder, o.IdProduct);
                }
                _orderProductRepository.Save();
            }
            _supplierRepository.Delete(IdSupplier);
            _supplierRepository.Save();
            return RedirectToAction("Index", "Supplier");
        }

        public void ExportExcel()
        {
            List<SupplierViewModel> supplierViewModelList = _supplierRepository.GetAll().Select(o => new SupplierViewModel()
            {
                Name = o.Name,
                Phone = o.Phone,
                Email = o.Email,
                Address = o.Address
            }).ToList();

            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");

            Sheet.Cells[string.Format("A1")].Value = "Name";
            Sheet.Cells[string.Format("B1")].Value = "Phone";
            Sheet.Cells[string.Format("C1")].Value = "Email";
            Sheet.Cells[string.Format("D1")].Value = "Address";

            int row = 2;

            foreach (var item in supplierViewModelList)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.Name;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.Phone;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.Email;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.Address;

                row++;
            }

            Sheet.Cells["A:AZ"].AutoFitColumns();
            Sheet.Cells["A:AZ"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[1, 1].EntireRow.Style.Font.Bold = true;

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=SuppliersReport.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();
        }
    }
}