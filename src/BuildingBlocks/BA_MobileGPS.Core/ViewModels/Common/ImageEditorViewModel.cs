using BA_MobileGPS.Core.Constant;

using Prism.Commands;
using Prism.Navigation;

using Syncfusion.SfImageEditor.XForms;

using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ImageEditorViewModel : ViewModelBase
    {
        private string location;

        private ImageSource image;
        public ImageSource Image { get => image; set => SetProperty(ref image, value); }

        public ICommand ImageSavedCommand { get; private set; }
        public ICommand ImageSavingCommand { get; private set; }

        public ImageEditorViewModel(INavigationService navigationService) : base(navigationService)
        {
            ImageSavedCommand = new DelegateCommand<ImageSavedEventArgs>(OnImageSaved);
            ImageSavingCommand = new DelegateCommand<ImageSavingEventArgs>(OnImageSaving);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var imageFile = parameters?.GetValue<string>("ImagePath");

            if (imageFile != null)
            {
                Image = ImageSource.FromFile(imageFile);
            }
        }

        private void OnImageSaved(ImageSavedEventArgs args)
        {
        }

        private async void OnImageSaving(ImageSavingEventArgs args)
        {
            args.Cancel = true;

            location = DependencyService.Get<ISaveService>().Save(args.Stream);

            Task.Delay(500).Wait();

            var @params = new NavigationParameters
            {
                { ParameterKey.ImageStream, args.Stream },
                { ParameterKey.ImageLocation, location }
            };

            await NavigationService.GoBackAsync(@params, useModalNavigation: true, true);
        }
    }
}