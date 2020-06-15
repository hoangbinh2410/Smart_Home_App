using System.ComponentModel;

namespace BA_MobileGPS.Utilities.Enums
{
    /// <summary>
    /// Loại cập nhật verion
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// Namth  29/1/2018   created
    /// </Modified>
    public enum VersionNumberTypes
    {
        [Description("Khi có bản cập nhật nhỏ, chỉ thông báo.")]
        AlertOnly = 1,

        [Description("Khi có bản cập nhật lớn, ép phải cài lại phần mềm.")]
        ReinstallRequired = 2,
    }
}