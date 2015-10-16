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

namespace MrSparklyWPF.Suppliers
{
    class SuppliersListViewModel : ViewModelBase
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        private ObservableCollection<Supplier> _suppliers;
        public ObservableCollection<Supplier> Suppliers
        {
            get { return _suppliers; }
            set { SetProperty(ref _suppliers, value); }
        }

        private Supplier _newSupplier;
        public Supplier NewSupplier
        {
            get { return _newSupplier; }
            set { SetProperty(ref _newSupplier, value); }
        }

        private ObservableCollection<Supplier> _searchResults;
        public ObservableCollection<Supplier> SearchResults
        {
            get { return _searchResults; }
            set { SetProperty(ref _searchResults, value); }
        }

        private MrSparklyEntities db = new MrSparklyEntities();

        public SuppliersListViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;

            try
            {
                var suppliers = db.Suppliers.Include(s => s.Suburb);
                Suppliers = new ObservableCollection<Supplier>(suppliers.ToList());
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }

            _newSupplier = new Supplier();
            _searchResults = new ObservableCollection<Supplier>();

            DeleteSupplierCommand = new RelayCommand<Supplier>(onDeleteSupplier);
            UpdateSupplierCommand = new RelayCommand<Supplier>(onUpdateSupplier);
            AddSupplierCommand = new RelayCommand<Supplier>(onAddSupplier);
            FindSuppliersCommand = new RelayCommand<string>(onFindSuppliers);
        }

        public RelayCommand<Supplier> DeleteSupplierCommand { get; private set; }
        public RelayCommand<Supplier> UpdateSupplierCommand { get; private set; }
        public RelayCommand<Supplier> AddSupplierCommand { get; private set; }
        public RelayCommand<string> FindSuppliersCommand { get; private set; }

        public void onDeleteSupplier(Supplier sup)
        {

            if (sup != null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you wish to delete this supplier?", "Confirm Delete", System.Windows.MessageBoxButton.YesNo);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    int supplierid = sup.supplierID;
                    Supplier supToRemove = (Supplier)Suppliers.Single(s => s.supplierID == supplierid);

                    db.Suppliers.Remove(supToRemove);
                    db.SaveChanges();
                    Suppliers = UpdateSuppliersCollection(Suppliers);
                }
            }
        }
        public void onUpdateSupplier(Supplier sup)
        {
            var supplier = sup;

            if (supplier != null)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                Suppliers = UpdateSuppliersCollection(Suppliers);

                MessageBoxResult msgBox = MessageBox.Show("Changes Saved", "Success");

            }
        }

        public void onAddSupplier(Supplier sup)
        {
            var newSupplier = sup;

            if (newSupplier != null)
            {
                db.Suppliers.Add(newSupplier);
                db.SaveChanges();
                Suppliers = UpdateSuppliersCollection(Suppliers);
                MessageBoxResult msgBox = MessageBox.Show("Supplier Added", "Success");
            }
        }

        public void onFindSuppliers(string searchString)
        {
            var queryResult = from s in db.Suppliers
                              where s.supplierName.Contains(searchString)
                              select s;
            SearchResults = new ObservableCollection<Supplier>(queryResult.ToList());

        }

        public ObservableCollection<Supplier> UpdateSuppliersCollection(ObservableCollection<Supplier> sups)
        {
            var suppliers = db.Suppliers.Include(s => s.Suburb);
            sups = new ObservableCollection<Supplier>(suppliers);
            return sups;
        }

    }
}
