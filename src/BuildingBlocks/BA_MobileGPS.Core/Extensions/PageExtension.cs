namespace Xamarin.Forms.Extensions
{
    public static class PageExtension
    {
        public static TControl GetControl<TControl>(this Page page, string control)
        {
            return page.FindByName<TControl>(control);
        }

        public static void SetFocus(this Page page, string control)
        {
            page.FindByName<VisualElement>(control)?.Focus();
        }
    }
}