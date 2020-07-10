using BA_MobileGPS.Core;

using Syncfusion.SfDataGrid.XForms;

using Xamarin.Forms;

namespace BA_MobileGPS.Controls
{
    /// <summary>
    /// Derived from DataGridStyle to add the custom styles
    /// </summary>
    public class CustomDataGridStyle : DataGridStyle
    {
        /// <summary>
        /// Initializes a new instance of the Green class.
        /// </summary>
        public CustomDataGridStyle()
        {
        }

        /// <summary>
        /// Overrides this method to write a custom style for header back ground color
        /// </summary>
        /// <returns>Returns From R g b Color</returns>
        public override Color GetHeaderBackgroundColor()
        {
            return (Color)App.Current.Resources["BlueDarkColor"];
        }

        /// <summary>
        /// Overrides this method to write a custom style for header foreground color
        /// </summary>
        /// <returns>Returns From R g b Color</returns>
        public override Color GetHeaderForegroundColor()
        {
            return (Color)App.Current.Resources["BlueDarkColor"];
        }
    }
}