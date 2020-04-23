using System.Collections.Generic;

using WorkingStandards.Entities.External;
using WorkingStandards.Storages;

namespace WorkingStandards.Services
{
    /// <summary>
    /// Обработчик сервисного слоя для класса участков предприятия - [Area]
    /// </summary>
    public static class AreasService
    {
        /// <summary>
        /// Получение коллекции [Участков предприятия]
        /// </summary>
        public static List<Area> GetAll()
        {
            return AreasStorage.GetAll();
        }
    }
}
