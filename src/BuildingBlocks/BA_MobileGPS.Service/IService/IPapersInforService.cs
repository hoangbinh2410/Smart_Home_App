using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Entities.ResponeEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.IService
{

    public interface IPapersInforService
    {
        Task<List<InsuranceCategory>> GetPaperCategories();
        Task<PapersIdResponse> InsertRegistrationPaper(PaperRegistrationInsertRequest data);
        Task<PapersIdResponse> InsertInsurancePaper(PaperInsuranceInsertRequest data);
        Task<PapersIdResponse> InsertSignPaper(PaperCabSignInsertRequest data);
        Task<List<PaperCategory>> GetInsuranceCategories(int companyID);
        Task<PaperRegistrationInsertRequest> GetLastPaperRegistrationByVehicleId(int companyID, long vehicleId);
        Task<PaperInsuranceInsertRequest> GetLastPaperInsuranceByVehicleId(int companyID, long vehicleId);
        Task<PaperCabSignInsertRequest> GetLastPaperSignByVehicleId(int companyID, long vehicleId);


    }
}
