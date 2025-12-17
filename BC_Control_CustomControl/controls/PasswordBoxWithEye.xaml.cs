using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace BC_Control_CustomControl.Controls
{
    public partial class PasswordBoxWithEye : UserControl, INotifyPropertyChanged
    {
        public PasswordBoxWithEye()
        {
            InitializeComponent();
        }

        private bool _isPasswordVisible = false;
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set
            {
                _isPasswordVisible = value;
                OnPropertyChanged(nameof(IsPasswordVisible));
                EyeIcon = _isPasswordVisible
                    ? "pack://application:,,,/ZC_Control_System;component/Resources/eye_hide.png"
                    : "pack://application:,,,/ZC_Control_System;component/Resources/eye_show.png";

                // 仅在切换显示状态时同步一次内容
                if (_isPasswordVisible)
                    VisibleTextBox.Text = HiddenPasswordBox.Password;
                else
                    HiddenPasswordBox.Password = VisibleTextBox.Text;
            }
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                nameof(Password),
                typeof(string),
                typeof(PasswordBoxWithEye),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnPasswordChanged)
            );

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBoxWithEye control)
            {
                string newPassword = e.NewValue as string ?? string.Empty;

                // 只更新可见的 TextBox，避免重置 PasswordBox 导致光标乱跳
                if (control.IsPasswordVisible)
                {
                    if (control.VisibleTextBox.Text != newPassword)
                        control.VisibleTextBox.Text = newPassword;
                }
            }
        }

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        private string _eyeIcon = "pack://application:,,,/ZC_Control_System;component/Resources/eye_show.png";
        public string EyeIcon
        {
            get => _eyeIcon;
            set
            {
                _eyeIcon = value;
                OnPropertyChanged(nameof(EyeIcon));
            }
        }

        private void ToggleVisibility_Click(object sender, RoutedEventArgs e)
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        private void HiddenPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // 仅在隐藏状态时同步 Password
            if (!IsPasswordVisible)
                Password = HiddenPasswordBox.Password;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
