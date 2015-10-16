using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MrSparklyMVC.Models
{
    [MetadataType(typeof(RawMaterialMetadata))]
    public partial class RawMaterial
    {
    }

    public class RawMaterialMetadata
    {
        [Display(Name="Material ID")]
        public int rawMaterialsID { get; set; }

        [Display(Name="Material Name")]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string rawMaterialsName { get; set; }

        [Display(Name="Price")]
        [Required]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue)]
        public Nullable<decimal> rawMaterialsPrice { get; set; }

        [Display(Name="Quantity")]
        [Required]
        [Range(0,short.MaxValue)]
        public Nullable<short> rawMaterialsQty { get; set; }
    }
}
