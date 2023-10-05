using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CompanyManagement.DAL;

namespace CompanyManagement.Models
{
    public class OrderDetailViewModel
    {
        
        public int IdOrder { get; set; }
        
        public int IdProduct { get; set; }
        
        public Nullable<int> Qty { get; set; }
        
        public  OrdersViewModel Order { get; set; }
        
        public  ProductsViewModel Product { get; set; }
    }
}