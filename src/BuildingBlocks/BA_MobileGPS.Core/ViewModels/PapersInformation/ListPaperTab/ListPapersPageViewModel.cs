using BA_MobileGPS.Service.IService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ListPapersPageViewModel : ViewModelBase
    {
        public ICommand GotoAddPaperPageCommand { get; } 
        private readonly IPapersInforService paperinforService;
        public ListPapersPageViewModel(INavigationService navigationService, IPapersInforService paperinforService) : base(navigationService)
        {
            this.paperinforService = paperinforService;
            GotoAddPaperPageCommand = new DelegateCommand(GotoAddPaperPage);
            CheckUserPermission();
        }


        private void GotoAddPaperPage()
        {
            SafeExecute(async () =>
            {
                var a = await NavigationService.NavigateAsync("NavigationPage/AddPaperInfoPage", null, true, true);
            });
        }

        private bool insertVisible;
        public bool InsertVisible
        {
            get { return insertVisible; }
            set { SetProperty(ref insertVisible, value); }
        }

        private void CheckUserPermission()
        {
            InsertVisible = true;
            //var userPer = UserInfo.Permissions.Distinct();
            //var insertPer = (int)PermissionKeyNames.AdminEmployeeAdd;
            //// var updatePer = (int)PermissionKeyNames.AdminEmployeeUpdate;
            //var deletePer = (int)PermissionKeyNames.AdminEmployeeDelete;
            //if (userPer.Contains(insertPer))
            //{
            //    InsertVisible = true;
            //}
        }
    }
}
