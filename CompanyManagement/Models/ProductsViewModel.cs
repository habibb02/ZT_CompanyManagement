using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CompanyManagement.Models
{
    public class ProductsViewModel
    {
        
        public int IdProduct { get; set; }
        
        public string Description { get; set; }
        
        public decimal? Price { get; set; }
        
        public string Name { get; set; }
        
        public string Type { get; set; }
        
        public HttpPostedFileBase Attachment { get; set; }
        
        public byte[] AttachmentByteArr { get; set; }
        
        public string AttachmentFileName { get; set; }
        
        
        public virtual List<OrderDetailViewModel> OrderProduct { get; set; }
    }
}