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
        Task<List<PaperCategory>> GetPaperCategories();
        Task<PapersIdResponse> InsertRegistrationPaper(PaperRegistrationInsertRequest data);
        Task<PapersIdResponse> InsertInsurancePaper(PaperInsuranceInsertRequest data);
        Task<PapersIdResponse> InsertSignPaper(PaperCabSignInforRequest data);
        Task<List<InsuranceCategory>> GetInsuranceCategories(int companyID);                   
        Task<PapersIdResponse> UpdateRegistrationPaper(PaperRegistrationInsertRequest data);
        Task<PapersIdResponse> UpdateInsurancePaper(PaperInsuranceInsertRequest data);
        Task<PapersIdResponse> UpdateSignPaper(PaperCabSignInforRequest data);
        Task<List<PaperItemInfor>> GetListPaper(int companyId,int orderBy =0,int sortOrder=0);
        Task<List<PaperItemHistoryModel>> GetListPaperHistory(int companyId, int pageSize = 0, int pageIndex = 0, int orderBy = 0, int sortOrder = 0);
        Task<DateTime?> GetLastPaperDateByVehicle(int companyID, long vehicleId,PaperCategoryTypeEnum paperType);
        Task<PaperInfoDetailResponse> GetLastPaperByVehicleId(int companyID, PaperCategoryTypeEnum paperType, long vehicleId);
    }
}
