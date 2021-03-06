using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Navigation;

using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class InsertLocalDBPageViewModel : ViewModelBase
    {
        private readonly IResourceService resourceService;
        private readonly ILanguageService languageTypeService;
        private readonly IDBVersionService dBVersionService;

        public InsertLocalDBPageViewModel(INavigationService navigationService, IResourceService resourceService,
            ILanguageService languageTypeService, IDBVersionService dBVersionService)
            : base(navigationService)
        {
            this.resourceService = resourceService;
            this.languageTypeService = languageTypeService;
            this.dBVersionService = dBVersionService;
        }

        public override void OnPageAppearingFirstTime()
        {
            GetVersionDB();
        }

        private void GetVersionDB()
        {
            RunOnBackground(async () =>
            {
                return await dBVersionService.GetVersionDataBase((int)App.AppType);
            },
            async (response) =>
            {
                if (response != null && response.Count > 0)
                {
                    // get db local
                    var dbLocal = dBVersionService.All();

                    foreach (var item in response)
                    {
                        var successConvert = Enum.TryParse<LocalDBNames>(item.TableName.ToUpper(), out var tableName);
                        if (successConvert)
                        {
                            var result = false;
                            switch (tableName)
                            {
                                case LocalDBNames.LANGUAGERESOURCES:  // update lại bảng mobile resources
                                    if (dbLocal != null && dbLocal.Count() > 0)
                                    {
                                        var lastUpdateDBLocal = dbLocal.Where(x => x.TableName == item.TableName).FirstOrDefault();

                                        // khác version và có thời gian update lớn hơn thời gian lưu trong db thì mới cập nhật
                                        if (lastUpdateDBLocal != null && lastUpdateDBLocal.UpdatedDate <= item.UpdatedDate && item.VersionDB != lastUpdateDBLocal.VersionDB)
                                        {
                                            result = await GetResourceLanguage(lastUpdateDBLocal.UpdatedDate);
                                            if (result)
                                            {
                                                // update vào bảng local db
                                                lastUpdateDBLocal.VersionDB = item.VersionDB;
                                                lastUpdateDBLocal.UpdatedDate = DateTime.Now;
                                                dBVersionService.Update(lastUpdateDBLocal);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        result = await GetResourceLanguage(null);
                                        if (result)
                                        {
                                            // thêm mới vào bảng local db
                                            item.UpdatedDate = DateTime.Now;
                                            dBVersionService.Add(item);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }

                Device.BeginInvokeOnMainThread(async () =>
                {
                    Settings.IsChangeDataLocalDB = false;
                    //Đánh dấu là đã cài app 1 lần rồi
                    Settings.IsFistInstallApp = true;

                    _ = await NavigationService.NavigateAsync("/LoginPage");
                });
            });
        }

        private async Task<bool> GetResourceLanguage(DateTime? LastUpdateDate)
        {
            var result = false;
            try
            {
                var unixTime = LastUpdateDate == null ? (long?)null : ((DateTimeOffset)LastUpdateDate).ToUnixTimeMilliseconds();
                var lstResource = await resourceService.GetAllResources((int)App.AppType, "", unixTime);
                if (lstResource != null && lstResource.Count > 0)
                {
                    foreach (var item in lstResource)
                    {
                        // nếu chưa get về lần nào thì mặc định thêm mới
                        if (unixTime != null)
                        {
                            var model = resourceService.Get(r => r.Name == item.Name && r.FK_LanguageID == item.FK_LanguageID);
                            if (model != null)
                            {
                                if (model.Value != item.Value)
                                {
                                    model.Value = item.Value;
                                    var temp = resourceService.Update(model);
                                }
                            }
                            else
                            {
                                // Thêm resouce vào realm db
                                resourceService.Add(item);
                            }
                        }
                        else
                        {
                            resourceService.Add(item);
                        }
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }
    }
}