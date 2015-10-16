using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MrSparklyWPF.Employees;
using MrSparklyWPF.Products;
using MrSparklyWPF.PurchaseOrders;
using MrSparklyWPF.ViewModels;
using MrSparklyWPF.RawMaterials;
using MrSparklyWPF.Retailers;
using MrSparklyWPF.Suppliers;

namespace MrSparklyWPF
{
    class MainWindowViewModel : ViewModelBase
    {
        //instantiate the viewmodels
        private EmployeesListViewModel _employeesListViewModel = new EmployeesListViewModel();
        private ProductsListViewModel _productsListViewModel = new ProductsListViewModel();
        private PurchaseOrdersListViewModel _purchaseOrdersListViewModel = new PurchaseOrdersListViewModel();
        private RawMaterialsListViewModel _rawMaterialsListViewModel = new RawMaterialsListViewModel();
        private RetailersListViewModel _retailersListViewModel = new RetailersListViewModel();
        private SuppliersListViewModel _suppliersListViewModel = new SuppliersListViewModel();

        private ViewModelBase _CurrentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set { SetProperty(ref _CurrentViewModel, value); }
        }

        public RelayCommand<string> NavCommand { get; private set; }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "employees":
                    CurrentViewModel = _employeesListViewModel;
                    break;
                case "products":
                    CurrentViewModel = _productsListViewModel;
                    break;
                case "purchaseorders":
                    CurrentViewModel = _purchaseOrdersListViewModel;
                    break;
                case "rawmaterials":
                    CurrentViewModel = _rawMaterialsListViewModel;
                    break;
                case "retailers":
                    CurrentViewModel = _retailersListViewModel;
                    break;
                case "suppliers":
                    CurrentViewModel = _suppliersListViewModel;
                    break;

            }
        }

        public MainWindowViewModel()
        {
            NavCommand = new RelayCommand<string>(OnNav);
        }
    }
}
