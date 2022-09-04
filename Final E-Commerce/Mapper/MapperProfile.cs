using AutoMapper;
using Final_E_Commerce.DTO.Bio_DTO;
using Final_E_Commerce.Entities;
using WebApi.DTO.CategoryDTO;
using WebApi.DTO.Product_DTOs;

namespace Final_E_Commerce.Mapper
{
    public class MapperProfile: AutoMapper.Profile
    {
        public MapperProfile()
        {
            CreateMap<Bio, BioReturnDTO>()
                .ForMember(d => d.Logo, map => map.MapFrom(s => s.Logo))
                .ForMember(d => d.Headertext, map => map.MapFrom(s => s.Headertext))
                .ForMember(d => d.Address, map => map.MapFrom(s => s.Address))
                .ForMember(d => d.Phone, map => map.MapFrom(s => s.Phone))
                .ForMember(d => d.Email, map => map.MapFrom(s => s.Email))
                .ForMember(d => d.Author, map => map.MapFrom(s => s.Author))
                .ForMember(d => d.Facebook, map => map.MapFrom(s => s.Facebook))
                .ForMember(d => d.Twitter, map => map.MapFrom(s => s.Twitter))
                .ForMember(d => d.Instagram, map => map.MapFrom(s => s.Instagram))
                .ForMember(d => d.Linkedin, map => map.MapFrom(s => s.Linkedin))
                .ForMember(d => d.NewsLetterText, map => map.MapFrom(s => s.NewsLetterText))
                .ForMember(d => d.NewsLetterHeader, map => map.MapFrom(s => s.NewsLetterHeader));
            CreateMap<Category, CategoryReturnDto>()
                .ForMember(d => d.ImageUrl, map => map.MapFrom(s => s.ImageUrl))
                .ForMember(d => d.Name, map => map.MapFrom(s => s.Name))
                .ForMember(d => d.ProductCount, map => map.MapFrom(s => s.Products.Count));
            CreateMap<Product, ProductReturnDto>()
                .ForMember(d => d.Name, map => map.MapFrom(s => s.Name))
                .ForMember(d => d.Price, map => map.MapFrom(s => s.Price))
                .ForMember(d => d.DiscountPercent, map => map.MapFrom(s => s.DiscountPercent))
                .ForMember(d => d.DiscountPrice, map => map.MapFrom(s => s.DiscountPrice))
                .ForMember(d => d.Description, map => map.MapFrom(s => s.Description))
                .ForMember(d => d.ReleaseDate, map => map.MapFrom(s => s.ReleaseDate))
                .ForMember(d => d.GPU, map => map.MapFrom(s => s.GPU))
                .ForMember(d => d.Body, map => map.MapFrom(s => s.Body))
                .ForMember(d => d.Chipset, map => map.MapFrom(s => s.Chipset))
                .ForMember(d => d.Display, map => map.MapFrom(s => s.Display))
                .ForMember(d => d.OperationSystem, map => map.MapFrom(s => s.OperationSystem))
                .ForMember(d => d.Memory, map => map.MapFrom(s => s.Memory))
                .ForMember(d => d.FrontCamera, map => map.MapFrom(s => s.FrontCamera))
                .ForMember(d => d.RearCamera, map => map.MapFrom(s => s.RearCamera))
                .ForMember(d => d.Battery, map => map.MapFrom(s => s.Battery))
                .ForMember(d => d.Weight, map => map.MapFrom(s => s.Weight))
                .ForMember(d => d.Instock, map => map.MapFrom(s => s.InStock))
                .ForMember(d => d.Views, map => map.MapFrom(s => s.Views))
                .ForMember(d => d.Category, map => map.MapFrom(s => s.Category));
            CreateMap<Product, ProductReturnDto>();
            CreateMap<List<ProductReturnDto>, ProductListDto>()
                .ForMember(d => d.Total, map => map.MapFrom(s => s.Count))
                .ForMember(d => d.Items, map => map.MapFrom(s => s));
        }
    }
}