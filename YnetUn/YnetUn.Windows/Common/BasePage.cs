using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace YnetUn.Common
{
    public class BasePage : Page
    {
        protected static readonly YnetManager YnetManager = new YnetManager();
        private readonly NavigationHelper _navigationHelper;

        protected BasePage()
        {
            this._navigationHelper = new NavigationHelper(this);
            this.SizeChanged += MainPage_SizeChanged; 
        }

        protected bool CheckNet()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {

                MessageDialog dialog = new MessageDialog("... אנא בדוק את חיבור האינטרנט שלך");
                dialog.ShowAsync();
                return false;
            }
            return true;
        }


        void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 900)
            {
                VisualStateManager.GoToState(this, "Minimal", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", true);
            }
        }

        protected NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Assign a bindable collection of items to this.DefaultViewModel["Items"]
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New) this.NavigationHelper.LoadState += navigationHelper_LoadState;
            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back) this.NavigationHelper.LoadState -= navigationHelper_LoadState;
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
