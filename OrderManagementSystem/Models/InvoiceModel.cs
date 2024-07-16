﻿namespace OrderManagementSystem.Models
{
    public class InvoiceModel
    {
        public int? InvoiceId { get; set; }
        public int ?OrderId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
