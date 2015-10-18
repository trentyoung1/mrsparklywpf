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

namespace MrSparklyWPF.PurchaseOrders
{
    class PurchaseOrdersListViewModel : ViewModelBase
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        private ObservableCollection<PurchaseOrder> _purchaseOrders;
        public ObservableCollection<PurchaseOrder> PurchaseOrders
        {
            get { return _purchaseOrders; }
            set { SetProperty(ref _purchaseOrders, value); }
        }

        private PurchaseOrder _newPurchaseOrder;
        public PurchaseOrder NewPurchaseOrder
        {
            get { return _newPurchaseOrder; }
            set { SetProperty(ref _newPurchaseOrder, value); }
        }

        private PurchaseOrder _selectedPurchaseOrder;
        public PurchaseOrder SelectedPurchaseOrder
        {
            get { return _selectedPurchaseOrder; }
            set { SetProperty(ref _selectedPurchaseOrder, value); }
        }

        private ObservableCollection<Supplier> _suppliers;
        public ObservableCollection<Supplier> Suppliers
        {
            get { return _suppliers; }
            set { SetProperty(ref _suppliers, value); }
        }

        private ObservableCollection<PurchaseOrderLine> _purchaseOrderLines;
        public ObservableCollection<PurchaseOrderLine> PurchaseOrderLines
        {
            get { return _purchaseOrderLines; }
            set { SetProperty(ref _purchaseOrderLines, value); }
        }

        private ObservableCollection<RawMaterial> _rawMaterials;
        public ObservableCollection<RawMaterial> RawMaterials
        {
            get { return _rawMaterials; }
            set { SetProperty(ref _rawMaterials, value); }
        }

        private ObservableCollection<PurchaseOrder> _searchResults;
        public ObservableCollection<PurchaseOrder> SearchResults
        {
            get { return _searchResults; }
            set { SetProperty(ref _searchResults, value); }
        }

        private MrSparklyEntities db = new MrSparklyEntities();

        public PurchaseOrdersListViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;

            try
            {
                var purchaseOrders = db.PurchaseOrders.Include(p => p.Supplier);
                PurchaseOrders = new ObservableCollection<PurchaseOrder>(purchaseOrders.ToList());
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
            try
            {
                var suppliers = db.Suppliers.ToList();
                Suppliers = new ObservableCollection<Supplier>(suppliers);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
            try
            {
                var rawMaterials = db.RawMaterials.ToList();
                RawMaterials = new ObservableCollection<RawMaterial>(rawMaterials);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }

            _newPurchaseOrder = new PurchaseOrder();
            _newPurchaseOrder.PurchaseOrderLines = new ObservableCollection<PurchaseOrderLine>();
            _purchaseOrderLines = new ObservableCollection<PurchaseOrderLine>();
            _searchResults = new ObservableCollection<PurchaseOrder>();

            DeletePurchaseOrderCommand = new RelayCommand<PurchaseOrder>(onDeletePurchaseOrder);
            UpdatePurchaseOrderCommand = new RelayCommand<PurchaseOrder>(onUpdatePurchaseOrder);
            AddPurchaseOrderCommand = new RelayCommand<PurchaseOrder>(onAddPurchaseOrder);
            AddPurchaseOrderLineCommand = new RelayCommand<PurchaseOrderLine>(onAddPurchaseOrderLine);
            SelectPurchaseOrderCommand = new RelayCommand<PurchaseOrder>(onSelectPurchaseOrder);
            FindPurchaseOrdersCommand = new RelayCommand<string>(onFindPurchaseOrders);

            DeletePurchaseOrderLineCommand = new RelayCommand<PurchaseOrderLine>(onDeletePurchaseOrderLine);
        }

        public RelayCommand<PurchaseOrder> DeletePurchaseOrderCommand { get; private set; }
        public RelayCommand<PurchaseOrder> UpdatePurchaseOrderCommand { get; private set; }
        public RelayCommand<PurchaseOrder> AddPurchaseOrderCommand { get; private set; }
        public RelayCommand<PurchaseOrderLine> AddPurchaseOrderLineCommand { get; private set; }
        public RelayCommand<PurchaseOrder> SelectPurchaseOrderCommand { get; private set; }
        public RelayCommand<string> FindPurchaseOrdersCommand { get; private set; }

        public RelayCommand<PurchaseOrderLine> DeletePurchaseOrderLineCommand { get; private set; }

        public void onDeletePurchaseOrder(PurchaseOrder ord)
        {

            if (ord != null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you wish to delete this purchaseOrder?", "Confirm Delete", System.Windows.MessageBoxButton.YesNo);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    int purchaseOrderid = ord.purchaseOrderID;
                    PurchaseOrder ordToRemove = (PurchaseOrder)PurchaseOrders.Single(p => p.purchaseOrderID == purchaseOrderid);

                    db.PurchaseOrders.Remove(ordToRemove);
                    db.SaveChanges();
                    PurchaseOrders = UpdatePurchaseOrdersCollection(PurchaseOrders);
                }
            }
        }
        public void onUpdatePurchaseOrder(PurchaseOrder ord)
        {
            var purchaseOrder = ord;

            if (purchaseOrder != null)
            {
                db.Entry(purchaseOrder).State = EntityState.Modified;
                db.SaveChanges();
                PurchaseOrders = UpdatePurchaseOrdersCollection(PurchaseOrders);

                MessageBoxResult msgBox = MessageBox.Show("Changes Saved", "Success");

            }
        }

