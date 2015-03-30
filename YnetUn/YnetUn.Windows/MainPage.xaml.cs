using Windows.System;
using Windows.UI.Xaml.Media.Imaging;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using YnetUn.Common;

namespace YnetUn
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : BasePage
    {
        private string _navLink;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                GetMainHeader();
                GetMainPage(); 
            }
           
            base.OnNavigatedTo(e);
        }

        private async void GetMainPage()
        {
            var channels = await YnetManager.GetMainPage();
            grdNews.ItemsSource = channels.Items;

            ShowFirstMain.BeginTime = TimeSpan.FromSeconds(2);
            ShowFirstMain.Begin();
        }

        private async void GetMainHeader()
        {
            var channel = await YnetManager.GetMainHeader();
            SetMainNew(channel);
            lstMainHeader.ItemsSource = channel.Items;
        }

        private void SetMainNew(MainStripChannel channel)
        {
            var mainNew = channel.Items.FirstOrDefault();
            txtMainName.Text = mainNew.Title;
            imgMain.Source = new BitmapImage() { UriSource = new System.Uri(mainNew.Link) };
            txtMainDesc.Text = mainNew.Description;
            channel.Items.RemoveAt(0);
            _navLink = string.Format("http://www.ynet.co.il/Iphone/Html/0,13406,L-Article-V7-{0}-WP8,00.html", mainNew.YnetGuid);

        }

        private void LstMainHeader_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var headline = e.ClickedItem as Headline;
            WebPopup popup = new WebPopup();
            popup.Show(string.Format("http://www.ynet.co.il/Iphone/Html/0,13406,L-Article-V7-{0}-WP8,00.html", headline.YnetGuid));
        }

        private void ImgMain_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            WebPopup popup = new WebPopup();
            popup.Show(_navLink);
        }

        private void GrdNews_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            var item = e.ClickedItem as Items;
            if (frame != null && item != null) frame.Navigate(typeof(SubCategory), item.MainGuid);
        }
    }
}
