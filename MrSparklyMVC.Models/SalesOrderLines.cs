using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace MrSparklyMVC.Models
{
    [MetadataType(typeof(SalesOrderLinesMetadata))]
    public partial class SalesOrderLine
    {
    }

    public class SalesOrderLinesMetadata
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]
        public int salesOrderLinesID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Nullable<int> salesOrderID { get; set; }

        public Nullable<int> productID { get; set; }

        [Display(Name="Quantity")]
        [Required]
        [Range(0, short.MaxValue)]
        public Nullable<short> salesOrderItemQty { get; set; }

        [Display(Name="Price")]
        [Required]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue)]
        public Nullable<decimal> salesOrderItemPrice { get; set; }

        [Display(Name="Subtotal")]
        [Required]
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public Nullable<decimal> salesOrderLinesSubtotal { get; set; }
    }
}
