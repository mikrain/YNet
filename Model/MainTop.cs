using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Model
{
    [XmlRoot(ElementName = "rss")]
    public class RssMain
    {
        [XmlElement(ElementName = "channel")]
        public MainTopChannel Channel { get; set; }
    }

    [XmlRoot(ElementName = "rss")]
    public class StripRssMain
    {
        [XmlElement(ElementName = "channel")]
        public MainStripChannel Channel { get; set; }
    }

    public class MainTopChannel
    {
        [XmlElement(ElementName = "item")]
        public List<Items> Items { get; set; }
    }

    public class MainStripChannel
    {
        [XmlElement(ElementName = "item")]
        public List<Headline> Items { get; set; }
    }

    public class Items
    {

        [XmlElement(ElementName = "mainTitle")]
        public string MainTitle { get; set; }

        [XmlElement(ElementName = "mainGuid")]
        public string MainGuid { get; set; }

        [XmlElement(ElementName = "headline")]
        public List<Headline> Headline { get; set; }
    }


    public class Headline
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "subTitle")]
        public string SubTitle { get; set; }

        [XmlElement(ElementName = "link")]
        public string Link { get; set; }

        [XmlElement(ElementName = "link169")]
        public string Link169 { get; set; }

        [XmlElement(ElementName = "pubDate")]
        public string PubDate { get; set; }

        [XmlElement(ElementName = "type")]
        public string YnetType { get; set; }

        [XmlElement(ElementName = "guid")]
        public string YnetGuid { get; set; }

        [XmlElement(ElementName = "isVideo")]
        public string IsVideo { get; set; }

        [XmlElement(ElementName = "ynetlive")]
        public string Ynetlive { get; set; }

        [XmlElement(ElementName = "mainIcon")]
        public string MainIcon { get; set; }

        [XmlElement(ElementName = "author")]
        public string Author { get; set; }

        [XmlIgnore]
        public string RelatedTopic { get; set; }

        [XmlIgnore]
        public string RelatedGuid { get; set; }
    }
}
