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
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using YnetUn.Common;
using System.Net.NetworkInformation;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Windows.Storage;

namespace YnetUn
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage1 : BasePage
    {
        private Headline _navLink;

        public MainPage1()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            App.Current.Resuming += Current_Resuming;
        }

        private void Current_Resuming(object sender, object e)
        {
            var settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.ContainsKey("time"))
            {
                var asd = settings.Values["time"];
                var dateCache = DateTime.Parse(asd.ToString());
                if (DateTime.Now.AddMinutes(-15) > dateCache)
                {
                    try
                    {
                        ShowFirstMain.Stop();
                        GetMainHeader();
                    }
                    finally
                    {
                        ShowFirstMain.Begin();
                    }
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                if (!CheckNet())
                {
                    prgWait.Visibility = Visibility.Collapsed;
                    return;
                }

                GetMainHeader();

            }

            base.OnNavigatedTo(e);
        }

        private async Task<MainTopChannel> GetMainPage()
        {
            var channels = await YnetManager.GetMainPage();
            return channels;
        }

        private async void GetMainHeader()
        {
            var result = await GetMainPage();

            var channel = await YnetManager.GetMainHeader();

            var settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.ContainsKey("time"))
            {
                settings.Values["time"] = DateTime.Now.ToString();
            }
            else
            {
                settings.Values.Add("time", DateTime.Now.ToString());
            }


            SetMainNew(channel.Items);
            channel.Items.RemoveAt(0);

            List<Items> headLines = new List<Items>();

            foreach (var chItem in channel.Items)
            {
                var desc = chItem.SubTitle;
                for (int i = desc.Length - 1; i > 0; i--)
                {
                    if (desc[i] == '(')
                    {
                        var topic = desc.Substring(i).Replace("(", "").Replace(")", "");
                        chItem.RelatedTopic = topic;
                        chItem.SubTitle = desc.Remove(i);
                        break;
                    }
                }
            }

            headLines.Add(new Items() { Headline = channel.Items, MainGuid = "", MainTitle = "" });

            headLines.AddRange(result.Items);

            SetContent(headLines);

            result.Items.Insert(0, new Items() { MainTitle = "עכשיו" });
            lstHeaders.ItemsSource = result.Items;
            lstHeadersVert.ItemsSource = result.Items;
            lstHeaders.SelectedIndex = 0;

            ShowFirstMain.BeginTime = TimeSpan.FromSeconds(2);
            ShowFirstMain.Begin();
        }

        private void SetContent(List<Items> headLines)
        {
            foreach (var item in headLines)
            {
                if (!string.IsNullOrEmpty(item.MainTitle))
                {
                    foreach (var itemMain in item.Headline)
                    {
                        itemMain.RelatedTopic = item.MainTitle;
                        itemMain.RelatedGuid = item.MainGuid;
                    }
                }

            }

            for (int i = 0; i < headLines.Count; i++)
            {
                var heads = headLines[i].Headline;

                var a = 1;
                var b = 2;
                var c = 0;

                if (b + i < headLines.Count - 1)
                {
                    var first = headLines[b + i].Headline[a];
                    var sec = headLines[c + 1].Headline[b];

                    if (b + i < headLines.Count && c + i < headLines.Count)
                    {
                        if (b < headLines[c + i].Headline.Count && a < headLines[b + i].Headline.Count)
                        {
                            headLines[b + i].Headline[a] = sec;
                            headLines[c + i].Headline[b] = first;
                        }
                    }

                }

                if (a < heads.Count && b < headLines.Count)
                {
                    if (b < headLines[b].Headline.Count)
                    {
                        var heada = heads[a];
                        var headb = headLines[b].Headline[b];

                        headLines[b].Headline[b] = heada;
                        headLines[i].Headline[a] = headb;
                    }
                }
            }

            grdNews.ItemsSource = headLines;
        }

        private void SetMainNew(List<Headline> channel)
        {
            var mainNew = channel.FirstOrDefault();
            txtMainName.Text = mainNew.Title;
            imgMain.Source = new BitmapImage() { UriSource = new System.Uri(mainNew.Link) };
            if (!string.IsNullOrEmpty(mainNew.Description))
            {
                txtMainDesc.Text = mainNew.Description;
            }

            _navLink = mainNew;

        }

        private void LstMainHeader_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CheckNet()) return;

            var headline = e.ClickedItem as Headline;
            if (App.RootFrame.ActualWidth < 900)
            {
                Launcher.LaunchUriAsync(new Uri(string.Format("http://www.ynet.co.il/articles/0,7340,L-{0},00.html", headline.YnetGuid)));
            }
            else
            {
                WebPopup popup = new WebPopup();
                popup.Show(string.Format("http://www.ynet.co.il/Iphone/Html/0,13406,L-Article-V7-{0}-WP8,00.html", headline.YnetGuid));
            }

        }


        private void ImgMain_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (!CheckNet()) return;
            if (App.RootFrame.ActualWidth < 900)
            {
                Launcher.LaunchUriAsync(
                    new Uri(string.Format("http://www.ynet.co.il/articles/0,7340,L-{0},00.html", _navLink.YnetGuid)));
            }
            else
            {
                WebPopup popup = new WebPopup();
                popup.Show(string.Format("http://www.ynet.co.il/Iphone/Html/0,13406,L-Article-V7-{0}-WP8,00.html", _navLink.YnetGuid));
            }
        }

        private void GrdNews_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CheckNet()) return;
            var frame = Window.Current.Content as Frame;
            var item = e.ClickedItem as Items;
            if (frame != null && item != null) frame.Navigate(typeof(SubCategory), item.MainGuid);
        }

        private async void lstHeaders_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!CheckNet()) return;

            try
            {
                ShowFirstMain.Stop();
                var slIdx = lstHeaders.Items.IndexOf(e.ClickedItem);
                lstHeaders.SelectedIndex = slIdx;
                if (slIdx == 0)
                {
                    GetMainHeader();
                }
                else
                {
                    var item = e.ClickedItem as Items;

                    var channels = await YnetManager.GetSubCategory(item.MainGuid);
                    SetMainNew(channels.Items.FirstOrDefault().Headline);
                    channels.Items.RemoveAt(0);
                    SetContent(channels.Items);
                }
            }
            finally
            {
                ShowFirstMain.Begin();
            }


        }

        private void Root2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            border1.Visibility = Visibility.Collapsed;
        }

        private void appBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
