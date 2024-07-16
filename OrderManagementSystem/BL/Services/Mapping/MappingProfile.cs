
using AutoMapper;
using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.BL.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerModel,Customer>();
            CreateMap<OrderModel,Order>();
            CreateMap<OrderItemModel,OrderItem>();
            CreateMap<InvoiceModel,Invoice>();  
            CreateMap<ProductModel,Product>();
            CreateMap<UserModel,User>();

        }

    }
}
