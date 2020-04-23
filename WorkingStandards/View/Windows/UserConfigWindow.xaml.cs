using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

using WorkingStandards.Util;
using WorkingStandards.View.Util;

namespace WorkingStandards.View.Windows
{
	/// <summary>
	/// Окно пользовательских настроек.
	/// </summary>
	/// <inheritdoc cref="Window" />
	public partial class UserConfigWindow 
	{
		public UserConfigWindow()
		{
			InitializeComponent();
			AdditionalInitializeComponent();
			VisualInitializeComponent();
		}

		/// <summary>
		/// Визуальная инициализация окна (цвета и размеры шрифтов контролов)
		/// </summary>
		private void VisualInitializeComponent()
		{
			FontSize = Constants.FontSize;
			Background = Constants.BackColor5_WaikawaGray;
			Foreground = Constants.ForeColor2_PapayaWhip;

			// Цвета Labels и TextBlocks
			var mainLabels = FieldsWrapperGrid.Children.OfType<Label>();
			foreach (var label in mainLabels)
			{
				label.Foreground = Constants.ForeColor2_PapayaWhip;
			}
			FontSizeLabel.Foreground = Constants.ForeColor2_PapayaWhip;
			IsRunFullscreenTextBlock.Foreground = Constants.ForeColor2_PapayaWhip;

			// Фоны
			BackgroundRectangle.Fill = Constants.BackColor3_SanJuan;
			HotkeysStackPanel.Background = Constants.BackColor4_BlueBayoux;

			// Панель хоткеев
			var helpLabels = HotkeysStackPanel.Children.OfType<Label>();
			foreach (var helpLabel in helpLabels)
			{
				helpLabel.Foreground = Constants.ForeColor2_PapayaWhip;
			}
		}

		/// <summary>
		/// Получение и отображение значений пользовательских параметров в нужные поля ввода и надписей хоткеев
		/// </summary>
		private void AdditionalInitializeComponent()
		{
			const string closeWindowHotkey = PageLiterals.HotkeyLabelCloseWindow;
			HotkeysTextBlock.Text = closeWindowHotkey;

            var nu67Dbf = Properties.Settings.Default.Nu67Dbf;

            var foxproDbFolderCi = Properties.Settings.Default.FoxProDbFolder_Foxpro_CI;
			var foxproDbFolderBase = Properties.Settings.Default.FoxProDbFolder_Base;
			var foxproDbFolderTrudnorm = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
			var foxproDbFolderFox60ArmBase = Properties.Settings.Default.FoxProDbFolder_Fox60_arm_Base;
			var foxproDbFolderTemp = Properties.Settings.Default.FoxProDbFolder_Temp;


			var isRunInFullscreen = Properties.Settings.Default.IsRunInFullscreen;
			var fontSize = Properties.Settings.Default.FontSize;

		    NameNu67DbfTextBox.Text = nu67Dbf;
			FoxproDbFolderCiTextBox.Text = foxproDbFolderCi;
			FoxproDbFolderBaseTextBox.Text = foxproDbFolderBase;
			FoxproDbFolderTrudnormTextBox.Text = foxproDbFolderTrudnorm;
			FoxproDbFolderFox60ArmBaseTextBox.Text = foxproDbFolderFox60ArmBase;
			FoxproDbFolderTempTextBox.Text = foxproDbFolderTemp;
			
			IsRunFullscreenCheckBox.IsChecked = isRunInFullscreen;

			FontSizeDoubleUpDown.Minimum = 8D;
			FontSizeDoubleUpDown.Value = fontSize;
			FontSizeDoubleUpDown.Maximum = 30D;

		}

		private void Window_OnPreviewEscapeKeyDownCloseWindow(object senderIsWindow, KeyEventArgs eventArgs)
		{
			if (eventArgs.Key != Key.Escape)
			{
				return;
			}
			eventArgs.Handled = true;
			Close();
		}

