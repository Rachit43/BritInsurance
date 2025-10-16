using AutoMapper;
using BritInsurance.Application.Dto;
using BritInsurance.Domain.Entities;

namespace BritInsurance.Application.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, GetProductDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<Item, GetItemDto>().ReverseMap();
            CreateMap<Item, CreateItemDto>().ReverseMap();
            CreateMap<Item, UpdateItemDto>().ReverseMap();
            CreateMap<Item, UpdateItemFromProductDto>().ReverseMap();
            CreateMap<Item, CreateItemFromProductDto>().ReverseMap();
        }
    }
}
