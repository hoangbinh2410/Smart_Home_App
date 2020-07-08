using Prism.Behaviors;
using System.Linq;

using Xamarin.Forms;

namespace VMS_MobileGPS.Behaviors
{
    public class AutoScrollBehavior : BehaviorBase<StackLayout>
    {
        protected override void OnAttachedTo(StackLayout bindable)
        {
            bindable.ChildAdded += Stack_ChildAdded;

            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(StackLayout bindable)
        {
            bindable.ChildAdded -= Stack_ChildAdded;

            base.OnDetachingFrom(bindable);
        }

        private void Stack_ChildAdded(object sender, ElementEventArgs e)
        {
            if (AssociatedObject.Parent is ScrollView scrollView)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    scrollView.ScrollToAsync(((StackLayout)sender).Children.Last(), ScrollToPosition.End, false);
                });
            }
        }
    }
}