using AutoMapper;
using ERP2024.Models.DTOs.AparatZaVodu;
using ERP2024.Models.Entities;

namespace ERP2024.Profiles
{
    public class AparatZaVoduProfile : Profile
    {
        public AparatZaVoduProfile() 
        {
            CreateMap<AparatZaVodu, AparatZaVoduDto>().ReverseMap();
            CreateMap<AparatZaVodu, AparatZaVoduCreationDto>().ReverseMap();
            CreateMap<AparatZaVodu, AparatZaVoduUpdateDto>().ReverseMap();
        }
    }
}
