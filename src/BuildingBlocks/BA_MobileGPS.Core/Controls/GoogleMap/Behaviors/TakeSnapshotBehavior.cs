using Prism.Behaviors;

using System.IO;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace BA_MobileGPS.Core.GoogleMap.Behaviors
{
    [Preserve(AllMembers = true)]
    public sealed class TakeSnapshotBehavior : BehaviorBase<Map>
    {
        public static readonly BindableProperty RequestProperty = BindableProperty.Create("Request", typeof(TakeSnapshotRequest), typeof(TakeSnapshotBehavior), null, propertyChanged: OnRequestChanged);

        private static void OnRequestChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((TakeSnapshotBehavior)bindable).OnRequestChanged(oldValue as TakeSnapshotRequest, newValue as TakeSnapshotRequest);
        }

        private void OnRequestChanged(TakeSnapshotRequest oldValue, TakeSnapshotRequest newValue)
        {
            if (oldValue != null)
            {
                oldValue.TakeSnapshotBehavior = null;
            }
            if (newValue != null)
            {
                newValue.TakeSnapshotBehavior = this;
            }
        }

        public Task<Stream> TakeSnapshot()
        {
            return AssociatedObject.TakeSnapshot();
        }
    }
}