//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MrSparklyMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PurchaseOrder
    {
        public PurchaseOrder()
        {
            this.PurchaseOrderLines = new HashSet<PurchaseOrderLine>();
        }
    
        public int purchaseOrderID { get; set; }
        public Nullable<int> supplierID { get; set; }
        public string purchaseOrderNo { get; set; }
        public Nullable<System.DateTime> purchaseOrderDate { get; set; }
        public string deliveryAddress { get; set; }
    
        public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
