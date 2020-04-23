﻿using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Security.Principal;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using WorkingStandards.View.Util;
using WorkingStandards.View.Windows;

namespace WorkingStandards.Util
{
	/// <summary>
	/// Общие утилитарные методы приложения
	/// </summary>
	public static class Common
	{
		/// <summary>
		/// Получение заголовка главного окна приложения.
		/// В заголовке название, версия, дата/время сборки, текущий пользователь и т.д.
		/// </summary>
		public static string GetApplicationTitle(Assembly assembly)
		{
			var currentIdentity = WindowsIdentity.GetCurrent();                           // текущий пользователь
																						  // ReSharper disable once ConditionIsAlwaysTrueOrFalse
			var user = currentIdentity != null ? currentIdentity.Name : string.Empty;   // имя пользователя
			var versionInfo = GetFileVersionInfo(assembly);
			var appName = versionInfo.Comments;
			var productVersion = versionInfo.ProductVersion;
			var buildDate = GetBuildDateTime(assembly);
			var titlePattern = GenerateTitlePattern(buildDate != null);
			return string.Format(titlePattern, appName, productVersion, buildDate != null
					? string.Format(Constants.BuildDateTimePattern, buildDate)
					: string.Empty, user);
		}

		/// <summary>
		/// Формирование шаблона заголовка окна приложения, в зависимости от наличия DateTime сборки приложения
		/// </summary>
		private static string GenerateTitlePattern(bool isbuildDateExist)
		{
			const string titlePatternPart1 = "{0}, {1}";
			const string titlePatternPart2WithDate = " [Build: {2}]";
			const string titlePatternPart2WithouDate = "{2}";
			const string titlePatternPart3 = " [{3}]";
			return titlePatternPart1 + (isbuildDateExist
					   ? titlePatternPart2WithDate
					   : titlePatternPart2WithouDate) + titlePatternPart3;
		}

		/// <summary>
		/// Получение объекта класса FileVersionInfo из указанной Assembly
		/// (Требуется для чтения аттрибутов сборки приложения: название, версия и т.д.)
		/// </summary>
		private static FileVersionInfo GetFileVersionInfo(Assembly assembly)
		{
			return FileVersionInfo.GetVersionInfo(assembly.Location);
		}

		/// <summary>
		/// Метод получения даты/времени компиляции указанной сборки
		/// </summary>
		private static DateTime? GetBuildDateTime(Assembly assembly)
		{
			var current = DateTime.Now;
			var universal = current.ToUniversalTime();
			var gmtOffset = (current - universal).TotalHours;
			try
			{
				var file = assembly.Location;
				const int headerOffset = 60;
				const int linkerTimestampOffset = 8;
				var buffer = new byte[2048];
				using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
				{
					stream.Read(buffer, 0, 2048);
				}
				var offset = BitConverter.ToInt32(buffer, headerOffset);
				var startIdnex = offset + linkerTimestampOffset;
				var secondsSince1970 = BitConverter.ToInt32(buffer, startIdnex);
				var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				var linkTimeUtc = epoch.AddSeconds(secondsSince1970);
				return linkTimeUtc.AddHours(gmtOffset);
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// Получения объекта класса цвета визуальных элементов, по hex-коду цвета
		/// </summary>
		public static SolidColorBrush BrushHex(string hexColor)
		{
			var solidColorBrush = (SolidColorBrush)new BrushConverter().ConvertFrom(hexColor);
			if (solidColorBrush != null && !solidColorBrush.IsFrozen)
			{
				// NOTE: Делаем цвет нередактируемым, так как без этого сильно просаживается производительность.
				// Подробнее (Freezable Objects): https://msdn.microsoft.com/en-us/library/aa970683(v=vs.85).aspx
				solidColorBrush.Freeze();
			}
			return solidColorBrush;
		}

		/// <summary>
		/// Русская локаль приложения и десятичный разделитель 'точка' по-умолчанию
		/// </summary>
		public static void SetRussianLocaleAndDecimalSeparatorDot()
		{
			const string decimalSeparator = ".";
			const string russianLocale = "ru-RU";
			var russianCultureInfo = new CultureInfo(russianLocale)
			{
				NumberFormat = { NumberDecimalSeparator = decimalSeparator }
			};
			Thread.CurrentThread.CurrentCulture = russianCultureInfo;
			Thread.CurrentThread.CurrentUICulture = russianCultureInfo;
			var cultureIetfLanguageTag = CultureInfo.CurrentCulture.IetfLanguageTag;
			var xmlLanguage = XmlLanguage.GetLanguage(cultureIetfLanguageTag);
			var frameworkPropertyMetadata = new FrameworkPropertyMetadata(xmlLanguage);
			FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), frameworkPropertyMetadata);
		}

