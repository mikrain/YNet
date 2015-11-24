using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace YnetUn.Common
{
    public class NewsSelector : DataTemplateSelector
    {
        public DataTemplate AdTemplate
        {
            get;
            set;
        }

        public DataTemplate NewsTemplate
        {
            get;
            set;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is Headline)
            {
                return NewsTemplate;
            }
            else if (item is Mediator)
            {
                return AdTemplate;
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}
