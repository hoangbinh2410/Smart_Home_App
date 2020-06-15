using AutoMapper;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RealmEntity;
using BA_MobileGPS.Models;
using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<MobileResourceRespone, MobileResourceRealm>();

            CreateMap<LanguageRespone, LanguageRealm>();

            CreateMap<DatabaseVersionsResponse, DBLocalVersionRealm>().ForMember(x => x.UpdatedDate, opt => opt.MapFrom(src => DateTimeHelper.ConvertDatetimeOffsetToDatetime(src.UpdatedDate)));

            CreateMap<ShowHideColumnResponse, ShowHideColumnReportRealm>();

            CreateMap<HomeMenuItemViewModel, HomeMenuItem>();

            CreateMap<VehicleOnlineViewModel, VehicleOnline>();

            CreateMap<VehicleOnlineMessage, VehicleOnline>();

            CreateMap<VehicleOnlineViewModel, VehicleOnlineMessage>();

            CreateMap<HelperAdvance, HelperAdvanceRealm>();

            if (App.AppType == AppType.VMS)
            {
                CreateMap<VMSVehicleOnlineViewModel, VehicleOnline>();

                CreateMap<MessageSOS, MessageSOSRealm>();

                CreateMap<MessageOnline, MessageOnlineRealm>();

                CreateMap<LandmarkResponse, BoundaryRealm>();

                CreateMap<SOSHistory, SOSHistoryRealm>();

                CreateMap<FishTrip, FishTripRealm>();

                CreateMap<FishTripQuantity, FishTripQuantityRealm>();
            }       
        }
    }
}