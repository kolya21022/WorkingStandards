using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WorkingStandards.Entities.External;
using WorkingStandards.Storages;
using WorkingStandards.Util;

namespace WorkingStandards.View.Windows
{
    public partial class LoginWindow
    {
        // Указанные пользователем значения
        private Login _selectedLogin;
        private string _password;

        public LoginWindow()
        {
            InitializeComponent();
            VisualInitializeComponent();

            LoginComboBox.ItemsSource = AutorizationsStorage.GetLogin();
            LoginComboBox.SelectedIndex = 1;
        }

        /// <summary>
        /// Визуальная инициализация страницы (цвета и размеры шрифтов контролов)
        /// </summary>
        private void VisualInitializeComponent()
        {
            FontSize = Constants.FontSize;

            BobyGrid.Background = Constants.BackColor4_BlueBayoux;

            var titleLabels = WorkGuildWrapperGrid.Children.OfType<Label>();
            foreach (var titleLabel in titleLabels)
            {
                titleLabel.Foreground = Constants.ForeColor7_White;
            }

            var titleLabels2 = PasswordWrapperGrid.Children.OfType<Label>();
            foreach (var titleLabel in titleLabels2)
            {
                titleLabel.Foreground = Constants.ForeColor7_White;
            }
        }

        public Login GetLogin()
        {
            return _selectedLogin;
        }

        /// <summary>
        /// Нажатие кнопки [Подтверждение]
        /// </summary>
        private void ConfirmButton_OnClick(object senderIsButton, RoutedEventArgs eventArgs)
        {
            _selectedLogin = LoginComboBox.SelectedItem as Login;
            _password = PasswordTextBox.Text;

            if (_selectedLogin == null)
            {
                return;
            }

            var isLogin = AutorizationsStorage.Autorization(_selectedLogin.Workguild, _password);
            if (isLogin)
            {
                ErrorLabel.Content = string.Empty;
                ErrorLabel.Visibility = Visibility.Hidden;

                DialogResult = true;
                Close();
            }
            else
            {
                ErrorLabel.Content = $"Неверный пароль для {_selectedLogin.Display}!";
                ErrorLabel.Visibility = Visibility.Visible;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
