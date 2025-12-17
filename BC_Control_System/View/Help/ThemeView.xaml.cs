using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BC_Control_System.Theme;

namespace BC_Control_System.View.Help
{
    /// <summary>
    /// ThemeView.xaml 的交互逻辑
    /// </summary>
    public partial class ThemeView : UserControl
    {
        private ThemeManager _themeManager;
        public ThemeView()
        {
            InitializeComponent();
            _themeManager = new ThemeManager();
            _themeManager.RegisterTheme("Light", "BC_Control_System", "Theme/LightTheme.xaml");
            _themeManager.RegisterTheme("Dark", "BC_Control_System", "Theme/DarkTheme.xaml");
            ApplyButton.Click += ApplyButton_Click;
            ApplyButton1.Click += ApplyButton_Click1;
            
        }
        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {            
            _themeManager.ApplyTheme("Dark");
        }
        private void ApplyButton_Click1(object sender, RoutedEventArgs e)
        {
            _themeManager.ApplyTheme("Light");
        }
    }
}
