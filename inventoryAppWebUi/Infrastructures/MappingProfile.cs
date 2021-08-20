using AutoMapper;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Dtos;
using inventoryAppDomain.Entities.MonnifyDtos;
using inventoryAppDomain.Repository;
using inventoryAppWebUi.Models;

namespace inventoryAppWebUi.Infrastructures
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<EditProfileViewModel, Cashier>()
                .ForMember(Cashier => Cashier.ApplicationUser, act => act.Ignore())
                .ForMember(Cashier => Cashier.ApplicationUserId, act => act.Ignore());

            Mapper.CreateMap<EditProfileViewModel, StoreManager>()
                .ForMember(Cashier => Cashier.ApplicationUser, act => act.Ignore())
                .ForMember(Cashier => Cashier.ApplicationUserId, act => act.Ignore());

            Mapper.CreateMap<UpdateUserRoleViewModel, MockViewModel>();


            Mapper.CreateMap<SupplierViewModel, Supplier>()
               .ForMember(s => s.Status, s => s.Ignore());

             Mapper.CreateMap<Supplier, SupplierViewModel>();

            Mapper.CreateMap<ProductViewModel, Product>().ReverseMap();


            Mapper.CreateMap<ProductCategoryViewModel, ProductCategory>().ReverseMap();

            Mapper.CreateMap<EditCategoryViewModel, ProductCategory>().ReverseMap();


            Mapper.CreateMap<OrderViewModel, Order>()
                 .ForMember(order => order.OrderItems, act => act.Ignore())
                 .ForMember(order => order.Price, act => act.Ignore());

             Mapper.CreateMap<Report, ReportViewModel>()
                 .ForMember(model => model.Orders, act => act.MapFrom(report => report.Orders))
                 .ForMember(model => model.ReportProducts, act => act.MapFrom(report => report.ReportProducts))
                 .ForMember(model => model.TotalRevenueForReport, act => act.MapFrom(report => report.TotalRevenueForReport));

             Mapper.CreateMap<TransactionViewModel, TransactionDetails>()
                 .ForMember(details => details.amount, act => act.Ignore())
                 .ForMember(details => details.txRef, act => act.Ignore())
                 .ForMember(details => details.PBFPubKey, act => act.Ignore())
                 .ForMember(details => details.cvv, act => act.MapFrom(model => model.Cvv))
                 .ForMember(details => details.cardno, act => act.MapFrom(model => model.CardNumber))
                 .ForMember(details => details.email, act => act.MapFrom(model => model.Email))
                 .ForMember(details => details.expirymonth, act => act.MapFrom(model => model.ExpiryMonth))
                 .ForMember(details => details.expiryyear, act => act.MapFrom(model => model.ExpiryYear));

        }
    }
}