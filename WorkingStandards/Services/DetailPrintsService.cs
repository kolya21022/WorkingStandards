using System.Collections.Generic;

using WorkingStandards.Entities.External;
using WorkingStandards.Storages;

namespace WorkingStandards.Services
{
    /// <summary>
    /// Обработчик сервисного слоя для класса [Деталь и их отчеты]
    /// </summary>
    public class DetailPrintsService
    {
        /// <summary>
        /// Получение коллекции [Деталь и их отчеты]
        /// </summary>
        public static List<DetailPrint> GetAll()
        {
            return DetailPrintsStorage.GetAll();
        }

        /// <summary>
        /// Установление признака печати детали в бд для завода
        /// </summary>
        public static void UpdateIsPrintFabrik(bool isFabrik, DetailPrint detailPrint)
        {
            DetailPrintsStorage.UpdateIsPrintFabrik(isFabrik, detailPrint);
        }

        /// <summary>
        /// Установление признака печати детали в бд для цехов
        /// </summary>
        public static void UpdateIsPrintWorkGuild(bool isWorkGuild, DetailPrint detailPrint)
        {
            DetailPrintsStorage.UpdateIsPrintWorkGuild(isWorkGuild, detailPrint);
        }

        /// <summary>
        /// Установление признака печати детали в бд для цеха 2
        /// </summary>
        public static void UpdateIsPrintWorkGuild02(bool isWorkGuild02, DetailPrint detailPrint)
        {
            DetailPrintsStorage.UpdateIsPrintWorkGuild02(isWorkGuild02, detailPrint);
        }

        /// <summary>
        /// Установление признака печати детали в бд для цеха 3
        /// </summary>
        public static void UpdateIsPrintWorkGuild03(bool isWorkGuild03, DetailPrint detailPrint)
        {
            DetailPrintsStorage.UpdateIsPrintWorkGuild03(isWorkGuild03, detailPrint);
        }

        /// <summary>
        /// Установление признака печати детали в бд для цеха 4
        /// </summary>
        public static void UpdateIsPrintWorkGuild04(bool isWorkGuild04, DetailPrint detailPrint)
        {
            DetailPrintsStorage.UpdateIsPrintWorkGuild04(isWorkGuild04, detailPrint);
        }

        /// <summary>
        /// Установление признака печати детали в бд для цеха 5
        /// </summary>
        public static void UpdateIsPrintWorkGuild05(bool isWorkGuild05, DetailPrint detailPrint)
        {
            DetailPrintsStorage.UpdateIsPrintWorkGuild05(isWorkGuild05, detailPrint);
        }
    }
}
