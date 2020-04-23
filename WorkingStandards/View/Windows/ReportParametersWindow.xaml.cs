using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using WorkingStandards.Db;
using WorkingStandards.Services;
using WorkingStandards.Entities.External;
using WorkingStandards.View.Util;
using WorkingStandards.Util;

namespace WorkingStandards.View.Windows
{
    /// <summary>
    /// Окно получения параметров отчётов.
    /// </summary>
    /// <inheritdoc cref="Window" />
    public partial class ReportParametersWindow
    {
        /// <summary>
        /// Локальное хранилище списка для поиска [Изделий].
        /// (загружается при создании страницы и служит неизменяемым источником данных при фильтрации)
        /// </summary>
        private List<Product> _searchProductStorage = new List<Product>();

        private List<Detail> _searchDetailStorage = new List<Detail>();

        // Флаги отображаемых вводимых в окне параметров
        private readonly bool _isPeriod; // Период
        private readonly bool _isMonthOrYear; // Начало отсчета месяц или год
        private readonly bool _isDate; // дата
        private readonly bool _isDatePeriod; // дата
        private readonly bool _isKoefT; // коэффициент ТР_ТЬ
        private readonly bool _isKoefZ; // коэффициент ЗАРПЛ.
        private readonly bool _isWorkGuild; //цех
        private readonly bool _isArea; //участок
        private readonly bool _isWorkGuildSpecifiedOrAll; //Весь завод/Цех
        private readonly bool _isProduct; //Изделие
        private readonly bool _isDetail; //Деталь
        private readonly bool _isProductSpecifiedOrAll; // По всем изделиям/Указанное
        private readonly bool _isAssemblyUnit; // Сборочноя еденица
        private readonly bool _isMonthYear;   // месяц и год
        private readonly bool _isTimeFund; // Фонд времени
        private readonly bool _isProcentageOfLossTime; // Процент потерянного времени
        private readonly bool _isProcentageOfPerformanceStandarts; // Процент выполнения норм выработки
        private readonly bool _isAreaSpecifiedOrAll; //Все участки/Указанный


        private readonly string _hintMessage; // посказка ввода для пользователя

        // Указанные пользователем значения
		private string _isSelectedPeriod;
		private string _monthOrYear;
		private WorkGuild _specifiedWorkGuild;
		private WorkGuild _specifiedisWorkGuildSpecifiedOrAll;
		private Area _specifiedArea;
        private Area _specifiedisAreaSpecifiedOrAll;
        private Product _selectedProduct;
        private Detail _selectedDetail;
        private Product _specifiedisProductSpecifiedOrAll;
        private DateTime? _specifiedDateTime;
		private DateTime? _specifiedDateTimeStart;
		private DateTime? _specifiedDateTimeEnd;
        private Tuple<int?, int?> _specifiedMonthAndYear;



        public ReportParametersWindow(bool isPeriod, bool isMonthOrYear, bool isDate, bool isDatePeriod,
			bool isKoefT, bool isKoefZ, bool isWorkGuild, bool isArea, bool isWorkGuildSpecifiedOrAll, bool isProduct, bool isDetail,
			bool isProductSpecifiedOrAll, bool isAssemblyUnit, bool isMonthYear, 
            bool isTimeFund, bool isProcentageOfLossTime, bool isProcentageOfPerformanceStandarts, 
            bool isAreaSpecifiedOrAll, string hintMessage)
		{
			_isPeriod = isPeriod;
			_isDate = isDate;
			_isDatePeriod = isDatePeriod;
			_isMonthOrYear = isMonthOrYear;
			_isKoefT = isKoefT;
			_isKoefZ = isKoefZ;
			_isWorkGuild = isWorkGuild;
			_isArea = isArea;
			_hintMessage = hintMessage;
			_isWorkGuildSpecifiedOrAll = isWorkGuildSpecifiedOrAll;
			_isProduct = isProduct;
		    _isDetail = isDetail;
			_isProductSpecifiedOrAll = isProductSpecifiedOrAll;
		    _isAssemblyUnit = isAssemblyUnit;
		    _isMonthYear = isMonthYear;
		    _isTimeFund = isTimeFund;
		    _isProcentageOfLossTime = isProcentageOfLossTime;
		    _isProcentageOfPerformanceStandarts = isProcentageOfPerformanceStandarts;
		    _isAreaSpecifiedOrAll = isAreaSpecifiedOrAll;

            InitializeComponent();
			VisualInitializeComponent();
			AdditionalInitializeComponent();
		}

