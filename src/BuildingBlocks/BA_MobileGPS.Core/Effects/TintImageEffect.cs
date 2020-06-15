using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Effects
{
    public class TintImageEffect : RoutingEffect
    {
        public const string GroupName = "BA_MobileGPS";
        public const string Name = "TintImageEffect";

        public Color TintColor { get; set; }

        public TintImageEffect() : base($"{GroupName}.{Name}") { }
    }
}
