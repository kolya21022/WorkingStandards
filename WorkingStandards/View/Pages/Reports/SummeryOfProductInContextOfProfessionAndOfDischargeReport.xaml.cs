﻿using System;
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
	/// Предпросмотр и печать отчёта [Сводки по изделиям в разрезе профессий по разрядам]
	/// </summary>
	/// <inheritdoc cref="Page" />
	public partial class SummeryOfProductInContextOfProfessionAndOfDischargeReport : IPageable
	{
		private const string ReportFileName = "SummeryOfProductInContextOfProfessionAndOfDischarge.rdlc";

		private string _reportFile;                             // Абсолютный путь к файлу отчёта
		private ReportDataSource _reportDataSource;             // Источник данных печатаемого списка
		private IEnumerable<ReportParameter> _reportParameters; // Одиночные строковые параметры отчёта
		public SummeryOfProductInContextOfProfessionAndOfDischargeReport()
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
			const bool isKoefT = true;
			const bool isKoefZ = true;
			const bool isWorkGuild = false;
			const bool isArea = false;
			const bool isWorkGuildSpecifiedOrAll = false;
			const bool isProduct = true;
		    const bool isDetail = false;

            const bool isProductSpecifiedOrAll = false;
		    const bool isAssemblyUnit = false;
		    const bool isMonthYear = false;

		    const bool isTimeFund = false;
		    const bool isProcentageOfLossTime = false;
		    const bool isProcentageOfPerformanceStandarts = false;
		    const bool isAreaSpecifiedOrAll = false;

            const string message = "Выберите изделие и введите коэффициенты";
			var parametersWindow = new ReportParametersWindow(isPeriod, isMounthOrYeath, isDate, isDatePeriod, isKoefT,
			    isKoefZ, isWorkGuild, isArea, isWorkGuildSpecifiedOrAll, isProduct, isDetail, isProductSpecifiedOrAll, 
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
			var loadDateTime = DateTime.Today;

			var nullableKoeft = parametersWindow.KoefTDecimalUpDown.Value;
			var nullableKoefz = parametersWindow.KoefZDecimalUpDown.Value;

			if (nullableKoeft == null || nullableKoefz == null)
			{
				const string errorMessage = "Не указаны коэффициенты";
				const MessageBoxButton buttons = MessageBoxButton.OK;
				const MessageBoxImage messageType = MessageBoxImage.Error;
				MessageBox.Show(errorMessage, PageLiterals.HeaderValidation, buttons, messageType);
				return;
			}

			var koefT = nullableKoeft;
			var koefZ = nullableKoefz;

			var nullableProduct = parametersWindow.SelectedProduct();

			if (nullableProduct == null)
			{
				const string errorMessage = "Не указано изделие";
				const MessageBoxButton buttons = MessageBoxButton.OK;
				const MessageBoxImage messageType = MessageBoxImage.Error;
				MessageBox.Show(errorMessage, PageLiterals.HeaderValidation, buttons, messageType);
				return;
			}

			var product = nullableProduct;

			// Формирование одиночных строковых параметров отчёта
			_reportParameters = new[] { new ReportParameter("Date", loadDateTime.ToShortDateString()),
				new ReportParameter("Koeft", koefT.ToString()),
				new ReportParameter("Koefz", koefZ.ToString()),
				new ReportParameter("ProductId", product.Id.ToString(CultureInfo.InvariantCulture))
			};
			try
			{
				var resultReportList = SummeryOfProductInContextOfProfessionAndOfDischargeService.GetSummeryOfProductInContextOfProfessionAndOfDischarge(product.Id);
				const string dataSourceName = "SummeryOfProductInContextOfProfessionAndOfDischarge";
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