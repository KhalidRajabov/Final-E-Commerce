using AutoMapper;
using Final_E_Commerce.DTO.Bio_DTO;
using Final_E_Commerce.Entities;

namespace Final_E_Commerce.Mapper
{
    public class MapperProfile:Profile
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
        }
    }
}
