using BA_MobileGPS.Entities.ResponeEntity.Issues;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class IssuesDetailPageViewModel : ViewModelBase
    {
        public IssuesDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Danh sách yêu cầu hỗ trợ";
            isNotFinish = false;
        }

        #region Property

        private ObservableCollection<IssuesDetailRespone> listIssue = new ObservableCollection<IssuesDetailRespone>();
        public ObservableCollection<IssuesDetailRespone> ListIssue { get => listIssue; set => SetProperty(ref listIssue, value); }

        private bool isNotFinish;
        public bool IsNotFinish { get => isNotFinish; set => SetProperty(ref isNotFinish, value); }

        #endregion Property

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            GetListIssue();
        }

        public override void OnDestroy()
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        #endregion Lifecycle

        #region PrivateMethod

        private void GetListIssue()
        {
            var lst = new List<IssuesDetailRespone>();
            lst.Add(new IssuesDetailRespone()
            {
                Id = new Guid("A2B22DF4-88FA-4AF6-A05F-F0FACC43CAA0"),
                DueDate = DateTime.Now,
                Status = Entities.Enums.IssuesStatusEnums.SendRequestIssue,
                IssueCode = "",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                IsFinishStep = true
            });
            lst.Add(new IssuesDetailRespone()
            {
                Id = new Guid("A2B22DF4-88FA-4AF6-A05F-F0FACC43CAA1"),
                DueDate = DateTime.Now,
                Status = Entities.Enums.IssuesStatusEnums.CSKHInReceived,
                IssueCode = "",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                IsFinishStep = false
            });
            //lst.Add(new IssuesDetailRespone()
            //{
            //    Id = new Guid("A2B22DF4-88FA-4AF6-A05F-F0FACC43CAA3"),
            //    DueDate = DateTime.Now,
            //    Status = Entities.Enums.IssuesStatusEnums.EngineeringIsInprogress,
            //    IssueCode = "",
            //    CreatedDate = DateTime.Now,
            //    UpdatedDate = DateTime.Now,
            //    IsFinishStep = false
            //});
            //lst.Add(new IssuesDetailRespone()
            //{
            //    Id = new Guid("A2B22DF4-88FA-4AF6-A05F-F0FACC43CAA4"),
            //    DueDate = DateTime.Now,
            //    Status = Entities.Enums.IssuesStatusEnums.Finish,
            //    IssueCode = "",
            //    CreatedDate = DateTime.Now,
            //    UpdatedDate = DateTime.Now,
            //    IsFinishStep = false
            //});

            var isFinish = lst.FirstOrDefault(x => x.Status == Entities.Enums.IssuesStatusEnums.Finish);
            if (isFinish == null)
            {
                IsNotFinish = true;
            }

            ListIssue = lst.ToObservableCollection();
        }

        #endregion PrivateMethod
    }
}