using System.Collections.Generic;
using System.Data.OleDb;

using WorkingStandards.Db;
using WorkingStandards.Entities.External;

namespace WorkingStandards.Storages
{
    /// <summary>
    /// Обработчик запросов хранилища данных для таблицы деталь и их отчеты [DetailPrint]
    /// </summary>
    public class DetailPrintsStorage
    {
        /// <summary>
        /// Получение коллекции [Детали и их отчеты]
        /// </summary>
        public static List<DetailPrint> GetAll()
        {
            var dbFolder = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
            const string query = "SELECT detal, name, obozn, pr_pech, pr_pechc, " +
                                 "pr02, pr03, pr04, pr05 FROM [izd_pech]";
            var detailPrints = new List<DetailPrint>();
            try
            {
                using (var oleDbConnection = DbControl.GetConnection(dbFolder))
                {
                    oleDbConnection.TryConnectOpen();
                    oleDbConnection.VerifyInstalledEncoding("izd_pech");

                    using (var oleDbCommand = new OleDbCommand(query, oleDbConnection))
                    {
                        using (var reader = oleDbCommand.ExecuteReader())
                        {
                            while (reader != null && reader.Read())
                            {
                                var codeDetail = reader.GetDecimal(0);
                                var name = reader.GetString(1).Trim();
                                var mark = reader.GetString(2).Trim();
                                var isPrintFabrik = reader.GetString(3).Trim() == "+";
                                var isPrintWorkGuild = reader.GetString(4).Trim() == "+";
                                var isPrintWorkGuild02 = reader.GetString(5).Trim() == "+";
                                var isPrintWorkGuild03 = reader.GetString(6).Trim() == "+";
                                var isPrintWorkGuild04 = reader.GetString(7).Trim() == "+";
                                var isPrintWorkGuild05 = reader.GetString(8).Trim() == "+";
                                var detailPrint = new DetailPrint
                                {
                                    CodeDetail = codeDetail,
                                    Name = name,
                                    Mark = mark,
                                    IsPrintFabrik = isPrintFabrik,
                                    IsPrintWorkGuild = isPrintWorkGuild,
                                    IsPrintWorkGuild02 = isPrintWorkGuild02,
                                    IsPrintWorkGuild03 = isPrintWorkGuild03,
                                    IsPrintWorkGuild04 = isPrintWorkGuild04,
                                    IsPrintWorkGuild05 = isPrintWorkGuild05
                                };
                                detailPrints.Add(detailPrint);
                            }
                        }
                    }
                }
                return detailPrints;
            }
            catch (OleDbException ex)
            {
                throw DbControl.HandleKnownDbFoxProAndMssqlServerExceptions(ex);
            }
        }

