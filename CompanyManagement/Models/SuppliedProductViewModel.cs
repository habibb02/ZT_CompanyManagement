using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CompanyManagement.DAL;

namespace CompanyManagement.Models
{
    public class SuppliedProductViewModel
    {
        public int IdSupplier { get; set; }
        public int IdProduct { get; set; }
        public decimal? Price { get; set; }
        public decimal? Date { get; set; }

        public virtual Product Product { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}