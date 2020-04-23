using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using WorkingStandards.Entities.External;
using WorkingStandards.Services;
using WorkingStandards.View.Util;
using WorkingStandards.Util;
using WorkingStandards.View.Pages.Reports;
using WorkingStandards.View.Pages.TableView;

namespace WorkingStandards.View.Menus
{
	/// <summary>
	/// Логика взаимодействия c боковым меню
	/// </summary>
	/// <inheritdoc cref="UserControl" />
	public partial class SideMenu
	{
	    public Login Login;

		public static SideMenu Instance { get; private set; }
		public SideMenu()
		{
			Instance = this;
			InitializeComponent();
			VisualInitializeComponent();
		}

		/// <summary>
		/// Визуальная инициализация меню (цвета и размеры шрифтов контролов)
		/// </summary>
		private void VisualInitializeComponent()
		{
			// Экспандеры, вложенные в них StackPanel и вложенные в них Buttons
			var expanders = WrapperStackPanel.Children.OfType<Expander>();
			foreach (var expander in expanders)
			{
				expander.Background = Constants.BackColor4_BlueBayoux;
				expander.BorderBrush = Constants.LineBorderColor2_Nepal;
				expander.Foreground = Constants.ForeColor2_PapayaWhip;
				expander.FontSize = Constants.FontSize;

				var stackPanel = expander.Content as StackPanel;
				if (stackPanel == null)
				{
					continue;
				}

				stackPanel.Background = Constants.BackColor4_BlueBayoux;

				var buttons = new List<Button>();
				buttons.AddRange(stackPanel.Children.OfType<Button>());
				foreach (var button in buttons)
				{
					button.Foreground = Constants.ForeColor1_BigStone;
				}
			}
		}

	    public void LvlVisual()
	    {
	        if (Login.Lvl == 0)
	        {
	            CalculationExpander.Visibility = Visibility.Collapsed;
	            DetailsForPrintButton.Visibility = Visibility.Collapsed;
	            ReleaseTableButton.Visibility = Visibility.Collapsed;

	        }
	        else
	        {
	            CalculationExpander.Visibility = Visibility.Visible;
	            DetailsForPrintButton.Visibility = Visibility.Visible;
	            ReleaseTableButton.Visibility = Visibility.Visible;
            }
	    }

        /// <summary>
        /// Выбор пункта меню [Сводная по изделиям в разрезе цехов]
        /// </summary>
        private void SummeryOfProductsInContextOfWorkGuildButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        PageSwitcher.Switch(new SummeryOfProductsInContextOfWorkGuildReport());
	    }

	    /// <summary>
	    /// Нажатие кнопки [Сводная по изделиям в разрезе цехов по разрядам]
	    /// </summary>
	    private void SummeryOfProductsInContextOfWorkGuildOfDischargesButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        PageSwitcher.Switch(new SummeryOfProductInContextOfWorkGuildAndOfDischargeReport());
	    }

	    /// <summary>
	    /// Нажатие кнопки [Сводная по изделиям в разрезе цехов, участков(для цехов)]
	    /// </summary>
	    private void SummeryOfProductsInContextOfWorkGuildAndAreaForWorkGuildButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        PageSwitcher.Switch(new SummeryOfProductInContextOfWorkGuildAndAreaForWorkGuildReport());
	    }

        /// <summary>
        /// Нажатие кнопки [Сводная по изделиям по профессиям в разрезе разрядов]
        /// </summary>
        private void SummeryOfProductsInContextOfProfessionOfDischargesButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        PageSwitcher.Switch(new SummeryOfProductInContextOfProfessionAndOfDischargeReport());
	    }

        /// <summary>
        /// Нажатие кнопки [Сводная по изделиям по профессиям в разрезе цехов, участков]
        /// </summary>
        private void SummeryOfProductsOfProfessionInContextOfWorkGuildAndAreaButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        PageSwitcher.Switch(new SummeryOfProductOfProfessionInContextOfWorkGuildAndOfAreaReport());
	    }

        /// <summary>
        /// Нажатие кнопки [Печать по изделиям в разрезе детале-операций("сжатая печать")]
        /// </summary>
        private void PrintingOfProductInContextOfDetalOperationsButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        PageSwitcher.Switch(new PrintingOfProsuctInContextOfDetalOperationsReport());
	    }

	    /// <summary>
	    /// Выбор пункта меню [Печать по изделиям в разрезе деталей]
	    /// </summary>
	    private void PrintingOfProductInContextOfDetailsButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        PageSwitcher.Switch(new PrintingOfProsuctInContextOfDetailsReport());
	    }

        /// <summary>
        /// Нажатие кнопки [Отметка деталей для расчета]
        /// </summary>
        private void DetailsForCalculationButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        PageSwitcher.Switch(new DetailsForCalculationTable());
	    }

	    /// <summary>
	    /// Нажатие кнопки [Отметка деталей для печати]
	    /// </summary>
	    private void DetailsForPrintButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        PageSwitcher.Switch(new DetailsForPrintTable());
	    }

	    /// <summary>
	    /// Нажатие кнопки [Расчет]
	    /// </summary>
	    private void CalculationButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        const MessageBoxButton messageButtonsYesNo = MessageBoxButton.YesNo;
	        const MessageBoxImage messageTypeQuestion = MessageBoxImage.Warning;
	        var nu67Dbf = Properties.Settings.Default.Nu67Dbf;
	        var messsage = "Расчет будет выполняться 10-15 мин. ";
	        if (nu67Dbf != "nu67")
	        {
	            messsage += $"\n\nДля расчета будет использоваться база {nu67Dbf}.dbf \n";

	        }

	        messsage += "\nВы хотите продолжить?";
            var dialogResult = MessageBox.Show(messsage, "Внимание!", 
                                                messageButtonsYesNo, messageTypeQuestion);
	        if (dialogResult == MessageBoxResult.No)
	        {
	            return;
	        }
            CalculationWorkingStandarts.Calculation();
	        MessageBox.Show("Расчет закончен удачно.", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Нажатие кнопки [Редактирование выпуска изделий]
        /// </summary>
        private void ReleaseTableButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        PageSwitcher.Switch(new ReleaseTable());
        }

        /// <summary>
        /// Нажатие кнопки [Просмотр аннулируемых изделий]
        /// </summary>
        private void CancelledDetailButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        PageSwitcher.Switch(new CancelledDetailTable());
        }

	    /// <summary>
	    /// Нажатие кнопки [Трудоемкости и зарплаты на сбор.ед. по цехам]
	    /// </summary>
	    private void ComplexityAndSalaryOnUnitByWorkGuildsButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        PageSwitcher.Switch(new ComplexityAndSalaryOnUnitByWorkGuildsReport());
        }

        /// <summary>
        /// Нажатие кнопки [Печать аннулируемых изделий]
        /// </summary>
        private void PrintCancelledDetailButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        PageSwitcher.Switch(new CancelledDetailsReport());
        }

	    /// <summary>
	    /// Нажатие кнопки [Расчёт численности работников по цехам на выпуск]
	    /// </summary>
	    private void CalculationNumberWorkguildWorkersRealaseButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        PageSwitcher.Switch(new CalculationNumberWorkguildWorkersRealasesReport());
	    }

	    /// <summary>
	    /// Нажатие кнопки [Печать аннулируемых изделий за месяц]
	    /// </summary>
        private void PrintCancelledDetailForMonthButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        PageSwitcher.Switch(new CancelledDetailsForMonthReport());
        }
	}
}
