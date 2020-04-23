using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;

using WorkingStandards.Db;
using WorkingStandards.Entities.External;

namespace WorkingStandards.Storages
{
    /// <summary>
    /// Обработчик запросов хранилища данных для таблицы выпуска изделий [RealaseProduct]
    /// </summary>
    public class RealaseProductsStorage
    {
        /// <summary>
        /// Получение коллекции [Выпуска изделий] (dbf izd_rasc)
        /// </summary>
        public static List<RealaseProduct> GetAll()
        {
            var dbFolder = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
            const string query = "SELECT DISTINCT detal, " +
                                 "naim, " +
                                 "obozn, " +
                                 "vypusk FROM [izd_rasc]";

            var realaseProducts = new List<RealaseProduct>();
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
                                var codeDetail = reader.GetDecimal(0);
                                var name = reader.GetString(1).Trim();
                                var mark = reader.GetString(2).Trim();
                                var realase = reader.GetDecimal(3);

                                var realaseProduct = new RealaseProduct
                                {
                                    CodeDetail = codeDetail,
                                    Name = name,
                                    Mark = mark,
                                    Realase = realase
                                };
                                realaseProducts.Add(realaseProduct);
                            }
                        }
                    }
                }

                return realaseProducts;
            }
            catch (DbException ex)
            {
                throw DbControl.HandleKnownDbFoxProAndMssqlServerExceptions(ex);
            }
        }

        /// <summary>
        /// Обновление [Выпуска изделий] (dbf izd_rasc)
        /// </summary>
        public static void Update(RealaseProduct realaseProduct)
        {
            var dbFolder = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
            const string update = "UPDATE [izd_rasc] SET vypusk = ? WHERE detal = ?";
            try
            {
                using (var oleDbConnection = DbControl.GetConnection(dbFolder))
                {
                    oleDbConnection.TryConnectOpen();

                    using (var oleDbCommand = new OleDbCommand(update, oleDbConnection))
                    {
                        oleDbCommand.Parameters.AddWithValue("@vypusk", realaseProduct.Realase);
                        oleDbCommand.Parameters.AddWithValue("@detal", realaseProduct.CodeDetail);

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
