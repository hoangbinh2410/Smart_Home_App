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
        }

        #region Property

        private IssuesRespone issue;
        public IssuesRespone Issue { get => issue; set => SetProperty(ref issue, value); }

        private ObservableCollection<IssueStatusRespone> listIssue = new ObservableCollection<IssueStatusRespone>();
        public ObservableCollection<IssueStatusRespone> ListIssue { get => listIssue; set => SetProperty(ref listIssue, value); }

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
            else if (parameters.ContainsKey(ParameterKey.IssuesKey) && parameters.TryGetValue(ParameterKey.IssuesKey, out string issuecode))
            {
                Issue = new IssuesRespone();
                Issue.IssueCode = issuecode;
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
                if (result != null)
                {
                    Issue.ContentRequest = result.ContentRequest;
                    Issue.DateRequest = result.DateRequest;
                    Issue.DueDate = result.DueDate;
                    Issue.IssueCode = result.IssueCode;
                    var lst = result.IssueStatus.OrderBy(x => x.DateChangeStatus).ToList();
                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (i == lst.Count - 1)
                        {
                            lst[i].IsLastItem = true;
                        }
                        else
                        {
                            lst[i].IsLastItem = false;
                        }
                    }
                    ListIssue = lst.ToObservableCollection();
                }
            });
        }

        #endregion PrivateMethod
    }
}