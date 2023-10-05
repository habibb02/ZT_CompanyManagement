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

        
        public SupplierViewModel Supplier { get; set; }
        
        public List<OrderDetailViewModel> OrderDetail { get; set; }
        
        public List<SelectListItem> SupplierList { get; set; }
        
        public List<SelectListItem> ProductsList { get; set; }

        #region Custom Fields

        private string year = Convert.ToString(DateTime.Now.Year);
        
        public string OrderCode => $"{IdOrder}-{year.Substring(year.Length - 2)}";

        #endregion
    }
}