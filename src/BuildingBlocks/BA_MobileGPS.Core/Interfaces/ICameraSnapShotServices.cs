namespace BA_MobileGPS.Core.Interfaces
{
    public interface ICameraSnapShotServices
    {
        string GetFolderPath();

        bool SaveSnapShotToGalery(string oldPath);
    }
}