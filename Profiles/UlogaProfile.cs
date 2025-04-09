using AutoMapper;
using ERP2024.Models.DTOs.Uloga;
using ERP2024.Models.DTOs.Zaposleni;
using ERP2024.Models.Entities;

namespace ERP2024.Profiles
{
    public class UlogaProfile : Profile
    {
        public UlogaProfile()
        {
            CreateMap<Uloga, UlogaDto>().ReverseMap();
            CreateMap<Uloga, UlogaCreationDto>().ReverseMap();
            CreateMap<Uloga, UlogaUpdateDto>().ReverseMap();
        }
    }
}
