
using System;
using System.Collections.ObjectModel;

namespace rlask_engine
{
    /// <summary>
    /// Data manipulation commands. Each command follows the same model: it creates database context, retrieves or saves data
    /// using LINQ and calls DBContext.SaveChanges() if necessary
    /// </summary>
    public static class Commands
    {

        //    public static string ListAllInvoices()
        //    {
        //        using (var db = new InvoicingContext())
        //        {
        //            var invoices = db.Invoices
        //                .AsNoTracking()
        //                .Include(n => n.Contractor)
        //                .Include(n => n.Customer)
        //                .Include(n => n.InvoiceRows)
        //                .ThenInclude(n => n.Product)
        //                .ToList();
        //            return string.Join('\n', invoices);
        //        }
        //    }

        //    public static ObservableCollection<Invoice> GetRawInvoices()
        //    {
        //        using (var db = new InvoicingContext())
        //        {
        //            return new ObservableCollection<Invoice>(db.Invoices
        //                .AsNoTracking()
        //                .Include(n => n.Contractor)
        //                .Include(n => n.Customer)
        //                .Include(n => n.InvoiceRows)
        //                .ThenInclude(n => n.Product));
        //        }
        //    }

        //    public static string GetInvoiceById()
        //    {
        //        Console.WriteLine("Anna haluamasi laskun numero");
        //        int id = Convert.ToInt32(Console.ReadLine());

        //        using (var db = new InvoicingContext())
        //        {
        //            return db.Invoices
        //                .AsNoTracking()
        //                .Where(n => n.InvoiceId == id)
        //                .Include(n => n.Contractor)
        //                .Include(n => n.Customer)
        //                .Include(n => n.InvoiceRows)
        //                .ThenInclude(n => n.Product)
        //                .Single()
        //                .ToString();
        //        }
        //    }

        //    public static string ListAllProducts()
        //    {
        //        using (var db = new InvoicingContext())
        //        {
        //            var products = db.Products
        //                .AsNoTracking()
        //                .Where(n => n.IsMaterial)
        //                .ToList();

        //            return String.Join('\n', products);
        //        }
        //    }

        //    public static string ListInvoicesByCustomerId()
        //    {
        //        Console.WriteLine("Anna asiakkaan numero");
        //        int id = Convert.ToInt32(Console.ReadLine());

        //        using (var db = new InvoicingContext())
        //        {
        //            var invoicesByCustomer = db.Invoices
        //                .AsNoTracking()
        //                .Include(n => n.Contractor)
        //                .Include(n => n.Customer)
        //                .Include(n => n.InvoiceRows)
        //                .ThenInclude(n => n.Product)
        //                .Where(n => n.Customer.CustomerId == id)
        //                .ToList();

        //            return String.Join('\n', invoicesByCustomer);
        //        }
        //    }

        //    public static string ListInvoicesByProductId()
        //    {
        //        Console.WriteLine("Anna tuotteen numero");
        //        int id = Convert.ToInt32(Console.ReadLine());

        //        using (var db = new InvoicingContext())
        //        {
        //            var invoicesByCustomer = db.Invoices
        //                .AsNoTracking()
        //                .Include(n => n.Contractor)
        //                .Include(n => n.Customer)
        //                .Include(n => n.InvoiceRows)
        //                .ThenInclude(n => n.Product)
        //                .Where(n => n.InvoiceRows.Any(n => n.Product.ProductId == id))
        //                .ToList();

        //            return String.Join('\n', invoicesByCustomer);
        //        }
        //    }

        //    public static Invoice GenerateInvoice(InvoicingContext context, string contractorName, string customerName, int daysLeftToPay, string extraDetails = "")
        //    {
        //        var contractor = context.Contractors.Single(n => n.ContractorName == contractorName);
        //        var customer = context.Customers.Single(n => n.CustomerName == customerName);

