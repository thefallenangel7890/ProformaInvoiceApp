using System;

namespace ProformaInvoiceApp.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        public string Buyer { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string PackSize { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount => Quantity * Rate;
        public string BaseProductCode { get; set; } = string.Empty;
        public decimal OriginalPrice { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}