using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace YnetUn
{
    public sealed partial class WebPopup : UserControl
    {
        Popup popup = new Popup();
        private string _url;

        public WebPopup()
        {
            this.InitializeComponent();
        }

        public void Show(string url)
        {
            _url = url;
            this.Width = App.RootFrame.ActualWidth;
            this.Height = App.RootFrame.ActualHeight;
            webView.Height = Height;
            webView.Navigate(new Uri(url));
            popup.Child = this;
            popup.IsOpen = true;
            popup.IsLightDismissEnabled = true;
            ShowPopup.Begin();
        }
        private void WebMain_OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (args.Uri!=null)
            {
                if (args.Uri.ToString() != _url)
                {
                    args.Cancel = true;
                    if (args.Uri.ToString().Contains(".mp4"))
                    {
                        mediaElement.Source = args.Uri;
                        VisualStateManager.GoToState(this, "Video", true);
                    }
                    else
                    {
                        Launcher.LaunchUriAsync(args.Uri);
                    }
                }
            }
           
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            popup.IsOpen = false;
        }
    }
}