		/// <summary>
		/// Получение Mutex'а приложения: состоит из названия, версии и guid продукта
		/// </summary>
		public static Mutex GetApplicationMutex(out bool isCreatedNew)
		{
			const string mutexFormat = "[{0} : {1} : {2}]";
			var assembly = Assembly.GetExecutingAssembly();
			var product = ProductName();
			var version = ProductVersion();
			var guid = ((GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value;
			return new Mutex(true, string.Format(mutexFormat, product, version, guid), out isCreatedNew);
		}

		/// <summary>
		/// Получение названия приложения из текущей сборки
		/// </summary>
		private static string ProductName()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var versionInfo = GetFileVersionInfo(assembly);
			return versionInfo.ProductName;
		}

		/// <summary>
		/// Получение версии приложения из текущей сборки
		/// </summary>
		private static string ProductVersion()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var versionInfo = GetFileVersionInfo(assembly);
			return versionInfo.ProductVersion;
		}

		/// <summary>
		/// Переключение языка ввода на язык текущей локали (русский)
		/// </summary>
		public static void SetKeyboardInputFromCurrentLocale()
		{
			InputLanguageManager.Current.CurrentInputLanguage = Thread.CurrentThread.CurrentCulture;
		}

		/// <summary>
		/// Обработка неотловленных исключений в стартовом классе приложения (App)
		/// В конце выполнения метода приложение завершается, и исключение со стектрейсом записывается 
		/// в текстовый error-файл в пользовательский каталог операционной системы [ c:\Users\username\Documents\ ] 
		/// </summary>
		public static void RootExceptionHandler(object senderIsDispatcher, DispatcherUnhandledExceptionEventArgs ex)
		{
			const string filenamePattern = "Error-log [{0}, {1}].txt";
			const string fileMessagePattern = "{0} {1} {2} {1} {3} {1} {4} {5} {1}";
			const string messageBoxHeader = PageLiterals.HeaderCriticalError;
			const string messageBoxPattern = "Необработанная исключительная ситуация:" +
											 "{0} {1} {0} {2} {0} " +
											 "Копия сообщения записана в файл: {3} {4} {0}" +
											 "Приложение будет завершено";
			var newLine = Environment.NewLine;
			var doubleNewLine = newLine + newLine;

			var versionInfo = GetFileVersionInfo(Assembly.GetExecutingAssembly());
			var appName = versionInfo.Comments;
			var productVersion = versionInfo.ProductVersion;
			var errorsLogFile = string.Format(filenamePattern, appName, productVersion);

			// Запись неотловленных исключений со стектрейсом в error-файл
			var exceptionMessage = ex.Exception.Message;
			var exceptionStackTrace = ex.Exception.StackTrace;
			var personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var pathErrorLogFile = Path.Combine(personalFolder, errorsLogFile);
			var messageToFile = string.Format(fileMessagePattern, DateTime.Now, newLine, ex, exceptionMessage,
				exceptionStackTrace, doubleNewLine);
			AppendToBeginFile(pathErrorLogFile, messageToFile);

			// Показ проблемы пользователю
			const MessageBoxImage messageBoxType = MessageBoxImage.Error;
			const MessageBoxButton messageBoxButtons = MessageBoxButton.OK;
			var messageToMessageBox = string.Format(messageBoxPattern, doubleNewLine, exceptionMessage,
				exceptionStackTrace, newLine, pathErrorLogFile);
			MessageBox.Show(messageToMessageBox, messageBoxHeader, messageBoxButtons, messageBoxType);

			ex.Handled = true;
			Application.Current.Shutdown();
		}

