﻿using System.Windows;
using System.Windows.Controls;
using WorkingStandards.Entities.External;
using WorkingStandards.Services;
using WorkingStandards.View.Windows;
using WorkingStandards.Util;

namespace WorkingStandards.View.Menus
{
	/// <summary>
	/// Верхнее меню главного окна приложения.
	/// </summary>
	/// <inheritdoc cref="UserControl" />
	public partial class TopMenu 
	{
	    public Login Login;

        public TopMenu()
		{
			InitializeComponent();
			VisualInitializeComponent();
		}

		/// <summary>
		/// Визуальная инициализация меню (цвета и размеры шрифтов контролов)
		/// </summary>
		private void VisualInitializeComponent()
		{
			WindowMenu.FontSize = Constants.FontSize;
			WindowMenu.Background = Constants.BackColor3_SanJuan;
			foreach (var menuItem in WindowMenu.Items)
			{
				var menuItemControl = menuItem as MenuItem;
				if (menuItemControl == null)
				{
					continue;
				}
				menuItemControl.Background = Constants.BackColor4_BlueBayoux;
				menuItemControl.Foreground = Constants.ForeColor2_PapayaWhip;
			}
		}

	    public void LvlVisual()
	    {
	        UpdateMenuItem.Visibility = Login.Lvl == 0 ? Visibility.Collapsed : Visibility.Visible;
	    }

        /// <summary>
        /// Выбор пункта меню [Пользовательские настройки]
        /// </summary>
        private void ConfigMenuItem_OnClick(object senderIsMenuItem, RoutedEventArgs eventArgs)
		{
			var userConfigWindow = new UserConfigWindow
			{
				Owner = Window.GetWindow(this)
			};
			userConfigWindow.ShowDialog();
		}

		/// <summary>
		/// Выбор пункта меню [Выход]
		/// </summary>
		private void ExitMenuItem_OnClick(object senderIsMenuItem, RoutedEventArgs eventArgs)
		{
			Application.Current.Shutdown();
		}

	    private void UpdateDetailsForCalculationMenuItem_OnClick(object sender, RoutedEventArgs e)
	    {
	        IzdPechAndIzdRascService.IzdRascUpdate();
	        MessageBox.Show("Обновление закончено.", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

	    private void UpdateDetailsForPrintMenuItem_OnClick(object sender, RoutedEventArgs e)
	    {
	        IzdPechAndIzdRascService.IzdPechUpdate();
	        MessageBox.Show("Обновление закончено.", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }
	}
}
