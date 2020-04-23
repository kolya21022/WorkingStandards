using System.Data.OleDb;
using System.Data.Common;
using System.Collections.Generic;

using WorkingStandards.Db;
using WorkingStandards.Entities.External;

namespace WorkingStandards.Storages
{
    /// <summary>
	/// Обработчик запросов хранилища данных для таблицы цехов предприятия [WorkGuild]
	/// </summary>
	public static class WorkGuildStorage
	{
		/// <summary>
		/// Получение коллекции [Цехов предприятия]
		/// </summary>
		public static List<WorkGuild> GetAll()
		{
			var dbFolder = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
			const string query = "SELECT DISTINCT kc FROM [Advx03] WHERE kc=2 or kc=3 or kc=4 or kc=5";

			var workGuilds = new List<WorkGuild>();
			try
			{
				using (var connection = DbControl.GetConnection(dbFolder))
				{
					connection.TryConnectOpen();
					// Проверки наличия установленных кодировок в DBF-файлах и проверки соединений с этими файлами
					connection.VerifyInstalledEncoding("Advx03");

					using (var oleDbCommand = new OleDbCommand(query, connection))
					{
						using (var reader = oleDbCommand.ExecuteReader())
						{
							while (reader != null && reader.Read())
							{
							var id = reader.GetDecimal(0);
								var workGuild = new WorkGuild { Id = id};
								workGuilds.Add(workGuild);
							}
						}
					}
				}

				return workGuilds;
			}
			catch (DbException ex)
			{
				throw DbControl.HandleKnownDbFoxProAndMssqlServerExceptions(ex);
			}
		}
	}
}
