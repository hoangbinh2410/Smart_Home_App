using BA_MobileGPS.Utilities;

using System.Text.RegularExpressions;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.Behaviors
{
    public class DangerousCharsValidatorBehavior : Behavior<Entry>
    {
        private Regex punctuationRule = new Regex("['\"<>/&]");

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += OnEntryTextChanged;

            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= OnEntryTextChanged;

            base.OnDetachingFrom(bindable);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                entry.TextChanged -= OnEntryTextChanged;

                if (e.OldTextValue != null && e.NewTextValue != null && e.NewTextValue.Length > e.OldTextValue.Length && punctuationRule.IsMatch(entry.Text))
                {
                    entry.Text = e.OldTextValue;
                }

                entry.TextChanged += OnEntryTextChanged;
            }
        }
    }

    public class DangerousCharsValidatorBehaviorEditor : Behavior<Editor>
    {
        private Regex punctuationRule = new Regex("['\"<>/&]");

        protected override void OnAttachedTo(Editor bindable)
        {
            bindable.TextChanged += OnEntryTextChanged;

            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Editor bindable)
        {
            bindable.TextChanged -= OnEntryTextChanged;

            base.OnDetachingFrom(bindable);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Editor editor)
            {
                editor.TextChanged -= OnEntryTextChanged;

                if (e.OldTextValue != null && e.NewTextValue != null && e.NewTextValue.Length > e.OldTextValue.Length && punctuationRule.IsMatch(editor.Text))
                {
                    editor.Text = e.OldTextValue;
                }

                editor.TextChanged += OnEntryTextChanged;
            }
        }
    }

    public class RemoveVnCharBehaviorEditor : Behavior<Editor>
    {
        protected override void OnAttachedTo(Editor bindable)
        {
            bindable.TextChanged += OnEntryTextChanged;

            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Editor bindable)
        {
            bindable.TextChanged -= OnEntryTextChanged;

            base.OnDetachingFrom(bindable);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Editor editor)
            {
                editor.TextChanged -= OnEntryTextChanged;

                if (e.OldTextValue != null && e.NewTextValue != null && e.NewTextValue.Length > e.OldTextValue.Length && StringHelper.IsVietNamChar.IsMatch(editor.Text))
                {
                    editor.Text = e.OldTextValue;
                }

                editor.TextChanged += OnEntryTextChanged;
            }
        }
    }
}