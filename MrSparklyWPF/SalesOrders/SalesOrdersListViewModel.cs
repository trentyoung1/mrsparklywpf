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

namespace MrSparklyWPF.SalesOrders
{
    class SalesOrdersListViewModel : ViewModelBase
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        private ObservableCollection<SalesOrder> _salesOrders;
        public ObservableCollection<SalesOrder> SalesOrders
        {
            get { return _salesOrders; }
            set { SetProperty(ref _salesOrders, value); }
        }

        private SalesOrder _newSalesOrder;
        public SalesOrder NewSalesOrder
        {
            get { return _newSalesOrder; }
            set { SetProperty(ref _newSalesOrder, value); }
        }

        private SalesOrder _selectedSalesOrder;
        public SalesOrder SelectedSalesOrder
        {
            get { return _selectedSalesOrder; }
            set { SetProperty(ref _selectedSalesOrder, value); }
        }

        private ObservableCollection<Retailer> _retailers;
        public ObservableCollection<Retailer> Retailers
        {
            get { return _retailers; }
            set { SetProperty(ref _retailers, value); }
        }

        private ObservableCollection<SalesOrderLine> _salesOrderLines;
        public ObservableCollection<SalesOrderLine> SalesOrderLines
        {
            get { return _salesOrderLines; }
            set { SetProperty(ref _salesOrderLines, value); }
        }

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }

        private ObservableCollection<SalesOrder> _searchResults;
        public ObservableCollection<SalesOrder> SearchResults
        {
            get { return _searchResults; }
            set { SetProperty(ref _searchResults, value); }
        }

        private MrSparklyEntities db = new MrSparklyEntities();

        public SalesOrdersListViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;

            try
            {
                var salesOrders = db.SalesOrders.Include(p => p.Retailer);
                SalesOrders = new ObservableCollection<SalesOrder>(salesOrders.ToList());
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
            try
            {
                var retailers = db.Retailers.ToList();
                Retailers = new ObservableCollection<Retailer>(retailers);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
            try
            {
                var products = db.Products.ToList();
                Products = new ObservableCollection<Product>(products);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }

            _newSalesOrder = new SalesOrder();
            _newSalesOrder.SalesOrderLines = new ObservableCollection<SalesOrderLine>();
            _salesOrderLines = new ObservableCollection<SalesOrderLine>();
            _searchResults = new ObservableCollection<SalesOrder>();

            DeleteSalesOrderCommand = new RelayCommand<SalesOrder>(onDeleteSalesOrder);
            UpdateSalesOrderCommand = new RelayCommand<SalesOrder>(onUpdateSalesOrder);
            AddSalesOrderCommand = new RelayCommand<SalesOrder>(onAddSalesOrder);
            AddSalesOrderLineCommand = new RelayCommand<SalesOrderLine>(onAddSalesOrderLine);
            SelectSalesOrderCommand = new RelayCommand<SalesOrder>(onSelectSalesOrder);
            FindSalesOrdersCommand = new RelayCommand<string>(onFindSalesOrders);

            DeleteSalesOrderLineCommand = new RelayCommand<SalesOrderLine>(onDeleteSalesOrderLine);
            DeleteNewSalesOrderLineCommand = new RelayCommand<SalesOrderLine>(onDeleteNewSalesOrderLine);
        }

        public RelayCommand<SalesOrder> DeleteSalesOrderCommand { get; private set; }
        public RelayCommand<SalesOrder> UpdateSalesOrderCommand { get; private set; }
        public RelayCommand<SalesOrder> AddSalesOrderCommand { get; private set; }
        public RelayCommand<SalesOrderLine> AddSalesOrderLineCommand { get; private set; }
        public RelayCommand<SalesOrder> SelectSalesOrderCommand { get; private set; }
        public RelayCommand<string> FindSalesOrdersCommand { get; private set; }

        public RelayCommand<SalesOrderLine> DeleteSalesOrderLineCommand { get; private set; }
        public RelayCommand<SalesOrderLine> DeleteNewSalesOrderLineCommand { get; private set; }

        public void onDeleteSalesOrder(SalesOrder ord)
        {

            if (ord != null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you wish to delete this salesOrder?", "Confirm Delete", System.Windows.MessageBoxButton.YesNo);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    int salesOrderid = ord.salesOrderID;
                    SalesOrder ordToRemove = (SalesOrder)SalesOrders.Single(p => p.salesOrderID == salesOrderid);

                    db.SalesOrders.Remove(ordToRemove);
                    db.SaveChanges();
                    SalesOrders = UpdateSalesOrdersCollection(SalesOrders);
                }
            }
        }

        public void onUpdateSalesOrder(SalesOrder ord)
        {
            var salesOrder = ord;

            if (salesOrder != null)
            {
                db.Entry(salesOrder).State = EntityState.Modified;
                db.SaveChanges();
                SalesOrders = UpdateSalesOrdersCollection(SalesOrders);

                MessageBoxResult msgBox = MessageBox.Show("Changes Saved", "Success");

            }
        }

        public void onAddSalesOrder(SalesOrder ord)
        {
            var newSalesOrder = ord;
            var ordSup = from s in db.Retailers where s.retailerID == ord.retailerID select s;
            newSalesOrder.Retailer = (Retailer)ordSup.Single();

            if (newSalesOrder != null)
            {
                db.SalesOrders.Add(newSalesOrder);
                db.SaveChanges();
                SalesOrders = UpdateSalesOrdersCollection(SalesOrders);
                MessageBoxResult msgBox = MessageBox.Show("SalesOrder Added", "Success");
            }
        }

        public void onAddSalesOrderLine(SalesOrderLine line)
        {
            var newSalesOrderLine = line;

            if (newSalesOrderLine != null)
                NewSalesOrder.SalesOrderLines.Add(newSalesOrderLine);
        }

        public void onSelectSalesOrder(SalesOrder ord)
        {
            SelectedSalesOrder = ord;
            SalesOrderLines = new ObservableCollection<SalesOrderLine>(SelectedSalesOrder.SalesOrderLines.ToList());
        }

        public void onFindSalesOrders(string searchString)
        {
            var queryResult = from o in db.SalesOrders
                              where o.salesOrderNo.Contains(searchString)
                              select o;
            SearchResults = new ObservableCollection<SalesOrder>(queryResult.ToList());

        }

        public ObservableCollection<SalesOrder> UpdateSalesOrdersCollection(ObservableCollection<SalesOrder> ords)
        {
            var salesOrders = db.SalesOrders.Include(s => s.Retailer).ToList();
            ords = new ObservableCollection<SalesOrder>(salesOrders);
            return ords;
        }

        public void onDeleteSalesOrderLine(SalesOrderLine line)
        {

            if (line != null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you wish to delete this item?", "Confirm Delete", System.Windows.MessageBoxButton.YesNo);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    int salesOrderLineid = line.salesOrderLinesID;
                    SalesOrderLine lineToRemove = (SalesOrderLine)SalesOrderLines.Single(p => p.salesOrderLinesID == salesOrderLineid);

                    db.SalesOrderLines.Remove(lineToRemove);
                    db.SaveChanges();
                    SalesOrders = UpdateSalesOrdersCollection(SalesOrders);
                }
            }
        }

        public void onDeleteNewSalesOrderLine(SalesOrderLine line)
        {

            if (line != null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you wish to delete this item?", "Confirm Delete", System.Windows.MessageBoxButton.YesNo);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    int salesOrderLineid = line.salesOrderLinesID;
                    SalesOrderLine lineToRemove = (SalesOrderLine)NewSalesOrder.SalesOrderLines.Single(p => p.salesOrderLinesID == salesOrderLineid);

                    NewSalesOrder.SalesOrderLines.Remove(lineToRemove);
                }
            }
        }

    }
}
