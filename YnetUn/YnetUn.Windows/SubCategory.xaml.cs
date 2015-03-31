using Model;
using YnetUn.Common;
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

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace YnetUn
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class SubCategory : BasePage
    {
        public SubCategory()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetContent(e);
            base.OnNavigatedTo(e);
        }

        private async void GetContent(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                var mainGuid = e.Parameter.ToString();
                var channels = await YnetManager.GetSubCategory(mainGuid);
                channels.Items.RemoveAll(items => items.Headline.Count == 0);
                grdNews.ItemsSource = channels.Items;
                prgBar.Visibility = Visibility.Collapsed;
            }
        }

        private void LstMainHeader_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CheckNet()) return;
            var headline = e.ClickedItem as Headline;
            if (App.RootFrame.ActualWidth < 900)
            {
                Launcher.LaunchUriAsync(
                    new Uri(string.Format("http://www.ynet.co.il/articles/0,7340,L-{0},00.html", headline.YnetGuid)));
            }
            else
            {
                
                WebPopup popup = new WebPopup();
                popup.Show(string.Format("http://www.ynet.co.il/Iphone/Html/0,13406,L-Article-V7-{0}-WP8,00.html",
                    headline.YnetGuid));
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationHelper.GoBack();
        }
    }
}
