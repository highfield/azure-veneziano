using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Cet.UI.Chart
{
    internal class TypeNameTemplateSelector
        : DataTemplateSelector
    {
        private string _actualPattern;
        private string _pattern;
        
        public string Pattern {
            get { return this._pattern; }
            set
            {
                this._pattern = value;
                this._actualPattern = GetActualPattern(value);
            }
        }


        private static string GetActualPattern(string source)
        {
            return source
                .Replace("*", "{0}");
        }


        public override DataTemplate SelectTemplate(
            object item,
            DependencyObject container)
        {
            if (item != null &&
                string.IsNullOrWhiteSpace(this.Pattern) == false)
            {
                var typeName = item.GetType().Name;
                var tplName = string.Format(this._actualPattern, typeName);
                return ((FrameworkElement)container).TryFindResource(tplName) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }

    }
}