		private void SaveButton_OnClick(object sender, RoutedEventArgs e)
		{

			// Получение названий полей и значений
            var nu67DbfLabel = NameNu67DbfLabel.Content.ToString();
		    var nu67Dbf = NameNu67DbfTextBox.Text.Trim();

            var foxproDbFolderCiLabel = FoxproDbFolderCiLabel.Content.ToString();
			var foxproDbFolderCi = FoxproDbFolderCiTextBox.Text.Trim();

			var foxproDbFolderBaseLabel = FoxproDbFolderBaseLabel.Content.ToString();
			var foxproDbFolderBase = FoxproDbFolderBaseTextBox.Text.Trim();

			var foxproDbFolderTrudnormLabel = FoxproDbFolderTrudnormLabel.Content.ToString();
			var foxproDbFolderTrudnorm = FoxproDbFolderTrudnormTextBox.Text.Trim();

			var foxproDbFolderFox60ArmBaseLabel = FoxproDbFolderFox60ArmBaseLabel.Content.ToString();
			var foxproDbFolderFox60ArmBase = FoxproDbFolderFox60ArmBaseTextBox.Text.Trim();

			var foxproDbFolderTempLabel = FoxproDbFolderTempLabel.Content.ToString();
			var foxproDbFolderTemp = FoxproDbFolderTempTextBox.Text.Trim();

			var isRunInFullscreen = IsRunFullscreenCheckBox.IsChecked ?? false;

			var fontSizeLabel = FontSizeLabel.Content.ToString();
			var nullableFontSize = FontSizeDoubleUpDown.Value;

			// Валидация на пустоту
			var isValid = true;
			var errorMessage = string.Empty;
			var messagePattern = "Поле [{0}] пустое / не указано" + Environment.NewLine;

		    isValid &= !string.IsNullOrWhiteSpace(nu67Dbf);
		    errorMessage += string.IsNullOrWhiteSpace(nu67Dbf)
		        ? string.Format(messagePattern, nu67DbfLabel)
		        : string.Empty;

            isValid &= !string.IsNullOrWhiteSpace(foxproDbFolderCi);
			errorMessage += string.IsNullOrWhiteSpace(foxproDbFolderCi)
				? string.Format(messagePattern, foxproDbFolderCiLabel)
				: string.Empty;

			isValid &= !string.IsNullOrWhiteSpace(foxproDbFolderBase);
			errorMessage += string.IsNullOrWhiteSpace(foxproDbFolderBase)
				? string.Format(messagePattern, foxproDbFolderBaseLabel)
				: string.Empty;

			isValid &= !string.IsNullOrWhiteSpace(foxproDbFolderTrudnorm);
			errorMessage += string.IsNullOrWhiteSpace(foxproDbFolderTrudnorm)
				? string.Format(messagePattern, foxproDbFolderTrudnormLabel)
				: string.Empty;

			isValid &= !string.IsNullOrWhiteSpace(foxproDbFolderFox60ArmBase);
			errorMessage += string.IsNullOrWhiteSpace(foxproDbFolderFox60ArmBase)
				? string.Format(messagePattern, foxproDbFolderFox60ArmBaseLabel)
				: string.Empty;

			isValid &= !string.IsNullOrWhiteSpace(foxproDbFolderTemp);
			errorMessage += string.IsNullOrWhiteSpace(foxproDbFolderTemp)
				? string.Format(messagePattern, foxproDbFolderTempLabel)
				: string.Empty;

			isValid &= nullableFontSize != null;
			errorMessage += nullableFontSize == null ? string.Format(messagePattern, fontSizeLabel) : string.Empty;

			if (!isValid) // Если какое-то из полей не указано
			{
				const MessageBoxImage messageBoxType = MessageBoxImage.Error;
				const MessageBoxButton messageBoxButtons = MessageBoxButton.OK;
				MessageBox.Show(errorMessage, PageLiterals.HeaderValidation, messageBoxButtons, messageBoxType);
				return;
			}

			// Сохранение параметров в пользовательский config-файл этой версии приложения и закрытие окна
			// Ориентировочный путь: [ c:\Users\Username\AppData\Local\OJSC_GZSU\ProductStockManager_Url... ]

			var fontSize = (double)nullableFontSize;

		    Properties.Settings.Default.Nu67Dbf = nu67Dbf;

            Properties.Settings.Default.FoxProDbFolder_Foxpro_CI = foxproDbFolderCi;
			Properties.Settings.Default.FoxProDbFolder_Base = foxproDbFolderBase;
			Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm = foxproDbFolderTrudnorm;
			Properties.Settings.Default.FoxProDbFolder_Fox60_arm_Base = foxproDbFolderFox60ArmBase;
			Properties.Settings.Default.FoxProDbFolder_Temp = foxproDbFolderTemp;

			Properties.Settings.Default.IsRunInFullscreen = isRunInFullscreen;
			Properties.Settings.Default.FontSize = fontSize;

			Properties.Settings.Default.Save();
			Close();
		}

		/// <summary>
		/// Нажатие кнопки [Отмена (Закрыть окно)]
		/// </summary>
		private void CloseButton_OnClick(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
