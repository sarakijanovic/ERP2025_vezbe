using AutoMapper;
using ERP2024.Models.DTOs.Zaposleni;
using ERP2024.Models.Entities;

namespace ERP2024.Profiles
{
    public class ZaposleniProfile : Profile
    {
        public ZaposleniProfile()
        {
            CreateMap<Zaposleni, ZaposleniDto>().ReverseMap();
            CreateMap<Zaposleni, ZaposleniCreationDto>().ReverseMap();
            CreateMap<Zaposleni, ZaposleniUpdateDto>().ReverseMap();
        }
    }
}
