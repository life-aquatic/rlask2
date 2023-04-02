using rlask_engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace rlask_gui
{
    /// <summary>
    /// Interaction logic for AddInvoiceWindow.xaml
    /// </summary>
    public partial class AddInvoiceWindow : Window
    {
        private InvoicingService service;
        Guid NewInvoiceId = Guid.NewGuid();
        
        public AddInvoiceWindow(InvoicingService Service)
        {
            
            InitializeComponent();

            service = Service;

            comCustomer.ItemsSource = service.GetCustomers();
            comProductColumn.ItemsSource = service.GetProducts();

            DataContext = new Invoice() { InvoiceId = NewInvoiceId, InvoiceDate = DateTime.Now, DaysToPay = 14, ContractorId = 1, ExtraDetails = "", InvoiceRows = new List<InvoiceRow>() };
        }

        

        private void SaveInvoice(object sender, RoutedEventArgs e)
        {
            var invoice = (Invoice)DataContext;

            if (comCustomer.SelectedItem is null)
                MessageBox.Show("Valitse asiakas", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else if (!invoice.InvoiceRows.Any())
                MessageBox.Show("Laskussa ei ole tuotteja", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else 
            { 
                invoice.CustomerId = ((Customer)comCustomer.SelectedItem).CustomerId;
                service.SaveInvoice(invoice);
                Close();
            }
        }
    }
}
