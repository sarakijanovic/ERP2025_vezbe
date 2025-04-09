using AutoMapper;
using ERP2024.Models.DTOs.AparatPorudzbina;
using ERP2024.Models.DTOs.Zaposleni;
using ERP2024.Models.Entities;

namespace ERP2024.Profiles
{
    public class AparatPorudzbinaProfile : Profile
    {
        public AparatPorudzbinaProfile()
        {
            CreateMap<AparatPorudzbina, AparatPorudzbinaDto>().ReverseMap();
            CreateMap<AparatPorudzbina, AparatPorudzbinaCreationDto>().ReverseMap();
            CreateMap<AparatPorudzbina, AparatPorudzbinaUpdateDto>().ReverseMap();
        }
    }
}
