using System;
using System.Collections.Generic;
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
	/// Предпросмотр и печать отчёта [Расчёт численности рабочих по цехам на выпуск]
	/// </summary>
	/// <inheritdoc cref="Page" />
	public partial class CalculationNumberWorkguildWorkersRealasesReport : IPageable
	{
		private const string ReportFileName = "CalculationNumberWorkguildWorkersRealase.rdlc";
		private string _reportFile;                             // Абсолютный путь к файлу отчёта
		private ReportDataSource _reportDataSource;             // Источник данных печатаемого списка
		private IEnumerable<ReportParameter> _reportParameters; // Одиночные строковые параметры отчёта
		public CalculationNumberWorkguildWorkersRealasesReport()
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
			const bool isProduct = false;
		    const bool isDetail = false;
			const bool isProductSpecifiedOrAll = false;
		    const bool isAssemblyUnit = false;
		    const bool isMonthYear = false;

            const bool isTimeFund = true;
			const bool isProcentageOfLossTime = true;
			const bool isProcentageOfPerformanceStandarts = true;
			const bool isAreaSpecifiedOrAll = true;

			const string message = "Укажите цех(цех и участок), фонд времени, процент потери времени \nи процент выполнения норм выработки.";
			var parametersWindow = new ReportParametersWindow(isPeriod, isMounthOrYeath, isDate, isDatePeriod, isKoefT, isKoefZ,
				isWorkGuild, isArea, isWorkGuildSpecifiedOrAll, isDetail,
				isProduct, isProductSpecifiedOrAll, isAssemblyUnit, isMonthYear, isTimeFund, 
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

			//Получение параметра фонда времени

			var nullabletimeFund = parametersWindow.TimeFundDecimalUpDown.Value;

			if (nullabletimeFund == null)
			{
				const string errorMessage = "Не указан фонд времени";
				const MessageBoxButton buttons = MessageBoxButton.OK;
				const MessageBoxImage messageType = MessageBoxImage.Error;
				MessageBox.Show(errorMessage, PageLiterals.HeaderValidation, buttons, messageType);
				return;
			}

			var timeFund = nullabletimeFund;

			//Получение параметра процента потерь времени

			var nullableprocentageOfLossTime = parametersWindow.ProcentageOfLossTimeDecimalUpDown.Value;

			if (nullableprocentageOfLossTime == null)
			{
				const string errorMessage = "Не указан процент потери времени";
				const MessageBoxButton buttons = MessageBoxButton.OK;
				const MessageBoxImage messageType = MessageBoxImage.Error;
				MessageBox.Show(errorMessage, PageLiterals.HeaderValidation, buttons, messageType);
				return;
			}

			var procentageOfLossTime = nullableprocentageOfLossTime;
			//Получение параметра процента выполнения норм выработки

			var nullableprocentageOfPerformaneStandarts = parametersWindow.ProcentageOfPerformanceStandartsDecimalUpDown.Value;

			if (nullableprocentageOfPerformaneStandarts == null)
			{
				const string errorMessage = "Не указан процент выполнения норм выработки";
				const MessageBoxButton buttons = MessageBoxButton.OK;
				const MessageBoxImage messageType = MessageBoxImage.Error;
				MessageBox.Show(errorMessage, PageLiterals.HeaderValidation, buttons, messageType);
				return;
			}

			var procentageOfPerformaneStandarts = nullableprocentageOfPerformaneStandarts;

		    //Получение параментра номера цеха
            var workGuild = parametersWindow.SelectedWorkGuildOrAll();

            //Получение параментра номера участка
            var area = parametersWindow.SelectedAreaOrAll();



		    string workGuildArea;
		    if (workGuild == null)
		    {
		        workGuildArea = "по всему заводу";
		    }
		    else
		    {
		        workGuildArea = area == null 
		            ? $"по цеху {workGuild.Id} по всем участкам" 
		            : $"по цеху {workGuild.Id} по участку {area.Id}";
		    }

			// Формирование одиночных строковых параметров отчёта
			_reportParameters = new[] { new ReportParameter("FondVrem", timeFund.ToString()),
				new ReportParameter("Procpoter", procentageOfLossTime.ToString()),
				new ReportParameter("Procnorm", procentageOfPerformaneStandarts.ToString()),
			    new ReportParameter("WorkGuildArea", workGuildArea)
			};
			try
			{
				var resultReportList = CalculationNumberWorkguildWorkersRealasesService.GetCalculationNumberWorkguildWorkersRealases(workGuild, area);
				const string dataSourceName = "CalculationNumberWorkguildWorkersRealase";
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

