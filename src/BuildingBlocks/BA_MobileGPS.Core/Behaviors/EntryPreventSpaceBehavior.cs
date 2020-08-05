using Xamarin.Forms;

namespace BA_MobileGPS.Core.Behaviors
{
    public class EntryPreventSpaceBehavior : Behavior<Entry>
    {
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

                if (e.OldTextValue != null && e.NewTextValue != null && e.NewTextValue.Length > e.OldTextValue.Length && string.IsNullOrWhiteSpace(e.NewTextValue))
                {
                    entry.Text = e.OldTextValue;
                }

                entry.TextChanged += OnEntryTextChanged;
            }
        }
    }

    public class EntryPreventSpaceBehaviorEditor : Behavior<Editor>
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

                if (e.OldTextValue != null && e.NewTextValue != null && e.NewTextValue.Length > e.OldTextValue.Length && string.IsNullOrWhiteSpace(e.NewTextValue))
                {
                    editor.Text = e.OldTextValue;
                }

                editor.TextChanged += OnEntryTextChanged;
            }
        }
    }
}