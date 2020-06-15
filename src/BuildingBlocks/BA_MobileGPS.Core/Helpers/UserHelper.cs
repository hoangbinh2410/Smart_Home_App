using BA_MobileGPS.Entities;

namespace BA_MobileGPS.Core
{
    public class UserHelper
    {
        //Người dùng Admin
        public static bool isUserAdmin(LoginResponse loginResponse)
        {
            var result = false;

            if (loginResponse.CompanyType == CompanyType.EndUser && loginResponse.UserType == UserType.CustomerAdmin)
            {
                result = true;
            }
            return result;
        }

        //Người dùng Thường
        public static bool isUserCustomer(LoginResponse loginResponse)
        {
            var result = false;

            if (loginResponse.CompanyType == CompanyType.EndUser && loginResponse.UserType == UserType.CustomerUser)
            {
                result = true;
            }
            return result;
        }

        //Đại lý
        public static bool isCompanyPartner(LoginResponse loginResponse)
        {
            var result = false;

            if (loginResponse.CompanyType == CompanyType.Partner)
            {
                result = true;
            }
            return result;
        }

        //Công ty thường
        public static bool isCompanyEndUser(CompanyType companyType)
        {
            var result = false;

            if (companyType == CompanyType.EndUser)
            {
                result = true;
            }
            return result;
        }

        //Đại lý
        public static bool isCompanyPartner(CompanyType companyType)
        {
            var result = false;

            if (companyType == CompanyType.Partner)
            {
                result = true;
            }
            return result;
        }

        //Công ty thường
        public static bool isCompanyEndUserWithPermisstion(CompanyType companyType)
        {
            var result = false;
            //1 - Đối với TK companytype = 3 có nhóm đội , không có quyền xem khách lẻ : Ẩn icon chọn công ty, hiện icon nhóm đội (petajicohn/12341234)
            //2 - Đối với TK companytype = 3 có nhóm đội , có quyền xem khách lẻ(PermissionID = 18) : Hiện icon chọn công ty, hiện icon nhóm đội(admin1002/ 12341234)
            if (companyType == CompanyType.EndUser && StaticSettings.User.Permissions.IndexOf((int)PermissionKeyNames.CompanyOddCustomer) != -1 && StaticSettings.User.SubCustomer == CustomerType.CustomerRetail)
            {
                result = true;
            }
            return result;
        }
    }
}