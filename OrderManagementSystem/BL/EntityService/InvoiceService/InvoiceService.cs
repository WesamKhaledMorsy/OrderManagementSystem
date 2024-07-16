using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.BL.Helper;
using OrderManagementSystem.BL.Repository;
using OrderManagementSystem.BL.UnitOfWork;
using OrderManagementSystem.DL;
using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.BL.EntityService.InvoiceService
{
    public class InvoiceService: IInvoiceService
    {

        private readonly AppDBContext _context;
        private readonly IGenericRepository<Invoice> _IGenericRepository;
        private readonly IUnitOfWork<Invoice> _IUnitOfWork;
        private readonly IMapper _mapper;
        private readonly MailHelper _mailHelper;

        public InvoiceService(AppDBContext context , IUnitOfWork<Invoice> iUnitOfWork, IGenericRepository<Invoice> iGenericRepository,IMapper mapper,
            MailHelper mailHelper)
        {
            _context = context;
            _IGenericRepository = iGenericRepository;
            _IUnitOfWork = iUnitOfWork;
            _mapper = mapper;
            _mailHelper = mailHelper;
        }

        public Invoice GenerateInvoiceAsync(InvoiceModel invoice)
        {
            var myInvoic = _mapper.Map<Invoice> (invoice);
            var amount = _context.Orders.Where(x => x.OrderId == invoice.OrderId).Sum(x => x.TotalAmount);
            myInvoic.TotalAmount = amount;
            var ReqResult = _IGenericRepository.Insert(myInvoic);
            _IUnitOfWork.Save();
            var custId = _context.Orders.Where(x=>x.OrderId == invoice.OrderId).FirstOrDefault().CustomerId;
            SendInvoiceMailAsync(custId);
            return ReqResult;
        }
        public async Task SendInvoiceMailAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            var totalAmount = _context.Orders.Select(x => x.TotalAmount).Sum();
            if (customer != null)
            {
                // Assuming you have an EmailService that sends emails
                var message = $"{customer.Name.ToUpper()} Invoice";
                var amount = $"Your invoice details: {totalAmount}";
                var emailService = SendMail(customer.Name, customer.Email, message, amount);
                //await emailService.SendEmailAsync(customer.Email, "Your Invoice", $"Your invoice details: {invoice.TotalAmount}");
            }
        }
        public async Task<IActionResult> SendMail(string customerName, string customerEmail, string Subject, string body)
        {

            //senf mail before return 
            //MailHelper.sendMail
            string res = _mailHelper.SendMail("wkmorsy2022@gmail.com", "embaby97.ae@gmail.com", Subject, body);
            return new JsonResult(res);
        }

    }
}
