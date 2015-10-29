using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MrSparklyWPF.Models;
using MrSparklyWPF.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using NLog;
using System.Windows;

namespace MrSparklyWPF.Employees
{
    class EmployeesListViewModel : ViewModelBase
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { SetProperty(ref _employees, value); }
        }

        private Employee _newEmployee;
        public Employee NewEmployee
        {
            get { return _newEmployee; }
            set { SetProperty(ref _newEmployee, value); }
        }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { SetProperty(ref _selectedEmployee, value); }
        }

        private ObservableCollection<Suburb> _suburbs;
        public ObservableCollection<Suburb> Suburbs
        {
            get { return _suburbs; }
            set { SetProperty(ref _suburbs, value); }
        }

        private ObservableCollection<Employee> _searchResults;
        public ObservableCollection<Employee> SearchResults
        {
            get { return _searchResults; }
            set { SetProperty(ref _searchResults, value); }
        }

        private MrSparklyEntities db = new MrSparklyEntities();
        
        public EmployeesListViewModel() 
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;

            try
            {
                var employees = db.Employees.Include(e => e.Suburb);
                Employees = new ObservableCollection<Employee>(employees.ToList());                
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
            try
            {
                var suburbs = db.Suburbs.ToList();
                Suburbs = new ObservableCollection<Suburb>(suburbs);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }

            _newEmployee = new Employee();
            _searchResults = new ObservableCollection<Employee>();

            DeleteEmployeeCommand = new RelayCommand<Employee>(onDeleteEmployee);
            UpdateEmployeeCommand = new RelayCommand<Employee>(onUpdateEmployee);
            AddEmployeeCommand = new RelayCommand<Employee>(onAddEmployee);
            FindEmployeesCommand = new RelayCommand<string>(onFindEmployees);
            SelectEmployeeCommand = new RelayCommand<Employee>(onSelectEmployee);
        }

        public RelayCommand<Employee> DeleteEmployeeCommand { get; private set; }
        public RelayCommand<Employee> UpdateEmployeeCommand { get; private set; }
        public RelayCommand<Employee> AddEmployeeCommand { get; private set; }
        public RelayCommand<string> FindEmployeesCommand { get; private set; }
        public RelayCommand<Employee> SelectEmployeeCommand { get; private set; }

        public void onDeleteEmployee(Employee emp)
        {            
            
            if (emp != null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you wish to delete this employee?", "Confirm Delete", System.Windows.MessageBoxButton.YesNo);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    int employeeid = emp.employeeID;
                    Employee empToRemove = (Employee)Employees.Single(e => e.employeeID == employeeid);

                    db.Employees.Remove(empToRemove);
                    db.SaveChanges();
                    Employees = UpdateEmployeesCollection(Employees);
                }
            }
        }
        public void onUpdateEmployee(Employee emp)
        {
            var employee = emp;

            if (employee != null)
            {                
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                Employees = UpdateEmployeesCollection(Employees);

                MessageBoxResult msgBox = MessageBox.Show("Changes Saved", "Success");             
                
            }
        }

        public void onAddEmployee(Employee emp)
        {
            var newEmployee = emp;
            var empSub = from s in db.Suburbs where s.suburbID == emp.suburbID select s;
            newEmployee.Suburb = (Suburb)empSub.Single();

            if (newEmployee != null)
            {
                db.Employees.Add(newEmployee);
                db.SaveChanges();
                Employees = UpdateEmployeesCollection(Employees);
                MessageBoxResult msgBox = MessageBox.Show("Employee Added", "Success");
            }
        }

        public void onFindEmployees(string searchString)
        {
            var queryResult = from e in db.Employees
                      where e.employeeFirstName.Contains(searchString)
                      select e;
            SearchResults = new ObservableCollection<Employee>(queryResult.ToList());
            
        }

        public void onSelectEmployee(Employee emp)
        {
            SelectedEmployee = emp;
        }

        public ObservableCollection<Employee> UpdateEmployeesCollection(ObservableCollection<Employee> emps)
        {
            var employees =  db.Employees.Include(e => e.Suburb).ToList();
            emps = new ObservableCollection<Employee>(employees);
            return emps;
        }

    }
}
