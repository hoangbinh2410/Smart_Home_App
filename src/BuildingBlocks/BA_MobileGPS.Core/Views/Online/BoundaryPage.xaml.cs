using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using Syncfusion.XForms.Buttons;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BoundaryPage : ContentPage
    {
        public BoundaryPage()
        {
            InitializeComponent();

            ckbAllTitleCompany.StateChanged += CkbAllTitleCompany_StateChanged;
            ckbAllBoudaryCompany.StateChanged += CkbAllBoudaryCompany_StateChanged;
            ckbAllNameCompany.StateChanged += CkbAllNameCompany_StateChanged;

            ckbAllTitle.StateChanged += CkbAllTitle_StateChanged;
            ckbAllBoudary.StateChanged += CkbAllBoudary_StateChanged;
            ckbAllName.StateChanged += CkbAllName_StateChanged;
            btnView.Text = MobileResource.Common_Button_View;
        }

        private void CkbAllTitleCompany_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (!e.IsChecked.GetValueOrDefault())
            {
                ckbAllBoudaryCompany.IsChecked = false;
                ckbAllBoudaryCompany.IsEnabled = false;

                ckbAllNameCompany.IsChecked = false;
                ckbAllNameCompany.IsEnabled = false;
            }
            else
            {
                ckbAllBoudaryCompany.IsEnabled = true;
                ckbAllNameCompany.IsEnabled = true;
            }

            foreach (var item in ListLandmarkGroup.Children)
            {
                if (item.FindByName("ckbTitleCompany") is SfCheckBox ckbTitleCompany && item.BindingContext is UserLandmarkGroupRespone landmark)
                {
                    ckbTitleCompany.StateChanged -= CkbTitleCompany_StateChanged;

                    landmark.IsVisible = ckbAllTitleCompany.IsChecked.Value;

                    landmark.IsDisplayName = ckbAllTitleCompany.IsChecked == true ? landmark.IsDisplayName : false;

                    landmark.IsDisplayBound = ckbAllTitleCompany.IsChecked == true ? landmark.IsDisplayBound : false;

                    ckbTitleCompany.StateChanged += CkbTitleCompany_StateChanged;
                }
            }
        }

        private void CkbTitleCompany_StateChanged(object sender, StateChangedEventArgs e)
        {
            ckbAllTitleCompany.StateChanged -= CkbAllTitleCompany_StateChanged;

            if (BindingContext is BoundaryViewModel viewModel && viewModel.ListLandmarkGroup.Count > 0)
            {
                var check = viewModel.ListLandmarkGroup.All(l => l.IsVisible);

                ckbAllTitleCompany.IsChecked = check;
                ckbAllBoudaryCompany.IsEnabled = check;
                ckbAllNameCompany.IsEnabled = check;

                if (!e.IsChecked.GetValueOrDefault())
                {
                    var respones = viewModel.ListLandmarkGroup.Where(l => !l.IsVisible).ToList();

                    foreach (var item in viewModel.ListLandmarkGroup)
                    {
                        if (respones.Find(l => l.PK_LandmarksGroupID == item.PK_LandmarksGroupID) is UserLandmarkGroupRespone landmark)
                        {
                            landmark.IsDisplayName = false;
                            landmark.IsDisplayBound = false;
                        }
                    }
                }
            }
            else
            {
                ckbAllTitleCompany.IsChecked = false;
            }

            ckbAllTitleCompany.StateChanged += CkbAllTitleCompany_StateChanged;
        }

        private void CkbAllNameCompany_StateChanged(object sender, StateChangedEventArgs e)
        {
            foreach (var item in ListLandmarkGroup.Children)
            {
                if (item.FindByName("ckbNameCompany") is SfCheckBox ckbNameCompany && item.BindingContext is UserLandmarkGroupRespone landmark)
                {
                    ckbNameCompany.StateChanged -= CkbNameCompany_StateChanged;

                    landmark.IsDisplayName = ckbAllNameCompany.IsChecked.Value;

                    ckbNameCompany.StateChanged += CkbNameCompany_StateChanged;
                }
            }
        }

        private void CkbNameCompany_StateChanged(object sender, StateChangedEventArgs e)
        {
            ckbAllNameCompany.StateChanged -= CkbAllNameCompany_StateChanged;

            if (BindingContext is BoundaryViewModel viewModel && viewModel.ListLandmarkGroup.Count > 0)
            {
                var check = viewModel.ListLandmarkGroup.All(l => l.IsDisplayName);
                ckbAllNameCompany.IsChecked = check;
                ckbAllNameCompany.IsEnabled = ckbAllTitleCompany.IsChecked == true ? true : check;
            }
            else
            {
                ckbAllNameCompany.IsChecked = false;
                ckbAllNameCompany.IsEnabled = false;
            }

            ckbAllNameCompany.StateChanged += CkbAllNameCompany_StateChanged;
        }

        private void CkbAllBoudaryCompany_StateChanged(object sender, StateChangedEventArgs e)
        {
            foreach (var item in ListLandmarkGroup.Children)
            {
                if (item.FindByName("ckbBoudaryCompany") is SfCheckBox ckbBoudaryCompany && item.BindingContext is UserLandmarkGroupRespone landmark)
                {
                    ckbBoudaryCompany.StateChanged -= CkbBoudaryCompany_StateChanged;

                    landmark.IsDisplayBound = ckbAllBoudaryCompany.IsChecked.Value;

                    ckbBoudaryCompany.StateChanged += CkbBoudaryCompany_StateChanged;
                }
            }
        }

        private void CkbBoudaryCompany_StateChanged(object sender, StateChangedEventArgs e)
        {
            ckbAllBoudaryCompany.StateChanged -= CkbAllBoudaryCompany_StateChanged;

            if (BindingContext is BoundaryViewModel viewModel && viewModel.ListLandmarkGroup.Count > 0)
            {
                var check = viewModel.ListLandmarkGroup.All(l => l.IsDisplayBound);
                ckbAllBoudaryCompany.IsChecked = check;
                ckbAllBoudaryCompany.IsEnabled = ckbAllTitleCompany.IsChecked == true ? true : check;
            }
            else
            {
                ckbAllBoudaryCompany.IsChecked = false;
                ckbAllBoudaryCompany.IsEnabled = false;
            }

            ckbAllBoudaryCompany.StateChanged += CkbAllBoudaryCompany_StateChanged;
        }

        private void CkbAllTitle_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (!e.IsChecked.GetValueOrDefault())
            {
                ckbAllBoudary.IsChecked = false;
                ckbAllBoudary.IsEnabled = false;
                ckbAllName.IsChecked = false;
                ckbAllName.IsEnabled = false;
            }
            else
            {
                ckbAllBoudary.IsEnabled = true;
                ckbAllName.IsEnabled = true;
            }

            foreach (var item in ListLandmarkCategory.Children)
            {
                if (item.FindByName("ckbTitle") is SfCheckBox ckbTitle && item.BindingContext is UserLandmarkGroupRespone landmark)
                {
                    ckbTitle.StateChanged -= CkbTitle_StateChanged;

                    landmark.IsVisible = ckbAllTitle.IsChecked.Value;

                    landmark.IsDisplayName = ckbAllTitle.IsChecked == true ? landmark.IsDisplayName : false;

                    landmark.IsDisplayBound = ckbAllTitle.IsChecked == true ? landmark.IsDisplayBound : false;

                    ckbTitle.StateChanged += CkbTitle_StateChanged;
                }
            }
        }

        private void CkbTitle_StateChanged(object sender, StateChangedEventArgs e)
        {
            ckbAllTitle.StateChanged -= CkbAllTitle_StateChanged;

            if (BindingContext is BoundaryViewModel viewModel && viewModel.ListLandmarkCategory.Count > 0)
            {
                var check = viewModel.ListLandmarkCategory.All(l => l.IsVisible);

                ckbAllTitle.IsChecked = check;
                ckbAllBoudary.IsEnabled = check;
                ckbAllName.IsEnabled = check;

                if (!e.IsChecked.GetValueOrDefault())
                {
                    var respones = viewModel.ListLandmarkCategory.Where(l => !l.IsVisible).ToList();

                    foreach (var item in viewModel.ListLandmarkCategory)
                    {
                        if (respones.Find(l => l.PK_LandmarksGroupID == item.PK_LandmarksGroupID) is UserLandmarkGroupRespone landmark)
                        {
                            landmark.IsDisplayName = false;
                            landmark.IsDisplayBound = false;
                        }
                    }
                }
            }
            else
            {
                ckbAllTitle.IsChecked = false;
            }

            ckbAllTitle.StateChanged += CkbAllTitle_StateChanged;
        }

        private void CkbAllName_StateChanged(object sender, StateChangedEventArgs e)
        {
            foreach (var item in ListLandmarkCategory.Children)
            {
                if (item.FindByName("ckbName") is SfCheckBox ckbName && item.BindingContext is UserLandmarkGroupRespone landmark)
                {
                    ckbName.StateChanged -= CkbName_StateChanged;

                    landmark.IsDisplayName = ckbAllName.IsChecked.Value;

                    ckbName.StateChanged += CkbName_StateChanged;
                }
            }
        }

        private void CkbName_StateChanged(object sender, StateChangedEventArgs e)
        {
            ckbAllName.StateChanged -= CkbAllName_StateChanged;

            if (BindingContext is BoundaryViewModel viewModel && viewModel.ListLandmarkCategory.Count > 0)
            {
                var check = viewModel.ListLandmarkCategory.All(l => l.IsDisplayName);
                ckbAllName.IsChecked = check;
                ckbAllName.IsEnabled = ckbAllTitle.IsChecked == true ? true : check;
            }
            else
            {
                ckbAllName.IsChecked = false;
                ckbAllName.IsEnabled = false;
            }

            ckbAllName.StateChanged += CkbAllName_StateChanged;
        }

        private void CkbAllBoudary_StateChanged(object sender, StateChangedEventArgs e)
        {
            foreach (var item in ListLandmarkCategory.Children)
            {
                if (item.FindByName("ckbBoudary") is SfCheckBox ckbBoudary && item.BindingContext is UserLandmarkGroupRespone landmark)
                {
                    ckbBoudary.StateChanged -= CkbBoudary_StateChanged;

                    landmark.IsDisplayBound = ckbAllBoudary.IsChecked.Value;

                    ckbBoudary.StateChanged += CkbBoudary_StateChanged;
                }
            }
        }

        private void CkbBoudary_StateChanged(object sender, StateChangedEventArgs e)
        {
            ckbAllBoudary.StateChanged -= CkbAllBoudary_StateChanged;

            if (BindingContext is BoundaryViewModel viewModel && viewModel.ListLandmarkCategory.Count > 0)
            {
                var check = viewModel.ListLandmarkCategory.All(l => l.IsDisplayBound);
                ckbAllBoudary.IsChecked = check;
                ckbAllBoudary.IsEnabled = ckbAllTitle.IsChecked == true ? true : check;
            }
            else
            {
                ckbAllBoudary.IsChecked = false;
                ckbAllBoudary.IsEnabled = false;
            }

            ckbAllBoudary.StateChanged += CkbAllBoudary_StateChanged;
        }
    }
}