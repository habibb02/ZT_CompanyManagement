using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CompanyManagement.DAL;
using CompanyManagement.Models;
using CompanyManagement.Repository;
using CompanyManagement.Repository.General;

namespace CompanyManagement.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        private IEOrderRepository _orderRepository;
        private IESupplierRepository _supplierRepository;
        private IEProductRepository _productRepository;
        private IEOrderDetailRepository _orderDetailRepository;

        public OrderController()
        {
            UnitOfWork uow = new UnitOfWork();
            _orderDetailRepository = new OrderDetailRepository(uow);
            _orderRepository = new OrderRepository(uow);
            _supplierRepository = new SupplierRepository(uow);
            _productRepository = new ProductRepository(uow);
        }
        public OrderController(IEOrderRepository orderRepository, IESupplierRepository supplierRepository, IEProductRepository productRepository, IEOrderDetailRepository OrderDetailRepository)
        {
            _orderDetailRepository = OrderDetailRepository;
            _orderRepository = orderRepository;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
        }

        private List<SelectListItem> GetSuppliersList()
        {
            return _supplierRepository.GetAll().Select(s => new SelectListItem() { Text = s.Name, Value = Convert.ToString(s.IdSupplier) }).ToList();
        }
        
        private List<SelectListItem> GetProductsList()
        {
            return _productRepository.GetAll().Select(s => new SelectListItem() { Text = s.Name, Value = Convert.ToString(s.IdProduct) }).ToList();
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<OrdersViewModel> orderViewModelList = _orderRepository.GetAll().Select(o => new OrdersViewModel()
            {
                IdOrder = o.IdOrder,
                Date = o.Date,
                Description = o.Description,
                Price = o.Price,
                IdSupplier = o.IdSupplier,
                OrderDetail = o.OrderDetail.Select(p => new OrderDetailViewModel()
                {
                    Qty = p.Qty,
                    IdProduct = p.IdProduct,
                    Product = new ProductsViewModel()
                    {
                        IdProduct = p.Product.IdProduct,
                        Description = p.Product.Description,
                        Name = p.Product.Name,
                        Price = p.Product.Price,
                    },
                }).ToList(),
                Supplier = new SupplierViewModel()
                {
                    Name = o.Supplier.Name,
                }
            }).ToList();

            return View(orderViewModelList);
        }

        [HttpGet]
        public ActionResult SearchOrder()
        {
            List<OrdersViewModel> orderViewModelList = _orderRepository.GetAll().Select(o => new OrdersViewModel()
            {
                SupplierList = GetSuppliersList(),
                ProductsList = GetProductsList(),
                IdOrder = o.IdOrder,
                Date = o.Date,
                Description = o.Description,
                Price = o.Price,
                IdSupplier = o.IdSupplier,
                State = o.State ?? false,
                OrderDetail = o.OrderDetail.Select(p => new OrderDetailViewModel()
                {
                    Qty = p.Qty,
                    Product = new ProductsViewModel()
                    {
                        IdProduct = p.Product.IdProduct,
                        Description = p.Product.Description,
                        Name = p.Product.Name,
                        Price = p.Product.Price,
                    },
                }).ToList(),
                Supplier = new SupplierViewModel()
                {
                    Name = o.Supplier.Name,
                }
            }).ToList();

            return View(orderViewModelList);
        }

        [HttpPost]
        public ActionResult SearchOrder(string IdOrder, string SupplierName)
        {
            IEnumerable<Order> orderModel = _orderRepository.GetOrdersByIdName(IdOrder, SupplierName);
            List<OrdersViewModel> ordersViewModelList = new List<OrdersViewModel>();

            foreach (var o in orderModel)
            {
                ordersViewModelList.Add(new OrdersViewModel()
                {
                    SupplierList = GetSuppliersList(),
                    ProductsList = GetProductsList(),
                    IdOrder = o.IdOrder,
                    IdSupplier = o.IdSupplier,
                    Description = o.Description,
                    Price = o.Price,
                    Date = o.Date,
                    State = o.State ?? false,
                    Supplier = new SupplierViewModel()
                    {
                        IdSupplier = o.Supplier.IdSupplier,
                        Name = o.Supplier.Name,
                    },
                    OrderDetail = o.OrderDetail.Select(op => new OrderDetailViewModel()
                    {
                        IdOrder = op.IdOrder,
                        IdProduct = op.IdProduct,
                        Qty = op.Qty,
                        Product = new ProductsViewModel()
                        {
                            IdProduct = op.IdProduct,
                            Name = op.Product.Name,
                            Price = op.Product.Price,
                        }
                    }).ToList()
                });
            }

            return View(ordersViewModelList);
        }

        [HttpGet]
        public ActionResult _AddOrder()
        {
            OrdersViewModel ordersViewModel = new OrdersViewModel()
            {
                SupplierList = GetSuppliersList(),
                ProductsList = GetProductsList(),
                OrderDetail = new List<OrderDetailViewModel>() { new OrderDetailViewModel() },
                Supplier = new SupplierViewModel(),

            };
            return PartialView(ordersViewModel);
        }

        [HttpPost]
        [HandleError]
        public ActionResult _AddOrder(OrdersViewModel ordersViewModel)  //fare somma della quantità quando i prodotti inseriti in un ordine sono uguali.
        {
            if (ModelState.IsValid)
            {
                ordersViewModel.Price = !ordersViewModel.Price.HasValue ? 0 : ordersViewModel.Price;

                foreach (var od in ordersViewModel.OrderDetail)
                {
                    Product productModel = _productRepository.GetById(od.IdProduct);
                    ordersViewModel.Price += (productModel.Price * od.Qty);
                }

                Order order = new Order()
                {
                    IdOrder = ordersViewModel.IdOrder,
                    Date = Convert.ToDateTime(ordersViewModel.Date),
                    Description = ordersViewModel.Description,
                    Price = ordersViewModel.Price,
                    IdSupplier = ordersViewModel.IdSupplier,
                    State = false,
                };
                _orderRepository.Add(order);

                if (_orderRepository.Save() > 0 && ordersViewModel.OrderDetail.Count > 0)
                {
                    for (int a = 0; a < ordersViewModel.OrderDetail.Count; a++)
                        for (int b = a + 1; b < ordersViewModel.OrderDetail.Count; b++)
                        {
                            if (ordersViewModel.OrderDetail[a].Qty != 0 && ordersViewModel.OrderDetail[a].IdProduct == ordersViewModel.OrderDetail[b].IdProduct)
                            {
                                ordersViewModel.OrderDetail[a].Qty = ordersViewModel.OrderDetail[a].Qty + ordersViewModel.OrderDetail[b].Qty;
                                ordersViewModel.OrderDetail[b].Qty = 0;
                            }
                        }

                    foreach (var item in ordersViewModel.OrderDetail)
                    {
                        if (item.Qty > 0)
                        {
                            OrderDetail OrderDetail = new OrderDetail()
                            {
                                IdOrder = order.IdOrder,
                                IdProduct = item.IdProduct,
                                Qty = item.Qty,
                            };
                            _orderDetailRepository.Add(OrderDetail);
                        }
                    }
                    _orderDetailRepository.Save();
                }
                return RedirectToAction("Index", "Order");
            }
            else
                return PartialView();
        }

        [HttpGet]
        public ActionResult _DeleteOrder(int IdOrder)
        {
            Order orderModel = _orderRepository.GetById(IdOrder);
            OrdersViewModel ordersViewModel = new OrdersViewModel()
            {
                SupplierList = GetSuppliersList(),
                ProductsList = GetProductsList(),
                IdOrder = orderModel.IdOrder,
                IdSupplier = orderModel.IdSupplier,
                Description = orderModel.Description,
                Price = orderModel.Price,
                Date = orderModel.Date,
                Supplier = new SupplierViewModel()
                {
                    IdSupplier = orderModel.Supplier.IdSupplier,
                    Name = orderModel.Supplier.Name,
                },
                OrderDetail = orderModel.OrderDetail.Select(op => new OrderDetailViewModel()
                {
                    IdOrder = op.IdOrder,
                    IdProduct = op.IdProduct,
                    Qty = op.Qty,
                    Product = new ProductsViewModel()
                    {
                        IdProduct = op.IdProduct,
                        Name = op.Product.Name,
                        Price = op.Product.Price
                    }
                }).ToList()
            };
            return PartialView(ordersViewModel);
        }

        [HttpPost]
        public ActionResult Delete(int IdOrder)
        {
            Order orderModel = _orderRepository.GetById(IdOrder);
            OrdersViewModel orderViewModel = new OrdersViewModel()
            {
                OrderDetail = orderModel.OrderDetail.Select(op => new OrderDetailViewModel()
                {
                    IdProduct = op.IdProduct,
                }).ToList()
            };
            _orderRepository.Delete(IdOrder);
            _orderRepository.Save();

            foreach (var x in orderModel.OrderDetail)
            {
                _orderDetailRepository.Delete(IdOrder, x.IdProduct);
                _orderDetailRepository.Save();
            }
            return RedirectToAction("Index", "Order");
        }

        [HttpGet]
        public ActionResult _EditOrder(int IdOrder)
        {
            Order orderModel = _orderRepository.GetById(IdOrder);
            OrdersViewModel ordersViewModel = new OrdersViewModel()
            {
                SupplierList = GetSuppliersList(),
                ProductsList = GetProductsList(),
                IdOrder = orderModel.IdOrder,
                IdSupplier = orderModel.IdSupplier,
                Description = orderModel.Description,
                Price = orderModel.Price,
                Date = orderModel.Date,
                State = orderModel.State.Value,
                OrderDetail = orderModel.OrderDetail.Select(p => new OrderDetailViewModel()
                {
                    Qty = p.Qty,
                    IdOrder = p.IdOrder,
                    IdProduct = p.IdProduct,
                    Product = new ProductsViewModel()
                    {
                        IdProduct = p.Product.IdProduct,
                        Name = p.Product.Name,
                    },
                }).ToList(),
                Supplier = new SupplierViewModel()
                {
                    IdSupplier = orderModel.Supplier.IdSupplier
                }
            };

            return PartialView(ordersViewModel);
        }

        [HttpPost]
        public ActionResult _EditOrder(OrdersViewModel ordersViewModel)
        {
            if (ModelState.IsValid)
            {
                ordersViewModel.Price = 0;

                foreach (var od in ordersViewModel.OrderDetail)
                {
                    Product productModel = _productRepository.GetById(od.IdProduct);
                    ordersViewModel.Price += (productModel.Price * od.Qty);
                }

                Order order = new Order()
                {
                    IdOrder = ordersViewModel.IdOrder,
                    IdSupplier = ordersViewModel.IdSupplier,
                    Date = ordersViewModel.Date,
                    Description = ordersViewModel.Description,
                    Price = ordersViewModel.Price,
                    State = ordersViewModel.State,
                };

                _orderRepository.Update(order);
                _orderRepository.Save();

                Order orderModel = _orderRepository.GetById(order.IdOrder);
                List<int> ProductKeys = new List<int>();

                foreach (var op in orderModel.OrderDetail)
                {
                    ProductKeys.Add(op.IdProduct);
                }

                foreach (int key in ProductKeys)
                {
                    _orderDetailRepository.Delete(order.IdOrder, key);
                }
                _orderDetailRepository.Save();

                for (int a = 0; a < ordersViewModel.OrderDetail.Count; a++)
                    for (int b = a + 1; b < ordersViewModel.OrderDetail.Count; b++)
                    {
                        if (ordersViewModel.OrderDetail[a].Qty != 0 && ordersViewModel.OrderDetail[a].IdProduct == ordersViewModel.OrderDetail[b].IdProduct)
                        {
                            ordersViewModel.OrderDetail[a].Qty = ordersViewModel.OrderDetail[a].Qty + ordersViewModel.OrderDetail[b].Qty;
                            ordersViewModel.OrderDetail[b].Qty = 0;
                        }
                    }

                foreach (var op in ordersViewModel.OrderDetail)
                {
                    if (op.Qty > 0)
                    {
                        OrderDetail OrderDetail = new OrderDetail()
                        {
                            IdOrder = op.IdOrder,
                            IdProduct = op.IdProduct,
                            Qty = op.Qty,
                        };

                        _orderDetailRepository.Add(OrderDetail);
                        _orderDetailRepository.Save();
                    }
                }
                return RedirectToAction("Index", "Order");
            }
            else
                return PartialView(ordersViewModel);
        }

        [HttpGet]
        public ActionResult _AddOrderDetail(int index)
        {
            OrdersViewModel ordersViewModel = new OrdersViewModel()
            {
                ProductsList = GetProductsList(),
                OrderDetail = new List<OrderDetailViewModel>(),
            };

            for (int i = 0; i <= index; i++)
                ordersViewModel.OrderDetail.Add(new OrderDetailViewModel());

            return PartialView(ordersViewModel);
        }
        public void ExportExcel(string IdOrder, string SupplierName)
        {

            List<OrdersViewModel> orderViewModelList = _orderRepository.GetAll().Select(o => new OrdersViewModel()
            {
                IdOrder = o.IdOrder,
                Date = o.Date,
                Description = o.Description,
                Price = o.Price,
                IdSupplier = o.IdSupplier,
                OrderDetail = o.OrderDetail.Select(p => new OrderDetailViewModel()
                {
                    Qty = p.Qty,
                    IdProduct = p.IdProduct,
                    Product = new ProductsViewModel()
                    {
                        IdProduct = p.Product.IdProduct,
                        Description = p.Product.Description,
                        Name = p.Product.Name,
                        Price = p.Product.Price,
                    },
                }).ToList(),
                Supplier = new SupplierViewModel()
                {
                    Name = o.Supplier.Name,
                }
            }).ToList();

            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");

            Sheet.Cells[string.Format("A1")].Value = "OrderId";
            Sheet.Cells[string.Format("B1")].Value = "Supplier Name";
            Sheet.Cells[string.Format("C1")].Value = "Qty";
            Sheet.Cells[string.Format("D1")].Value = "Product";
            Sheet.Cells[string.Format("E1")].Value = "Product Price";
            Sheet.Cells[string.Format("F1")].Value = "Description";
            Sheet.Cells[string.Format("G1")].Value = "Price";
            Sheet.Cells[string.Format("H1")].Value = "Date";
            Sheet.Cells[string.Format("I1")].Value = "State";

            int row = 2, row1 = 2;

            foreach (var item in orderViewModelList)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.IdOrder;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.Supplier.Name;
                foreach (var op in item.OrderDetail)
                {
                    Sheet.Cells[string.Format("C{0}", row1)].Value = op.Qty;
                    Sheet.Cells[string.Format("D{0}", row1)].Value = op.Product.Name;
                    Sheet.Cells[string.Format("E{0}", row1)].Value = op.Product.Price;

                    row1++;
                }
                Sheet.Cells[string.Format("F{0}", row)].Value = item.Description;
                Sheet.Cells[string.Format("G{0}", row)].Value = item.Price;
                Sheet.Cells[string.Format("H{0}", row)].Value = item.Date;
                Sheet.Cells[string.Format("I{0}", row)].Value = item.State;

                row1++;
                row = row1;
            }

            Sheet.Cells["A:AZ"].AutoFitColumns();
            Sheet.Cells["A:AZ"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[1, 1].EntireRow.Style.Font.Bold = true;

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=OrdersReport.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();
        }

    }
}