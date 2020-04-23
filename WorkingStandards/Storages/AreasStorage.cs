using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;

using WorkingStandards.Db;
using WorkingStandards.Entities.External;

namespace WorkingStandards.Storages
{
    /// <summary>
    /// Обработчик запросов хранилища данных для таблицы участков предприятия [Area]
    /// </summary>
    public static class AreasStorage
    {
        /// <summary>
        /// Получение коллекции [Участков предприятия]
        /// </summary>
        public static List<Area> GetAll()
        {
            var dbFolder = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
            const string query = "SELECT DISTINCT uch FROM [Advx03]";

            var areas = new List<Area>();
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
                                var area = new Area { Id = id };
                                areas.Add(area);
                            }
                        }
                    }
                }
                return areas;
            }
            catch (DbException ex)
            {
                throw DbControl.HandleKnownDbFoxProAndMssqlServerExceptions(ex);
            }
        }
    }
}
