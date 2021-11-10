using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SupportClientPage : ContentPage
    {
        public SupportClientPage()
        {
            InitializeComponent();
            this.title.Text = MobileResource.SupportClient_Label_Title;          
            this.supportCategory.Text = MobileResource.SupportClient_Label_SupportCategory;
            this.textSupport.Text = MobileResource.SupportClient_Label_TextSupport;
            //this.entrySearch.Placeholder = "Chọn phương tiện";
        }
    }
}