		/// <summary>
		/// Отображение/скрытие соответсвующих полей окна, загрузка списов из БД (если нужно)
		/// </summary>
		private void AdditionalInitializeComponent()
		{

			const Visibility show = Visibility.Visible;
			const Visibility hide = Visibility.Collapsed;

			// Отображение/скрытие полей
			PeriodWrapperGrid.Visibility = _isPeriod ? show : hide;
			MonthOrYearWrapperGrid.Visibility = _isMonthOrYear ? show : hide;
			DateWrapperGrid.Visibility = _isDate ? show : hide;
			DatePeriodWrapperGrid.Visibility = _isDatePeriod ? show : hide;
			KoefTWrapperGrid.Visibility = _isKoefT ? show : hide;
			KoefZWrapperGrid.Visibility = _isKoefZ ? show : hide;
			CehWrapperGrid.Visibility = _isWorkGuild ? show : hide;
			AreaWrapperGrid.Visibility = _isArea ? show : hide;
			WorkGuildSpecifiedOrAllWrapperGrid.Visibility = _isWorkGuildSpecifiedOrAll ? show : hide;
			ProductWrapperGrid.Visibility = _isProduct ? show : hide;
		    DetailWrapperGrid.Visibility = _isDetail ? show : hide;
			ProductSpecifiedOrAllWrapperGrid.Visibility = _isProductSpecifiedOrAll ? show : hide;
		    AssemblyUnitWrapperGrid.Visibility = _isAssemblyUnit ? show : hide;
		    MonthYearWrapperGrid.Visibility = _isMonthYear ? show : hide;
		    TimeFundWrapperGrid.Visibility = _isTimeFund ? show : hide;
		    ProcentageOfLossTimeWrapperGrid.Visibility = _isProcentageOfLossTime ? show : hide;
		    ProcentageOfPerformanceStandartsWrapperGrid.Visibility = _isProcentageOfPerformanceStandarts ? show : hide;
		    AreaSpecifiedOrAllWrapperGrid.Visibility = _isAreaSpecifiedOrAll ? show : hide;

            MessageLabel.Content = _hintMessage;

			// Если вводится [Дата] - значение по-умолчанию сегодняшняя
			if (_isDate)
			{
				DatePicker.SelectedDate = DateTime.Today;
			}

			if (_isWorkGuild)
			{
				try
				{
					WorkGuildsComboBox.ItemsSource = WorkGuildsService.GetAll();
				}
				catch (StorageException ex)
				{
					Common.ShowDetailExceptionMessage(ex);
				}
			}

			// Если вводится [Изделие] - загрузка списка из БД и заполнение Textbox
			if (_isProductSpecifiedOrAll)
			{
				try
				{
					_searchProductStorage = ProductsService.GetProducts();
					SearchProductDataGrid.ItemsSource = _searchProductStorage;
				}
				catch (StorageException ex)
				{
					Common.ShowDetailExceptionMessage(ex);
					return;
				}
				ProductAllRadioButton.IsChecked = true;
			}

			// Если вводится [Цех] - загрузка списка из БД и заполнение ComboBox
			if (_isWorkGuildSpecifiedOrAll)
			{
				try
				{
					WorkguildSpecifiedComboBox.ItemsSource = WorkGuildsService.GetAll();
				}
				catch (StorageException ex)
				{
					Common.ShowDetailExceptionMessage(ex);
					return;
				}
				WorkguildAllRadioButton.IsChecked = true;
			}

            // Если вводится [Участок] - загрузка списка из БД и заполнение ComboBox
		    if (_isAreaSpecifiedOrAll)
		    {
		        try
		        {
		            AreaSpecifiedComboBox.ItemsSource = AreasService.GetAll();
		        }
		        catch (StorageException ex)
		        {
		            Common.ShowDetailExceptionMessage(ex);
		            return;
		        }

		        AreaAllRadioButton.IsChecked = true;
		    }

            if (_isArea)
			{
				try
				{
					AreaComboBox.ItemsSource = AreasService.GetAll();
				}
				catch (StorageException ex)
				{
					Common.ShowDetailExceptionMessage(ex);
				}
			}

			// Если вводится [Изделие] - загрузка списка из БД и заполнение Textbox
			if (_isProduct)
			{
				try
				{
					_searchProductStorage = ProductsService.GetProducts();
					SearchProductDataGrid.ItemsSource = _searchProductStorage;
				}
				catch (StorageException ex)
				{
					Common.ShowDetailExceptionMessage(ex);
				}
			}

		    // Если вводится [Деталь] - загрузка списка из БД и заполнение Textbox
            if (_isDetail)
		    {
		        try
		        {
		            _searchDetailStorage = DetailsService.GetAll();
		            SearchDetailDataGrid.ItemsSource = _searchDetailStorage;
		        }
		        catch (StorageException ex)
		        {
		            Common.ShowDetailExceptionMessage(ex);
		        }
		    }

            // Если вводится [Сборочноя еденица] - загрузка списка из БД и заполнение Textbox
            if (_isAssemblyUnit)
		    {
		        try
		        {
		            _searchProductStorage = ProductsService.GetAssemblyUnits();
		            SearchAssemblyUnitDataGrid.ItemsSource = _searchProductStorage;
		        }
		        catch (StorageException ex)
		        {
		            Common.ShowDetailExceptionMessage(ex);
		        }
		    }

		    // Если вводится [Месяц/Год] - получение Dictionary месяцев и заполнение ComboBox, год текущий
		    // ReSharper disable once InvertIf
		    if (_isMonthYear)
		    {
		        const int monthOffset = 1;
		        var today = DateTime.Today;
		        MonthComboBox.ItemsSource = Common.MonthsFullNames();
		        MonthComboBox.SelectedIndex = today.Month - monthOffset;
		        YearIntegerUpDown.Value = today.Year;
		    }

		    ConfirmButton.Focus();
        }

