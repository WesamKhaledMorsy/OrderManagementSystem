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

        public InvoiceService(AppDBContext context , IUnitOfWork<Invoice> iUnitOfWork, IGenericRepository<Invoice> iGenericRepository,IMapper mapper)
        {
            _context = context;
            _IGenericRepository = iGenericRepository;
            _IUnitOfWork = iUnitOfWork;
            _mapper = mapper;
            
        }

        public Invoice GenerateInvoiceAsync(InvoiceModel invoice)
        {
            var myInvoic = _mapper.Map<Invoice> (invoice);
            var ReqResult = _IGenericRepository.Insert(myInvoic);
            _IUnitOfWork.Save();
            var custId = _context.Orders.Where(x=>x.OrderId == invoice.OrderId).FirstOrDefault().CustomerId;
          
            return ReqResult;
        }
        public Invoice GetInvoiceByID(int id)
        {
            return _IGenericRepository.GetById(id);
        }
 

    }
}
