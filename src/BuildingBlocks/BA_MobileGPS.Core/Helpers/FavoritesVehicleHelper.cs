using System;
using System.Linq;

namespace BA_MobileGPS.Core
{
    public class FavoritesVehicleHelper
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

        public static void UpdateFavoritesVehicleOnline(string vehiclePlate)
        {
            if (Settings.FavoritesVehicleOnline == string.Empty) // lần đầu
            {
                Settings.FavoritesVehicleOnline = vehiclePlate.ToString();
            }
            else
            {
                var split = Settings.FavoritesVehicleOnline.Split(',');

                string[] temp = new string[split.Length];

                var splitVehicle = split.Where(x => x == vehiclePlate).ToArray();

                if (splitVehicle.Length > 0) // đã có trong list thì xóa đi
                {
                    split = split.Where(x => x != vehiclePlate).ToArray();

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

                    temp[split.Length] = vehiclePlate;
                }

                Settings.FavoritesVehicleOnline = string.Join(",", temp);
            }
        }

        public static bool IsFavoritesVehicleOnline(string vehiclePlate)
        {
            if (string.IsNullOrEmpty(Settings.FavoritesVehicleOnline))
            {
                return false;
            }
            else
            {
                var split = Settings.FavoritesVehicleOnline.Split(',');
                var splitVehicle = split.Where(x => x == vehiclePlate).ToArray();
                if (splitVehicle.Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static void UpdateFavoritesIssue(Guid id)
        {
            if (Settings.FavoritesIssue == string.Empty) // lần đầu
            {
                Settings.FavoritesIssue = id.ToString();
            }
            else
            {
                var split = Settings.FavoritesIssue.Split(',');

                string[] temp = new string[split.Length];

                var splitVehicle = split.Where(x => x == id.ToString()).ToArray();

                if (splitVehicle.Length > 0) // đã có trong list thì xóa đi
                {
                    split = split.Where(x => x != id.ToString()).ToArray();

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

                    temp[split.Length] = id.ToString();
                }

                Settings.FavoritesIssue = string.Join(",", temp);
            }
        }

        public static bool IsFavoritesIssue(Guid id)
        {
            if (string.IsNullOrEmpty(Settings.FavoritesIssue))
            {
                return false;
            }
            else
            {
                var split = Settings.FavoritesIssue.Split(',');
                var splitVehicle = split.Where(x => x == id.ToString()).ToArray();
                if (splitVehicle.Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}