		/// <summary>
		/// Визуальная инициализация страницы (цвета и размеры шрифтов контролов)
		/// </summary>
		private void VisualInitializeComponent()
		{
			FontSize = Constants.FontSize;
		}

		/// <summary>
		/// Указывает выборку делать по месяцу c начало года или за период
		/// </summary>
		public string SelectedPeriod()
		{
			return _isSelectedPeriod;
		}

		/// <summary>
		/// Указывает выборку делать по месяцу или с начало года
		/// </summary>
		public string SelectedMonthOrYear()
		{
			return _monthOrYear;
		}

		/// <summary>
		/// Указанная пользователем [Дата]
		/// </summary>
		public DateTime? SelectedDateTime()
		{
			return _specifiedDateTime;
		}

		/// <summary>
		/// Указанная пользователем [Дата с периода]
		/// </summary>
		public DateTime? SelectedDateTimeStart()
		{
			return _specifiedDateTimeStart;
		}

		/// <summary>
		/// Указанная пользователем [Дата по периода]
		/// </summary>
		public DateTime? SelectedDateTimeEnd()
		{
			return _specifiedDateTimeEnd;
		}

		/// <summary>
		/// Указанный пользователем [Цех]
		/// </summary>
		public WorkGuild SelectedWorkGuild()
		{
			return _specifiedWorkGuild;
		}

		/// <summary>
		/// Указанный пользователем [Цех]. Если указаны [По всему заводу], возвращается null
		/// </summary>
		public Product SelectedProductOrAll()
		{
			return _specifiedisProductSpecifiedOrAll;
		}

        /// <summary>
        /// Указанный пользователем [Продукт]. Если указаны [По всему продуктам], возвращается null
        /// </summary>
        public WorkGuild SelectedWorkGuildOrAll()
        {
            return _specifiedisWorkGuildSpecifiedOrAll;
        }

        /// <summary>
        /// Указанный пользователем [Участок]. Если указаны [По всем участкам], возвращается null
        /// </summary>
        public Area SelectedAreaOrAll()
        {
            return _specifiedisAreaSpecifiedOrAll;
        }

        /// <summary>
        /// Указанный пользователем [Участок]
        /// </summary>
        public Area SelectedArea()
		{
			return _specifiedArea;
		}

		/// <summary>
		/// Указанный пользователем [Изделие]/[Сборочная еденица].
		/// </summary>
		public Product SelectedProduct()
		{
			return _selectedProduct;
		}

        /// <summary>
        /// Указанный пользователем [Деталь].
        /// </summary>
        public Detail SelectedDetail()
        {
            return _selectedDetail;
        }

        /// <summary>
        /// Указанные пользователем [Месяц/Год]
        /// </summary>
        public Tuple<int?, int?> SelectedMonthAndYear()
        {
            return _specifiedMonthAndYear;
        }


