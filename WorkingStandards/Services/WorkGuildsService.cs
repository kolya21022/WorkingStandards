using System.Collections.Generic;

using WorkingStandards.Storages;
using WorkingStandards.Entities.External;

namespace WorkingStandards.Services
{
	/// <summary>
	/// Обработчик сервисного слоя для класса цехов предприятия - [WorkGuild]
	/// </summary>
	public static class WorkGuildsService
	{
		/// <summary>
		/// Получение коллекции [Цехов предприятия]
		/// </summary>
		public static List<WorkGuild> GetAll()
		{
			return WorkGuildStorage.GetAll();
		}
	}
}
