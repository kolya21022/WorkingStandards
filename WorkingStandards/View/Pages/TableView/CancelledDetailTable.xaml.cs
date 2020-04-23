﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using WorkingStandards.Db;
using WorkingStandards.Entities.Reports;
using WorkingStandards.Services.Reports;
using WorkingStandards.Util;
using WorkingStandards.View.Util;

namespace WorkingStandards.View.Pages.TableView
{
    /// <summary>
    /// Страница с таблицей [Аннулируемых деталей]
    /// </summary>
    /// <inheritdoc cref="Page" />
    public partial class CancelledDetailTable : IPageable
    {
        /// <summary>
        /// Критерии фильтрации главного DataGrid страницы
        /// </summary>
        private readonly FilterCriterias _filterCriterias = new FilterCriterias();

        private List<CancelledDetail> _cancelledDetails;

        public CancelledDetailTable()
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
                _cancelledDetails = CancelledDetailsService.GetCancelledDetailsOnDate(new DateTime(1990,1,1), DateTime.Today);
                PageDataGrid.ItemsSource = _cancelledDetails;
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
        /// Перезаполнение данных главной таблицы с учётом фильтров
        /// </summary>
        private void PageDataGrid_Refilling()
        {
            PageUtil.PageDataGrid_RefillingWithFilters(PageDataGrid, _filterCriterias, MapFilterPredicate);
            ShowCountItemsPageDataGrid();   // Показ нового к-ва записей таблицы
                                            // Установка фокуса нужна для срабатывания Esc для закрытия
            PageUtil.SelectSpecifiedOrFirstDataGridRow<CancelledDetail>(PageDataGrid, null);
            PageUtil.SetFocusOnSelectedRowInDataGrid(PageDataGrid);
        }

        /// <summary>
        /// Метод-предикат (булевый) текущей записи коллекции сущностей, который возвращает true или 
        /// false в зависимости от попадания в диапазон фильтра по всем полям фильтрации.
        /// </summary>
        private bool MapFilterPredicate(object rawEntity)
        {
            var cancelledDetail = (CancelledDetail)rawEntity;
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
                      || FilterCriterias.ContainsLine(cancelledDetail.CodeDetail.ToString(CultureInfo.InvariantCulture), buffer);
            result &= !filter.GetValue("NameDetail", out buffer) || FilterCriterias.ContainsLine(cancelledDetail.NameDetail, buffer);
            result &= !filter.GetValue("OboznDetail", out buffer) || FilterCriterias.ContainsLine(cancelledDetail.OboznDetail, buffer);
            result &= !filter.GetValue("Operac", out buffer) || FilterCriterias.ContainsDecimal(cancelledDetail.Operac, buffer);
            result &= !filter.GetValue("WorkGuildId", out buffer) 
                      || FilterCriterias.ContainsDecimal(cancelledDetail.WorkGuildId, buffer);
            result &= !filter.GetValue("AreaId", out buffer)
                      || FilterCriterias.ContainsDecimal(cancelledDetail.AreaId, buffer);
            result &= !filter.GetValue("TehnoperId", out buffer)
                      || FilterCriterias.ContainsDecimal(cancelledDetail.TehnoperId, buffer);
            result &= !filter.GetValue("NameTehnoper", out buffer) || FilterCriterias.ContainsLine(cancelledDetail.NameTehnoper, buffer);
            result &= !filter.GetValue("Koefvr", out buffer)
                      || FilterCriterias.ContainsDecimal(cancelledDetail.Koefvr, buffer);
            result &= !filter.GetValue("ProfId", out buffer)
                      || FilterCriterias.ContainsDecimal(cancelledDetail.ProfId, buffer);
            result &= !filter.GetValue("NameProf", out buffer) || FilterCriterias.ContainsLine(cancelledDetail.NameProf, buffer);
            result &= !filter.GetValue("Kolrab", out buffer)
                      || FilterCriterias.ContainsDecimal(cancelledDetail.Kolrab, buffer);
            result &= !filter.GetValue("Razr", out buffer)
                      || FilterCriterias.ContainsDecimal(cancelledDetail.Razr, buffer);
            result &= !filter.GetValue("Koldet", out buffer)
                      || FilterCriterias.ContainsDecimal(cancelledDetail.Koldet, buffer);
            result &= !filter.GetValue("Ednorm", out buffer)
                      || FilterCriterias.ContainsDecimal(cancelledDetail.Ednorm, buffer);
            result &= !filter.GetValue("Tarset", out buffer)
                      || FilterCriterias.ContainsDecimal(cancelledDetail.Tarset, buffer);
            result &= !filter.GetValue("Vidnorm", out buffer)
                      || FilterCriterias.ContainsDecimal(cancelledDetail.Vidnorm, buffer);
            result &= !filter.GetValue("Razmpart", out buffer)
                      || FilterCriterias.ContainsDecimal(cancelledDetail.Razmpart, buffer);
            result &= !filter.GetValue("Tpz", out buffer)
                      || FilterCriterias.ContainsDecimal(cancelledDetail.Tpz, buffer);
            result &= !filter.GetValue("Vst", out buffer)
                      || FilterCriterias.ContainsDecimal(cancelledDetail.Vst, buffer);
            result &= !filter.GetValue("Koefneos", out buffer)
                      || FilterCriterias.ContainsDecimal(cancelledDetail.Koefneos, buffer);
            result &= !filter.GetValue("Vstk", out buffer)
                      || FilterCriterias.ContainsDecimal(cancelledDetail.Vstk, buffer);
            result &= !filter.GetValue("Rstk", out buffer)
                      || FilterCriterias.ContainsDecimal(cancelledDetail.Rstk, buffer);
            result &= !filter.GetValue("Nomizv", out buffer) || FilterCriterias.ContainsLine(cancelledDetail.Nomizv, buffer);
            result &= !filter.GetValue("DateIzv", out buffer) || FilterCriterias.ContainsDate(cancelledDetail.DateIzv, buffer);

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

