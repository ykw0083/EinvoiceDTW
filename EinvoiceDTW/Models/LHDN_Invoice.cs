namespace EinvoiceDTW.Models
{
    public class LHDN_Invoice
    {
        public int InvoiceID { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string SupplierName { get; set; }
        public string SupplierTaxID { get; set; }
        public string SupplierAddress { get; set; }
        public string CustomerName { get; set; }
        public string CustomerTaxID { get; set; }
        public string CustomerAddress { get; set; }
        public int ItemLine { get; set; }
        public string ItemDescription { get; set; }
        public decimal ItemQuantity { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public decimal ItemTotal { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentTerms { get; set; }
        public string Notes { get; set; }
        public bool Confirm { get; set; }
        public string MyFileName { get; set; }
        public string LHDN_Status { get; set; }
        public string LHDN_Response { get; set; }
    }
}
