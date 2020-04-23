using System.Collections.Generic;

using WorkingStandards.Entities.External;
using WorkingStandards.Storages;

namespace WorkingStandards.Services
{
    /// <summary>
    /// Обработчик сервисного слоя для класса продукции - [Product]
    /// </summary>
    public static class ProductsService
    {
        /// <summary>
        /// Получение коллекции [Изделий]
        /// </summary>
        public static List<Product> GetProducts()
        {
            return ProductsStorage.GetProducts();
        }

        /// <summary>
        /// Получение коллекции [Сборочных единиц]
        /// </summary>
        public static List<Product> GetAssemblyUnits()
        {
            return ProductsStorage.GetAssemblyUnits();
        }

        /// <summary>
        /// Получение коллекции [Деталей] из su73
        /// </summary>
        public static List<Product> GetDetailSu73()
        {
            return ProductsStorage.GetDetailSu73();
        }
    }
}
