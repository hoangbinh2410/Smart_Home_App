namespace BA_MobileGPS.Entities
{
    /// <summary>
    /// [dbo].[XCV.MobileVersions]
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// Namth  13/1/2018   created
    /// </Modified>
    public class MobileVersionModel
    {
        /// <summary>
        /// Loại App (KH, LX)
        /// </summary>
        public string AppType { get; set; }

        /// <summary>
        /// OperatingSystem (Hệ điều hành gì)
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        /// Tên phiên bản
        /// </summary>
        public string VersionName { get; set; }

        /// <summary>
        /// Số phiên bản
        /// </summary>
        public int VersionNumber { get; set; }

        public bool IsMustUpdate { get; set; }

        /// <summary>
        /// Link download
        /// </summary>
        public string LinkDownload { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
    }
}