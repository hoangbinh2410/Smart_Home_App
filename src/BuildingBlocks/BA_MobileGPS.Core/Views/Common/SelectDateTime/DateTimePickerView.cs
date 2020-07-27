using BA_MobileGPS.Core.Controls;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;

using Prism.Events;
using Prism.Ioc;
using Prism.Navigation;

using System;
using System.Diagnostics;
using System.Windows.Input;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public enum PickerMode
    {
        Default = 0,
        Popup = 1
    }

    public enum IconPosition
    {
        Start = 0,
        End = 1
    }

    public class DateTimePickerView : ContentView
    {
        public static readonly BindableProperty PickerModeProperty = BindableProperty.Create(nameof(PickerMode), typeof(PickerMode), typeof(DateTimePickerView), PickerMode.Default);

        public PickerMode PickerMode
        {
            get { return (PickerMode)GetValue(PickerModeProperty); }
            set { SetValue(PickerModeProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(DateTimePickerView),
            (Color)Application.Current.Resources["GrayColor"], propertyChanged: OnBorderColorChanged);

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double?), typeof(DateTimePickerView), null, propertyChanged: OnFontSizeChanged);

        public double? FontSize
        {
            get { return (double?)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly BindableProperty IconColorProperty = BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(DateTimePickerView), Color.DimGray, propertyChanged: OnIconColorChanged);

        public Color IconColor
        {
            get { return (Color)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        public static readonly BindableProperty IconSizeProperty = BindableProperty.Create(nameof(IconSize), typeof(double?), typeof(DateTimePickerView), null, propertyChanged: OnIconSizeChanged);

        public double? IconSize
        {
            get { return (double?)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        public static readonly BindableProperty IconPositionProperty = BindableProperty.Create(nameof(IconPosition), typeof(IconPosition), typeof(DateTimePickerView), IconPosition.End, propertyChanged: OnIconPositionChanged);

        public IconPosition IconPosition
        {
            get { return (IconPosition)GetValue(IconPositionProperty); }
            set { SetValue(IconPositionProperty, value); }
        }

        public static readonly BindableProperty DateTimeProperty = BindableProperty.Create(nameof(DateTime), typeof(DateTime), typeof(DateTimePickerView), DateTime.Now, BindingMode.TwoWay, propertyChanged: OnDateTimeChanged);

        public DateTime DateTime
        {
            get { return (DateTime)GetValue(DateTimeProperty); }
            set { SetValue(DateTimeProperty, value); }
        }

        public static readonly BindableProperty DateSelectedCommandProperty = BindableProperty.Create(nameof(DateSelectedCommand), typeof(ICommand), typeof(DateTimePickerView), default);

        public ICommand DateSelectedCommand
        {
            get { return (ICommand)GetValue(DateSelectedCommandProperty); }
            set { SetValue(DateSelectedCommandProperty, value); }
        }

        public static readonly BindableProperty DateSelectedCommandParameterProperty = BindableProperty.Create(nameof(DateSelectedCommandParameter), typeof(object), typeof(DateTimePickerView), default);

        public object DateSelectedCommandParameter
        {
            get { return GetValue(DateSelectedCommandParameterProperty); }
            set { SetValue(DateSelectedCommandParameterProperty, value); }
        }

        public event EventHandler<DateChangedEventArgs> DateSelected;

        private readonly INavigationService navigationService;
        private readonly IEventAggregator eventAggregator;

        private readonly Frame contentFrame;
        private readonly Label dateTimeText;
        private readonly IconView icon;

        public DateTimePickerView()
        {
            navigationService = Prism.PrismApplicationBase.Current.Container.Resolve<INavigationService>();
            eventAggregator = Prism.PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();

            dateTimeText = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Text = DateTime.FormatDateTime()
            };

            if (FontSize != null)
            {
                dateTimeText.FontSize = FontSize.Value;
            }

            icon = new IconView
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 20,
                WidthRequest = 20,
                Foreground = IconColor,
                Source = "ic_date_range_black.png"
            };

            if (IconSize != null)
            {
                icon.HeightRequest = IconSize.Value;
                icon.WidthRequest = IconSize.Value;
            }

            contentFrame = new Frame
            {
                BackgroundColor = Color.White,
                Padding = 5,
                CornerRadius = CornerRadius,
                HasShadow = false,
                IsClippedToBounds = true,
                BorderColor = BorderColor,
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 2,
                    Children = {
                        dateTimeText,
                        icon
                    }
                },
                GestureRecognizers =
                {
                    new TapGestureRecognizer
                    {
                        Command = new Command(async(tcs) =>
                        {
                            var parameters = new NavigationParameters
                            {
                                { "DataPicker", DateTime }
                            };

                            eventAggregator.GetEvent<SelectDateTimeEvent>().Subscribe(OnDateTimeSelected);

                            if (PickerMode == PickerMode.Default)
                                await navigationService.NavigateAsync("SelectDateTimeCalendar", parameters, useModalNavigation: true);
                            else
                                await navigationService.NavigateAsync("SelectDateTimeCalendarPopup", parameters, useModalNavigation: true);
                        })
                    }
                }
            };

            Content = contentFrame;
        }

        private void OnDateTimeSelected(PickerDateTimeResponse response)
        {
            eventAggregator.GetEvent<SelectDateTimeEvent>().Unsubscribe(OnDateTimeSelected);
            if (response.Value != DateTime)
            {
                OnDateTimeSelected(DateTime, response.Value);
            }
        }

        private void OnDateTimeSelected(DateTime oldDate, DateTime newDate)
        {
            try
            {
                DateTime = newDate;

                var param = DateSelectedCommandParameter ?? new DateChangedEventArgs(oldDate, newDate);

                if (DateSelectedCommand != null && DateSelectedCommand.CanExecute(param))
                {
                    DateSelectedCommand.Execute(param);
                }

                DateSelected?.Invoke(this, new DateChangedEventArgs(oldDate, newDate));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private static void OnBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is DateTimePickerView control))
                return;

            if (newValue.Equals(oldValue) || newValue == null)
            {
                return;
            }

            control.contentFrame.BorderColor = (Color)newValue;
        }

        private static void OnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is DateTimePickerView control))
                return;

            if (newValue.Equals(oldValue) || newValue == null)
            {
                return;
            }

            control.dateTimeText.FontSize = (double)newValue;
        }

        private static void OnIconColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is DateTimePickerView control))
                return;

            if (newValue.Equals(oldValue) || newValue == null)
            {
                return;
            }

            control.icon.Foreground = (Color)newValue;
        }

        private static void OnIconSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is DateTimePickerView control))
                return;

            if (newValue.Equals(oldValue) || newValue == null)
            {
                return;
            }

            control.icon.HeightRequest = (double)newValue;
            control.icon.WidthRequest = (double)newValue;
        }

        private static void OnIconPositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is DateTimePickerView control))
                return;

            if (newValue.Equals(oldValue) || newValue == null)
            {
                return;
            }

            if (control.IconPosition == IconPosition.Start)
            {
                control.contentFrame.Content = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 2,
                    Children = {
                        control.icon,
                        control.dateTimeText
                    }
                };
            }
            else
            {
                control.contentFrame.Content = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 2,
                    Children = {
                        control.dateTimeText,
                        control.icon
                    }
                };
            }
        }

        private static void OnDateTimeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is DateTimePickerView control))
                return;

            if (newValue == oldValue || newValue == null)
            {
                return;
            }

            if (!DateTime.TryParse(newValue.ToString(), out DateTime dateTime))
                return;

            control.dateTimeText.Text = dateTime.FormatDateTime();
        }

        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(DateTimePickerView),
           (float)5, BindingMode.TwoWay, propertyChanged: OnCornerRadiusChanged);

        public float CornerRadius
        {
            get { return (float)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        private static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            
            if (!(bindable is DateTimePickerView control))
                return;

            if (newValue.Equals(oldValue) || newValue == null)
            {
                return;
            }

            control.contentFrame.CornerRadius = (float)newValue;
        }
    }
}