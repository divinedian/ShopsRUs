using AutoMapper;
using ShopsRUs.Core.Core.Application.Command;
using ShopsRUs.Core.Core.Application.Commands;
using ShopsRUs.Core.Core.Application.Queries;
using ShopsRUs.Data.Models;

namespace ShopsRUs.Core.Core.Profiles
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CreateACustomerCommand>().ReverseMap();
            CreateMap<Discount, AddDiscountTypeCommand>().ReverseMap();
            CreateMap<Category, CreateCategoryCommand>().ReverseMap();
            CreateMap<Item, CreateItemCommand>().ReverseMap();
            CreateMap<Order, CreateInvoiceFromBillCommand>().ReverseMap();
            
        }
    }
}
