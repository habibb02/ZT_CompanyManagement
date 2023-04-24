using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CompanyManagement.Models
{
    public class OrdersViewModel
    {
        public int IdOrder { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int IdSupplier { get; set; }
        public bool State { get; set; }
        public string IdSupplierTest { get; set; }

        public SupplierViewModel Supplier { get; set; }
        public List<OrderProductViewModel> OrderProduct { get; set; }
        public List<SelectListItem> SupplierList { get; set; }
        public List<SelectListItem> ProductsList { get; set; }
    }
}