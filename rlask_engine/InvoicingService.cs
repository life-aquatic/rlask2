using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Drawing.Imaging;
using System.Drawing;

namespace rlask_engine
{
    public class InvoicingService
    {

        private string localMySql = Environment.GetEnvironmentVariable("ConnectionStringMySql");
        
        
        public ObservableCollection<Invoice> GetInvoices()
        {
            var invoices = new ObservableCollection<Invoice>();
            using (MySqlConnection conn = new MySqlConnection(localMySql))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM invoices", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    invoices.Add(new Invoice()
                    {
                        InvoiceId = dr.GetGuid("invoice_id"),
                        InvoiceDate = dr.GetDateTime("invoice_date"),
                        DaysToPay = dr.GetInt32("days_to_pay"),
                        ContractorId = dr.GetInt32("contractor_id"),
                        CustomerId = dr.GetInt32("customer_id"),
                        ExtraDetails = dr.GetString("extra_details")
                    });
                }
            }
            return invoices;
        }

        public ObservableCollection<InvoiceView> GetInvoicesTextView()
        {
            var invoices = new ObservableCollection<InvoiceView>();
            using (MySqlConnection conn = new MySqlConnection(localMySql))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM invoices_text_view", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    invoices.Add(new InvoiceView()
                    {
                        InvoiceId = dr.GetGuid("invoice_id"),
                        InvoiceDate = dr.GetDateTime("invoice_date"),
                        DueDate = dr.GetDateTime("due_date"),
                        TotalPrice = dr.GetDecimal("total_sum"),
                        CustomerName = dr.GetString("customer_name"),
                        CustomerAddress = dr.GetString("customer_address"),
                        ExtraDetails = dr.GetString("extra_details")
                    });
                }
            }
            return invoices;
        }

        public ObservableCollection<InvoiceView> GetInvoicesTextViewByCustomerId(int? CustomerId)
        {
            var invoices = new ObservableCollection<InvoiceView>();
            using (MySqlConnection conn = new MySqlConnection(localMySql))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("call get_invoices_text_view_by_customer_id (@customer_id);", conn);
                cmd.Parameters.AddWithValue("@customer_id", CustomerId);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    invoices.Add(new InvoiceView()
                    {
                        InvoiceId = dr.GetGuid("invoice_id"),
                        InvoiceDate = dr.GetDateTime("invoice_date"),
                        DueDate = dr.GetDateTime("due_date"),
                        TotalPrice = dr.GetDecimal("total_sum"),
                        CustomerName = dr.GetString("customer_name"),
                        CustomerAddress = dr.GetString("customer_address"),
                        ExtraDetails = dr.GetString("extra_details")
                    });
                }
            }
            return invoices;
        }


        public ObservableCollection<InvoiceRowView> GetInvoiceRowsById(Guid? InvoiceId)
        {
            var rows = new ObservableCollection<InvoiceRowView>();
            using (MySqlConnection conn = new MySqlConnection(localMySql))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("call get_rows_by_invoice_id(@invoice_id)", conn);
                cmd.Parameters.AddWithValue("@invoice_id", InvoiceId);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rows.Add(new InvoiceRowView()
                    {
                        ProductName = dr.GetString("product_name"),
                        Amount = dr.GetDecimal("amount"),
                        Unit = dr.GetString("unit"),
                        UnitPrice = dr.GetDecimal("unit_price"),
                        Subtotal = dr.GetDecimal("subtotal"),
                        IsMaterial = dr.GetBoolean("is_material")
                    });
                }
            }
            return rows;
        }


        public ObservableCollection<Product> GetProducts()
        {
            var products = new ObservableCollection<Product>();
            using (MySqlConnection conn = new MySqlConnection(localMySql))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM products WHERE is_deleted = 0", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    products.Add(new Product()
                    {
                        ProductId = dr.GetGuid("product_id"),
                        ProductName = dr.GetString("product_name"),
                        Unit = dr.GetString("unit"),
                        UnitPrice = dr.GetDecimal("unit_price"),
                        IsMaterial = dr.GetBoolean("is_material")
                    });
                }
            }
            return products;
        }

        public ObservableCollection<ProductDetail> GetProductDetailsByProductId(Guid? ProductId)
        {
            var rows = new ObservableCollection<ProductDetail>();
            using (MySqlConnection conn = new MySqlConnection(localMySql))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT product_detail_id, property_name, property_value, product_id FROM product_details WHERE product_id = @product_id", conn);
                cmd.Parameters.AddWithValue("@product_id", ProductId);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rows.Add(new ProductDetail()
                    {
                        ProductDetailId= dr.GetInt32("product_detail_id"),
                        PropertyName = dr.GetString("property_name"),
                        PropertyValue = dr.GetString("property_value"),
                        ProductId = dr.GetGuid("product_id")
                    });
                }
            }
            return rows;
        }
        




        public ObservableCollection<Customer> GetCustomers()
        {
            var customers = new ObservableCollection<Customer>();
            using (MySqlConnection conn = new MySqlConnection(localMySql))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM customers where is_deleted = 0", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    customers.Add(new Customer()
                    {
                        CustomerId = dr.GetInt32("customer_id"),
                        CustomerName = dr.GetString("customer_name"),
                        CustomerAddress = dr.GetString("customer_address"),
                    });
                }
            }
            return customers;
        }

        



        public void SaveInvoice(Invoice invoice)
        {

            using (MySqlConnection conn = new MySqlConnection(localMySql))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO invoices (invoice_id, invoice_date, days_to_pay, contractor_id, customer_id, extra_details) VALUES (@invoice_id, @invoice_date, @days_to_pay, @contractor_id, @customer_id, @extra_details);", conn);
                cmd.Parameters.AddWithValue("@invoice_id", invoice.InvoiceId);
                cmd.Parameters.Add("@invoice_date", MySqlDbType.Date).Value = invoice.InvoiceDate;
                cmd.Parameters.AddWithValue("@days_to_pay", invoice.DaysToPay);
                cmd.Parameters.AddWithValue("@contractor_id", invoice.ContractorId);
                cmd.Parameters.AddWithValue("@customer_id", invoice.CustomerId);
                cmd.Parameters.AddWithValue("@extra_details", invoice.ExtraDetails);

                cmd.ExecuteNonQuery();

                foreach (var row in invoice.InvoiceRows)
                {
                    MySqlCommand cmdIns = new MySqlCommand("call insert_row_freeze_price (@invoice_id, @product_id, @amount)", conn);
                    cmdIns.Parameters.AddWithValue("@product_id", row.ProductId);
                    cmdIns.Parameters.AddWithValue("@amount", row.Amount);
                    cmdIns.Parameters.AddWithValue("@invoice_id", invoice.InvoiceId);
                    cmdIns.ExecuteNonQuery();
                }
            }
        }

        public void SaveProduct(Product product)
        {

            using (MySqlConnection conn = new MySqlConnection(localMySql))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO products (product_id, product_name, unit, unit_price, is_material, picture, picture_size) VALUES (@product_id, @product_name, @unit, @unit_price, @is_material, @picture, @picture_size)", conn);
                cmd.Parameters.AddWithValue("@product_id", product.ProductId);
                cmd.Parameters.AddWithValue("@product_name", product.ProductName);
                cmd.Parameters.AddWithValue("@unit", product.Unit);
                cmd.Parameters.AddWithValue("@unit_price", product.UnitPrice);
                cmd.Parameters.AddWithValue("@is_material", product.IsMaterial);
                cmd.Parameters.AddWithValue("@picture", product.ProductPicture);
                cmd.Parameters.AddWithValue("@picture_size", product.PictureSize);

                cmd.ExecuteNonQuery();

                foreach (var productDetail in product.ProductDetails)
                {
                    MySqlCommand cmdIns = new MySqlCommand("INSERT INTO product_details (property_name, property_value, product_id) VALUES (@property_name, @property_value, @invoice_id)", conn);
                    cmdIns.Parameters.AddWithValue("@property_name", productDetail.PropertyName);
                    cmdIns.Parameters.AddWithValue("@property_value", productDetail.PropertyValue);
                    cmdIns.Parameters.AddWithValue("@invoice_id", product.ProductId);
                    cmdIns.ExecuteNonQuery();
                }
            }
        }

        public void SaveCustomer(Customer customer)
        {

            using (MySqlConnection conn = new MySqlConnection(localMySql))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO customers (customer_name, customer_address) VALUES (@customer_name, @customer_address)", conn);
                cmd.Parameters.AddWithValue("@customer_name", customer.CustomerName);
                cmd.Parameters.AddWithValue("@customer_address", customer.CustomerAddress);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCustomerbyId(Customer customer)
        {
            using (MySqlConnection conn = new MySqlConnection(localMySql))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("update customers set is_deleted = 1 where customer_id = @customer_id", conn);
                cmd.Parameters.AddWithValue("@customer_id", customer.CustomerId);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteProductById(Product product)
        {
            using (MySqlConnection conn = new MySqlConnection(localMySql))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("update products set is_deleted = 1 where product_id = @product_id", conn);
                cmd.Parameters.AddWithValue("@product_id", product.ProductId);
                cmd.ExecuteNonQuery();
            }
        }

        public Bitmap GetPicFromDb(Guid? ProductId)
        {
            int fileLength;
            byte[] bitmap = new byte[1];

            using (MySqlConnection conn = new MySqlConnection(localMySql))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT picture, picture_size FROM products WHERE product_id = @product_id;", conn);
                cmd.Parameters.AddWithValue("@product_id", ProductId);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    fileLength = dr.GetInt32("picture_size");
                    bitmap = new byte[fileLength];
                    dr.GetBytes(dr.GetOrdinal("picture"), 0, bitmap, 0, fileLength);
                }
            }

            return new Bitmap(new MemoryStream(bitmap));
        }
    }
}