        /// <summary>
        /// Нажатие кнопки [Подтверждение]
        /// </summary>
        private void ConfirmButton_OnClick(object senderIsButton, RoutedEventArgs eventArgs)
		{
			// Валидация полей
			if (_isPeriod)
			{
				var isMonthPeriod = MonthPeriodRadioButton.IsChecked == true;
				var isYearPeriod = YearPeriodRadioButton.IsChecked == true;
				var isPeriodDate = PeriodRadioButton.IsChecked == true;
				if (!isMonthPeriod && !isYearPeriod && !isPeriodDate)
				{
					const string errorMessage = "Выберите период";
					const MessageBoxButton buttons = MessageBoxButton.OK;
					const MessageBoxImage messageType = MessageBoxImage.Error;
					MessageBox.Show(errorMessage, PageLiterals.HeaderValidation, buttons, messageType);
					return;
				}

				if (isMonthPeriod || isYearPeriod)
				{
					_specifiedDateTime = DatePicker.SelectedDate;
				}
				else
				{
					_specifiedDateTimeStart = DateStartPicker.SelectedDate;
					_specifiedDateTimeEnd = DateEndPicker.SelectedDate;
				}
			}

			if (_isMonthOrYear)
			{
				var isMonth = MonthRadioButton.IsChecked == true;
				var isYear = YearRadioButton.IsChecked == true;
				if (!isMonth && !isYear)
				{
					const string errorMessage = "Выберите период";
					const MessageBoxButton buttons = MessageBoxButton.OK;
					const MessageBoxImage messageType = MessageBoxImage.Error;
					MessageBox.Show(errorMessage, PageLiterals.HeaderValidation, buttons, messageType);
					return;
				}


			}

			var isValidFields = IsValidFieldsWithShowMessageOtherwise();
			if (!isValidFields)
			{
				return;
			}

			if (_isMonthOrYear)
			{
				var isMonth = MonthRadioButton.IsChecked == true;
				if (isMonth)
				{
					_monthOrYear = "m";
				}
				else
				{
					_monthOrYear = "y";
				}
			}

			if (_isDate)
			{
				_specifiedDateTime = DatePicker.SelectedDate ?? DateTime.Today;
			}

			if (_isDatePeriod)
			{
				_specifiedDateTimeStart = DateStartPicker.SelectedDate;
				_specifiedDateTimeEnd = DateEndPicker.SelectedDate;
			}

			if (_isWorkGuild)
			{
				_specifiedWorkGuild = WorkGuildsComboBox.SelectedItem as WorkGuild;
			}

			if (_isWorkGuildSpecifiedOrAll)
			{
				const WorkGuild allWorkGuilds = null;
				var isAllWorkGuilds = WorkguildAllRadioButton.IsChecked == true;
				_specifiedisWorkGuildSpecifiedOrAll =
					isAllWorkGuilds ? allWorkGuilds : WorkguildSpecifiedComboBox.SelectedItem as WorkGuild;
			}

			if (_isArea)
			{
				_specifiedArea = AreaComboBox.SelectedItem as Area;
			}

		    if (_isAreaSpecifiedOrAll)
		    {
		        const Area allAreas = null;
		        var isAllAreas = AreaAllRadioButton.IsChecked == true;
		        _specifiedisAreaSpecifiedOrAll = isAllAreas ? allAreas : AreaSpecifiedComboBox.SelectedItem as Area;
		    }

            if (_isProductSpecifiedOrAll)
            {
                const Product allProducts = null;
                var isAllProducts = ProductAllRadioButton.IsChecked == true;
                _specifiedisProductSpecifiedOrAll = 
                    isAllProducts ? allProducts : _selectedProduct;
            }
	
            if (_isMonthYear)
		    {
		        var nullableMonthKeyValuePair = MonthComboBox.SelectedItem as KeyValuePair<int, string>?;
		        var month = nullableMonthKeyValuePair == null
		            ? (int?)null
		            : ((KeyValuePair<int, string>)nullableMonthKeyValuePair).Key;
		        var year = YearIntegerUpDown.Value;

		        _specifiedMonthAndYear = new Tuple<int?, int?>(month, year);
		    }

            DialogResult = true;
			Close();
		}

