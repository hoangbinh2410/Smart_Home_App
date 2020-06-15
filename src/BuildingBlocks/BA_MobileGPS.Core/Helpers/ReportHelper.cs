using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using System;

namespace BA_MobileGPS.Core
{
    public static class ReportHelper
    {
        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  07/05/2020   created
        /// </Modified>
        public static string GetFileName(string name)
        {
            name = StringHelper.ConvertToUnderlinedVn(name);
            string userName = StaticSettings.User.UserName;
            string strFileExcel = $"{name}_{userName}_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.xlsx";
            return strFileExcel;
        }

    }
}