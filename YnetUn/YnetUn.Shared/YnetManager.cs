using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace YnetUn
{
    public class YnetManager
    {
        private readonly NetworkManager _networkManager;
        private readonly XmlHelper _xmlHelper;

        public YnetManager()
        {
            _networkManager = new NetworkManager();
            _xmlHelper = new XmlHelper();
        }

        public async Task<MainStripChannel> GetMainHeader()
        {
            var stripsString = await _networkManager.GetContent(EndPointsConstants.MainToStrips);
            var mainStrips = _xmlHelper.Serilalize<StripRssMain>(stripsString);
            return mainStrips.Channel;
        }

        public async Task<MainTopChannel> GetMainPage()
        {
            var categoriesString = await _networkManager.GetContent(EndPointsConstants.MainCategories);
            var mainCategories = _xmlHelper.Serilalize<RssMain>(categoriesString);
            return mainCategories.Channel;
        }

        internal async Task<MainTopChannel> GetSubCategory(string mainGuid)
        {
            var stripsString = await _networkManager.GetContent(string.Format(EndPointsConstants.MainSubCategory, mainGuid));
            var mainStrips = _xmlHelper.Serilalize<RssMain>(stripsString);
            return mainStrips.Channel;
        }
    }
}
