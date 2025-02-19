using AutoMapper;
using OrderManagementAPI.Dtos;
using OrderManagementAPI.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        
    }
}
