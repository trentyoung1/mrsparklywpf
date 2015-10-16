using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MrSparklyMVC.Models
{
    [MetadataType(typeof(SupplierMetadata))]
    public partial class Supplier
    {
    }

    public class SupplierMetadata
    {
        [Display(Name = "Supplier")]
        [Required]
        [StringLength(50, MinimumLength=1)]
        public string supplierName { get; set; }

        [Display(Name = "Contact")]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string contactName { get; set; }

        [Display(Name = "Street")]
        [StringLength(150, MinimumLength = 1)]
        public string supplierStreet { get; set; }

        [Display(Name = "Phone")]
        [StringLength(20, MinimumLength = 1)]
        public string supplierPhone { get; set; }

        [Display(Name = "Mobile")]
        [StringLength(20, MinimumLength = 1)]
        public string supplierMobile { get; set; }

        [Display(Name = "Fax")]
        [StringLength(20, MinimumLength = 1)]
        public string supplierFax { get; set; }

        [Display(Name = "E-Mail")]
        [Required]
        [StringLength(60, MinimumLength = 1)]
        public string supplierEmail { get; set; }
    }
}
