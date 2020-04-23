using System.Data.OleDb;

using WorkingStandards.Db;

namespace WorkingStandards.Services
{
	/// <summary>
	/// Обработчик сервисного слоя для класса [Детали из IzdPechи из IzdRasc]
	/// </summary>
	public class IzdPechAndIzdRascService
    {
        /// <summary>
        /// Обновление бд IzdRasc
        /// </summary>
        public static void IzdRascUpdate()
        {
            var dbPathTrudnorm = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
            var dbPathArmBase = Properties.Settings.Default.FoxProDbFolder_Fox60_arm_Base;

            var queryDelete = "DELETE FROM [izd_rasc] " +
                                       "WHERE izd_rasc.detal " +
                                       "NOT IN (" +
                                       "SELECT prdsetmc.kod_mater " +
                                       "FROM \"" + dbPathArmBase + "prdsetmc.dbf\" " +
                                       "JOIN izd_rasc ON prdsetmc.kod_mater = izd_rasc.detal)";

            var queryUpdate = "UPDATE [izd_rasc] " +
                                "SET naim = prdsetmc.naim, " +
                                    "obozn =  prdsetmc.marka " +
                              "FROM prdsetmc " +
                              "WHERE izd_rasc.detal = prdsetmc.kod_mater " +
                                "AND (izd_rasc.naim <> prdsetmc.naim OR izd_rasc.obozn =  prdsetmc.marka)";

            var queryInsert = "INSERT INTO [izd_rasc] (detal, naim, obozn, pr_rasc, kol, vypusk) " +
                              "SELECT prdsetmc.kod_mater, prdsetmc.naim, prdsetmc.marka, '', 0, 0 " +
                              "FROM \"" + dbPathArmBase + "prdsetmc.dbf\" " +
                              "WHERE prdsetmc.kod_mater NOT IN " +
                              "(SELECT prdsetmc.kod_mater " +
                              "FROM \"" + dbPathArmBase + "prdsetmc.dbf\" " +
                              "JOIN izd_rasc ON prdsetmc.kod_mater = izd_rasc.detal) and kod_mater >= 100000000000 " +
                              "and kod_mater % 100000000000 < 10000";

            // Удаление деталей из izd_rasc которых нет в prdsetmc
            try
            {
                using (var oleDbConnection = DbControl.GetConnection(dbPathTrudnorm))
                {
                    oleDbConnection.TryConnectOpen();
                    // Удаление деталей из izd_rasc которых нет в prdsetmc
                    using (var oleDbCommand = new OleDbCommand(queryDelete, oleDbConnection))
                    {
                        oleDbCommand.ExecuteNonQuery();
                    }
                    // Обнавление наименований деталей izd_rasc
                    using (var oleDbCommand = new OleDbCommand(queryUpdate, oleDbConnection))
                    {
                        oleDbCommand.ExecuteNonQuery();
                    }
                    // Добавление деталей которых нет из prdsetmc в izd_rasc
                    using (var oleDbCommand = new OleDbCommand(queryInsert, oleDbConnection))
                    {
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
        /// Обновление бд IzdPech
        /// </summary>
        public static void IzdPechUpdate()
        {
            var dbPathTrudnorm = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
            var dbPathArmBase = Properties.Settings.Default.FoxProDbFolder_Fox60_arm_Base;

            var queryDelete = "DELETE FROM [izd_pech] " +
                                       "WHERE izd_pech.detal " +
                                       "NOT IN (" +
                                       "SELECT prdsetmc.kod_mater " +
                                       "FROM \"" + dbPathArmBase + "prdsetmc.dbf\" " +
                                       "JOIN izd_pech ON prdsetmc.kod_mater = izd_pech.detal)";

            var queryUpdate = "UPDATE [izd_pech] " +
                              "SET name = prdsetmc.naim, " +
                              "obozn =  prdsetmc.marka " +
                              "FROM prdsetmc " +
                              "WHERE izd_pech.detal = prdsetmc.kod_mater " +
                              "AND (izd_pech.name <> prdsetmc.naim OR izd_pech.obozn =  prdsetmc.marka)";

            var queryInsert = "INSERT INTO [izd_pech] (detal, name, obozn, pr_pech, pr_pechc, " +
                              "pr02, pr03, pr04, pr05, pr06, pr07) " +
                              "SELECT prdsetmc.kod_mater, prdsetmc.naim, prdsetmc.marka, '', '', " +
                              "'', '', '', '', '', '' " +
                              "FROM \"" + dbPathArmBase + "prdsetmc.dbf\" " +
                              "WHERE prdsetmc.kod_mater NOT IN " +
                              "(SELECT prdsetmc.kod_mater " +
                              "FROM \"" + dbPathArmBase + "prdsetmc.dbf\" " +
                              "JOIN izd_pech ON prdsetmc.kod_mater = izd_pech.detal) and kod_mater >= 100000000000 " +
                              "and kod_mater % 100000000000 < 10000";

            try
            {
                using (var oleDbConnection = DbControl.GetConnection(dbPathTrudnorm))
                {
                    oleDbConnection.TryConnectOpen();
                    // Удаление деталей из izd_pech которых нет в prdsetmc
                    using (var oleDbCommand = new OleDbCommand(queryDelete, oleDbConnection))
                    {
                        oleDbCommand.ExecuteNonQuery();
                    }
                    // Обнавление наименований деталей izd_pech
                    using (var oleDbCommand = new OleDbCommand(queryUpdate, oleDbConnection))
                    {
                        oleDbCommand.ExecuteNonQuery();
                    }
                    // Добавление деталей которых нет из prdsetmc в izd_pech
                    using (var oleDbCommand = new OleDbCommand(queryInsert, oleDbConnection))
                    {
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
