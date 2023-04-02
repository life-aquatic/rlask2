using rlask_engine;
using System;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Windows.Resources;

namespace rlask_gui
{
    /// <summary>
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        InvoicingService service;
        Guid NewProductId = Guid.NewGuid();
        Stream PictureStream;
        
        public AddProductWindow(InvoicingService Service)
        {
            InitializeComponent();
            service = Service;
            DataContext = new Product() { ProductId = NewProductId };
            PictureStream = Application.GetResourceStream(new Uri("pack://application:,,,/Images/default.png")).Stream;
            grdProductDetails.ItemsSource = ((Product)DataContext).ProductDetails;
        }

        private void SelectPicture(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Kuvat (*.png)|*.png|Kaikki tiedostot (*.*)|*.*" };
            if (openFileDialog.ShowDialog() == true)
            {
                PictureFilePath.Text = openFileDialog.FileName;
                PictureStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
            }
            
        }

        private void SaveProduct(object sender, RoutedEventArgs e)
        {
            var product = (Product)DataContext;

            
            if (product.UnitPrice <= 0)
                MessageBox.Show("Virheellinen hinta", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                ((Product)DataContext).PictureSize = (int)PictureStream.Length;
                
                BinaryReader reader = new BinaryReader(PictureStream);
                ((Product)DataContext).ProductPicture = reader.ReadBytes((int)PictureStream.Length);
                reader.Close();

                service.SaveProduct(product);
                PictureStream.Close();
                Close();
            }
        }
    }
}


