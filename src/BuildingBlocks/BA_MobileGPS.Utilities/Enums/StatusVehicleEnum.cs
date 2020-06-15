namespace BA_MobileGPS.Utilities
{
    /// <summary>
    /// trạng thái đổ bê tông
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// hoangdt  3/26/2019   created
    /// </Modified>
    public enum ConcreteEnum
    {
        Normal = 0,
        Mixer = 16384,
        Concreting = 32768
    }

    /// <summary>
    /// Trạng thái thẻ nhớ
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// hoangdt  3/26/2019   created
    /// </Modified>
    public enum MemoryStatusEnum
    {
        //Bình thường
        Normal = 0,

        //Không được khởi tạo
        NotInit = 1,

        //Bị mất
        Lost = 2,
    }
}