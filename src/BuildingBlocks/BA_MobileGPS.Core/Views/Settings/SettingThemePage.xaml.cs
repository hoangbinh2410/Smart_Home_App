﻿using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Core.Themes;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class SettingThemePage : ContentPage
    {
        public SettingThemePage()
        {
            InitializeComponent();
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Settings.CurrentTheme == Theme.Light.ToString())
            {
                light.IsChecked = true;
            }
            else if (Settings.CurrentTheme == Theme.Dark.ToString())
            {
                dark.IsChecked = true;
            }
            else
            {
                custom.IsChecked = true;
            }
        }

        private void radioGroup_CheckedChanged(object sender, Syncfusion.XForms.Buttons.CheckedChangedEventArgs e)
        {
            var themeServices = PrismApplicationBase.Current.Container.Resolve<IThemeServices>();
            if (e.CurrentItem.Text == MobileResource.Settings_CheckBox_Light)
            {
                themeServices.ChangeTheme(Theme.Light);
            }
            else if (e.CurrentItem.Text == MobileResource.Settings_Checkbox_Dark)
            {
                themeServices.ChangeTheme(Theme.Dark);
            }
            else
            {
                themeServices.ChangeTheme(Theme.Custom);
            }
        }
    }
}
