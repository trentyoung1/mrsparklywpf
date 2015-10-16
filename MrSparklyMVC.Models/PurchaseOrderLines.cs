using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MrSparklyMVC.Models
{
    [MetadataType(typeof(PurchaseOrderLinesMetadata))]
    public partial class PurchaseOrderLine
    {
    }

    public class PurchaseOrderLinesMetadata
    {
        [HiddenInput(DisplayValue = false)]
        public int purchaseOrderLinesID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Nullable<int> purchaseOrderID { get; set; }

        public Nullable<int> rawMaterialsID { get; set; }

        [Display(Name="Quantity")]
        [Required]
        [Range(0, short.MaxValue)]
        public Nullable<short> purchaseOrderItemQty { get; set; }

        [Display(Name="Price")]
        [Required]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue)]
        public Nullable<decimal> purchaseOrderItemPrice { get; set; }

        [Display(Name="Subtotal")]
        [Required]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue)]
        public Nullable<decimal> purchaseOrderLineSubtotal { get; set; }
    }
}
