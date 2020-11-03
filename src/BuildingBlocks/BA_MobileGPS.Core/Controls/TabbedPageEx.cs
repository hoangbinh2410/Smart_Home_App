using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Controls
{
    public class TabbedPageEx : TabbedPage
    {
        public static readonly BindableProperty IsHiddenProperty = BindableProperty.Create("IsHidden", typeof(bool), typeof(TabbedPageEx), false);

        public bool IsHidden
        {
            set { SetValue(IsHiddenProperty, value); }
            get { return (bool)GetValue(IsHiddenProperty); }
        }

    }
}
