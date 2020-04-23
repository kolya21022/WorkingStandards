using System;
using System.Windows;

using WorkingStandards.View.Windows;
using WorkingStandards.View.Pages;

namespace WorkingStandards.View.Util
{
	/// <summary>
	/// Служебный класс коммутатор (переключатель) страниц главного окна
	/// </summary>
	class PageSwitcher
	{
		/// <summary>
		/// Замена текущей отображаемой страницы главного окна
		/// </summary>
		public static void Switch(IPageable page)
		{
			const string windowTypeNotExpected = "Не удаётся получить главное окно или его тип не подходящий.";
			var mainWindow = Application.Current.MainWindow as MainWindow;
			if (mainWindow != null)
			{
				mainWindow.Navigate(page);
			}
			else
			{
				var message = string.Format(PageLiterals.LogicErrorPattern, windowTypeNotExpected);
				throw new ApplicationException(message);
			}
		}
	}
}
