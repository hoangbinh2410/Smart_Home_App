using System.Linq;

namespace BA_MobileGPS.Core
{
    public class FavoritesVehicleImageHelper
    {
        public static void UpdateFavoritesVehicleImage(string VehiclePlate)
        {
            if (Settings.FavoritesVehicleImage == string.Empty) // lần đầu
            {
                Settings.FavoritesVehicleImage = VehiclePlate;
            }
            else
            {
                var split = Settings.FavoritesVehicleImage.Split(',');

                string[] temp = new string[split.Length];

                var splitVehicle = split.Where(x => x == VehiclePlate).ToArray();

                if (splitVehicle.Length > 0) // đã có trong list thì xóa đi
                {
                    split = split.Where(x => x != VehiclePlate).ToArray();

                    temp = new string[split.Length];

                    for (int i = 0; i < split.Length; i++)
                    {
                        temp[i] = split[i];
                    }
                }
                else
                {
                    temp = new string[split.Length + 1];

                    for (int i = 0; i < split.Length; i++)
                    {
                        temp[i] = split[i];
                    }

                    temp[split.Length] = VehiclePlate;
                }

                Settings.FavoritesVehicleImage = string.Join(",", temp);
            }
        }
    }
}
