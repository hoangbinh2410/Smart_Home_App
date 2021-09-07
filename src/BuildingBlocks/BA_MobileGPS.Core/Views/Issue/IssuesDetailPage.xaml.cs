using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class IssuesDetailPage : ContentPage
    {
        public IssuesDetailPage()
        {
            InitializeComponent();
            lbContentIssue.Text = MobileResource.DetailIssue_Label_ContentIssue;
            lbfolowwissue.Text = MobileResource.DetailIssue_Label_StepIssue;
        }
    }
}
