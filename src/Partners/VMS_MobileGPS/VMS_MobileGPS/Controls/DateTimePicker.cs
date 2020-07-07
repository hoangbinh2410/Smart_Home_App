using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Controls;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using FontAwesome;
using Prism.Common;
using Prism.Events;
using Prism.Ioc;
using Prism.Navigation;

using System;

using VMS_MobileGPS.ViewModels;

using Xamarin.Forms;

namespace VMS_MobileGPS.Controls
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

    public class DateTimePicker : ContentView
    {
        public static readonly BindableProperty PickerModeProperty = BindableProperty.Create(nameof(PickerMode), typeof(PickerMode), typeof(DateTimePicker), PickerMode.Default);

        public PickerMode PickerMode
        {
            get { return (PickerMode)GetValue(PickerModeProperty); }
            set { SetValue(PickerModeProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(DateTimePicker),
            (Color)Application.Current.Resources["TextSecondaryColor"], propertyChanged: OnBorderColorChanged);

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty IconColorProperty = BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(DateTimePicker), Color.DimGray, propertyChanged: OnIconColorChanged);

        public Color IconColor
        {
            get { return (Color)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        public static readonly BindableProperty IconPositionProperty = BindableProperty.Create(nameof(IconPosition), typeof(IconPosition), typeof(DateTimePicker), IconPosition.End, propertyChanged: OnIconPositionChanged);

        public IconPosition IconPosition
        {
            get { return (IconPosition)GetValue(IconPositionProperty); }
            set { SetValue(IconPositionProperty, value); }
        }

        public static readonly BindableProperty DateTimeProperty = BindableProperty.Create(nameof(DateTime), typeof(DateTime), typeof(DateTimePicker), DateTime.Now, BindingMode.TwoWay, propertyChanged: OnDateTimeChanged);

        public DateTime DateTime
        {
            get { return (DateTime)GetValue(DateTimeProperty); }
            set { SetValue(DateTimeProperty, value); }
        }

        private readonly IEventAggregator eventAggregator;

        private Frame contentFrame;
        private Label dateTimeText;
        private FontAwesomeIcon icon;

        public DateTimePicker()
        {
            eventAggregator = Prism.PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();

            dateTimeText = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Text = DateTime.FormatDateTime()
            };

            icon = new FontAwesomeIcon
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 20,
                WidthRequest = 20,
                TextColor = IconColor,
                Text = FontAwesomeIcons.CalendarAlt
            };

            contentFrame = new Frame
            {
                Padding = 5,
                CornerRadius = 5,
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
                        Command = new Command(async() =>
                        {
                            var p = new NavigationParameters
                            {
                                { "DataPicker", DateTime }
                            };

                            eventAggregator.GetEvent<SelectDateTimeEvent>().Subscribe(OnDateTimeSelected);

                            var navigationService = ((VMSBaseViewModel)PageUtilities.GetCurrentPage(Application.Current.MainPage).BindingContext).NavigationService;

                            if (PickerMode == PickerMode.Default)
                                await navigationService.NavigateAsync("SelectDateTimeCalendar", p);
                            else
                                await navigationService.NavigateAsync("SelectDateTimeCalendarPopup", p);
                        })
                    }
                }
            };

            Content = contentFrame;
        }

        private void OnDateTimeSelected(PickerDateTimeResponse response)
        {
            DateTime = response.Value;

            eventAggregator.GetEvent<SelectDateTimeEvent>().Unsubscribe(OnDateTimeSelected);
        }

        private static void OnBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is DateTimePicker control))
                return;

            if (newValue.Equals(oldValue) || newValue == null)
            {
                return;
            }

            control.contentFrame.BorderColor = (Color)newValue;
        }

        private static void OnIconColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is DateTimePicker control))
                return;

            if (newValue.Equals(oldValue) || newValue == null)
            {
                return;
            }

            control.icon.TextColor = (Color)newValue;
        }

        private static void OnIconPositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is DateTimePicker control))
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
            if (!(bindable is DateTimePicker control))
                return;

            if (newValue == oldValue || newValue == null)
            {
                return;
            }

            if (!DateTime.TryParse(newValue.ToString(), out DateTime dateTime))
                return;

            control.dateTimeText.Text = dateTime.FormatDateTime();
        }
    }
}