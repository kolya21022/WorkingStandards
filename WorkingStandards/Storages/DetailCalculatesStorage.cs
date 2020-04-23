using System.Collections.Generic;
using System.Data.OleDb;

using WorkingStandards.Db;
using WorkingStandards.Entities.External;

namespace WorkingStandards.Storages
{
    /// <summary>
    /// Обработчик запросов хранилища данных для таблицы деталей расчета сводных трудовых нормативов [DetailCalculate]
    /// </summary>
    public class DetailCalculatesStorage
    {
        /// <summary>
        /// Получение коллекции [Продуктов расчета сводных трудовых нормативов]
        /// </summary>
        public static List<DetailCalculate> GetAll()
        {
            var dbFolder = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
            const string query = "SELECT detal, naim, obozn, pr_rasc FROM [izd_rasc]";
            var detailCalculates = new List<DetailCalculate>();
            try
            {
                using (var oleDbConnection = DbControl.GetConnection(dbFolder))
                {
                    oleDbConnection.TryConnectOpen();
                    oleDbConnection.VerifyInstalledEncoding("izd_rasc");

                    using (var oleDbCommand = new OleDbCommand(query, oleDbConnection))
                    {
                        using (var reader = oleDbCommand.ExecuteReader())
                        {
                            while (reader != null && reader.Read())
                            {
                                var codeDetail = reader.GetDecimal(0);
                                var name = reader.GetString(1).Trim();
                                var mark = reader.GetString(2).Trim();
                                var isCalculate = reader.GetString(3).Trim() == "+";
                                var detailCalculate = new DetailCalculate
                                {
                                    CodeDetail = codeDetail,
                                    Name = name,
                                    Mark = mark,
                                    IsCalculate = isCalculate
                                };
                                detailCalculates.Add(detailCalculate);
                            }
                        }
                    }
                }
                return detailCalculates;
            }
            catch (OleDbException ex)
            {
                throw DbControl.HandleKnownDbFoxProAndMssqlServerExceptions(ex);
            }
        }

        /// <summary>
        /// Установление признака детали в бд
        /// </summary>
        public static void UpdateIsCalculate(bool isCalculate, DetailCalculate detailCalculate)
        {
            var dbFolder = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
            const string update = "UPDATE [izd_rasc] SET pr_rasc = ? WHERE detal = ?";
            try
            {
                using (var oleDbConnection = DbControl.GetConnection(dbFolder))
                {
                    oleDbConnection.TryConnectOpen();

                    using (var oleDbCommand = new OleDbCommand(update, oleDbConnection))
                    {
                        oleDbCommand.Parameters.AddWithValue("@pr_rasc", isCalculate ? "+" : "" );
                        oleDbCommand.Parameters.AddWithValue("@detal", detailCalculate.CodeDetail);

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
