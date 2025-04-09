using AutoMapper;
using ERP2024.Models.DTOs.Klijent;
using ERP2024.Models.Entities;

namespace ERP2024.Profiles
{
    public class KlijentProfile : Profile
    {
        public KlijentProfile()
        {
            CreateMap<Klijent, KlijentDto>().ReverseMap();
            CreateMap<KlijentCreationDto, Klijent>()
                .ForMember(dest => dest.datumRodjenja, opt => opt.MapFrom(src => DateOnly.Parse(src.datumRodjenja)));

            CreateMap<Klijent, KlijentCreationDto>()
                .ForMember(dest => dest.datumRodjenja, opt => opt.MapFrom(src => src.datumRodjenja.ToString()));

            CreateMap<KlijentUpdateDto, Klijent>()
                .ForMember(dest => dest.datumRodjenja, opt => opt.MapFrom(src => DateOnly.Parse(src.datumRodjenja)));

            CreateMap<Klijent, KlijentUpdateDto>()
                .ForMember(dest => dest.datumRodjenja, opt => opt.MapFrom(src => src.datumRodjenja.ToString()));
        }
    }
}
