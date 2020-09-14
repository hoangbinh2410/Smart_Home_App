using AutoMapper;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RealmEntity;
using BA_MobileGPS.Models;
using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<MobileResourceRealm, MobileResourceRespone>();

            CreateMap<LanguageRealm, LanguageRespone>();

            CreateMap<DBLocalVersionRealm, DatabaseVersionsResponse>().ForMember(x => x.UpdatedDate, opt => opt.MapFrom(src => DateTimeHelper.ConvertDatetimeToDatetimeOffset(src.UpdatedDate)));

            CreateMap<ShowHideColumnReportRealm, ShowHideColumnResponse>();

            CreateMap<MenuItemRespone, HomeMenuItemViewModel>();

            CreateMap<HomeMenuItem, HomeMenuItemViewModel>();

            CreateMap<VehicleOnline, VehicleOnlineViewModel>();

            CreateMap<VehicleOnline, VehicleOnlineMessage>();

            CreateMap<VehicleOnlineMessage, VehicleOnlineViewModel>();

            if (App.AppType == AppType.VMS)
            {
                CreateMap<VehicleOnline, VMSVehicleOnlineViewModel>();

                CreateMap<MessageSOSRealm, MessageSOS>();

                CreateMap<MessageOnlineRealm, MessageOnline>();

                CreateMap<BoundaryRealm, LandmarkResponse>();

                CreateMap<SOSHistoryRealm, SOSHistory>();

                CreateMap<FishTripRealm, FishTrip>();

                CreateMap<FishTripQuantityRealm, FishTripQuantity>();
            }
        }
    }
}