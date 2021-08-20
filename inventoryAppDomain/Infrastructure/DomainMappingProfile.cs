using System.Globalization;
using AutoMapper;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Dtos;
using inventoryAppDomain.Entities.MonnifyDtos;

namespace inventoryAppDomain.Infrastructure
{
    public class DomainMappingProfile : Profile
    {
        public DomainMappingProfile()
        {
            Mapper.CreateMap<TransactionDetails, FinalRequestPayload>()
                .ForMember(details => details.pin, act => act.Ignore())
                .ForMember(details => details.suggested_auth, act => act.Ignore());

            Mapper.CreateMap<Order, TransactionPayload>()
                .ForMember(payload => payload.amount,
                    act => act.MapFrom(order => order.Price))
                .ForMember(payload => payload.customerEmail, act => act.MapFrom(order => order.Email))
                .ForMember(payload => payload.customerName,
                    act => act.MapFrom(order => $"{order.FirstName} {order.LastName}"));

            Mapper.CreateMap<object, ResponseDto>();
        }
    }
}