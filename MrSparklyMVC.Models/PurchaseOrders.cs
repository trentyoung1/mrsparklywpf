using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MrSparklyMVC.Models
{
    [MetadataType(typeof(PurchaseOrderMetaData))]
    public partial class PurchaseOrder
    {
    }

    public class PurchaseOrderMetaData
    {
        [Display(Name="Order ID")]
        public int purchaseOrderID { get; set; } 
       
        public Nullable<int> supplierID { get; set; }

        [Display(Name="Order Number")]
        [Required]
        [StringLength(50, MinimumLength=1)]
        public string purchaseOrderNo { get; set; }

        [Display(Name="Order Date")]
        [Required]
        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> purchaseOrderDate { get; set; }

        [Display(Name="Delivery Address")]
        [Required]
        [StringLength(150, MinimumLength=1)]
        public string deliveryAddress { get; set; }
    }
}
