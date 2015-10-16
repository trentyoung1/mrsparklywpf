using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MrSparklyMVC.Models
{
    [MetadataType(typeof(SalesOrderMetaData))]
    public partial class SalesOrder
    {
    }

    public class SalesOrderMetaData
    {
        [Display(Name="Order ID")]
        public int salesOrderID { get; set; }

        public Nullable<int> retailerID { get; set; }

        [Display(Name="Order Number")]
        [Required]
        [StringLength(50, MinimumLength=1)]
        public string salesOrderNo { get; set; }

        [Display(Name="Order Date")]
        [Required]
        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> salesOrderDate { get; set; }

        public virtual Retailer Retailer { get; set; }
    }
}
