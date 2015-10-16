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

namespace MrSparklyWPF.Products
{
    class ProductsListViewModel : ViewModelBase
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }

        private Product _newProduct;
        public Product NewProduct
        {
            get { return _newProduct; }
            set { SetProperty(ref _newProduct, value); }
        }

        private ObservableCollection<Product> _searchResults;
        public ObservableCollection<Product> SearchResults
        {
            get { return _searchResults; }
            set { SetProperty(ref _searchResults, value); }
        }

        private MrSparklyEntities db = new MrSparklyEntities();

        public ProductsListViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;

            try
            {
                var products = db.Products.ToList();
                Products = new ObservableCollection<Product>(products);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }

            _newProduct = new Product();
            _searchResults = new ObservableCollection<Product>();

            DeleteProductCommand = new RelayCommand<Product>(onDeleteProduct);
            UpdateProductCommand = new RelayCommand<Product>(onUpdateProduct);
            AddProductCommand = new RelayCommand<Product>(onAddProduct);
            FindProductsCommand = new RelayCommand<string>(onFindProducts);
        }

        public RelayCommand<Product> DeleteProductCommand { get; private set; }
        public RelayCommand<Product> UpdateProductCommand { get; private set; }
        public RelayCommand<Product> AddProductCommand { get; private set; }
        public RelayCommand<string> FindProductsCommand { get; private set; }

        public void onDeleteProduct(Product prod)
        {

            if (prod != null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you wish to delete this product?", "Confirm Delete", System.Windows.MessageBoxButton.YesNo);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    int productid = prod.productID;
                    Product prodToRemove = (Product)Products.Single(e => e.productID == productid);

                    db.Products.Remove(prodToRemove);
                    db.SaveChanges();
                    Products = UpdateProductsCollection(Products);
                }
            }
        }
        public void onUpdateProduct(Product prod)
        {
            var product = prod;

            if (product != null)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                Products = UpdateProductsCollection(Products);

                MessageBoxResult msgBox = MessageBox.Show("Changes Saved", "Success");

            }
        }

        public void onAddProduct(Product prod)
        {
            var newProduct = prod;

            if (newProduct != null)
            {
                db.Products.Add(newProduct);
                db.SaveChanges();
                Products = UpdateProductsCollection(Products);
                MessageBoxResult msgBox = MessageBox.Show("Product Added", "Success");
            }
        }

        public void onFindProducts(string searchString)
        {
            var queryResult = from p in db.Products
                              where p.productBrandName.Contains(searchString)
                              select p;
            SearchResults = new ObservableCollection<Product>(queryResult.ToList());

        }

        public ObservableCollection<Product> UpdateProductsCollection(ObservableCollection<Product> prods)
        {
            var products = db.Products.ToList();
            prods = new ObservableCollection<Product>(products);
            return prods;
        }

    }
}
