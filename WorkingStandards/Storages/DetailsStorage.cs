using System;
using System.Collections.Generic;
using System.Data.OleDb;
using WorkingStandards.Db;
using WorkingStandards.Entities.External;

namespace WorkingStandards.Storages
{
   public static class DetailsStorage
    {
        /// <summary>
        /// Получение коллекции [Деталей]
        /// </summary>
        public static List<Detail> GetDetails()
        {
            var dbFolder = Properties.Settings.Default.FoxProDbFolder_Foxpro_CI;
            var dbFolderBase = Properties.Settings.Default.FoxProDbFolder_Base;
            string query = "SELECT DISTINCT anul67.detal, su73.name, su73.obozn FROM [anul67] " +
                           "LEFT JOIN \"" + dbFolderBase + "su73.dbf\" on anul67.detal = su73.detal " +
                           "WHERE anul67.detal<>0";

            var details = new List<Detail>();
            try
            {
                using (var connection = DbControl.GetConnection(dbFolder))
                {
                    connection.TryConnectOpen();

                    // Проверки наличия установленных кодировок в DBF-файлах и проверки соединений с этими файлами
                    connection.VerifyInstalledEncoding("anul67");
                    using (var oleDbCommand = new OleDbCommand(query, connection))
                    {
                        using (var reader = oleDbCommand.ExecuteReader())
                        {
                            while (reader != null && reader.Read())
                            {
                                var id = reader.GetDecimal(0);
                                var name = reader.GetValue(1) != DBNull.Value
                                    ? reader.GetString(1).Trim()
                                    : string.Empty;
                                var mark = reader.GetValue(2) != DBNull.Value
                                    ? reader.GetString(2).Trim()
                                    : string.Empty;

                                var detail = new Detail()
                                {
                                    CodeDetail = id,
                                    Name = name,
                                    Mark = mark
                                };
                               details.Add(detail);
                            }
                        }
                    }
                }
                return details;
            }
            catch (OleDbException ex)
            {
                throw DbControl.HandleKnownDbFoxProAndMssqlServerExceptions(ex);
            }
        }
    }
}
