
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlask_engine
{
    public class InvoiceView
    {
        public Guid InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string ExtraDetails { get; set; }

    }
    public class InvoiceRowView
    {
        public string ProductName { get; set; }
        public decimal Amount { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
        public bool IsMaterial { get; set; }
    }
}