		/// <summary>
		/// Добавление текста в начало указанного текстового файла
		/// </summary>
		private static void AppendToBeginFile(string filePath, string message)
		{
			var oldFileContent = string.Empty;
			if (File.Exists(filePath))
			{
				oldFileContent = File.ReadAllText(filePath);
			}
			File.WriteAllText(filePath, message + oldFileContent);
		}

		/// <summary>
		/// Отображение детальной информации со стектрейсом о выброшеном исключении в отдельном окне.
		/// Для StorageException в отдельном поле, отображается 'возможная причина'.
		/// </summary>
		public static void ShowDetailExceptionMessage(Exception ex)
		{
			const float screenPersent = 0.75f; // относительный размер окна в случае, если родителького ещё нет 
			var ownerWindow = GetOwnerWindow();
			var errorDetailWindow = new ErrorDetailWindow(ex);
			if (ownerWindow == null)           // если родительское окно ещё не создано или ещё не было отображено
			{
				errorDetailWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				errorDetailWindow.Height = SystemParameters.PrimaryScreenHeight * screenPersent;
				errorDetailWindow.Width = SystemParameters.PrimaryScreenWidth * screenPersent;
			}
			else
			{
				errorDetailWindow.Owner = ownerWindow;
			}
			errorDetailWindow.ShowDialog();
		}

		/// <summary>
		/// Получение главного окна приложения, для установки, в качестве родительского, дочерним окнам.
		/// Требуется из-за того, что нельзя установить окно дочерним, если оно ещё не было отображено.
		/// </summary>
		public static Window GetOwnerWindow()
		{
			var mainWindow = Application.Current.MainWindow;
			return mainWindow != null && mainWindow.IsVisible ? mainWindow : null;
		}

		/// <summary>
		/// Получение каталога exe-файла приложения
		/// </summary>
		private static string ApplicationFolder()
		{
			return Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
		}

		/// <summary>
		/// Отображение сообщения об уже запущеной копии этого приложения в операционной системе
		/// </summary>
		public static void ShowMessageForMutexAlreadyRunning()
		{
			var isRunPattern = "В этой операционной системе уже запущен один экземпляр приложения:" +
							   Environment.NewLine + "{0} : {1}";
			var product = ProductName();
			var version = ProductVersion();
			var alreadyRunMessage = string.Format(isRunPattern, product, version);
			const MessageBoxResult defaultButton = MessageBoxResult.OK;
			const MessageBoxButton messageBoxButtons = MessageBoxButton.OK;
			const MessageBoxImage messageBoxType = MessageBoxImage.Information;
			MessageBox.Show(alreadyRunMessage, product, messageBoxButtons, messageBoxType, defaultButton);
		}

		/// <summary>
		/// Метод расширения для клонирования List объектов, реализующих интерфейс ICloneable
		/// </summary>
		public static List<T> SerializableListClone<T>(this IEnumerable<T> listToClone) where T : ICloneable
		{
			return listToClone.Select(item => (T)item.Clone()).ToList();
		}

		/// <summary>
		/// Чтение 1 байта по указанному смещению из указанного файла
		/// </summary>
		public static byte ReadOneByteFromFile(string file, long offset)
		{
			var buffer = new byte[1];
			using (var reader = new BinaryReader(new FileStream(file, FileMode.Open)))
			{
				reader.BaseStream.Seek(offset, SeekOrigin.Begin);
				reader.Read(buffer, 0, 1);
				return buffer[0];
			}
		}

