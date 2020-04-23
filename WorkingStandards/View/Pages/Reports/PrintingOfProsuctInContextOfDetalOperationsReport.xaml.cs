using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using WorkingStandards.Db;
using WorkingStandards.Services.Reports;
using WorkingStandards.View.Util;
using WorkingStandards.View.Windows;
using WorkingStandards.Util;
using Microsoft.Reporting.WinForms;

namespace WorkingStandards.View.Pages.Reports
{
	/// <summary>
	/// Предпросмотр и печать отчёта [Печать по изделиям в разрезе детале-операций("сжатая печать"))]
	/// </summary>
	/// <inheritdoc cref="Page" />
	public partial class PrintingOfProsuctInContextOfDetalOperationsReport : IPageable
	{
		private const string ReportFileName = "PrintingOfProsuctInContextOfDetalOperations.rdlc";
		private string _reportFile;                             // Абсолютный путь к файлу отчёта
		private ReportDataSource _reportDataSource;             // Источник данных печатаемого списка
		private IEnumerable<ReportParameter> _reportParameters; // Одиночные строковые параметры отчёта
		public PrintingOfProsuctInContextOfDetalOperationsReport()
		{
			InitializeComponent();
			VisualInitializeComponent();
			AdditionalInitializeComponent();
		}

		/// <summary>
		/// Визуальная инициализация страницы (цвета и размеры шрифтов контролов)
		/// </summary>
		/// <inheritdoc />
		public void VisualInitializeComponent()
		{
			FontSize = Constants.FontSize;

			// Заголовок страницы
			TitlePageGrid.Background = Constants.BackColor4_BlueBayoux;
			var titleLabels = TitlePageGrid.Children.OfType<Label>();
			foreach (var titleLabel in titleLabels)
			{
				titleLabel.Foreground = Constants.ForeColor2_PapayaWhip;
			}
		}

		/// <summary>
		/// Получение/формирование параметров отчёта, DataSource (список сущностей для таблицы), пути файла и заголовка
		/// </summary>
		/// <inheritdoc />
		public void AdditionalInitializeComponent()
		{
			_reportFile = Common.GetReportFilePath(ReportFileName); // Путь к файлу отчёта

			// Запрос параметров отчёта в отдельном окне
			const bool isPeriod = false;
			const bool isMounthOrYeath = false;
			const bool isDate = false;
			const bool isDatePeriod = false;
			const bool isKoefT = false;
			const bool isKoefZ = false;
			const bool isWorkGuild = false;
			const bool isArea = false;
			const bool isWorkGuildSpecifiedOrAll = true;
			const bool isProduct = true;
		    const bool isDetail = false;

            const bool isProductSpecifiedOrAll = false;
		    const bool isAssemblyUnit = false;
		    const bool isMonthYear = false;

		    const bool isTimeFund = false;
		    const bool isProcentageOfLossTime = false;
		    const bool isProcentageOfPerformanceStandarts = false;
		    const bool isAreaSpecifiedOrAll = false;

            const string message = "Укажите цех и изделие";
			var parametersWindow = new ReportParametersWindow(isPeriod, isMounthOrYeath, isDate, isDatePeriod, 
			    isKoefT, isKoefZ, isWorkGuild, isArea, isWorkGuildSpecifiedOrAll, isProduct, isDetail, isProductSpecifiedOrAll, 
			    isAssemblyUnit, isMonthYear, isTimeFund,
			    isProcentageOfLossTime, isProcentageOfPerformanceStandarts, isAreaSpecifiedOrAll, message)
			{
				Owner = Common.GetOwnerWindow()
			};
			parametersWindow.ShowDialog();
			if (!parametersWindow.DialogResult.HasValue || parametersWindow.DialogResult != true)
			{
				return;
			}
			// Получение введённых пользователем параметров

			var loadDateTime = DateTime.Today;

			var nullableProduct = parametersWindow.SelectedProduct();
			if (nullableProduct == null)
			{
				const string errorMessage = "Не указан код изделия";
				const MessageBoxButton buttons = MessageBoxButton.OK;
				const MessageBoxImage messageType = MessageBoxImage.Error;
				MessageBox.Show(errorMessage, PageLiterals.HeaderValidation, buttons, messageType);
				return;
			}

			var product = nullableProduct;

		    var workGuild = parametersWindow.SelectedWorkGuildOrAll();

            // Формирование одиночных строковых параметров отчёта
            _reportParameters = new[] { new ReportParameter("Date", loadDateTime.ToShortDateString()),
				new ReportParameter("WorkGuild", workGuild != null ? "цех " + workGuild.Id.ToString(CultureInfo.InvariantCulture) : "по всему заводу")
			};
			try
			{
				var resultReportList = PrintingOfProsuctInContextOfDetalOperationsService.GetPrintingOfProsuctInContextOfDetalOperations(product.Id, workGuild);
				const string dataSourceName = "PrintingOfProductInContextOfDetalOperations";
				_reportDataSource = new ReportDataSource(dataSourceName, resultReportList);
				ReportViewer.Load += ReportViewer_Load;     // Подписка на метод загрузки и отображения отчёта
			}
			catch (StorageException ex)
			{
				Common.ShowDetailExceptionMessage(ex);
			}
		}

		/// <summary>
		/// Инициализация и отображение отчёта
		/// </summary>
		private void ReportViewer_Load(object senderIsReportViewer, EventArgs eventArgs)
		{
			var report = senderIsReportViewer as ReportViewer;
			if (report == null)
			{
				return;
			}
			report.SetDisplayMode(DisplayMode.PrintLayout);         // Режим предпросмотра "Разметка страницы"
			report.LocalReport.ReportPath = _reportFile;            // Путь к файлу отчёта
			report.LocalReport.DataSources.Clear();
			report.ZoomMode = ZoomMode.PageWidth;                   // Режим масштабирования "По ширине страницы"
			report.Visible = true;

			report.LocalReport.SetParameters(_reportParameters);    // Одиночные строковые параметры
			report.LocalReport.DataSources.Add(_reportDataSource);  // Выводимый список
			report.RefreshReport();
		}

		/// <summary>
		/// Горячие клавиши текущей страницы
		/// </summary>
		/// <inheritdoc />
		public string PageHotkeys()
		{
			const string closePageBackToListHotkey = PageLiterals.HotkeyLabelClosePageBackToList;
			return closePageBackToListHotkey;
		}
		public void Page_OnKeyDown(object senderIsPageOrWindow, KeyEventArgs eventArgs)
		{
			if (eventArgs.Key != Key.Escape)
			{
				return;
			}
			// Если нажат [Esc] - выходим к списку доверенностей
			eventArgs.Handled = true;
			PageSwitcher.Switch(new StartPage());
		}
	}
}
