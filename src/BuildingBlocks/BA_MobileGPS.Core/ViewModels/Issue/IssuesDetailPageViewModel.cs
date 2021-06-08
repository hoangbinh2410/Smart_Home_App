using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities.ResponeEntity.Issues;
using BA_MobileGPS.Service;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class IssuesDetailPageViewModel : ViewModelBase
    {
        private readonly IIssueService _issueService;
        public ICommand ReloadCommand { get; private set; }

        public IssuesDetailPageViewModel(INavigationService navigationService, IIssueService issueService) : base(navigationService)
        {
            Title = "Chi tiết phản hồi";
            ReloadCommand = new DelegateCommand(Reload);
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
                    var lstStatus = new List<IssueStatusRespone>();
                    var lst = result.IssueStatus.OrderBy(x => x.DateChangeStatus).ToList();
                    bool isShowDueDate = true;
                    var modelRequest = new IssueStatusRespone()
                    {
                        DateChangeStatus = Issue.DateRequest,
                        IsShowLine = true,
                        LineColor = Color.FromHex("#A2E8FF"),
                        Status = "Gửi yêu cầu hỗ trợ"
                    };
                    if (lst != null && lst.Count > 0)
                    {
                        for (int i = 0; i < lst.Count; i++)
                        {
                            if (i == lst.Count - 1)
                            {
                                if (lst[i].DateChangeStatus < result.DueDate)
                                {
                                    lst[i].IsShowLine = true;
                                    lst[i].LineColor = Color.FromHex("#CED6E0");
                                }
                                else
                                {
                                    isShowDueDate = false;
                                    lst[i].IsShowLine = false;
                                }
                            }
                            else
                            {
                                lst[i].IsShowLine = true;
                                lst[i].LineColor = Color.FromHex("#A2E8FF");
                            }
                        }
                    }
                    else
                    {
                        modelRequest.LineColor = Color.FromHex("#CED6E0");
                    }
                    if (isShowDueDate)
                    {
                        lst.Add(new IssueStatusRespone()
                        {
                            DateChangeStatus = Issue.DueDate,
                            IsShowLine = false,
                            IsDueDate = true,
                            Status = "Lịch hẹn hoàn thành"
                        });
                    }
                    lstStatus.Add(modelRequest);
                    lstStatus.AddRange(lst);
                    ListIssue = lstStatus.ToObservableCollection();
                }
            }, showLoading: true);
        }

        private void Reload()
        {
            SafeExecute(() =>
            {
                if (Issue != null && !string.IsNullOrEmpty(Issue.IssueCode))
                {
                    GetListIssue();
                }
            });
        }

        #endregion PrivateMethod
    }
}