		/// <summary>
		/// Валидация (проверка корректности) значений полей окна, и вывод сообщения при некорректности
		/// </summary>
		private bool IsValidFieldsWithShowMessageOtherwise()
		{
			var isValid = true;
			var errorMessages = new StringBuilder();
			if (_isPeriod)
			{
				var isMonthPeriod = MonthPeriodRadioButton.IsChecked == true;
				var isYearPeriod = YearPeriodRadioButton.IsChecked == true;
				if (isMonthPeriod || isYearPeriod)
				{
					var fieldDate = DateRun.Text;
					var date = DatePicker.SelectedDate;
					isValid &= Validator.IsNotNullSelectedObject(date, fieldDate, errorMessages);
				}
				else
				{
					var fieldDateStart = DateStartRun.Text;
					var fieldDateEnd = DateEndRun.Text;
					var dateStart = DateStartPicker.SelectedDate;
					var dateEnd = DateEndPicker.SelectedDate;
					isValid &= Validator.IsNotNullSelectedObject(dateStart, fieldDateStart, errorMessages);
					isValid &= Validator.IsNotNullSelectedObject(dateEnd, fieldDateEnd, errorMessages);
				}
			}

			if (_isDate)
			{
				var fieldDate = DateRun.Text;
				var date = DatePicker.SelectedDate;
				isValid &= Validator.IsNotNullSelectedObject(date, fieldDate, errorMessages);
			}

			if (_isDatePeriod)
			{
				var fieldDateStart = DateStartRun.Text;
				var fieldDateEnd = DateEndRun.Text;
				var dateStart = DateStartPicker.SelectedDate;
				var dateEnd = DateEndPicker.SelectedDate;
				isValid &= Validator.IsNotNullSelectedObject(dateStart, fieldDateStart, errorMessages);
				isValid &= Validator.IsNotNullSelectedObject(dateEnd, fieldDateEnd, errorMessages);
			}


			if (_isKoefT)
			{
				var fieldKoefT = KoefTLabel.Content.ToString();
				var koefT = KoefTDecimalUpDown.Value;
				isValid &= Validator.IsNotNullSelectedObject(koefT, fieldKoefT, errorMessages);
			}

			if (_isKoefZ)
			{
				var fieldKoefZ = KoefZLabel.Content.ToString();
				var koefZ = KoefZDecimalUpDown.Value;
				isValid &= Validator.IsNotNullSelectedObject(koefZ, fieldKoefZ, errorMessages);
			}

			if (_isWorkGuild)
			{
				var fieldWorkGuild = CehRun.Text;
				var workGuild = WorkGuildsComboBox.SelectedItem;
				isValid &= Validator.IsNotNullSelectedObject(workGuild, fieldWorkGuild, errorMessages);
			}

			if (_isWorkGuildSpecifiedOrAll)
			{
				var isAllWorkGuilds = WorkguildAllRadioButton.IsChecked == true;
				if (!isAllWorkGuilds)
				{
					var fieldWorkGuilds = WorkGuildSpecifiedOrAllRun.Text;
					var workGuilds = WorkguildSpecifiedComboBox.SelectedItem;
					isValid &= Validator.IsNotNullSelectedObject(workGuilds, fieldWorkGuilds, errorMessages);
				}
			}

			if (_isArea)
			{
				var fieldArea = AreaRun.Text;
				var area = AreaComboBox.SelectedItem;
				isValid &= Validator.IsNotNullSelectedObject(area, fieldArea, errorMessages);
			}

			if (_isProduct)
			{
				var fieldProduct = ProductLabel.Content.ToString();
				isValid &= Validator.IsNotNullSelectedObject(_selectedProduct, fieldProduct, errorMessages);
			}

		    if (_isDetail)
		    {
		        var fieldDetail = DetailLabel.Content.ToString();
		        isValid &= Validator.IsNotNullSelectedObject(_selectedDetail, fieldDetail, errorMessages);
		    }

            if (_isProductSpecifiedOrAll)
			{
				var isAllProducts = ProductAllRadioButton.IsChecked == true;
				if (!isAllProducts)
				{
					var fieldProducts = ProductSpecifiedOrAllRun.Text;
					isValid &= Validator.IsNotNullSelectedObject(_selectedProduct, fieldProducts, errorMessages);
				}
			}

		    if (_isAssemblyUnit)
		    {
		        var fieldProduct = AssemblyUnitLabel.Content.ToString();
		        isValid &= Validator.IsNotNullSelectedObject(_selectedProduct, fieldProduct, errorMessages);
		    }

		    if (_isMonthYear)
		    {
		        var fieldMonth = MonthRun.Text;
		        var month = MonthComboBox.SelectedItem;
		        isValid &= Validator.IsNotNullSelectedObject(month, fieldMonth, errorMessages);
		        var fieldYear = YearRun.Text;
		        var year = YearIntegerUpDown.Value;
		        isValid &= Validator.IsNotNullSelectedObject(year, fieldYear, errorMessages);
		    }

            if (isValid)
			{
				return true;
			}

			const MessageBoxImage messageType = MessageBoxImage.Error;
			const MessageBoxButton messageButtons = MessageBoxButton.OK;
			const string validationHeader = PageLiterals.HeaderValidation;
			MessageBox.Show(errorMessages.ToString(), validationHeader, messageButtons, messageType);

			return false;
		}

		/// <summary>
		/// Обработка изменения выбраных значений RadioButton'ов, для скрытия/отображения ComboBox'ов
		/// </summary>
		private void RadioButton_OnChecked(object senderIsRadioButton, RoutedEventArgs e)
		{
			const int defaultComboBoxIndex = 0;
			const int disabledComboBoxIndex = -1;
			const string periodGroup = "PeriodRadioButtonGroup";
			const string workGuildGroup = "WorkGuildRadioButtonGroup";
			const string productGroup = "ProductRadioButtonGroup";
			const StringComparison comparisonIgnoreCase = StringComparison.OrdinalIgnoreCase;
			var radioButton = senderIsRadioButton as RadioButton;
			if (radioButton == null)
			{
				return;
			}

			var groupName = radioButton.GroupName;

			if (string.Equals(workGuildGroup, groupName, comparisonIgnoreCase))
			{
				if (string.Equals(radioButton.Name, WorkguildAllRadioButton.Name, comparisonIgnoreCase))
				{
					WorkguildSpecifiedComboBox.SelectedIndex = disabledComboBoxIndex;
					WorkguildSpecifiedComboBox.IsEnabled = false;
				}

				if (string.Equals(radioButton.Name, WorkguildSpecifiedRadioButton.Name, comparisonIgnoreCase))
				{
					WorkguildSpecifiedComboBox.IsEnabled = true;
					WorkguildSpecifiedComboBox.SelectedIndex = defaultComboBoxIndex;
				}
			}

			// ReSharper restore InvertIf
			if (string.Equals(periodGroup, groupName, comparisonIgnoreCase))
			{
				if (string.Equals(radioButton.Name, MonthPeriodRadioButton.Name, comparisonIgnoreCase))
				{
					DateWrapperGrid.Visibility = Visibility.Visible;
					DatePeriodWrapperGrid.Visibility = Visibility.Collapsed;
					_isSelectedPeriod = "m";
				}

				if (string.Equals(radioButton.Name, YearPeriodRadioButton.Name, comparisonIgnoreCase))
				{
					DateWrapperGrid.Visibility = Visibility.Visible;
					DatePeriodWrapperGrid.Visibility = Visibility.Collapsed;
					_isSelectedPeriod = "y";
				}

				if (string.Equals(radioButton.Name, PeriodRadioButton.Name, comparisonIgnoreCase))
				{
					DateWrapperGrid.Visibility = Visibility.Collapsed;
					DatePeriodWrapperGrid.Visibility = Visibility.Visible;
					_isSelectedPeriod = "p";
				}
			}

			if (string.Equals(productGroup, groupName, comparisonIgnoreCase))
			{
				if (string.Equals(radioButton.Name, ProductAllRadioButton.Name, comparisonIgnoreCase))
				{
					ProductTextBox.Clear();
					ProductTextBox.IsEnabled = false;
					ProductWrapperGrid.Visibility = Visibility.Collapsed;
				}

				if (string.Equals(radioButton.Name, ProductSpecifiedRadioButton.Name, comparisonIgnoreCase))
				{
					ProductTextBox.Clear();
					ProductTextBox.IsEnabled = true;
					ProductLabel.Content = "Укажите изделие :";
					ProductWrapperGrid.ColumnDefinitions[0].Width = new GridLength(140);
					ProductWrapperGrid.ColumnDefinitions[1].Width = new GridLength(420);
					ProductWrapperGrid.Visibility = Visibility.Visible;
				}
			}
		}

