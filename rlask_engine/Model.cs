
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlask_engine
{
    /// <summary>
    /// Database context is relying on Entity Framework Core and SQLite. The connection string is provided 
    /// using $env:connectionString environment variable. 
    /// The model relies on 5 data classes: Invoices, Contractors, Customers, InvoiceRows and Products
    /// </summary>

    //public class InvoicingContext : DbContext
    //{
    //    public DbSet<Invoice> Invoices { get; set; }
    //    public DbSet<Contractor> Contractors { get; set; }
    //    public DbSet<Customer> Customers { get; set; }
    //    public DbSet<InvoiceRow> InvoiceRows { get; set; }
    //    public DbSet<Product> Products { get; set; }

    //    public InvoicingContext() { }

    //    protected override void OnConfiguring(DbContextOptionsBuilder options)
    //        => options.UseSqlite(Environment.GetEnvironmentVariable("ConnectionString"));
    //}

    public class Contractor
    {
        public int ContractorId { get; set; }
        public string ContractorName { get; set; }
        public string ContractorAddress { get; set; }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
    }

    public class InvoiceRow
    {
        public int InvoiceRowId { get; set; }
        public Guid ProductId { get; set; }
        public decimal Amount { get; set; }
        public Guid InvoiceId { get; set; }
        public decimal Total { get; set; }
        //public string[] GetValuesAsArray() => 
        //    new string[] { Product.ProductName, Amount.ToString("N2"), Product.Unit, Product.UnitPrice.ToString("N2"), Total.ToString("N2") };
    }

    public class Product
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsMaterial { get; set; }
        public byte[] ProductPicture { get; set; }
        public int PictureSize { get; set; }
        public List<ProductDetail> ProductDetails { get; set; } = new();
        public override string ToString() => $"Tuote: {ProductName}, kappalehinta: {UnitPrice:N2}/{Unit}";
    }

    public class ProductDetail
    {
        public int ProductDetailId { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public Guid ProductId { get; set; }
    }
}
