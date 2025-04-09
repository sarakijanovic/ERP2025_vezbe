using AutoMapper;
using ERP2024.Models.DTOs.TipAparata;
using ERP2024.Models.Entities;

namespace ERP2024.Profiles
{
    public class TipAparataProfile : Profile
    {
        public TipAparataProfile()
        { 
            CreateMap<TipAparata, TipAparataDto>().ReverseMap();
            CreateMap<TipAparata, TipAparataCreationDto>().ReverseMap();
            CreateMap<TipAparata, TipAparataUpdateDto>().ReverseMap();
        }
    }
}
