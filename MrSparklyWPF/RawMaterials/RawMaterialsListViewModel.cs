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

namespace MrSparklyWPF.RawMaterials
{
    class RawMaterialsListViewModel : ViewModelBase
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        private ObservableCollection<RawMaterial> _rawMaterials;
        public ObservableCollection<RawMaterial> RawMaterials
        {
            get { return _rawMaterials; }
            set { SetProperty(ref _rawMaterials, value); }
        }

        private RawMaterial _newRawMaterial;
        public RawMaterial NewRawMaterial
        {
            get { return _newRawMaterial; }
            set { SetProperty(ref _newRawMaterial, value); }
        }

        private ObservableCollection<RawMaterial> _searchResults;
        public ObservableCollection<RawMaterial> SearchResults
        {
            get { return _searchResults; }
            set { SetProperty(ref _searchResults, value); }
        }

        private MrSparklyEntities db = new MrSparklyEntities();

        public RawMaterialsListViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;

            try
            {
                var rawMaterials = db.RawMaterials.ToList();
                RawMaterials = new ObservableCollection<RawMaterial>(rawMaterials);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }

            _newRawMaterial = new RawMaterial();
            _searchResults = new ObservableCollection<RawMaterial>();

            DeleteRawMaterialCommand = new RelayCommand<RawMaterial>(onDeleteRawMaterial);
            UpdateRawMaterialCommand = new RelayCommand<RawMaterial>(onUpdateRawMaterial);
            AddRawMaterialCommand = new RelayCommand<RawMaterial>(onAddRawMaterial);
            FindRawMaterialsCommand = new RelayCommand<string>(onFindRawMaterials);
        }

        public RelayCommand<RawMaterial> DeleteRawMaterialCommand { get; private set; }
        public RelayCommand<RawMaterial> UpdateRawMaterialCommand { get; private set; }
        public RelayCommand<RawMaterial> AddRawMaterialCommand { get; private set; }
        public RelayCommand<string> FindRawMaterialsCommand { get; private set; }

        public void onDeleteRawMaterial(RawMaterial rawMat)
        {

            if (rawMat != null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you wish to delete this rawMaterial?", "Confirm Delete", System.Windows.MessageBoxButton.YesNo);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    int rawMaterialid = rawMat.rawMaterialsID;
                    RawMaterial rawMatToRemove = (RawMaterial)RawMaterials.Single(r => r.rawMaterialsID == rawMaterialid);

                    db.RawMaterials.Remove(rawMatToRemove);
                    db.SaveChanges();
                    RawMaterials = UpdateRawMaterialsCollection(RawMaterials);
                }
            }
        }
        public void onUpdateRawMaterial(RawMaterial rawMat)
        {
            var rawMaterial = rawMat;

            if (rawMaterial != null)
            {
                db.Entry(rawMaterial).State = EntityState.Modified;
                db.SaveChanges();
                RawMaterials = UpdateRawMaterialsCollection(RawMaterials);

                MessageBoxResult msgBox = MessageBox.Show("Changes Saved", "Success");

            }
        }

        public void onAddRawMaterial(RawMaterial rawMat)
        {
            var newRawMaterial = rawMat;

            if (newRawMaterial != null)
            {
                db.RawMaterials.Add(newRawMaterial);
                db.SaveChanges();
                RawMaterials = UpdateRawMaterialsCollection(RawMaterials);
                MessageBoxResult msgBox = MessageBox.Show("RawMaterial Added", "Success");
            }
        }

        public void onFindRawMaterials(string searchString)
        {
            var queryResult = from r in db.RawMaterials
                              where r.rawMaterialsName.Contains(searchString)
                              select r;
            SearchResults = new ObservableCollection<RawMaterial>(queryResult.ToList());

        }

        public ObservableCollection<RawMaterial> UpdateRawMaterialsCollection(ObservableCollection<RawMaterial> rawMats)
        {
            var rawMaterials = db.RawMaterials.ToList();
            rawMats = new ObservableCollection<RawMaterial>(rawMaterials);
            return rawMats;
        }

    }
}
