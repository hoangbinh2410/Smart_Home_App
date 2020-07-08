using Xamarin.Forms;

namespace BA_MobileGPS.Core.Controls
{
    public class CustomEditor : Editor
    {
        public CustomEditor()
        {
            this.TextChanged += (sender, e) =>
            {
                this.InvalidateMeasure();
            };
        }

        public static readonly BindableProperty MaxLinesProperty =
           BindableProperty.CreateAttached(
               "MaxLines",
               typeof(int),
               typeof(int),
               1);

        public int MaxLines
        {
            get { return (int)GetValue(MaxLinesProperty); }
            set { SetValue(MaxLinesProperty, value); }
        }
    }
}