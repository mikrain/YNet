using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
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

        private bool CheckNet()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageDialog dialog = new MessageDialog("... אנא בדוק את חיבור האינטרנט שלך ");
                dialog.ShowAsync();
                return false;
            }
            return true;
        }

        private void WebMain_OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
           
            if (args.Uri != null)
            {
                if (args.Uri.ToString() != _url)
                {
                    args.Cancel = true;
                    if (args.Uri.ToString().Contains(".mp4"))
                    {
                        if (!CheckNet()) return;
                        prgRing.IsActive = true;
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
            if (e.OriginalSource is Grid && (e.OriginalSource as Grid).Name == "grid")
            {
                popup.IsOpen = false;
            }

        }

        private void MediaElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (mediaElement.CurrentState == MediaElementState.Playing)
            {
                mediaElement.Pause();
            }
            else
            {
                mediaElement.Play();
            }
        }

        private void AppBarButton_OnClick(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void MediaElement_OnMediaOpened(object sender, RoutedEventArgs e)
        {
            prgRing.IsActive = false;
        }

        private void MediaElement_OnMediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
        }
    }
}
