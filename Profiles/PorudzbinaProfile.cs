using AutoMapper;
using ERP2024.Models.DTOs.Porudzbina;
using ERP2024.Models.DTOs.Zaposleni;
using ERP2024.Models.Entities;

namespace ERP2024.Profiles
{
    public class PorudzbinaProfile : Profile
    {
        public PorudzbinaProfile()
        {
            CreateMap<Porudzbina, PorudzbinaDto>().ReverseMap();

            CreateMap<PorudzbinaUpdateDto, Porudzbina>()
                .ForMember(dest => dest.datumPorudzbine, opt => opt.MapFrom(src => DateOnly.Parse(src.datumPorudzbine)));

            CreateMap<Porudzbina, PorudzbinaUpdateDto>()
                .ForMember(dest => dest.datumPorudzbine, opt => opt.MapFrom(src => src.datumPorudzbine.ToString()));

            CreateMap<PorudzbinaCreationDto, Porudzbina>()
                .ForMember(dest => dest.datumPorudzbine, opt => opt.MapFrom(src => DateOnly.Parse(src.datumPorudzbine)));

            CreateMap<Porudzbina, PorudzbinaCreationDto>()
                .ForMember(dest => dest.datumPorudzbine, opt => opt.MapFrom(src => src.datumPorudzbine.ToString()));
        }
    }

}
