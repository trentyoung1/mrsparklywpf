using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MrSparklyMVC.Models
{
    [MetadataType(typeof(SuburbMetadata))]
    public partial class Suburb
    {
    }

    public class SuburbMetadata 
    {
        [Display(Name="Suburb")]
        [Required]
        [StringLength(50, MinimumLength=1)]
        public string suburb1 { get; set; }

        [Display(Name="State")]
        [Required]
        [StringLength(5, MinimumLength=1)]
        public string state { get; set; }

        [Display(Name="Post Code")]
        [Required]
        [StringLength(10, MinimumLength=1)]
        public string zip { get; set; }
    }
}
