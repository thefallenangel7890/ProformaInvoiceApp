using System;

namespace ProformaInvoiceApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string VariationCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string PackSize { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsBaseProduct { get; set; } = true;
        public int BaseProductId { get; set; } = 0;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}