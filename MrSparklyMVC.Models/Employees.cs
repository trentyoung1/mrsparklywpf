using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MrSparklyMVC.Models
{
    [MetadataType(typeof(EmployeeMetaData))]
    public partial class Employee
    {
    }

    public class EmployeeMetaData
    {

        public int employeeID { get; set; }

        [Display(Name="Employee Name")]
        [Required]
        [StringLength(50, MinimumLength= 1)]
        public string employeeFirstName { get; set; }

        [Display(Name="Last Name")]
        [Required]
        [StringLength(50, MinimumLength=1)]
        public string employeeLastName { get; set; }

        [Display(Name="Street")]
        [Required]
        [StringLength(150, MinimumLength=1)]
        public string employeeStreet { get; set; }

        [Display(Name="Phone")]
        [StringLength(20, MinimumLength=0)]
        public string employeePhone { get; set; }

        [Display(Name="Mobile")]
        [StringLength(60, MinimumLength=0)]
        public string employeeMob { get; set; }

        [Display(Name="Fax")]
        [StringLength(20, MinimumLength=0)]
        public string employeeFax { get; set; }

        [Display(Name="E-Mail")]
        [Required]
        [StringLength(60, MinimumLength=1)]
        public string employeeEmail { get; set; }

        [Display(Name="Department")]
        [Required]
        [StringLength(60, MinimumLength=1)]
        public string employeeDepartment { get; set; }
    }
}
