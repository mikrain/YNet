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

namespace YnetUn
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string _navLink;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            YnetManager ynetManager = new YnetManager();
            GetMainHeader(ynetManager);
            GetMainPage(ynetManager);
            base.OnNavigatedTo(e);
        }

        private async void GetMainPage(YnetManager ynetManager)
        {
            var channels = await ynetManager.GetMainPage();
            grdNews.ItemsSource = channels.Items;

            ShowFirstMain.BeginTime = TimeSpan.FromSeconds(2);
            ShowFirstMain.Begin();
        }

        private async void GetMainHeader(YnetManager ynetManager)
        {
            var channel = await ynetManager.GetMainHeader();
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

        private async void LstMainHeader_OnItemClick(object sender, ItemClickEventArgs e)
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
    }
}
