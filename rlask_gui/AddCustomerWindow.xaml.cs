using rlask_engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace rlask_gui
{
    /// <summary>
    /// Interaction logic for AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window
    {
        private InvoicingService service;
        List<string> takenNames;
        public AddCustomerWindow(InvoicingService Service)
        {
            InitializeComponent();
            service = Service;
            takenNames = service.GetCustomers().Select(n => n.CustomerName).ToList();

            DataContext = new Customer();
        }


        private void SaveCustomer(object sender, RoutedEventArgs e)
        {
            Customer newCustomer = (Customer)DataContext;
            if (!takenNames.Contains(newCustomer.CustomerName))
            {
                service.SaveCustomer(newCustomer);
                Close();
            }
            else
            {
                MessageBox.Show($"Nimi {newCustomer.CustomerName} on jo varattu", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
}

        private void ValidateTextBoxes(object sender, RoutedEventArgs e)
        {
            if ( String.IsNullOrWhiteSpace(txtCustomerName.Text) || String.IsNullOrWhiteSpace(txtCustomerAddress.Text))
                btnSubmit.IsEnabled = false;
            else
                btnSubmit.IsEnabled = true;
        }
    }
}