		/// <summary>
		/// Проверка оканчивается ли указанная строка на указанную подстроку
		/// </summary>
		public static bool LineEndOn(string line, string ended)
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				return false;
			}
			const StringComparison compareIgnoreCase = StringComparison.OrdinalIgnoreCase;
			return line.Length > ended.Length &&
				   string.Equals(ended, line.Substring(line.Length - ended.Length), compareIgnoreCase);
		}

		public static DateTime? GetSafeFoxProDate(object rawDateTime)
		{
			DateTime? foxproNullDateTime = new DateTime(1899, 12, 30);
			var isDateTime = rawDateTime is DateTime;
			if (rawDateTime == null || rawDateTime is DBNull || !isDateTime)
			{
				return null;
			}
			var nullableDateTime = (DateTime?)rawDateTime;
			return foxproNullDateTime.Equals(nullableDateTime) ? null : nullableDateTime;
		}

		public static Dictionary<int, string> MonthsFullNames()
		{
			const int offset = 1;
			// ReSharper disable once PossibleNullReferenceException
			var arrayMonths = DateTimeFormatInfo.GetInstance(CultureInfo.CurrentCulture).MonthNames;
			var months = new Dictionary<int, string>();
			var i = 0;
			foreach (var month in arrayMonths)
			{
				var number = i + offset;
				months.Add(number, month);
				if (++i >= arrayMonths.Length - offset) // Тринадцатый пустой месяц
				{
					break;
				}
			}
			return months;
		}

		public static bool NullableSequenceEqual<T>(this IEnumerable<T> first, IEnumerable<T> second)
		{
			if (first == null || second == null)
			{
				return first == null && second == null;
			}
			var firstArray = first as T[] ?? first.ToArray();
			var secondArray = second as T[] ?? second.ToArray();
			return firstArray.Length == secondArray.Length && firstArray.SequenceEqual(secondArray);
		}


		/// <summary>
		/// Получение начала месяца со смещением, в зависимости от указанной даты 
		/// (если 1 число месяца, указывается начало предыдущего месяца)
		/// </summary>
		public static DateTime GetBeginOfMonthWithOffset(DateTime dateTime)
		{
			const int firstDayOfMonth = 1;
			if (dateTime.Day != firstDayOfMonth)
			{
				return new DateTime(dateTime.Year, dateTime.Month, firstDayOfMonth);
			}
			var dateTimeResult = dateTime.AddMonths(-1);
			return new DateTime(dateTimeResult.Year, dateTimeResult.Month, firstDayOfMonth);
		}

		/// <summary>
		/// Проверка попадает ли дата в диапазон
		/// </summary>
		public static bool IsDateRange(DateTime dateToCheck, DateTime startDate, DateTime endDate)
		{
			return dateToCheck >= startDate && dateToCheck <= endDate;
		}

		/// <summary>
		/// Число символов целой части Decimal
		/// </summary>
		public static int IntegerPartDigitCount(decimal value)
		{
			value = Math.Abs(value);
			var count = 1;
			while ((value /= 10M) > 1M)
			{
				++count;
			}
			return count;
		}

		/// <summary>
		/// Число символов части после запятой в Decimal
		/// </summary>
		public static int FractionalPartDigitCount(decimal value)
		{
			value = Math.Abs(value);
			var count = 0;
			while (value % 1M != 0M)
			{
				++count;
				value *= 10M;
			}
			return count;
		}

		/// <summary>
		/// Формирование пути к RDLC-файлу отчёта
		/// </summary>
		public static string GetReportFilePath(string reportFileName)
		{
			var appFolder = ApplicationFolder();
			const string dataFolder = Constants.AppDataFolder;
			const string reportFolder = Constants.ReportFolder;
			return Path.Combine(appFolder, dataFolder, reportFolder, reportFileName);
		}

		/// <summary>
		/// Если дата равна null
		/// </summary>
		private static bool IsDateEmpty(object rawDateTimeInDataTable)
		{
			var nullDateTime = new DateTime(1899, 12, 30);
			if (rawDateTimeInDataTable is DBNull)
			{
				return true;
			}
			var nullableValue = rawDateTimeInDataTable as DateTime?;
			return nullableValue == null || ((DateTime)nullableValue).Date.Equals(nullDateTime);
		}

		/// <summary>
		/// Если в combobox -> SelectionBoxItem нужно отделить Item от систимнего сообщения
		/// </summary>
		public static string CboParser(string controlString)
		{
			if (controlString.Contains(':'))
			{
				controlString = controlString.Split(':')[1].TrimStart(' ');
			}
			return controlString;
		}
	}
}
