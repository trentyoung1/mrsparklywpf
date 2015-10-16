using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MrSparklyMVC.Models
{
    [MetadataType(typeof(RetailerMetadata))]
    public partial class Retailer
    {
    }

    public class RetailerMetadata
    {
        [Display(Name="Employee Name")]
        public Nullable<int> employeeID { get; set; }

        [Display(Name="Retailer")]
        [Required]
        [StringLength(50, MinimumLength=1)]
        public string retailerName { get; set; }

        [Display(Name="Contact")]
        [Required]
        [StringLength(50, MinimumLength=1)]
        public string retailerContactName { get; set; }

        [Display(Name="Street")]
        [StringLength(150, MinimumLength=1)]
        public string retailerStreet { get; set; }

        [Display(Name="Phone")]
        [StringLength(20, MinimumLength=1)]
        public string retailerPhone { get; set; }

        [Display(Name="Mobile")]
        [StringLength(20, MinimumLength=1)]
        public string retailerMob { get; set; }

        [Display(Name="Fax")]
        [StringLength(20, MinimumLength=1)]
        public string retailerFax { get; set; }

        [Display(Name="E-Mail")]
        [Required]
        [StringLength(60, MinimumLength=1)]
        public string retailerEmail { get; set; }
    }
}
