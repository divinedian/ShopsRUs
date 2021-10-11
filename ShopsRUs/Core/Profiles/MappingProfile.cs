using AutoMapper;
using ShopsRUs.Core.Core.Application.Commands;
using ShopsRUs.Data.Models;

namespace ShopsRUs.Core.Core.Profiles
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CreateACustomerCommand>().ReverseMap();
            CreateMap<Discount, AddDiscountTypeCommand>().ReverseMap();
        }
    }
}
