using System;
using System.Collections.Generic;
using System.Data.OleDb;

using WorkingStandards.Db;
using WorkingStandards.Entities.External;

namespace WorkingStandards.Storages
{
    /// <summary>
    /// Обработчик запросов хранилища данных для изделий [Products]
    /// </summary>
    public static class ProductsStorage
    {
        /// <summary>
        /// Тестирование соединения с таблицей 
        /// (для проверки перед последующей модификацией, ввиду отсутствия транзакций для этого ввида таблиц)
        /// </summary>
        public static void TestConnection(OleDbConnection oleDbConnection)
        {
            const string query = "SELECT * FROM [izd_rasc]";
            using (var command = new OleDbCommand(query, oleDbConnection))
            {
                using (command.ExecuteReader())
                {

                }
            }
        }

        /// <summary>
        /// Получение коллекции [Изделий]
        /// </summary>
        public static List<Product> GetProducts()
        {
            var dbFolder = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
            const string query = "SELECT detal AS id, naim as name, obozn as mark FROM [izd_rasc]";

            var products = new List<Product>();
            try
            {
                using (var connection = DbControl.GetConnection(dbFolder))
                {
                    connection.TryConnectOpen();

                    // Проверки наличия установленных кодировок в DBF-файлах и проверки соединений с этими файлами
                    connection.VerifyInstalledEncoding("izd_rasc");
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

                                var product = new Product
                                {
                                    Id = id,
                                    Name = name,
                                    Mark = mark
                                };
                                products.Add(product);
                            }
                        }
                    }
                }
                return products;
            }
            catch (OleDbException ex)
            {
                throw DbControl.HandleKnownDbFoxProAndMssqlServerExceptions(ex);
            }
        }

        /// <summary>
        /// Получение коллекции [Сборочных единиц]
        /// </summary>
        public static List<Product> GetAssemblyUnits()
        {
            var bbPathArmBase = Properties.Settings.Default.FoxProDbFolder_Fox60_arm_Base;
            var dbPathBase = Properties.Settings.Default.FoxProDbFolder_Base;
            var query = "SELECT DISTINCT acux01.chto, su73.name, su73.obozn FROM acux01 " +
                        "LEFT JOIN \"" + dbPathBase + "su73.dbf\" on acux01.chto = su73.detal " +
                        "WHERE acux01.prin = 31 or acux01.prin = 32 or acux01.prin = 33 " +
                        "or acux01.prin = 81 or acux01.prin = 82 or acux01.prin = 83";

            var products = new List<Product>();
            try
            {
                using (var connection = DbControl.GetConnection(bbPathArmBase))
                {
                    connection.TryConnectOpen();

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
                               
                                var product = new Product
                                {
                                    Id = id,
                                    Name = name,
                                    Mark = mark
                                };
                                products.Add(product);
                            }
                        }
                    }
                }
                return products;
            }
            catch (OleDbException ex)
            {
                throw DbControl.HandleKnownDbFoxProAndMssqlServerExceptions(ex);
            }
        }

        /// <summary>
        /// Получение коллекции [Деталей] из su73
        /// </summary>
        public static List<Product> GetDetailSu73()
        {
            var dbPathBase = Properties.Settings.Default.FoxProDbFolder_Base;
            const string query = "SELECT detal AS id, name, obozn as mark FROM [su73]";

            var products = new List<Product>();
            try
            {
                using (var connection = DbControl.GetConnection(dbPathBase))
                {
                    connection.TryConnectOpen();

                    // Проверки наличия установленных кодировок в DBF-файлах и проверки соединений с этими файлами
                    connection.VerifyInstalledEncoding("su73");
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

                                var product = new Product
                                {
                                    Id = id,
                                    Name = name,
                                    Mark = mark
                                };
                                products.Add(product);
                            }
                        }
                    }
                }
                return products;
            }
            catch (OleDbException ex)
            {
                throw DbControl.HandleKnownDbFoxProAndMssqlServerExceptions(ex);
            }
        }
    }
}
