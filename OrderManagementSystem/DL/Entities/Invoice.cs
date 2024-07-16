namespace OrderManagementSystem.DL.Entities
{
    public class Invoice
    {
        public Invoice() { }
        public int InvoiceId { get; set; }
        public int OrderId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
