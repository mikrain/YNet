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


        public Dictionary<string, object> cache = new Dictionary<string, object>();

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
            var mainStrips = (RssMain)GetFromCache<RssMain>(mainGuid);
            if (mainStrips == null)
            {
                var stripsString = await _networkManager.GetContent(string.Format(EndPointsConstants.MainSubCategory, mainGuid));
                mainStrips = _xmlHelper.Serilalize<RssMain>(stripsString);
                AddToCache(mainGuid, mainStrips);
            }


            return mainStrips.Channel;
        }

        private void AddToCache(string cacheName, object mainStrips)
        {
            if (!cache.ContainsKey(cacheName))
            {
                cache.Add(cacheName, mainStrips);
            }
        }

        private object GetFromCache<T>(string cacheName)
        {
            if (cache.ContainsKey(cacheName))
            {
                return cache[cacheName];
            }
            return null;
        }
    }
}
