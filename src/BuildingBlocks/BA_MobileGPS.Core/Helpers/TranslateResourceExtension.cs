using BA_MobileGPS.Core.Resources;

using System;
using System.Diagnostics;
using System.Globalization;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Helpers
{
    [ContentProperty("Text")]
    public class TranslateResourceExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public IValueConverter Converter { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var translation = MobileResource.Get(Text);
            sw.Stop();
            Debug.WriteLine(string.Format("TranslateResourceExtension {0} : {1}", Text, sw.ElapsedMilliseconds));

            if (translation == null)
            {
#if DEBUG
#else
				translation = Text; // returns the key, which GETS DISPLAYED TO THE USER
#endif
            }

            if (Converter != null)
            {
                translation = Converter.Convert(translation, typeof(string), null, CultureInfo.CurrentCulture).ToString() ?? translation;
            }

            return translation;
        }
    }
}