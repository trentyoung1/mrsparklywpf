using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MrSparklyWPF.Models
{
    public partial class Employees : INotifyPropertyChanged
    {
        public string employeeFirstName { get; 
            set
            {
                if (employeeFirstName != value) {
                    employeeFirstName = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("employeeFirstName"));
                }
            }
       }
        public string employeeLastName { get; set; }
        public string employeeStreet { get; set; }
        public string employeePhone { get; set; }
        public string employeeMob { get; set; }
        public string employeeFax { get; set; }
        public string employeeEmail { get; set; }
        public string employeeDepartment { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
