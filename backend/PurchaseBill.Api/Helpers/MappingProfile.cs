using AutoMapper;
using PurchaseBill.Api.DTOs;
using PurchaseBill.Api.Entities;

namespace PurchaseBill.Api.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LocationDetail, LocationDto>();

        CreateMap<PurchaseBillItem, PurchaseBillItemResponseDto>();

        CreateMap<PurchaseBillHeader, PurchaseBillResponseDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
    }
}
