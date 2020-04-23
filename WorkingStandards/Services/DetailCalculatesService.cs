using System.Collections.Generic;

using WorkingStandards.Entities.External;
using WorkingStandards.Storages;

namespace WorkingStandards.Services
{
    /// <summary>
    /// Обработчик сервисного слоя для класса [Деталь расчета сводных трудовых нормативов]
    /// </summary>
    public class DetailCalculatesService
    {
        /// <summary>
        /// Получение коллекции [Деталь расчета сводных трудовых нормативов]
        /// </summary>
        public static List<DetailCalculate> GetAll()
        {
            return DetailCalculatesStorage.GetAll();
        }

        /// <summary>
        /// Установление признака детали в бд
        /// </summary>
        public static void UpdateIsCalculate(bool isCalculate, DetailCalculate detailCalculate)
        {
            DetailCalculatesStorage.UpdateIsCalculate(isCalculate, detailCalculate);
        }
    }
}