        public void onAddPurchaseOrder(PurchaseOrder ord)
        {
            var newPurchaseOrder = ord;
            var ordSup = from s in db.Suppliers where s.supplierID == ord.supplierID select s;
            newPurchaseOrder.Supplier = (Supplier)ordSup.Single();

            if (newPurchaseOrder != null)
            {
                db.PurchaseOrders.Add(newPurchaseOrder);
                db.SaveChanges();
                PurchaseOrders = UpdatePurchaseOrdersCollection(PurchaseOrders);
                MessageBoxResult msgBox = MessageBox.Show("PurchaseOrder Added", "Success");
            }
        }

        public void onAddPurchaseOrderLine(PurchaseOrderLine line)
        {
            var newPurchaseOrderLine = line;

            if (newPurchaseOrderLine != null)
            NewPurchaseOrder.PurchaseOrderLines.Add(newPurchaseOrderLine);
        }

        public void onSelectPurchaseOrder(PurchaseOrder ord)
        {
            SelectedPurchaseOrder = ord;
            PurchaseOrderLines = new ObservableCollection<PurchaseOrderLine>(SelectedPurchaseOrder.PurchaseOrderLines.ToList());
        }

        public void onFindPurchaseOrders(string searchString)
        {
            var queryResult = from o in db.PurchaseOrders
                              where o.purchaseOrderNo.Contains(searchString)
                              select o;
            SearchResults = new ObservableCollection<PurchaseOrder>(queryResult.ToList());

        }

        public ObservableCollection<PurchaseOrder> UpdatePurchaseOrdersCollection(ObservableCollection<PurchaseOrder> ords)
        {
            var purchaseOrders = db.PurchaseOrders.Include(s => s.Supplier).ToList();
            ords = new ObservableCollection<PurchaseOrder>(purchaseOrders);
            return ords;
        }

        public void onDeletePurchaseOrderLine(PurchaseOrderLine line)
        {

            if (line != null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you wish to delete this item?", "Confirm Delete", System.Windows.MessageBoxButton.YesNo);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    int purchaseOrderLineid = line.purchaseOrderLinesID;
                    PurchaseOrderLine lineToRemove = (PurchaseOrderLine)PurchaseOrderLines.Single(p => p.purchaseOrderLinesID == purchaseOrderLineid);

                    db.PurchaseOrderLines.Remove(lineToRemove);
                    db.SaveChanges();
                    PurchaseOrders = UpdatePurchaseOrdersCollection(PurchaseOrders);
                }
            }
        }

    }
}
