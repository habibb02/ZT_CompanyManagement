using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CompanyManagement.Models
{
    public class ProductPriceViewModel
    {
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Il campo Manpower Price è obbligatorio.")]
        public decimal Manpower { get; set; }
        public decimal Delivery { get; set; }

        [Required(ErrorMessage = "Il campo Materials Price è obbligatorio.")]
        public decimal Materials { get; set; }

        [Required]
        public int Qty { get; set; }

        [Required(ErrorMessage = "Il campo Fixed Costs è obbligatorio.")]
        public decimal FixedCosts { get; set; }
        public int PercentageIncrease { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

    }
}