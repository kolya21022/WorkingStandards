﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

using WorkingStandards.Db;
using WorkingStandards.Entities.External;
using WorkingStandards.Services;
using WorkingStandards.Util;
using WorkingStandards.View.Util;

namespace WorkingStandards.View.Pages.TableView
{
    /// <summary>
    /// Страница с таблицей [Детали и их отчеты]
    /// </summary>
    /// <inheritdoc cref="Page" />
    public partial class DetailsForPrintTable : IPageable
    {
        /// <summary>
        /// Критерии фильтрации главного DataGrid страницы
        /// </summary>
        private readonly FilterCriterias _filterCriterias = new FilterCriterias();

        private List<DetailPrint> _detailPrints;

        public DetailsForPrintTable()
        {
            InitializeComponent();
            AdditionalInitializeComponent();
            VisualInitializeComponent();
        }

        /// <summary>
        /// Загрузка списка объектов из базы данных, их отображение в таблице, указание их кол-ва в Label
        /// </summary>
        /// <inheritdoc />
        public void AdditionalInitializeComponent()
        {
            FilterBarCoverLabel.Content = PageLiterals.FilterBarCoverLabel; // Сообщение-заглушка панели фильтрации
            try
            {
                _detailPrints = DetailPrintsService.GetAll();
                if (_detailPrints != null && _detailPrints.Count > 0)
                {
                    // Критерии сортировки указаны в реализации интерфейса IComparable класса
                    _detailPrints.Sort();
                }
                PageDataGrid.ItemsSource = _detailPrints;
                ShowCountItemsPageDataGrid();
            }
            catch (StorageException ex)
            {
                Common.ShowDetailExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Визуальная инициализация страницы (цвета и размеры шрифтов контролов)
        /// </summary>
        /// <inheritdoc />
        public void VisualInitializeComponent()
        {
            FontSize = Constants.FontSize;
            PageDataGrid.AlternatingRowBackground = Constants.BackColor1_AthensGray;

            // Заголовок страницы
            TitlePageGrid.Background = Constants.BackColor4_BlueBayoux;
            var titleLabels = TitlePageGrid.Children.OfType<Label>();
            foreach (var titleLabel in titleLabels)
            {
                titleLabel.Foreground = Constants.ForeColor2_PapayaWhip;
            }

            // Панель фильтрации и контекстное меню фильтра главного DataGrid
            var filterBarCoverLabels = FilterBarCoverStackPanel.Children.OfType<Label>();
            foreach (var label in filterBarCoverLabels)
            {
                label.Foreground = Constants.ForeColor1_BigStone;
            }
            FilterBarCoverStackPanel.Background = Constants.BackColor1_AthensGray;
            if (PageDataGrid.ContextMenu != null)
            {
                PageDataGrid.ContextMenu.FontSize = Constants.FontSize;
            }
        }

        /// <summary>
        /// Горячие клавиши текущей страницы
        /// </summary>
        /// <inheritdoc />
        public string PageHotkeys()
        {
            const string filter = PageLiterals.HotkeyLabelFilter;
            const string closeApp = PageLiterals.HotkeyLabelCloseApp;
            const string separator = PageLiterals.HotkeyLabelsSeparator;
            const string displayed = filter + separator + closeApp;
            return displayed;
        }

        /// <summary>
        /// Обработка нажатия клавиш в фокусе всей страницы 
        /// </summary>
        /// <inheritdoc />
        public void Page_OnKeyDown(object senderIsPageOrWindow, KeyEventArgs eventArgs)
        {
            if (eventArgs.Key != Key.Escape)
            {
                return;
            }
            eventArgs.Handled = true;
            PageUtil.ConfirmCloseApplication(); // Если нажат [Esc] - запрос подтверждения выхода у пользователя
        }

        /// <summary>
        /// Отображение числа записей в Label над таблицей
        /// </summary>
        private void ShowCountItemsPageDataGrid()
        {
            const string countItemsPattern = PageLiterals.PatternCountItemsTable;
            var message = string.Format(countItemsPattern, PageDataGrid.Items.Count);
            CountItemsLabel.Content = message;
        }

        /// <summary>
        /// Выставление клавиатурного фокуса ввода на строку DataGrid
        /// </summary>
        private void PageDataGrid_OnLoaded(object senderIsDatagrid, RoutedEventArgs eventArgs)
        {
            PageUtil.SetFocusOnSelectedRowInDataGrid(senderIsDatagrid);
        }

        /// <summary>
        /// Событие установки флага по заводу
        /// </summary>
        private void CheckBoxPrintFabrik_Checked(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null)
            {
                return;
            }
            var detailPrint = frameworkElement.DataContext as DetailPrint;
            if (detailPrint == null)
            {
                return;
            }

            // Костыль
            if (detailPrint.IsPrintFabrik)
            {
                return;
            }
            DetailPrintsService.UpdateIsPrintFabrik(true, detailPrint);
        }

        /// <summary>
        /// Событие снятия флага по заводу
        /// </summary>
        private void CheckBoxPrintFabrik_Unchecked(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null)
            {
                return;
            }
            var detailPrint = frameworkElement.DataContext as DetailPrint;
            if (detailPrint == null)
            {
                return;
            }

            // Костыль
            if (!detailPrint.IsPrintFabrik)
            {
                return;
            }
            DetailPrintsService.UpdateIsPrintFabrik(false, detailPrint);
        }

