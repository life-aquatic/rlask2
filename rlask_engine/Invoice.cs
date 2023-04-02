using System.Collections.ObjectModel;
using System.ComponentModel;

namespace rlask_engine
{
    public class Invoice
    {
        public Guid InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get => InvoiceDate.AddDays(DaysToPay); }
        public int DaysToPay { get; set; }
        public int ContractorId { get; set; }
        public int CustomerId { get; set; }
        public List<InvoiceRow> InvoiceRows { get; set; } = new();
        public string ExtraDetails { get; set; } = string.Empty;
        public decimal TotalSum { get => InvoiceRows.Count() == 0 ? 0 : InvoiceRows.Sum(n => n.Total); }
    }
}
