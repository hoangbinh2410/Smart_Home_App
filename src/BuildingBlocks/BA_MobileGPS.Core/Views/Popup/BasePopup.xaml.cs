using BA_MobileGPS.Core.Controls;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasePopup : PopupPage
    {
        public string _title { get; set; }
        public string _messenger { get; set; }
        public IconPosititon _iconPositon { get; set; }
        public string _iconImageSource { get; set; }
        public string _noBtnText { get; set; }
        public string _yesBtnText { get; set; }
        public Color? _iconColor { get; set; }
        public Color? _buttonColor { get; set; }
        public Color? _buttonTextColor { get; set; }
        public Action<bool> _callBack { get; set; }
        public PopupType _popupType { get; set; }
        public Color? _textColor { get; set; }

        public BasePopup(string title, string messenger, IconPosititon iconPosititon, 
            PopupType type, string iconSource = null,
           string noBtnText = null, string yesBtnText = null,Color? textColor = null , Color? iconColor = null,
           Color? buttonColor = null, Color? buttonTextColor = null,
            Action<bool> callBack = null)
        {
            InitializeComponent();
            _title = title;
            _iconPositon = iconPosititon;
            _messenger = messenger;
            _iconImageSource = iconSource;
            _noBtnText = string.IsNullOrEmpty(noBtnText) ? "Từ chối" : noBtnText;
            _yesBtnText = string.IsNullOrEmpty(yesBtnText) ? "Đồng ý" : yesBtnText;
            _callBack = callBack;
            _iconColor = iconColor;
            _buttonColor = buttonColor;
            _buttonTextColor = buttonTextColor;
            _popupType = type;
            _textColor = textColor;
            DrawPopup();
        }

        private void DrawPopup()
        {           
            var content = new StackLayout();
            content.Spacing = 10;
            content.HorizontalOptions = LayoutOptions.CenterAndExpand;

            var body = new Grid();
            body.RowDefinitions = new RowDefinitionCollection() { new RowDefinition() { Height = GridLength.Auto}, 
                                                                  new RowDefinition() { Height = GridLength.Auto}};
            body.ColumnDefinitions = new ColumnDefinitionCollection() { new ColumnDefinition() { Width = GridLength.Auto},
                                                                  new ColumnDefinition() { Width = GridLength.Auto}};
            body.HorizontalOptions = LayoutOptions.Center; 

            var title = new Label();
            title.Margin = new Thickness(0, 10, 25, 0);          
            title.Text = _title;
            title.FontSize = 16;
            title.HorizontalOptions = LayoutOptions.Center;
            title.VerticalTextAlignment = TextAlignment.Center;

            var messenger =  new Label();            
            messenger.Text = _messenger;   
            messenger.FontSize = 13;
            messenger.HorizontalOptions = LayoutOptions.StartAndExpand;
              
            messenger.LineBreakMode = LineBreakMode.WordWrap;

            var icon = new IconView();                                     
            icon.WidthRequest = 60;
            icon.HeightRequest = 60;
            if (!string.IsNullOrEmpty(_iconImageSource))
            {                
                icon.Source = _iconImageSource;
                if (_iconColor != null)
                {
                    icon.Foreground = (Color)_iconColor;
                }
            }
            var footer = new Grid();
            switch (_iconPositon)
            {
                case IconPosititon.Left:
                    icon.Margin = new Thickness(20, 30, 10, 10);
                    title.SetValue(Grid.ColumnProperty, 1);
                    messenger.SetValue(Grid.RowProperty, 1);
                    messenger.SetValue(Grid.ColumnProperty, 1);
                    icon.SetValue(Grid.RowSpanProperty, 2);                
                    body.Children.Add(title);
                    body.Children.Add(messenger);
                    body.Children.Add(icon);
                    break;
                case IconPosititon.Right:
                    messenger.Text += "\n";
                    footer.HeightRequest = 65;
                    messenger.SetValue(Grid.RowProperty, 1);
                    messenger.Margin = new Thickness(20, 0, 0, 0);
                    icon.Margin = new Thickness(0, 20, 20, 0);
                    icon.HorizontalOptions = LayoutOptions.Center;
                    icon.VerticalOptions = LayoutOptions.Center;
                    icon.SetValue(Grid.ColumnProperty, 1);
                    icon.SetValue(Grid.RowProperty, 0);
                    icon.SetValue(Grid.RowSpanProperty, 2);
                    body.Children.Add(title);
                    body.Children.Add(messenger);
                    body.Children.Add(icon);
                    break;
                case IconPosititon.None:
                    panCake.Margin = new Thickness(20, 0);
                    messenger.Margin = new Thickness(20, 0,20,0);
                    messenger.HorizontalOptions = LayoutOptions.Center;
                    messenger.HorizontalTextAlignment = TextAlignment.Center;
                    title.Margin = new Thickness(0, 10, 0, 0);
                    body.Children.Add(title);
                    messenger.SetValue(Grid.RowProperty, 1);
                    body.Children.Add(messenger);
                    break;
                default:
                    break;
            }

            content.Children.Add(body);
           
            footer.HorizontalOptions = LayoutOptions.CenterAndExpand;
        
            
            var temp = new StackLayout();
            temp.Orientation = StackOrientation.Horizontal;
            temp.HorizontalOptions = LayoutOptions.CenterAndExpand;
            temp.Margin = new Thickness(0, 10, 0, 15);            
       
            temp.Spacing = 30;

          
            var btnNo = new Button();
            btnNo.CornerRadius = 5;
            btnNo.Text = _noBtnText;

            btnNo.Padding = 0;
            btnNo.HeightRequest = 25;

            btnNo.FontSize = 13;
            btnNo.Clicked += BtnNo_Clicked;

            var btnYes = new Button();
            btnYes.CornerRadius = 5;
            btnYes.Text = _yesBtnText;
            btnYes.BackgroundColor = _buttonColor == null ? Color.Default : (Color)_buttonColor;
            btnYes.TextColor = _buttonTextColor == null ? Color.Black : (Color)_buttonTextColor;
         
            btnYes.Padding = 0;
            btnYes.HeightRequest = 25;
            btnYes.FontSize = 13;
            btnYes.Clicked += BtnYes_Clicked;

            switch (_popupType)
            {
                case PopupType.Yes:
                    temp.Children.Add(btnYes);
                    break;
                case PopupType.YesNo:
                    btnYes.SetValue(Grid.ColumnProperty, 1);
                    temp.Children.Add(btnNo);
                    temp.Children.Add(btnYes);
                    break;
                default:
                    break;
            }
            footer.Children.Add(temp);
            content.Children.Add(footer);

            panCake.Content = content;
        }

        private void BtnYes_Clicked(object sender, EventArgs e)
        {
            _callBack?.Invoke(true);
            PopupNavigation.Instance.PopAsync();
        }

        private void BtnNo_Clicked(object sender, EventArgs e)
        {
            _callBack?.Invoke(false);
            PopupNavigation.Instance.PopAsync();
        }
    }

    public enum IconPosititon
    {
        Left,
        Right,
        None
    }

    public enum PopupType
    {   
        Yes,
        YesNo
    }
}