using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.BL.EntityService.InvoiceService
{
    public interface IInvoiceService
    {
        Invoice GenerateInvoiceAsync(InvoiceModel order);
        Invoice GetInvoiceByID(int id);
    }
}
