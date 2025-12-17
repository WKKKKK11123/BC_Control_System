using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZC_Control_EFAM.ProcessControl;

namespace BC_Control_System
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ZC_Control_EFAM.ProcessControl.ProcessControl processControl)
        {
            InitializeComponent();

            _processControl = processControl;
            SourceInitialized += MainWindow_SourceInitialized;
        }

        private void Button_Click(object sender, RoutedEventArgs e) { }
        
        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            // 获取窗口句柄
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            // 获取系统菜单
            IntPtr hmenu = GetSystemMenu(hwnd, false);

            if (hmenu != IntPtr.Zero)
            {
                // 禁用关闭菜单项
                EnableMenuItem(hmenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
            }
        }

        private ZC_Control_EFAM.ProcessControl.ProcessControl _processControl;

        // Win32 API声明
        private const int SC_CLOSE = 0xF060;
        private const int MF_BYCOMMAND = 0x00000000;
        private const int MF_GRAYED = 0x00000001;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        private void btn_EFAMAuto_Click(object sender, RoutedEventArgs e)
        {
            //_processControl.eFAM_Data.UpdataStationData();
            //if (!_processControl.eFAM_Data.IsConnected)
            //{
            //    MessageBox.Show("与EFAM的网络连接是断开的！", "Error");
            //    return;
            //}

            //if (
            //    MessageBox.Show("确认改变EFAM的控制状态！", "提示", MessageBoxButton.YesNo)
            //    == MessageBoxResult.Yes
            //)
            //{
            //    if (
            //        !_processControl.Auto
            //        && (
            //            _processControl.FTRPlaceSenser
            //            || _processControl.Opener1PlaceSenser
            //            || _processControl.Opener2PlaceSenser
            //            || _processControl.WHRPaceSenser
            //            || _processControl.HVPlaceSenser
            //        )
            //    )
            //    {
            //        MessageBox.Show("当前状态不允许切换状态！", "Error");
            //        return;
            //    }
            //    _processControl.Auto = !_processControl.Auto;
            //    if (_processControl.Auto)
            //    {
            //        btn_EFEMAuto.Background = Brushes.Green;
            //    }
            //    else
            //    {
            //        btn_EFEMAuto.Background = Brushes.Silver;
            //    }
            //}
        }

        private void ResetButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 按下时立即写 true
            //if (CommonMethods.Device.IsConnected)
            //{
            //    CommonMethods.CommonWrite("M2151", "true");
            //}
        }

        private async void ResetButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // 延迟 500ms 再写 false
            //await Task.Delay(500);
            //if (CommonMethods.Device.IsConnected)
            //{
            //    CommonMethods.CommonWrite("M2151", "false");
            //}
        }

        private void MuteButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 按下时立即写 true
            //if (CommonMethods.Device.IsConnected)
            //{
            //    CommonMethods.CommonWrite("M3000", "true");
            //}
        }

        private async void MuteButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // 延迟 500ms 再写 false
            //await Task.Delay(500);
            //if (CommonMethods.Device.IsConnected)
            //{
            //    CommonMethods.CommonWrite("M3000", "false");
            //}
        }

        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
