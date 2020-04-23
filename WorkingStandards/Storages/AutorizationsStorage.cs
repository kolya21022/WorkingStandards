using System.Collections.Generic;
using System.Data.OleDb;
using WorkingStandards.Db;
using WorkingStandards.Entities.External;

namespace WorkingStandards.Storages
{
    public class AutorizationsStorage
    {
        /// <summary>
        /// Авторизация.
        /// </summary>
        public static bool Autorization(decimal worguild, string password)
        {
            var dbFolder = Properties.Settings.Default.FoxProDbFolder_Temp;
            const string query = "SELECT * FROM WorkingStandardsPassword " +
                                 "WHERE workguild = ? AND password = ?";

            try
            {
                using (var oleDbConnection = DbControl.GetConnection(dbFolder))
                {
                    // Установка соединения и проверка кодировки
                    oleDbConnection.TryConnectOpen();
                    oleDbConnection.VerifyInstalledEncoding("BalancePassword");

                    using (var oleDbCommand = new OleDbCommand(query, oleDbConnection))
                    {
                        oleDbCommand.Parameters.AddWithValue("workguild", worguild);
                        oleDbCommand.Parameters.AddWithValue("password", password);
                        using (var reader = oleDbCommand.ExecuteReader())
                        {
                            return reader != null && reader.Read();
                        }
                    }
                }
            }
            catch (OleDbException ex)
            {
                throw DbControl.HandleKnownDbFoxProAndMssqlServerExceptions(ex);
            }
        }

        /// <summary>
        /// Получить список логинов и их допуск.
        /// </summary>
        /// <returns></returns>
        public static List<Login> GetLogin()
        {
            var dbFolder = Properties.Settings.Default.FoxProDbFolder_Temp;
            const string query = "SELECT " +
                                    "workguild, " +
                                    "lvl " +
                                 "FROM WorkingStandardsPassword " +
                                 "ORDER BY workguild ASC";
            var login = new List<Login>();
            try
            {
                using (var oleDbConnection = DbControl.GetConnection(dbFolder))
                {
                    oleDbConnection.TryConnectOpen();
                    oleDbConnection.VerifyInstalledEncoding("BalancePassword");

                    using (var oleDbCommand = new OleDbCommand(query, oleDbConnection))
                    {
                        using (var reader = oleDbCommand.ExecuteReader())
                        {
                            while (reader != null && reader.Read())
                            {
                                var workguild = reader.GetDecimal(0);
                                var lvl = reader.GetDecimal(1);
                                login.Add(new Login(workguild, lvl));
                            }
                        }
                    }
                    return login;
                }
            }
            catch (OleDbException ex)
            {
                throw DbControl.HandleKnownDbFoxProAndMssqlServerExceptions(ex);
            }
        }
    }
}