        /// <summary>
        /// Событие установки флага для цехов
        /// </summary>
        private void CheckBoxPrintWorkGuild_Checked(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null)
            {
                return;
            }
            var detailPrint = frameworkElement.DataContext as DetailPrint;
            if (detailPrint == null)
            {
                return;
            }

            // Костыль
            if (detailPrint.IsPrintWorkGuild)
            {
                return;
            }
            DetailPrintsService.UpdateIsPrintWorkGuild(true, detailPrint);
        }

        /// <summary>
        /// Событие снятия флага для цехов
        /// </summary>
        private void CheckBoxPrintWorkGuild_Unchecked(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null)
            {
                return;
            }
            var detailPrint = frameworkElement.DataContext as DetailPrint;
            if (detailPrint == null)
            {
                return;
            }

            // Костыль
            if (!detailPrint.IsPrintWorkGuild)
            {
                return;
            }
            DetailPrintsService.UpdateIsPrintWorkGuild(false, detailPrint);
        }

        /// <summary>
        /// Событие установки флага для цеха 02
        /// </summary>
        private void CheckBoxPrintWorkGuild02_Checked(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null)
            {
                return;
            }
            var detailPrint = frameworkElement.DataContext as DetailPrint;
            if (detailPrint == null)
            {
                return;
            }

            // Костыль
            if (detailPrint.IsPrintWorkGuild02)
            {
                return;
            }
            DetailPrintsService.UpdateIsPrintWorkGuild02(true, detailPrint);
        }

        /// <summary>
        /// Событие снятия флага для цеха 02
        /// </summary>
        private void CheckBoxPrintWorkGuild02_Unchecked(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null)
            {
                return;
            }
            var detailPrint = frameworkElement.DataContext as DetailPrint;
            if (detailPrint == null)
            {
                return;
            }

            // Костыль
            if (!detailPrint.IsPrintWorkGuild02)
            {
                return;
            }
            DetailPrintsService.UpdateIsPrintWorkGuild02(false, detailPrint);
        }

        /// <summary>
        /// Событие установки флага для цеха 03
        /// </summary>
        private void CheckBoxPrintWorkGuild03_Checked(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null)
            {
                return;
            }
            var detailPrint = frameworkElement.DataContext as DetailPrint;
            if (detailPrint == null)
            {
                return;
            }

            // Костыль
            if (detailPrint.IsPrintWorkGuild03)
            {
                return;
            }
            DetailPrintsService.UpdateIsPrintWorkGuild03(true, detailPrint);
        }

        /// <summary>
        /// Событие снятия флага для цеха 03
        /// </summary>
        private void CheckBoxPrintWorkGuild03_Unchecked(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null)
            {
                return;
            }
            var detailPrint = frameworkElement.DataContext as DetailPrint;
            if (detailPrint == null)
            {
                return;
            }

            // Костыль
            if (!detailPrint.IsPrintWorkGuild03)
            {
                return;
            }
            DetailPrintsService.UpdateIsPrintWorkGuild03(false, detailPrint);
        }

        /// <summary>
        /// Событие установки флага для цеха 04
        /// </summary>
        private void CheckBoxPrintWorkGuild04_Checked(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null)
            {
                return;
            }
            var detailPrint = frameworkElement.DataContext as DetailPrint;
            if (detailPrint == null)
            {
                return;
            }

            // Костыль
            if (detailPrint.IsPrintWorkGuild04)
            {
                return;
            }
            DetailPrintsService.UpdateIsPrintWorkGuild04(true, detailPrint);
        }

        /// <summary>
        /// Событие снятия флага для цеха 04
        /// </summary>
        private void CheckBoxPrintWorkGuild04_Unchecked(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null)
            {
                return;
            }
            var detailPrint = frameworkElement.DataContext as DetailPrint;
            if (detailPrint == null)
            {
                return;
            }

            // Костыль
            if (!detailPrint.IsPrintWorkGuild04)
            {
                return;
            }
            DetailPrintsService.UpdateIsPrintWorkGuild04(false, detailPrint);
        }

        /// <summary>
        /// Событие установки флага для цеха 05
        /// </summary>
        private void CheckBoxPrintWorkGuild05_Checked(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null)
            {
                return;
            }
            var detailPrint = frameworkElement.DataContext as DetailPrint;
            if (detailPrint == null)
            {
                return;
            }

            // Костыль
            if (detailPrint.IsPrintWorkGuild05)
            {
                return;
            }
            DetailPrintsService.UpdateIsPrintWorkGuild05(true, detailPrint);
        }

        /// <summary>
        /// Событие снятия флага для цеха 05
        /// </summary>
        private void CheckBoxPrintWorkGuild05_Unchecked(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null)
            {
                return;
            }
            var detailPrint = frameworkElement.DataContext as DetailPrint;
            if (detailPrint == null)
            {
                return;
            }

            // Костыль
            if (!detailPrint.IsPrintWorkGuild05)
            {
                return;
            }
            DetailPrintsService.UpdateIsPrintWorkGuild05(false, detailPrint);
        }

        /// <summary>
        /// Перезаполнение данных главной таблицы с учётом фильтров
        /// </summary>
        private void PageDataGrid_Refilling()
        {
            PageUtil.PageDataGrid_RefillingWithFilters(PageDataGrid, _filterCriterias, MapFilterPredicate);
            ShowCountItemsPageDataGrid();   // Показ нового к-ва записей таблицы
                                            // Установка фокуса нужна для срабатывания Esc для закрытия
            PageUtil.SelectSpecifiedOrFirstDataGridRow<DetailPrint>(PageDataGrid, null);
            PageUtil.SetFocusOnSelectedRowInDataGrid(PageDataGrid);
        }

        /// <summary>
        /// Метод-предикат (булевый) текущей записи коллекции сущностей, который возвращает true или 
        /// false в зависимости от попадания в диапазон фильтра по всем полям фильтрации.
        /// </summary>
        private bool MapFilterPredicate(object rawEntity)
        {
            var detailPrint = (DetailPrint)rawEntity;
            if (_filterCriterias.IsEmpty)
            {
                return true;
            }
            var result = true;

            // Проверка наличия полей сущности в критериях фильтрации и содержит ли поле искомое значение фильтра
            // Если в фильтре нет поля сущности, поле считается совпадающим по критерию
            string buffer;
            var filter = _filterCriterias;
            result &= !filter.GetValue("CodeDetail", out buffer)
                      || FilterCriterias.ContainsLine(detailPrint.CodeDetail.ToString(CultureInfo.InvariantCulture), buffer);
            result &= !filter.GetValue("Mark", out buffer) || FilterCriterias.ContainsLine(detailPrint.Mark, buffer);
            result &= !filter.GetValue("Name", out buffer) || FilterCriterias.ContainsLine(detailPrint.Name, buffer);
            return result;
        }

        /// <summary>
        /// Нажатие клавиши в контексном меню - исправление дефекта скрытия фильтра при переключении раскладки ввода
        /// </summary>
        private void PopupFilterContextMenu_OnKeyDown(object senderIsMenuItem, KeyEventArgs eventArgs)
        {
            var key = eventArgs.Key;
            eventArgs.Handled = key == Key.System || key == Key.LeftAlt || key == Key.RightAlt;
        }

        /// <summary>
        /// Обработка нажатия Enter в поле ввода фильтра
        /// </summary>
        private void PopupFilterValue_OnKeyDown(object senderIsTextBox, KeyEventArgs eventArgs)
        {
            if (eventArgs.Key != Key.Return)
            {
                return;
            }
            eventArgs.Handled = true;
            PopupFilterConfirmButton_OnClick(senderIsTextBox, eventArgs);
        }

        /// <summary>
        /// Обработка применения фильтра
        /// </summary>
        private void PopupFilterConfirmButton_OnClick(object senderIsButtonOrTextBox, RoutedEventArgs eventArgs)
        {
            PageUtil.Service_PageDataGridPopupFilterConfirm(senderIsButtonOrTextBox, _filterCriterias);
            FiltersDataGrid_Refilling();
            PageDataGrid_Refilling();
        }

        /// <summary>
        /// Перезаполнение фильтрующего DataGrid и скрытие/отображение соответ. панелей в завис. от критериев фильтра
        /// </summary>
        private void FiltersDataGrid_Refilling()
        {
            PageUtil.RefillingFilterTableAndShowHidePanel(FiltersDataGrid, _filterCriterias,
                FilterBarTableAndButtonGrid, FilterBarCoverStackPanel);
        }

        /// <summary>
        /// Подстановка имени столбца при открытии контексного меню фильтрации DataGrid, 
        /// установка Tag и перемещение фокуса ввода в поле
        /// </summary>
        private void PageDataGrid_OnContextMenuOpening(object senderIsDataGrid, ContextMenuEventArgs eventArgs)
        {
            PageUtil.Service_PageDataGridWithFilterContextMenuOpening(senderIsDataGrid);
        }

        /// <summary>
        /// Нажатие кнопки [Сброс фильтров] панели фильтрации - удаление всех введённых фильтров
        /// </summary>
        private void AllFilterDeleteButton_Click(object senderIsButton, RoutedEventArgs eventArgs)
        {
            _filterCriterias.ClearAll();    // очистка словаря критериев фильтрации
            FiltersDataGrid_Refilling();    // перезаполнение панели фильтров и скрытие/отображение с учётом критериев
            PageDataGrid_Refilling();       // перезаполнение главного DataGrid
        }

        /// <summary>
        /// Нажатие кнопки [УДЛ] DataGrid фильтрации - удаление указанного фильтра
        /// </summary>
        private void FilterDeleteButton_Click(object senderIsButton, RoutedEventArgs eventArgs)
        {
            var pressedButton = senderIsButton as Button;
            if (pressedButton == null)
            {
                return;
            }
            var deletedColumn = pressedButton.Tag as string; // получение столбца фильтра из св-ва Tag кнопки удаления
            if (string.IsNullOrWhiteSpace(deletedColumn))
            {
                return;
            }
            _filterCriterias.RemoveCriteria(deletedColumn); // удаление критерия фильтрации из словаря
            FiltersDataGrid_Refilling();    // перезаполнение панели фильтров и скрытие/отображение с учётом критериев
            PageDataGrid_Refilling();       // перезаполнение главного DataGrid
        }
    }
}