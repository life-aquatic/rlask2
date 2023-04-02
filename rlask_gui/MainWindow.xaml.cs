using rlask_engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO;

namespace rlask_gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private InvoicingService service;
        Guid? currentlySelectedInvoice;
        int? currentlySelectedCustomer;
        Guid? currentlySelectedProduct;

        public enum CurrentView { Invoices, Customers, Products };
        CurrentView currentView;
        


        public MainWindow()
        {
            CultureInfo dateFormatCultureInfo = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            dateFormatCultureInfo.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            Thread.CurrentThread.CurrentCulture = dateFormatCultureInfo;
            
            service = new InvoicingService();
            InitializeComponent();
        }
        private void SetCustomersView(object sender, EventArgs e)
        {
            currentView = CurrentView.Customers;

            MainMenu.Items.Clear();
            MenuItem AddNewCustomerMenuItem = new MenuItem() { Header = "Lisää uusi asiakas", Style = (Style)FindResource("MenuItemStyle")  };
            AddNewCustomerMenuItem.Click += OpenAddCustomerWindow;
            MainMenu.Items.Add(AddNewCustomerMenuItem);

            MenuItem DeleteCustomerMenuItem = new MenuItem() { Header = "Poista valittu asiakas", Style = (Style)FindResource("MenuItemStyle") };
            DeleteCustomerMenuItem.Click += DeleteSelectedCustomers;
            currentlySelectedCustomer = null;
            DeleteCustomerMenuItem.Click += SetCustomersView;
            MainMenu.Items.Add(DeleteCustomerMenuItem);

            grdMain.Children.Clear();
            grdSecondary.Children.Clear();

            var grdCustomers = new DataGrid() { IsReadOnly = true, AutoGenerateColumns = false, ItemsSource = service.GetCustomers(), Style = (Style)FindResource("DataGridStyle") };
            var CustomersColumnHeaders = new List<(string, string)>() { ("Asiakkaan Nimi", "CustomerName"), ("Osoite", "CustomerAddress") };
            CustomersColumnHeaders.ForEach(n => grdCustomers.Columns.Add(new DataGridTextColumn() { Header = n.Item1, Binding = new Binding(n.Item2) }));

            if (currentlySelectedCustomer != null)
                UpdateSecondaryInvoicesView();
            else
                grdSecondary.Children.Add(new Label() { Content = "Valitse asiakas nähdäksesi laskut", Style = (Style)FindResource("LabelWatermarkStyle")});

            grdCustomers.SelectionChanged += (o, args) => UpdateSecondaryInvoicesView();

            grdMain.Children.Add(grdCustomers);

            void UpdateSecondaryInvoicesView()
            {
                if (grdCustomers.SelectedItem != null)
                    currentlySelectedCustomer = ((Customer)grdCustomers.SelectedItem).CustomerId;
                var grdSecondaryInvoices = new DataGrid() { IsReadOnly = true, AutoGenerateColumns = false, ItemsSource = service.GetInvoicesTextViewByCustomerId(currentlySelectedCustomer), Style = (Style)FindResource("DataGridStyle") };

                if (grdSecondaryInvoices.Items.Count > 0)
                {
                    var SecondaryInvoicesColumnHeaders = new List<(string, string, string)>() { ("Laskun päivä", "InvoiceDate", "dd.MM.yyyy"), 
                        ("Eräpäivä", "DueDate", "dd.MM.yyyy"), 
                        ("Maksettava", "TotalPrice", "0.00"), 
                        ("Asiakkaan nimi", "CustomerName", ""), 
                        ("Asiakkaan osoite", "CustomerAddress", ""), 
                        ("Lisätiedot", "ExtraDetails", "") };
                    SecondaryInvoicesColumnHeaders.ForEach(n => grdSecondaryInvoices.Columns.Add(new DataGridTextColumn() { Header = n.Item1, Binding = new Binding(n.Item2) { StringFormat = n.Item3 } }));
                    grdSecondary.Children.Add(grdSecondaryInvoices);
                }
                else
                {
                    grdSecondary.Children.Clear();
                    grdSecondary.Children.Add(new Label() { Content = "Ei vielä laskuja", Style = (Style)FindResource("LabelWatermarkStyle") });
                }
            }
        }
        private void SetInvoicesView(object sender, EventArgs e)
        {
            currentView = CurrentView.Invoices;

            MainMenu.Items.Clear();
            MenuItem AddNewInvoiceMenuItem = new MenuItem() { Header = "Lisää uusi lasku", Style = (Style)FindResource("MenuItemStyle") };
            AddNewInvoiceMenuItem.Click += OpenAddInvoiceWindow;
            MainMenu.Items.Add(AddNewInvoiceMenuItem);
            
            grdMain.Children.Clear();
            grdSecondary.Children.Clear();

            var grdInvoices = new DataGrid() { IsReadOnly = true, AutoGenerateColumns = false, ItemsSource = service.GetInvoicesTextView(), Style= (Style)FindResource("DataGridStyle") };
            var InvoicesColumnHeaders = new List<(string, string, string)>() { ("Laskun päivä", "InvoiceDate", "dd.MM.yyyy"),
                        ("Eräpäivä", "DueDate", "dd.MM.yyyy"),
                        ("Maksettava", "TotalPrice", "0.00"),
                        ("Asiakkaan nimi", "CustomerName", ""),
                        ("Asiakkaan osoite", "CustomerAddress", ""),
                        ("Lisätiedot", "ExtraDetails", "") };
            InvoicesColumnHeaders.ForEach(n => grdInvoices.Columns.Add(new DataGridTextColumn() { Header = n.Item1, Binding = new Binding(n.Item2) { StringFormat = n.Item3 } }));

            if (currentlySelectedInvoice!= null)
                UpdateInvoiceRowsGridView();
            else
                grdSecondary.Children.Add(new Label() { Content = "Valitse lasku nähdäksesi laskun tiedot", Style = (Style)FindResource("LabelWatermarkStyle") });

            grdInvoices.SelectionChanged += (o, args) => UpdateInvoiceRowsGridView();

            grdMain.Children.Add(grdInvoices);

            void UpdateInvoiceRowsGridView()
            {
                if(grdInvoices.SelectedItem != null)
                    currentlySelectedInvoice = ((InvoiceView)grdInvoices.SelectedItem).InvoiceId;
                var grdInvoiceRows = new DataGrid() { IsReadOnly = true, AutoGenerateColumns = false, ItemsSource = service.GetInvoiceRowsById(currentlySelectedInvoice), Style = (Style)FindResource("DataGridStyle") };
                var InvoiceRowsColumnHeaders = new List<(string, string, string)>() { ("Tuote", "ProductName", ""), 
                    ("Määrä", "Amount", "0.00"), 
                    ("Yksikkö", "Unit", ""), 
                    ("A-hinta", "UnitPrice", "0.00"), 
                    ("Maksettava", "Subtotal", "0.00") };
                InvoiceRowsColumnHeaders.ForEach(n => grdInvoiceRows.Columns.Add(new DataGridTextColumn() { Header = n.Item1, Binding = new Binding(n.Item2) { StringFormat = n.Item3 } }));
                grdSecondary.Children.Add(grdInvoiceRows);
            }
        }

        

        private void SetProductsView(object sender, EventArgs e)
        {
            currentView = CurrentView.Products;

            MainMenu.Items.Clear();
            MenuItem AddNewProductMenuItem = new MenuItem() { Header = "Lisää uusi tuote", Style = (Style)FindResource("MenuItemStyle") };
            AddNewProductMenuItem.Click += OpenAddProductWindow;
            MainMenu.Items.Add(AddNewProductMenuItem);

            MenuItem RemoveProductMenuItem = new MenuItem() { Header = "Poista valittu tuote", Style = (Style)FindResource("MenuItemStyle") };
            RemoveProductMenuItem.Click += DeleteSelectedProducts;
            currentlySelectedProduct = null;
            RemoveProductMenuItem.Click += SetProductsView;
            MainMenu.Items.Add(RemoveProductMenuItem);

            grdMain.Children.Clear();
            grdSecondary.Children.Clear();


            var grdProducts = new DataGrid() { IsReadOnly = true, AutoGenerateColumns = false, ItemsSource = service.GetProducts(), Style = (Style)FindResource("DataGridStyle") };
            var ProductsColumnHeaders = new List<(string, string, string)>() { ("Nimike", "ProductName", ""), ("Yksikkö", "Unit", ""), ("A-hinta", "UnitPrice", "0.00") };
            ProductsColumnHeaders.ForEach(n => grdProducts.Columns.Add(new DataGridTextColumn() { Header = n.Item1, Binding = new Binding(n.Item2) { StringFormat = n.Item3 } }));

            if (currentlySelectedProduct != null)
                UpdateProductDetailsView();
            else
                grdSecondary.Children.Add(new Label() { Content = "Valitse tuote nähdäksesi lisätiedot", Style = (Style)FindResource("LabelWatermarkStyle") });

            grdProducts.SelectionChanged += (o, args) => UpdateProductDetailsView();

            grdMain.Children.Add(grdProducts);

            void UpdateProductDetailsView()
            {
                grdSecondary.Children.Clear();
                if (grdProducts.SelectedItem != null)
                    currentlySelectedProduct = ((Product)grdProducts.SelectedItem).ProductId;

                Canvas cnvProductDetails = new Canvas();
                grdSecondary.Children.Add(cnvProductDetails);

                var grdProductDetails = new DataGrid() { IsReadOnly = true, 
                    AutoGenerateColumns = false, 
                    ItemsSource = service.GetProductDetailsByProductId(currentlySelectedProduct), 
                    Style = (Style)FindResource("DataGridStyle"),
                    HeadersVisibility = DataGridHeadersVisibility.None,
                    Margin = new Thickness(10),
                    Effect = null
                };

                cnvProductDetails.Children.Add(GetImageByProductId(currentlySelectedProduct));
                
                Canvas.SetLeft(grdProductDetails, 220);

                if (grdProductDetails.Items.Count > 0)
                {
                    cnvProductDetails.Children.Add(grdProductDetails);
                    var ProductDetailsColumnHeaders = new List<(string, string)>() { ("Ominaisuus", "PropertyName"), ("Tiedot", "PropertyValue") };
                    ProductDetailsColumnHeaders.ForEach(n => grdProductDetails.Columns.Add(new DataGridTextColumn() { Header = n.Item1, Binding = new Binding(n.Item2) }));
                }
                else
                {
                    grdSecondary.Children.Add(new Label() { Content = "Ei lisätietoja", Style = (Style)FindResource("LabelWatermarkStyle") });
                }
            }
        

    }

        

        private void OpenAddInvoiceWindow(object sender, RoutedEventArgs e)
        {
            var addInvoiceWindow = new AddInvoiceWindow(service);
            addInvoiceWindow.Closed += (o, args) => SetInvoicesView(o, args);
            addInvoiceWindow.ShowDialog();
        }

        private void OpenAddCustomerWindow(object sender, RoutedEventArgs e)
        {
            var addCustomerWindow = new AddCustomerWindow(service);
            addCustomerWindow.Closed += (o, args) => SetCustomersView(o, args);
            addCustomerWindow.ShowDialog();
        }

        private void DeleteSelectedCustomers(object sender, RoutedEventArgs e)
        {
            if (currentView == CurrentView.Customers && ((DataGrid)grdMain.Children[0]).SelectedItems.Count > 0)
                foreach (Customer i in ((DataGrid)grdMain.Children[0]).SelectedItems)
                    service.DeleteCustomerbyId(i);
        }

        private void OpenAddProductWindow(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new AddProductWindow(service);
            addProductWindow.Closed += (o, args) => SetProductsView(o, args);
            addProductWindow.ShowDialog();
        }
        private void DeleteSelectedProducts(object sender, RoutedEventArgs e)
        {
            if (currentView == CurrentView.Products && ((DataGrid)grdMain.Children[0]).SelectedItems.Count > 0)
                foreach (Product i in ((DataGrid)grdMain.Children[0]).SelectedItems)
                    service.DeleteProductById(i);
        }


        System.Windows.Controls.Image GetImageByProductId(Guid? ProductId)
        {
            Bitmap picture = service.GetPicFromDb(ProductId);
            MemoryStream Ms = new MemoryStream();
            picture.Save(Ms, System.Drawing.Imaging.ImageFormat.Png);
            Ms.Position = 0;
            BitmapImage ProductPicture = new BitmapImage();
            ProductPicture.BeginInit();
            ProductPicture.StreamSource = Ms;
            ProductPicture.EndInit();
            return new System.Windows.Controls.Image() { Source = ProductPicture, Width = 200, Margin = new Thickness(10) };
        }
    }
}
