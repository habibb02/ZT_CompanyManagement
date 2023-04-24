using System.Collections.Generic;

namespace CompanyManagement.Models
{
    public class SupplierViewModel
    {
        public int IdSupplier { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public List<OrdersViewModel> Orders { get; set; }
    }
}