		/// <summary>
		/// Нажатие кнопки [Отмена]
		/// </summary>
		private void CancelButton_OnClick(object sender, RoutedEventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Событие получения фокуса Grid-обёрткой DataGrid и TextBox поиска: отображает DataGrid
		/// </summary>
		private void SearchFieldWrapperGrid_OnGotFocus(object senderIsGrid, RoutedEventArgs eventArgs)
		{
			PageUtil.SearchFieldWrapperGrid_OnGotFocusShowTable(senderIsGrid);
		}

		/// <summary>
		/// Событие утери фокуса Grid-обёрткой DataGrid и TextBox поиска: скрывает DataGrid
		/// </summary>
		private void SearchFieldWrapperGrid_OnLostFocus(object senderIsGrid, RoutedEventArgs eventArgs)
		{
			PageUtil.SearchFieldWrapperGrid_OnLostFocusHideTable(senderIsGrid);
		}

		/// <summary>
		/// Обработка события изменения текста в TextBox поиска [Продукта].
		/// (Перезаполнение DataGrid поиска сущности с учётом введённого текста)
		/// </summary>
		private void ProductTextBox_OnTextChanged(object senderIsTextBox, TextChangedEventArgs eventArgs)
		{
			// TextBox поиска
			var searchTextBox = senderIsTextBox as TextBox;
			if (searchTextBox == null)
			{
				return;
			}

			// Grid-обёртка DataGrid и TextBox поиска
			var searchWrapperGrid = VisualTreeHelper.GetParent(searchTextBox) as Grid;
			if (searchWrapperGrid == null)
			{
				return;
			}

			// DataGrid поиска сущности
			var searchDataGrid = searchWrapperGrid.Children.OfType<DataGrid>().FirstOrDefault();
			if (searchDataGrid == null)
			{
				return;
			}

			// Разделение введенного пользователем текста по пробелам на массив слов
			var searchResult = new List<Product>();
			var searchValues = searchTextBox.Text.Trim().Split(null);
			const StringComparison comparisonIgnoreCase = StringComparison.OrdinalIgnoreCase;
			foreach (var product in _searchProductStorage)
			{
				// Поиск совпадений всех значений массива по требуемым полям сущности
				var isCoincided = true;
			    var productDisplayCodeString = product.DisplayCodeString;
				var productName = product.Name;
				var productMark = product.Mark;
				foreach (var searchValue in searchValues)
				{
					isCoincided &= productName.IndexOf(searchValue, comparisonIgnoreCase) >= 0 
					               || productMark.IndexOf(searchValue, comparisonIgnoreCase) >= 0
				                   || productDisplayCodeString.IndexOf(searchValue, comparisonIgnoreCase) >= 0;
				}

				// Если в полях сущности есть введённые слова, добавляем объект в буферный список
				if (isCoincided)
				{
					searchResult.Add(product);
				}
			}

			// Перезаполнение DataGrid поиска сущности с учётом найденых значений
			searchDataGrid.ItemsSource = null;
			searchDataGrid.ItemsSource = searchResult;
		}

        /// <summary>
        /// Обработка нажатия клавиш [Enter] и [Up] в DataGrid поиска сущностей
        /// </summary>
        private void SearchDataGrid_OnPreviewKeyUp(object senderIsDataGrid, KeyEventArgs eventArgs)
		{
			const int startOfListIndex = 0;
			// DataGrid поиска сущности
			var searchDataGrid = senderIsDataGrid as DataGrid;
			if (searchDataGrid == null)
			{
				return;
			}

			// Grid-обёртка DataGrid и TextBox поиска
			var searchWrapperGrid = VisualTreeHelper.GetParent(searchDataGrid) as Grid;
			if (searchWrapperGrid == null)
			{
				return;
			}

			// TextBox поиска/добавления
			var searchTextBox = searchWrapperGrid.Children.OfType<TextBox>().FirstOrDefault();
			if (searchTextBox == null)
			{
				return;
			}

			// Если фокус ввода на первой записи DataGrid и нажата [Up] - перевод клавиатурного фокуса ввода к TextBox
			if (startOfListIndex == searchDataGrid.SelectedIndex && eventArgs.Key == Key.Up)
			{
				searchTextBox.Focus();
			}

			// Если записей не 0 и нажат [Enter] - заносим текст объекта в TextBox и переводим фокус к след. контролу
			else if (searchDataGrid.Items.Count > 0 && eventArgs.Key == Key.Enter)
			{
				// Выбранная строка (объект) DataGrid поиска сущности
				var rawSelectedItem = searchDataGrid.SelectedItem;
				if (rawSelectedItem == null)
				{
					return;
				}

				string displayed;
	
				var selectedItemType = rawSelectedItem.GetType();
				if (selectedItemType == typeof(Product)) // Если тип найденой сущности: [Изделие]
				{
					_selectedProduct = (Product) rawSelectedItem;
					displayed = _selectedProduct.DisplayCodeString;
		
				}
			   else if (selectedItemType == typeof(Detail)) // Если тип найденой сущности: [Деталь]
			    {
			        _selectedDetail = (Detail)rawSelectedItem;
			        displayed = _selectedDetail.CodeDetail.ToString();

			    }
                else
				{
					displayed = rawSelectedItem.ToString();
				}

				// Вывод выбраного значения в TextBox поиска/добавления
				searchTextBox.Text = displayed;

				// Перевод фокуса ввода на нижележащий визуальный элемент после [DataGrid] поиска сущности
				var request = new TraversalRequest(FocusNavigationDirection.Down)
				{
					Wrapped = false
				};
				eventArgs.Handled = true;
				if (searchDataGrid.MoveFocus(request))
				{
					searchDataGrid.Visibility = Visibility.Collapsed;
				}
			}
		}

		private void SearchDataGrid_OnPreviewKeyDown(object sender, KeyEventArgs eventArgs)
		{
			if (eventArgs.Key == Key.Enter)
			{
				eventArgs.Handled = true;
			}
		}

		/// <summary>
		/// Обработка нажатия мышки на строку DataGrid поиска сущностей
		/// </summary>
		private void SearchDataGrid_OnPreviewMouseDown(object senderIsDataGrid, MouseButtonEventArgs eventArgs)
		{
			// DataGrid поиска сущности
			var searchDataGrid = senderIsDataGrid as DataGrid;
			if (searchDataGrid == null)
			{
				return;
			}

			// Grid-обёртка DataGrid и TextBox поиска
			var searchWrapperGrid = VisualTreeHelper.GetParent(searchDataGrid) as Grid;
			if (searchWrapperGrid == null)
			{
				return;
			}

			// TextBox поиска/добавления
			var searchTextBox = searchWrapperGrid.Children.OfType<TextBox>().FirstOrDefault();
			if (searchTextBox == null)
			{
				return;
			}

			// Выбранная строка (объект) DataGrid поиска сущности
			var rawSelectedItem = searchDataGrid.SelectedItem;
			if (rawSelectedItem == null)
			{
				return;
			}

			string displayed;
			var selectedItemType = rawSelectedItem.GetType();
			if (selectedItemType == typeof(Product)) // Если тип найденой сущности: [Изделие]
			{
				_selectedProduct = (Product) rawSelectedItem;
				displayed = _selectedProduct.DisplayCodeString;
			}
		   else if (selectedItemType == typeof(Detail)) // Если тип найденой сущности: [Деталь]
		    {
		        _selectedDetail = (Detail)rawSelectedItem;
		        displayed = _selectedDetail.CodeDetail.ToString();
		    }
            else
			{
				displayed = rawSelectedItem.ToString();
			}

			// Вывод выбраного значения в TextBox поиска/добавления
			searchTextBox.Text = displayed;

			// Перевод фокуса ввода на нижележащий визуальный элемент после [DataGrid] поиска сущности
			var request = new TraversalRequest(FocusNavigationDirection.Down)
			{
				Wrapped = false
			};
			eventArgs.Handled = true;
			if (searchDataGrid.MoveFocus(request))
			{
				searchDataGrid.Visibility = Visibility.Collapsed;
			}
		}

		private void ProductTextBox_OnPreviewKeyUp(object senderIsTextBox, KeyEventArgs eventArgs)
		{
			// TextBox поиска
			var searchTextBox = senderIsTextBox as TextBox;
			if (searchTextBox == null)
			{
				return;
			}

			// Grid-обёртка DataGrid и TextBox поиска
			var searchWrapperGrid = VisualTreeHelper.GetParent(searchTextBox) as Grid;
			if (searchWrapperGrid == null)
			{
				return;
			}

			// DataGrid поиска сущности
			var searchDataGrid = searchWrapperGrid.Children.OfType<DataGrid>().FirstOrDefault();
			if (searchDataGrid == null)
			{
				return;
			}

			// Если нажата кнопка [Down] - перемещение клавиатурного фокуса на первую строку DataGrid поиска сущности
			if (eventArgs.Key == Key.Down)
			{
				if (searchDataGrid.Items.Count <= 0)
				{
					return;
				}

				searchDataGrid.SelectedIndex = 0;

				// NOTE: эта копипаста ниже не случайна, нужный функционал срабатывает только со второго раза.
				// Решение в указаном ответе: https://stackoverflow.com/a/27792628 Работает, не трогай
				var row = (DataGridRow) searchDataGrid.ItemContainerGenerator.ContainerFromIndex(0);
				if (row == null)
				{
					searchDataGrid.UpdateLayout();
					searchDataGrid.ScrollIntoView(searchDataGrid.Items[0]);
					row = (DataGridRow) searchDataGrid.ItemContainerGenerator.ContainerFromIndex(0);
				}

				if (row != null)
				{
					row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
				}
			}
		}

        private void DetailTextBox_OnPreviewKeyUp(object senderIsTextBox, KeyEventArgs eventArgs)
        {
            // TextBox поиска
            var searchTextBox = senderIsTextBox as TextBox;
            if (searchTextBox == null)
            {
                return;
            }

            // Grid-обёртка DataGrid и TextBox поиска
            var searchWrapperGrid = VisualTreeHelper.GetParent(searchTextBox) as Grid;
            if (searchWrapperGrid == null)
            {
                return;
            }

            // DataGrid поиска сущности
            var searchDataGrid = searchWrapperGrid.Children.OfType<DataGrid>().FirstOrDefault();
            if (searchDataGrid == null)
            {
                return;
            }

            // Если нажата кнопка [Down] - перемещение клавиатурного фокуса на первую строку DataGrid поиска сущности
            if (eventArgs.Key == Key.Down)
            {
                if (searchDataGrid.Items.Count <= 0)
                {
                    return;
                }

                searchDataGrid.SelectedIndex = 0;

                // NOTE: эта копипаста ниже не случайна, нужный функционал срабатывает только со второго раза.
                // Решение в указаном ответе: https://stackoverflow.com/a/27792628 Работает, не трогай
                var row = (DataGridRow)searchDataGrid.ItemContainerGenerator.ContainerFromIndex(0);
                if (row == null)
                {
                    searchDataGrid.UpdateLayout();
                    searchDataGrid.ScrollIntoView(searchDataGrid.Items[0]);
                    row = (DataGridRow)searchDataGrid.ItemContainerGenerator.ContainerFromIndex(0);
                }

                if (row != null)
                {
                    row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
        }
        /// <summary>
        /// Обработка события изменения текста в TextBox поиска [Детали].
        /// (Перезаполнение DataGrid поиска сущности с учётом введённого текста)
        /// </summary>
        private void DetailTextBox_OnTextChanged(object senderIsTextBox, TextChangedEventArgs e)
        {
            // TextBox поиска
            var searchTextBox = senderIsTextBox as TextBox;
            if (searchTextBox == null)
            {
                return;
            }

            // Grid-обёртка DataGrid и TextBox поиска
            var searchWrapperGrid = VisualTreeHelper.GetParent(searchTextBox) as Grid;
            if (searchWrapperGrid == null)
            {
                return;
            }

            // DataGrid поиска сущности
            var searchDataGrid = searchWrapperGrid.Children.OfType<DataGrid>().FirstOrDefault();
            if (searchDataGrid == null)
            {
                return;
            }

            // Разделение введенного пользователем текста по пробелам на массив слов
            var searchResult = new List<Detail>();
            var searchValues = searchTextBox.Text.Trim().Split(null);
            const StringComparison comparisonIgnoreCase = StringComparison.OrdinalIgnoreCase;
            foreach (var detail in _searchDetailStorage)
            {
                // Поиск совпадений всех значений массива по требуемым полям сущности
                var isCoincided = true;
                var detailCode = detail.CodeDetail.ToString();
                var detailName = detail.Name;
                var detailMark = detail.Mark;
                foreach (var searchValue in searchValues)
                {
                    isCoincided &= detailCode.IndexOf(searchValue,comparisonIgnoreCase) >= 0
                                   || detailName.IndexOf(searchValue, comparisonIgnoreCase) >= 0
                                   || detailMark.IndexOf(searchValue, comparisonIgnoreCase) >= 0;
                }

                // Если в полях сущности есть введённые слова, добавляем объект в буферный список
                if (isCoincided)
                {
                    searchResult.Add(detail);
                }

                // Перезаполнение DataGrid поиска сущности с учётом найденых значений
                searchDataGrid.ItemsSource = null;
                searchDataGrid.ItemsSource = searchResult;
            }
        }
    }
}
