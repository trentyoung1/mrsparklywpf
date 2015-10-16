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

namespace MrSparklyWPF.Retailers
{
    class RetailersListViewModel : ViewModelBase
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        private ObservableCollection<Retailer> _retailers;
        public ObservableCollection<Retailer> Retailers
        {
            get { return _retailers; }
            set { SetProperty(ref _retailers, value); }
        }

        private Retailer _newRetailer;
        public Retailer NewRetailer
        {
            get { return _newRetailer; }
            set { SetProperty(ref _newRetailer, value); }
        }

        private ObservableCollection<Retailer> _searchResults;
        public ObservableCollection<Retailer> SearchResults
        {
            get { return _searchResults; }
            set { SetProperty(ref _searchResults, value); }
        }

        private MrSparklyEntities db = new MrSparklyEntities();

        public RetailersListViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;

            try
            {
                var retailers = db.Retailers.Include(r => r.Employee).Include(r => r.Suburb);
                Retailers = new ObservableCollection<Retailer>(retailers.ToList());
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }

            _newRetailer = new Retailer();
            _searchResults = new ObservableCollection<Retailer>();

            DeleteRetailerCommand = new RelayCommand<Retailer>(onDeleteRetailer);
            UpdateRetailerCommand = new RelayCommand<Retailer>(onUpdateRetailer);
            AddRetailerCommand = new RelayCommand<Retailer>(onAddRetailer);
            FindRetailersCommand = new RelayCommand<string>(onFindRetailers);
        }

        public RelayCommand<Retailer> DeleteRetailerCommand { get; private set; }
        public RelayCommand<Retailer> UpdateRetailerCommand { get; private set; }
        public RelayCommand<Retailer> AddRetailerCommand { get; private set; }
        public RelayCommand<string> FindRetailersCommand { get; private set; }

        public void onDeleteRetailer(Retailer ret)
        {

            if (ret != null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you wish to delete this retailer?", "Confirm Delete", System.Windows.MessageBoxButton.YesNo);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    int retailerid = ret.retailerID;
                    Retailer retToRemove = (Retailer)Retailers.Single(r => r.retailerID == retailerid);

                    db.Retailers.Remove(retToRemove);
                    db.SaveChanges();
                    Retailers = UpdateRetailersCollection(Retailers);
                }
            }
        }
        public void onUpdateRetailer(Retailer ret)
        {
            var retailer = ret;

            if (retailer != null)
            {
                db.Entry(retailer).State = EntityState.Modified;
                db.SaveChanges();
                Retailers = UpdateRetailersCollection(Retailers);

                MessageBoxResult msgBox = MessageBox.Show("Changes Saved", "Success");

            }
        }

        public void onAddRetailer(Retailer ret)
        {
            var newRetailer = ret;

            if (newRetailer != null)
            {
                db.Retailers.Add(newRetailer);
                db.SaveChanges();
                Retailers = UpdateRetailersCollection(Retailers);
                MessageBoxResult msgBox = MessageBox.Show("Retailer Added", "Success");
            }
        }

        public void onFindRetailers(string searchString)
        {
            var queryResult = from r in db.Retailers
                              where r.retailerName.Contains(searchString)
                              select r;
            SearchResults = new ObservableCollection<Retailer>(queryResult.ToList());

        }

        public ObservableCollection<Retailer> UpdateRetailersCollection(ObservableCollection<Retailer> rets)
        {
            var retailers = db.Retailers.Include(r => r.Employee).Include(r => r.Suburb);
            rets = new ObservableCollection<Retailer>(retailers);
            return rets;
        }

    }
}
