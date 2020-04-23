using System.Collections.Generic;

using WorkingStandards.Entities.External;
using WorkingStandards.Storages;

namespace WorkingStandards.Services
{
    /// <summary>
    /// Обработчик сервисного слоя для [Класс выпуска изделия]
    /// </summary>
    public class RealaseProductsService
    {
        /// <summary>
        /// Получение коллекции [Выпуска изделия]
        /// </summary>
        public static List<RealaseProduct> GetAll()
        {
            return RealaseProductsStorage.GetAll();
        }

        /// <summary>
        /// Обновление [Выпуска изделия]
        /// </summary>
        public static void Update(RealaseProduct realaseProduct)
        {
            RealaseProductsStorage.Update(realaseProduct);
        }
    }
}
