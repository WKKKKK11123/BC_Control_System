using System;
using System.Collections.Generic;
using System.Windows;

namespace BC_Control_System.Theme
{
    public class ThemeManager
    {
        private Dictionary<string, ResourceDictionary> _themes = new Dictionary<string, ResourceDictionary>();

        public void RegisterTheme(string themeName, string assemblyName, string resourcePath)
        {
            string uri = $"pack://application:,,,/{assemblyName};component/{resourcePath}";

            ResourceDictionary resource = new ResourceDictionary();
            
            resource.Source = new Uri(uri, UriKind.RelativeOrAbsolute);

            _themes.Add(themeName, resource);
        }

        public void ApplyTheme(string themeName)
        {
            ResourceDictionary resource = _themes[themeName];

            Application.Current.Resources.MergedDictionaries.Add(resource);
        }
    }
}