        /// <summary>
        /// Установление признака печати детали в бд по заводу
        /// </summary>
        public static void UpdateIsPrintFabrik(bool isFabrik, DetailPrint detailPrint)
        {
            var dbFolder = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
            const string update = "UPDATE [izd_pech] SET pr_pech = ? WHERE detal = ?";
            try
            {
                using (var oleDbConnection = DbControl.GetConnection(dbFolder))
                {
                    oleDbConnection.TryConnectOpen();

                    using (var oleDbCommand = new OleDbCommand(update, oleDbConnection))
                    {
                        oleDbCommand.Parameters.AddWithValue("@pr_pech", isFabrik ? "+" : "");
                        oleDbCommand.Parameters.AddWithValue("@detal", detailPrint.CodeDetail);

                        oleDbCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (OleDbException ex)
            {
                throw DbControl.HandleKnownDbFoxProAndMssqlServerExceptions(ex);
            }
        }

        /// <summary>
        /// Установление признака печати детали в бд для цехов
        /// </summary>
        public static void UpdateIsPrintWorkGuild(bool isWorkGuild, DetailPrint detailPrint)
        {
            var dbFolder = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
            const string update = "UPDATE [izd_pech] SET pr_pechc = ? WHERE detal = ?";
            try
            {
                using (var oleDbConnection = DbControl.GetConnection(dbFolder))
                {
                    oleDbConnection.TryConnectOpen();

                    using (var oleDbCommand = new OleDbCommand(update, oleDbConnection))
                    {
                        oleDbCommand.Parameters.AddWithValue("@pr_pechc", isWorkGuild ? "+" : "");
                        oleDbCommand.Parameters.AddWithValue("@detal", detailPrint.CodeDetail);

                        oleDbCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (OleDbException ex)
            {
                throw DbControl.HandleKnownDbFoxProAndMssqlServerExceptions(ex);
            }
        }

        /// <summary>
        /// Установление признака печати детали в бд для цеха 02
        /// </summary>
        public static void UpdateIsPrintWorkGuild02(bool isWorkGuild02, DetailPrint detailPrint)
        {
            var dbFolder = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
            const string update = "UPDATE [izd_pech] SET pr02 = ? WHERE detal = ?";
            try
            {
                using (var oleDbConnection = DbControl.GetConnection(dbFolder))
                {
                    oleDbConnection.TryConnectOpen();

                    using (var oleDbCommand = new OleDbCommand(update, oleDbConnection))
                    {
                        oleDbCommand.Parameters.AddWithValue("@pr02", isWorkGuild02 ? "+" : "");
                        oleDbCommand.Parameters.AddWithValue("@detal", detailPrint.CodeDetail);

                        oleDbCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (OleDbException ex)
            {
                throw DbControl.HandleKnownDbFoxProAndMssqlServerExceptions(ex);
            }
        }

        /// <summary>
        /// Установление признака печати детали в бд для цеха 03
        /// </summary>
        public static void UpdateIsPrintWorkGuild03(bool isWorkGuild03, DetailPrint detailPrint)
        {
            var dbFolder = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
            const string update = "UPDATE [izd_pech] SET pr03 = ? WHERE detal = ?";
            try
            {
                using (var oleDbConnection = DbControl.GetConnection(dbFolder))
                {
                    oleDbConnection.TryConnectOpen();

                    using (var oleDbCommand = new OleDbCommand(update, oleDbConnection))
                    {
                        oleDbCommand.Parameters.AddWithValue("@pr03", isWorkGuild03 ? "+" : "");
                        oleDbCommand.Parameters.AddWithValue("@detal", detailPrint.CodeDetail);

                        oleDbCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (OleDbException ex)
            {
                throw DbControl.HandleKnownDbFoxProAndMssqlServerExceptions(ex);
            }
        }

        /// <summary>
        /// Установление признака печати детали в бд для цеха 04
        /// </summary>
        public static void UpdateIsPrintWorkGuild04(bool isWorkGuild04, DetailPrint detailPrint)
        {
            var dbFolder = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
            const string update = "UPDATE [izd_pech] SET pr04 = ? WHERE detal = ?";
            try
            {
                using (var oleDbConnection = DbControl.GetConnection(dbFolder))
                {
                    oleDbConnection.TryConnectOpen();

                    using (var oleDbCommand = new OleDbCommand(update, oleDbConnection))
                    {
                        oleDbCommand.Parameters.AddWithValue("@pr04", isWorkGuild04 ? "+" : "");
                        oleDbCommand.Parameters.AddWithValue("@detal", detailPrint.CodeDetail);

                        oleDbCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (OleDbException ex)
            {
                throw DbControl.HandleKnownDbFoxProAndMssqlServerExceptions(ex);
            }
        }

        /// <summary>
        /// Установление признака печати детали в бд для цеха 05
        /// </summary>
        public static void UpdateIsPrintWorkGuild05(bool isWorkGuild05, DetailPrint detailPrint)
        {
            var dbFolder = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
            const string update = "UPDATE [izd_pech] SET pr05 = ? WHERE detal = ?";
            try
            {
                using (var oleDbConnection = DbControl.GetConnection(dbFolder))
                {
                    oleDbConnection.TryConnectOpen();

                    using (var oleDbCommand = new OleDbCommand(update, oleDbConnection))
                    {
                        oleDbCommand.Parameters.AddWithValue("@pr05", isWorkGuild05 ? "+" : "");
                        oleDbCommand.Parameters.AddWithValue("@detal", detailPrint.CodeDetail);

                        oleDbCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (OleDbException ex)
            {
                throw DbControl.HandleKnownDbFoxProAndMssqlServerExceptions(ex);
            }
        }
    }
}
