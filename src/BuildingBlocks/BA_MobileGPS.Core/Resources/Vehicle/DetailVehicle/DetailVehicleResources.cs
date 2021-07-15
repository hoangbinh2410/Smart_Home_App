using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    /// <summary>
    /// Resource cho trang chi tiết xe
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// hoangdt  2/22/2019   created
    /// </Modified>
    public partial class MobileResource
    {
        public static string DetailVehicle_Label_TilePage => Get(MobileResourceNames.DetailVehicle_Label_TilePage, "Chi tiết xe", "Details");
        public static string DetailVehicleVMS_Label_TilePage => Get(MobileResourceNames.DetailVehicleVMS_Label_TilePage, "Thông tin tàu cá", "Fishing Boat Information");
        public static string DetailVehicle_Label_InforFee => Get(MobileResourceNames.DetailVehicle_Label_InforFee, "Thông tin phí", "Information Fee");
        public static string DetailVehicle_MessageFee => Get(MobileResourceNames.DetailVehicle_MessageFee, "Phương tiện sắp đến hạn thu phí ngày {0}", "The car is about to mature on a daily basis {0}");
        public static string DetailVehicle_Label_TileInforVehicle => Get(MobileResourceNames.DetailVehicle_Label_TileInforVehicle, "Thông tin chung", "Common information");
        public static string DetailVehicle_Label_VehiclePlate => Get(MobileResourceNames.DetailVehicle_Label_VehiclePlate, "Biển kiểm soát", "Vehicle Plate");
        public static string DetailVehicle_Label_Address => Get(MobileResourceNames.DetailVehicle_Label_Address, "Địa chỉ", "Address");
        public static string DetailVehicle_Label_Coordinates => Get(MobileResourceNames.DetailVehicle_Label_Coordinates, "Tọa độ", "Coordinates");
        public static string DetailVehicle_Label_Time => Get(MobileResourceNames.DetailVehicle_Label_Time, "Thời gian", "Time");
        public static string DetailVehicle_Label_Speed => Get(MobileResourceNames.DetailVehicle_Label_Speed, "Vận tốc", "Speed");
        public static string DetailVehicle_Label_Engineer => Get(MobileResourceNames.DetailVehicle_Label_Engineer, "Động cơ", "Engine speed");
        public static string DetailVehicle_Label_KilometInToDay => Get(MobileResourceNames.DetailVehicle_Label_KilometInToDay, "KM trong ngày", "Traveled distance today");
        public static string DetailVehicle_Label_KilometInMonth => Get(MobileResourceNames.DetailVehicle_Label_KilometInMonth, "KM trong tháng", "Traveled this month");
        public static string DetailVehicle_Label_KilometAccumulated => Get(MobileResourceNames.DetailVehicle_Label_KilometAccumulated, "KM tích luỹ", "KM accumulated");
        public static string DetailVehicle_Label_CountStopVehicle => Get(MobileResourceNames.DetailVehicle_Label_CountStopVehicle, "Dừng đỗ", "Stoping");
        public static string DetailVehicle_Label_ParkingVehicleNow => Get(MobileResourceNames.DetailVehicle_Label_ParkingVehicleNow, "Đang đỗ", "Parking");
        public static string DetailVehicle_Label_ParkingTurnOnVehecle => Get(MobileResourceNames.DetailVehicle_Label_ParkingTurnOnVehecle, "Dừng đỗ nổ máy", "Parking TurnOn");
        public static string DetailVehicle_Label_AirCondition => Get(MobileResourceNames.DetailVehicle_Label_AirCondition, "Điều hoà", "Air conditioner");

        public static string DetailVehicle_Label_CountOpenDoor => Get(MobileResourceNames.DetailVehicle_Label_CountOpenDoor, "Số lần mở cửa", "Count Open Door");
        public static string DetailVehicle_Label_Crane => Get(MobileResourceNames.DetailVehicle_Label_Crane, "Cẩu", "Crane");
        public static string DetailVehicle_Label_Ben => Get(MobileResourceNames.DetailVehicle_Label_Ben, "Ben", "Ben");
        public static string DetailVehicle_Label_Ben_Crane => Get(MobileResourceNames.DetailVehicle_Label_Ben, "Ben/Cẩu", "Ben/Crane");

        public static string DetailVehicle_Label_Door => Get(MobileResourceNames.DetailVehicle_Label_Door, "Cửa", "Door");
        public static string DetailVehicle_Label_Temperature => Get(MobileResourceNames.DetailVehicle_Label_Temperature, "Nhiệt độ", "Temperature");
        public static string DetailVehicle_Label_Fuel => Get(MobileResourceNames.DetailVehicle_Label_Fuel, "Nhiên liệu", "Fuel");
        public static string DetailVehicle_Label_Concrete => Get(MobileResourceNames.DetailVehicle_Label_Concrete, "Bê tông", "Concrete");
        public static string DetailVehicle_Label_MemoryStick => Get(MobileResourceNames.DetailVehicle_Label_MemoryStick, "Thẻ nhớ", "Memory status");
        public static string DetailVehicle_Label_SpeedOverCount => Get(MobileResourceNames.DetailVehicle_Label_SpeedOverCount, "Quá tốc độ", "Overspeed");
        public static string DetailVehicle_Label_MinutesOfDrivingTimeContinuous => Get(MobileResourceNames.DetailVehicle_Label_MinutesOfDrivingTimeContinuous, "Thời gian lái xe liên tục", "Continuous driving time");
        public static string DetailVehicle_Label_MinutesOfDrivingTimeInDay => Get(MobileResourceNames.DetailVehicle_Label_MinutesOfDrivingTimeInDay, "Thời gian lái xe trong ngày", "Driving time today");
        public static string DetailVehicle_Label_InforBGT => Get(MobileResourceNames.DetailVehicle_Label_InforBGT, "Thông tin BGT", "BGT information");

        public static string DetailVehicle_Label_InforSIM => Get(MobileResourceNames.DetailVehicle_Label_InforSIM, "Thông tin SIM", "SIM information");
        public static string DetailVehicle_Label_DriverName => Get(MobileResourceNames.DetailVehicle_Label_DriverName, "Lái xe", "Driver name");
        public static string DetailVehicle_Label_DriverLicense => Get(MobileResourceNames.DetailVehicle_Label_DriverLicense, "Bằng lái", "Driving license");
        public static string DetailVehicle_Label_DriverPhone => Get(MobileResourceNames.DetailVehicle_Label_DriverPhone, "Điện thoại", "Phone");
        public static string DetailVehicle_Label_DepartmentOfManagement => Get(MobileResourceNames.DetailVehicle_Label_DepartmentOfManagement, "Sở quản lý", "Managed by");

        public static string DetailVehicle_Label_ValueMemoryStick_Normal => Get(MobileResourceNames.DetailVehicle_Label_ValueMemoryStick_Normal, "Bình thường", "Normal");
        public static string DetailVehicle_Label_ValueMemoryStick_NotInit => Get(MobileResourceNames.DetailVehicle_Label_ValueMemoryStick_NotInit, "Không được khởi tạo", "Not initialized");
        public static string DetailVehicle_Label_ValueMemoryStick_Lost => Get(MobileResourceNames.DetailVehicle_Label_ValueMemoryStick_Lost, "Bị mất", "Lost");

        public static string DetailVehicle_Label_ValueConcrete_Normal => Get(MobileResourceNames.DetailVehicle_Label_ValueConcrete_Normal, "Dừng bê tông", "Stop");
        public static string DetailVehicle_Label_ValueConcrete_Mixer => Get(MobileResourceNames.DetailVehicle_Label_ValueConcrete_Mixer, "Trộn bê tông", "Mixer");
        public static string DetailVehicle_Label_ValueConcrete_Concreting => Get(MobileResourceNames.DetailVehicle_Label_ValueConcrete_Concreting, "Xả bê tông", "Concreting");

        public static string DetailVehicle_Label_SimPhoneNumber => Get(MobileResourceNames.DetailVehicle_Label_SimPhoneNumber, "Số SIM", "Sim Number");

        public static string DetailVehicle_Label_JoinSystemDate => Get(MobileResourceNames.DetailVehicle_Label_JoinSystemDate, "Ngày đăng ký", "Join System Date");

        public static string DetailVehicle_Label_Vehicle_Expiration_Date => Get(MobileResourceNames.DetailVehicle_Label_Vehicle_Expiration_Date, "Ngày hết hạn", "Expiration date");

        public static string DetailVehicle_Label_Vehicle_Sim_Surplus => Get(MobileResourceNames.DetailVehicle_Label_Vehicle_Sim_Surplus, "Số dư", "Surplus");

        public static string DetailVehicle_Label_Vehicle_Status_Current => Get(MobileResourceNames.DetailVehicle_Label_Vehicle_Status_Current, "Trạng thái xe hiện tại", "Current vehicle status");

        public static string DetailVehicle_Label_ExpirationDate => Get(MobileResourceNames.DetailVehicle_Label_ExpirationDate, "Ngày hết hạn phí", "Expiration Date");

        public static string DetailVehicle_Label_Status => Get(MobileResourceNames.DetailVehicle_Label_Status, "Hiện trạng", "Status");
        public static string DetailVehicle_Label_Packageinformation => Get(MobileResourceNames.DetailVehicle_Label_Packageinformation, "Thông tin gói cước", "Package information");
        public static string DetailVehicle_Label_PackageName => Get(MobileResourceNames.DetailVehicle_Label_PackageName, "Tên gói cước", "Package name");
        public static string DetailVehicle_Label_Provider => Get(MobileResourceNames.DetailVehicle_Label_Provider, "Nhà mạng", "Provider");
        public static string DetailVehicle_Label_Packagecapacity => Get(MobileResourceNames.DetailVehicle_Label_Packagecapacity, "Dung lượng gói cước", "Package capacity");
        public static string DetailVehicle_Label_DaysOfStorage => Get(MobileResourceNames.DetailVehicle_Label_DaysOfStorage, "Số ngày lưu trữ", "Number of days of storage");
        public static string DetailVehicle_Label_ChannelOfStorage => Get(MobileResourceNames.DetailVehicle_Label_ChannelOfStorage, "Số kênh lưu trữ", "Number of storage channels");
        public static string DetailVehicle_Label_Positioning => Get(MobileResourceNames.DetailVehicle_Label_Positioning, "Tính năng định vị", "Positioning feature");
        public static string DetailVehicle_Label_Video=> Get(MobileResourceNames.DetailVehicle_Label_Video, "Tính năng video", "Video features");
        public static string DetailVehicle_Label_Photo => Get(MobileResourceNames.DetailVehicle_Label_Photo, "Tính năng ảnh", "Photo features");
        public static string DetailVehicle_Label_DeviceInformation => Get(MobileResourceNames.DetailVehicle_Label_DeviceInformation, "Thông tin thiết bị", "Device Information");
        public static string DetailVehicle_Label_ChannelHasCamera => Get(MobileResourceNames.DetailVehicle_Label_ChannelHasCamera, "Kênh lắp camera :", "Camera mounting channel");
        public static string DetailVehicle_Label_ActiveChannel => Get(MobileResourceNames.DetailVehicle_Label_ActiveChannel, "Kênh hoạt động :", "Active channel");
        public static string DetailVehicle_Label_DriveCapacity => Get(MobileResourceNames.DetailVehicle_Label_DriveCapacity, "Dung lượng ổ :", "Drive capacity");
        public static string DetailVehicle_Label_Network => Get(MobileResourceNames.DetailVehicle_Label_Network, "Mạng kết nối :", "Network connection");
    }
}