using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities.ResponeEntity.Issues;
using BA_MobileGPS.Service;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class IssuesDetailPageViewModel : ViewModelBase
    {
        private readonly IIssueService _issueService;

        public IssuesDetailPageViewModel(INavigationService navigationService, IIssueService issueService) : base(navigationService)
        {
            Title = "Danh sách yêu cầu hỗ trợ";
            _issueService = issueService;
            isNotFinish = false;
        }

        #region Property

        private IssuesRespone issue;
        public IssuesRespone Issue { get => issue; set => SetProperty(ref issue, value); }

        private ObservableCollection<IssuesDetailRespone> listIssue = new ObservableCollection<IssuesDetailRespone>();
        public ObservableCollection<IssuesDetailRespone> ListIssue { get => listIssue; set => SetProperty(ref listIssue, value); }

        private bool isNotFinish;
        public bool IsNotFinish { get => isNotFinish; set => SetProperty(ref isNotFinish, value); }

        #endregion Property

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            if (parameters.ContainsKey(ParameterKey.IssuesKey) && parameters.TryGetValue(ParameterKey.IssuesKey, out IssuesRespone issue))
            {
                Issue = issue;
                GetListIssue();
            }
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
            RunOnBackground(async () =>
            {
                return await _issueService.GetIssueByIssueCode(Issue.IssueCode);
            }, (result) =>
            {
                if (result != null && result.Count > 0)
                {
                    var isFinish = result.FirstOrDefault(x => x.Status == Entities.Enums.IssuesStatusEnums.Finish);
                    if (isFinish == null)
                    {
                        IsNotFinish = true;
                    }
                    for (int i = 0; i < result.Count; i++)
                    {
                        var lastitem = result.Last();
                        if (lastitem.Status != Entities.Enums.IssuesStatusEnums.Finish)
                        {
                            result[i].IsFinishStep = false;
                        }
                        else
                        {
                            result[i].IsFinishStep = true;
                        }
                    }
                    ListIssue = result.OrderBy(x => x.CreatedDate).ToObservableCollection();
                }
            });
        }

        #endregion PrivateMethod
    }
}