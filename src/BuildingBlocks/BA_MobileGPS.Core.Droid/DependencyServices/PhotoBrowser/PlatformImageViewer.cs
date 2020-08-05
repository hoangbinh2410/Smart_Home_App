using Android.Content;

using Com.Facebook.Drawee.Backends.Pipeline;

namespace BA_MobileGPS.Core.Droid.DependencyServices
{
    public class PlatformImageViewer
    {
        public static Context Context { get; set; }

        public static void Init(Context context)
        {
            Context = context;

            Fresco.Initialize(Context);
        }
    }
}