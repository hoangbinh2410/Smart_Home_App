using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RealmEntity;
using BA_MobileGPS.Service;

using Prism.Commands;
using Prism.Navigation;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace VMS_MobileGPS.ViewModels
{
    public class BoundaryViewModel : ViewModelBase
    {
        #region Contructor

        private readonly IVehicleOnlineService vehicleOnlineService;
        private readonly IRealmBaseService<BoundaryRealm, LandmarkResponse> baseRepository;

        public ICommand CanelCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }

        public BoundaryViewModel(INavigationService navigationService,
            IVehicleOnlineService vehicleOnlineService,
            IRealmBaseService<BoundaryRealm, LandmarkResponse> baseRepository) : base(navigationService)
        {
            this.vehicleOnlineService = vehicleOnlineService;
            this.baseRepository = baseRepository;

            SearchCommand = new DelegateCommand<TextChangedEventArgs>(SearchListBoundary);
            UpdateCommand = new Command(Update);
            CanelCommand = new DelegateCommand(Canel);
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();

            GetListLandmark();
        }

        #endregion Lifecycle

        #region Property

        private CancellationTokenSource cts;

        private ObservableCollection<LandmarkResponse> listLandmark = new ObservableCollection<LandmarkResponse>();
        public ObservableCollection<LandmarkResponse> ListLandmark { get => listLandmark; set => SetProperty(ref listLandmark, value); }

        public List<LandmarkResponse> ListLandmarkOrigin = new List<LandmarkResponse>();

        private bool hasBoundary = false;
        public bool HasBoundary { get => hasBoundary; set => SetProperty(ref hasBoundary, value); }

        #endregion Property

        #region PrivateMethod

        private void GetListLandmark()
        {
            TryExecute(() =>
            {
                //lấy từ local DB
                var list = baseRepository.All()?.ToList();
                if (list != null && list.Count > 0)
                {
                    ListLandmarkOrigin = list;
                    ListLandmark = list.ToObservableCollection();
                }
                else
                {
                    RunOnBackground(async () =>
                    {
                        return await vehicleOnlineService.GetListBoundary();
                    },
                   (respones) =>
                   {
                       if (respones != null && respones.Count > 0)
                       {
                           foreach (var item in list)
                           {
                               if (respones.Find(l => l.PK_LandmarkID == item.PK_LandmarkID) is LandmarkResponse landmark)
                               {
                                   landmark.IsShowBoudary = item.IsShowBoudary;
                                   landmark.IsShowName = item.IsShowName;
                               }

                               baseRepository.Delete(item.Id);
                           }

                           foreach (var item in respones)
                           {
                               //thêm dữ liệu vào local database với bẳng là LanmarkReal
                               baseRepository.Add(item);
                           }
                           ListLandmarkOrigin = respones;
                           ListLandmark = respones.ToObservableCollection();
                       }
                   });
                }
            });
        }

        private async void Update()
        {
            foreach (var item in baseRepository.All())
            {
                if (ListLandmark.FirstOrDefault(l => l.PK_LandmarkID == item.PK_LandmarkID) is LandmarkResponse landmark)
                {
                    item.IsShowBoudary = landmark.IsShowBoudary;
                    item.IsShowName = landmark.IsShowName;

                    baseRepository.Update(item);
                }
            }

            await NavigationService.GoBackAsync();
        }

        private void SearchListBoundary(TextChangedEventArgs args)
        {
            if (cts != null)
                cts.Cancel(true);

            cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                await Task.Delay(500, cts.Token);

                return ListLandmarkOrigin.FindAll(v => v.Name.ToUpper().Contains(args.NewTextValue.ToUpper()));
            }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    ListLandmark = task.Result.ToObservableCollection();

                    HasBoundary = ListLandmark.Count > 0;
                }
            }));
        }

        private async void Canel()
        {
            await NavigationService.GoBackAsync();
        }

        #endregion PrivateMethod
    }
}