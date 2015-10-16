using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MrSparklyMVC.Models
{
    [MetadataType(typeof(ProductsMetaData))]
    public partial class Product
    {
    }

    public class ProductsMetaData
    {
        [Display(Name= "Brand Name")]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string productBrandName { get; set; }

        [Display(Name="Cost Price")]
        [Required]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue)]
        public Nullable<decimal> productCostPrice { get; set; }

        [Display(Name="Retail Price")]
        [Required]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue)]
        public Nullable<decimal> productRetailPrice { get; set; }

        [Display(Name = "Quantity")]
        [Required]
        [Range(0, short.MaxValue)]
        public Nullable<short> productQty { get; set; }
    }
}
