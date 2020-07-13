using BA_MobileGPS.Entities;

using Syncfusion.XForms.Buttons;

using System.Linq;

using VMS_MobileGPS.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VMS_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BoundaryPage : ContentPage
    {
        public BoundaryPage()
        {
            InitializeComponent();

            ckbAllBoudary.StateChanged += CkbAllBoudary_StateChanged;
            ckbAllName.StateChanged += CkbAllName_StateChanged;
        }

        private void CkbAllBoudary_StateChanged(object sender, StateChangedEventArgs e)
        {
            foreach (var item in ListCheck.Children)
            {
                if (item.FindByName("ckbBoudary") is SfCheckBox ckbBoudary && item.BindingContext is LandmarkResponse landmark)
                {
                    ckbBoudary.StateChanged -= CkbBoudary_StateChanged;

                    landmark.IsShowBoudary = ckbAllBoudary.IsChecked.Value;

                    ckbBoudary.StateChanged += CkbBoudary_StateChanged;
                }
            }
        }

        private void CkbBoudary_StateChanged(object sender, StateChangedEventArgs e)
        {
            ckbAllBoudary.StateChanged -= CkbAllBoudary_StateChanged;

            if (BindingContext is BoundaryViewModel viewModel && viewModel.ListLandmark.Count > 0)
            {
                ckbAllBoudary.IsChecked = viewModel.ListLandmark.All(l => l.IsShowBoudary);
            }
            else
            {
                ckbAllBoudary.IsChecked = false;
            }

            ckbAllBoudary.StateChanged += CkbAllBoudary_StateChanged;
        }

        private void CkbAllName_StateChanged(object sender, StateChangedEventArgs e)
        {
            foreach (var item in ListCheck.Children)
            {
                if (item.FindByName("ckbName") is SfCheckBox ckbName && item.BindingContext is LandmarkResponse landmark)
                {
                    ckbName.StateChanged -= CkbName_StateChanged;

                    landmark.IsShowName = ckbAllName.IsChecked.Value;

                    ckbName.StateChanged += CkbName_StateChanged;
                }
            }
        }

        private void CkbName_StateChanged(object sender, StateChangedEventArgs e)
        {
            ckbAllName.StateChanged -= CkbAllName_StateChanged;

            if (BindingContext is BoundaryViewModel viewModel && viewModel.ListLandmark.Count > 0)
            {
                ckbAllName.IsChecked = viewModel.ListLandmark.All(l => l.IsShowName);
            }
            else
            {
                ckbAllName.IsChecked = false;
            }

            ckbAllName.StateChanged += CkbAllName_StateChanged;
        }
    }
}