using AutoMapper;
using BackendForFrontend.Models.EFModels;
using MinimalAPIs.Models.DTOs;

namespace MinimalAPIs
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Coupon, CouponCreateDTO>().ReverseMap();
            CreateMap<Coupon, CouponDTO>()
                .ForMember(dest => dest.CouponID, opt => opt.MapFrom(src => src.CouponID))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Valid, opt => opt.MapFrom(src => src.Valid))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.AvailabilityCount, opt => opt.MapFrom(src => src.AvailabilityCount))
                .ForMember(dest => dest.MinimumValue, opt => opt.MapFrom(src => src.MinimumValue))
                .ForMember(dest => dest.DiscountValue, opt => opt.MapFrom(src => src.DiscountValue))
                .ForMember(dest => dest.DiscountLimit, opt => opt.MapFrom(src => src.DiscountLimit))
                .ReverseMap();
        }
    }
}