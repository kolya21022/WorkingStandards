using System.Collections.Generic;
using WorkingStandards.Entities.External;
using WorkingStandards.Storages;

namespace WorkingStandards.Services
{
    public class DetailsService
    {
        /// <summary>
        /// Получение коллекции [Детали]
        /// </summary>
        public static List<Detail> GetAll()
        {
            return DetailsStorage.GetDetails();
        }
    }
}