        //        return new Invoice()
        //        {
        //            InvoiceDate = DateTime.Now,
        //            DaysToPay = daysLeftToPay,
        //            Contractor = contractor,
        //            Customer = customer,
        //            ExtraDetails = extraDetails
        //        };
        //    }

        //    public static Invoice GenerateInvoice2(string contractorName, string customerName, int daysLeftToPay, string extraDetails = "")
        //    {
        //        using (var context = new InvoicingContext())
        //        {
        //            var contractor = context.Contractors.Single(n => n.ContractorName == contractorName);
        //            var customer = context.Customers.Single(n => n.CustomerName == customerName);

        //            return new Invoice()
        //            {
        //                InvoiceDate = DateTime.Now,
        //                DaysToPay = daysLeftToPay,
        //                Contractor = contractor,
        //                Customer = customer,
        //                ExtraDetails = extraDetails
        //            };
        //        }
        //    }


        //    public static void AddRowToInvoice(this Invoice invoice, InvoicingContext context, string ProductName, decimal amount)
        //    {
        //        var product = context.Products.Single(n => n.ProductName == ProductName);
        //        invoice.InvoiceRows.Add(new InvoiceRow() { Product = product, Amount = amount, Invoice = invoice, Total = product.UnitPrice * amount });
        //    }

        //    public static void SeedTestData()
        //    {
        //        using (var db = new InvoicingContext())
        //        {
        //            db.Database.EnsureDeleted();
        //            db.Database.EnsureCreated();

        //            db.Add(new Customer { CustomerName = "Malli Asiakas 1", CustomerAddress = "Asiakkaantie 1 80100 JOENSUU" });
        //            db.Add(new Customer { CustomerName = "Malli Asiakas 2", CustomerAddress = "Asiakkaantie 2 80100 JOENSUU" });
        //            db.Add(new Customer { CustomerName = "Malli Asiakas 3", CustomerAddress = "Asiakkaantie 3 80100 JOENSUU" });
        //            db.Add(new Contractor { ContractorName = "Rakennus Oy", ContractorAddress = "Karjalankatu 3 80200 JOENSUU" });
        //            db.Add(new Product { ProductName = "Työ: maalaus", Unit = "h", UnitPrice = 60m, IsMaterial = false });
        //            db.Add(new Product { ProductName = "Työ: asennus", Unit = "h", UnitPrice = 80m, IsMaterial = false });
        //            db.Add(new Product { ProductName = "Parketti", Unit = "m2", UnitPrice = 35m, IsMaterial = true });
        //            db.Add(new Product { ProductName = "Pesuallashana", Unit = "kpl", UnitPrice = 180m, IsMaterial = true });
        //            db.Add(new Product { ProductName = "Silikoni", Unit = "kpl", UnitPrice = 5.95m, IsMaterial = true });
        //            db.Add(new Product { ProductName = "Tiiviste", Unit = "kpl", UnitPrice = 1m, IsMaterial = true });
        //            db.Add(new Product { ProductName = "Kattopaneeli", Unit = "m", UnitPrice = 1.95m, IsMaterial = true });
        //            db.SaveChanges();

        //            Invoice invoice;

        //            invoice = GenerateInvoice(db, "Rakennus Oy", "Malli Asiakas 3", 12, "Putkiremontti ja sähkojärjestelmien uusiminen");
        //            invoice.AddRowToInvoice(db, "Työ: asennus", 3m);
        //            invoice.AddRowToInvoice(db, "Pesuallashana", 1m);
        //            db.Add(invoice);
        //            db.SaveChanges();

        //            invoice = GenerateInvoice(db, "Rakennus Oy", "Malli Asiakas 3", 12, "Kellarikäytävän lattian ja seinäpintojen maalaus");
        //            invoice.AddRowToInvoice(db, "Silikoni", 1m);
        //            invoice.AddRowToInvoice(db, "Parketti", 23.3m);
        //            invoice.AddRowToInvoice(db, "Työ: maalaus", 3.5m);
        //            db.Add(invoice);
        //            db.SaveChanges();
        //        }
        //    } 
    }
}