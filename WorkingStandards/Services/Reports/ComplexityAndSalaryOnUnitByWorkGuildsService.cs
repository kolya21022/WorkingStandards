using System;
using System.Collections.Generic;

using WorkingStandards.Entities.External;
using WorkingStandards.Entities.Reports;
using WorkingStandards.Util;

namespace WorkingStandards.Services.Reports
{
    /// <summary>
    /// Сервисный класс формирование листа записей отчета [Просмотр трудоемкости и зарплаты на сбор.единицы по цехам]
    /// </summary>
    public class ComplexityAndSalaryOnUnitByWorkGuildsService
    {
        private static readonly string DbPathCi = Properties.Settings.Default.FoxProDbFolder_Foxpro_CI;
        private static readonly string DbPathBase = Properties.Settings.Default.FoxProDbFolder_Base;
        private static readonly string DbPathArmBase = Properties.Settings.Default.FoxProDbFolder_Fox60_arm_Base;

        private static readonly string BodySqlQuery = "SELECT result.v_kizd, " +                                                   
                                                      "result.detal, " +
                                                      "result.detalName, " +
                                                      "result.detalMark, " +
                                                      "result.ceh, " +
                                                      "result.uch, " +
                                                      "sum(result.VSTK) as vstksum, " +
                                                      "sum(result.rstk) as rstksum, " +
                                                      "sum(result.nadb) as nadb, " +
                                                      "sum(result.prtnorm) as prtnorm " +

                                                      "FROM(" +

                                                      "SELECT acnzp.sb_ed as v_kizd, " +
                                                      "acnzp.kolsb, " +
                                                      "nu67.detal, " +
                                                      "su73.Name as detalName, " +
                                                      "su73.Obozn as detalMark, " +
                                                      "nu67.ceh, " +
                                                      "nu67.uch, " +
                                                      "nu67.vidnorm, " +
                                                      "round((sum(nu67.vstk)*kolsb)/60,3) as VSTK, " +
                                                      "round((sum(rstk)*acnzp.kolsb)/100,3) as RSTK, " +
                                                      "iif(nu67.vidnorm = 11, round(0.05 * (sum(rstk)*acnzp.kolsb)/100,3), 0) as NADB, " +
                                                      "round(0.05 * (sum(rstk)*acnzp.kolsb)/100,3) as PRTNORM " +
                                                      "FROM acnzp " +
                                                      "JOIN \"" + DbPathCi + "nu67.dbf\" on acnzp.chto = nu67.detal " +
                                                      "JOIN (" +

                                                      "SELECT detal, name, obozn FROM \"" + DbPathBase + "su73.dbf\" " +
                                                      "WHERE prin = 1 OR prin = 31 OR prin = 32 OR prin = 41 OR prin = 42 " +
                                                      "OR prin = 44 OR prin = 51 OR prin = 52 OR prin = 61 OR prin = 62 " +
                                                      "OR prin = 81" +

                                                      ") as su73 on nu67.detal = su73.detal " +
                                                      "WHERE acnzp.sb_ed = {0} " +
                                                      "GROUP BY acnzp.sb_ed ,acnzp.kolsb, nu67.detal, su73.Name, " +
                                                      "su73.Obozn, nu67.ceh, nu67.uch, nu67.vidnorm" +

                                                      ") as result ";

        // Сбор ее самой
        private static readonly string Body2SqlQuery = "SELECT result.detal, " +
                                                       "result.detalName, " +
                                                       "result.detalMark, " +
                                                       "result.ceh, " +
                                                       "result.uch, " +
                                                       "sum(result.VSTK) as vstksum, " +
                                                       "sum(result.rstk) as rstksum, " +
                                                       "sum(result.nadb) as nadb, " +
                                                       "sum(result.prtnorm) as prtnorm " +
                                                       "FROM(" +

                                                       "SELECT nu67.detal, " +
                                                       "su73.Name as detalName, " +
                                                       "su73.Obozn as detalMark, " +
                                                       "nu67.ceh, " +
                                                       "nu67.uch, " +
                                                       "nu67.vidnorm, " +
                                                       "round((sum(nu67.vstk)*1)/60,3) as VSTK, " +
                                                       "round((sum(nu67.rstk)*1)/100,3) as RSTK, " +
                                                       "iif(nu67.vidnorm = 11, round(0.05 * (sum(nu67.rstk)*1)/100,3), 0) as NADB, " +
                                                       "round(0.05 * (sum(nu67.rstk)*1)/100,3) as PRTNORM " +
                                                       "FROM nu67 " +
                                                       "JOIN( " +

                                                       "SELECT detal, name, obozn " +
                                                       "FROM \"" + DbPathBase + "su73.dbf\" " +
                                                       "WHERE prin = 1 OR prin = 31 OR prin = 32 OR prin = 41 " +
                                                       "OR prin = 42 OR prin = 44 OR prin = 51 OR prin = 52 " +
                                                       "OR prin = 61 OR prin = 62 OR prin = 81) as su73 " +

                                                       "on nu67.detal = su73.detal " +
                                                       "WHERE nu67.detal = {0} " +
                                                       "group by nu67.detal, su73.Name, su73.Obozn, " +
                                                       "nu67.ceh, nu67.uch, nu67.vidnorm" +

                                                       ") as result ";

        private const string SqlQueryCeh = "result.ceh = {0} ";
        private const string SqlQueryGroup = "GROUP BY result.v_kizd, result.detal, " +
                                             "result.detalName, result.detalMark, result.ceh, result.uch";

        private const string SqlQueryGroup2 = "GROUP BY result.detal, result.detalName, " +
                                              "result.detalMark, result.ceh, result.uch";

		/// <summary>
		/// Логика формирование листа записей отчета [Просмотр трудоемкости и зарплаты на сбор.единицы по цехам]
		/// </summary>
		public static List<ComplexityAndSalaryOnUnitByWorkGuild> GetPrintingOfProsuctInContextOfDetails(decimal codeProduct,
            WorkGuild workGuild)
        {
            var reportResultList = new List<ComplexityAndSalaryOnUnitByWorkGuild>();

            var buildSqlQuery = string.Format(BodySqlQuery, codeProduct);
            if (workGuild != null)
            {
                buildSqlQuery += "WHERE " + string.Format(SqlQueryCeh, workGuild.Id);
            }
            buildSqlQuery += SqlQueryGroup;

            var sqlResult = DataTableHelper.LoadDataTableByQuery(DbPathArmBase, buildSqlQuery, "SqlResult");

            foreach (var row in sqlResult.Select())
            {
                var detalId = (decimal) row["detal"];
                var detalName = row["detalName"] != DBNull.Value ? ((string) row["detalName"]).Trim() : string.Empty;
                var detalMark = row["detalMark"] != DBNull.Value ? ((string) row["detalMark"]).Trim() : string.Empty;
                var workGuildId = (decimal) row["ceh"];
                var areaId = (decimal)row["uch"];
                var vstk = (decimal) row["vstksum"];
                var rstk = (decimal) row["rstksum"];
                var nadb = (decimal)row["nadb"];
                var prtnorm = (decimal)row["prtnorm"];

                reportResultList.Add(new ComplexityAndSalaryOnUnitByWorkGuild()
                {
                    DetailId = detalId,
                    DetailName = detalName, 
                    DetailMark = detalMark,
                    WorkGuildId = workGuildId,
                    AreaId = areaId,
                    Vstk = vstk,
                    Rstk = rstk,
                    Nadb = nadb,
                    Prtnorm = prtnorm
                });

            }
            //////////// Сбор его самого (1 запись скореее всего(если в 1 цеху))/////////////////
            var build2SqlQuery = string.Format(Body2SqlQuery, codeProduct);
            if (workGuild != null)
            {
                build2SqlQuery += "WHERE " + string.Format(SqlQueryCeh, workGuild.Id);
            }
            build2SqlQuery += SqlQueryGroup2;

            var sql2Result = DataTableHelper.LoadDataTableByQuery(DbPathCi, build2SqlQuery, "Sql2Result");

            foreach (var row in sql2Result.Select())
            {
                var detalId = (decimal)row["detal"];
                var detalName = row["detalName"] != DBNull.Value ? ((string)row["detalName"]).Trim() : string.Empty;
                var detalMark = row["detalMark"] != DBNull.Value ? ((string)row["detalMark"]).Trim() : string.Empty;
                var workGuildId = (decimal)row["ceh"];
                var areaId = (decimal)row["uch"];
                var vstk = (decimal)row["vstksum"];
                var rstk = (decimal)row["rstksum"];
                var nadb = (decimal)row["nadb"];
                var prtnorm = (decimal)row["prtnorm"];

                reportResultList.Add(new ComplexityAndSalaryOnUnitByWorkGuild()
                {
                    DetailId = detalId,
                    DetailName = detalName,
                    DetailMark = detalMark,
                    WorkGuildId = workGuildId,
                    AreaId = areaId,
                    Vstk = vstk,
                    Rstk = rstk,
                    Nadb = nadb,
                    Prtnorm = prtnorm
                });

            }

            reportResultList.Sort();
            return reportResultList;
        }



        private static readonly string BodySqlQueryAll = "SELECT result.v_kizd, " +
                                                      "result.detal, " +
                                                      "result.detalName, " +
                                                      "result.detalMark, " +
                                                      "sum(result.VSTK) as vstksum, " +
                                                      "sum(result.rstk) as rstksum, " +
                                                      "sum(result.nadb) as nadb, " +
                                                      "sum(result.prtnorm) as prtnorm " +

                                                      "FROM(" +

                                                      "SELECT acnzp.sb_ed as v_kizd, " +
                                                      "acnzp.kolsb, " +
                                                      "nu67.detal, " +
                                                      "su73.Name as detalName, " +
                                                      "su73.Obozn as detalMark, " +
                                                      "nu67.vidnorm, " +
                                                      "round((sum(nu67.vstk)*kolsb)/60,3) as VSTK, " +
                                                      "round((sum(rstk)*acnzp.kolsb)/100,3) as RSTK, " +
                                                      "iif(nu67.vidnorm = 11, round(0.05 * (sum(rstk)*acnzp.kolsb)/100,3), 0) as NADB, " +
                                                      "round(0.05 * (sum(rstk)*acnzp.kolsb)/100,3) as PRTNORM " +
                                                      "FROM acnzp " +
                                                      "JOIN \"" + DbPathCi + "nu67.dbf\" on acnzp.chto = nu67.detal " +
                                                      "JOIN (" +

                                                      "SELECT detal, name, obozn FROM \"" + DbPathBase + "su73.dbf\" " +
                                                      "WHERE prin = 1 OR prin = 31 OR prin = 32 OR prin = 41 OR prin = 42 " +
                                                      "OR prin = 44 OR prin = 51 OR prin = 52 OR prin = 61 OR prin = 62 " +
                                                      "OR prin = 81" +

                                                      ") as su73 on nu67.detal = su73.detal " +
                                                      "WHERE acnzp.sb_ed = {0} " +
                                                      "GROUP BY acnzp.sb_ed ,acnzp.kolsb, nu67.detal, su73.Name, " +
                                                      "su73.Obozn, nu67.vidnorm" +

                                                      ") as result ";

        // Сбор ее самой
        private static readonly string Body2SqlQueryAll = "SELECT result.detal, " +
                                                       "result.detalName, " +
                                                       "result.detalMark, " +
                                                       "sum(result.VSTK) as vstksum, " +
                                                       "sum(result.rstk) as rstksum, " +
                                                       "sum(result.nadb) as nadb, " +
                                                       "sum(result.prtnorm) as prtnorm " +
                                                       "FROM(" +

                                                       "SELECT nu67.detal, " +
                                                       "su73.Name as detalName, " +
                                                       "su73.Obozn as detalMark, " +
                                                       "nu67.vidnorm, " +
                                                       "round((sum(nu67.vstk)*1)/60,3) as VSTK, " +
                                                       "round((sum(nu67.rstk)*1)/100,3) as RSTK, " +
                                                       "iif(nu67.vidnorm = 11, round(0.05 * (sum(nu67.rstk)*1)/100,3), 0) as NADB, " +
                                                       "round(0.05 * (sum(nu67.rstk)*1)/100,3) as PRTNORM " +
                                                       "FROM nu67 " +
                                                       "JOIN( " +

                                                       "SELECT detal, name, obozn " +
                                                       "FROM \"" + DbPathBase + "su73.dbf\" " +
                                                       "WHERE prin = 1 OR prin = 31 OR prin = 32 OR prin = 41 " +
                                                       "OR prin = 42 OR prin = 44 OR prin = 51 OR prin = 52 " +
                                                       "OR prin = 61 OR prin = 62 OR prin = 81) as su73 " +

                                                       "on nu67.detal = su73.detal " +
                                                       "WHERE nu67.detal = {0} " +
                                                       "group by nu67.detal, su73.Name, su73.Obozn, " +
                                                       "nu67.vidnorm" +

                                                       ") as result ";

        private const string SqlQueryGroupAll = "GROUP BY result.v_kizd, result.detal, " +
                                             "result.detalName, result.detalMark";

        private const string SqlQueryGroup2All = "GROUP BY result.detal, result.detalName, " +
                                              "result.detalMark";

        /// <summary>
        /// Логика формирование листа записей отчета [Просмотр трудоемкости и зарплаты на сбор.единицы по цехам]
        /// </summary>
        public static List<ComplexityAndSalaryOnUnitByWorkGuild> GetPrintingOfProsuctInContextOfDetailsAll(decimal codeProduct)
        {
            var reportResultList = new List<ComplexityAndSalaryOnUnitByWorkGuild>();

            var buildSqlQuery = string.Format(BodySqlQueryAll, codeProduct);
            buildSqlQuery += SqlQueryGroupAll;

            var sqlResult = DataTableHelper.LoadDataTableByQuery(DbPathArmBase, buildSqlQuery, "SqlResult");

            foreach (var row in sqlResult.Select())
            {
                var detalId = (decimal)row["detal"];
                var detalName = row["detalName"] != DBNull.Value ? ((string)row["detalName"]).Trim() : string.Empty;
                var detalMark = row["detalMark"] != DBNull.Value ? ((string)row["detalMark"]).Trim() : string.Empty;
                var vstk = (decimal)row["vstksum"];
                var rstk = (decimal)row["rstksum"];
                var nadb = (decimal)row["nadb"];
                var prtnorm = (decimal)row["prtnorm"];

                reportResultList.Add(new ComplexityAndSalaryOnUnitByWorkGuild()
                {
                    DetailId = detalId,
                    DetailName = detalName,
                    DetailMark = detalMark,
                    WorkGuildId = 99, // скрыть итоги в отчете по этому признаку
                    AreaId = 99, // скрыть итоги в отчете по этому признаку
                    Vstk = vstk,
                    Rstk = rstk,
                    Nadb = nadb,
                    Prtnorm = prtnorm
                });

            }
            //////////// Сбор его самого (1 запись скореее всего(если в 1 цеху))/////////////////
            var build2SqlQuery = string.Format(Body2SqlQueryAll, codeProduct);
            build2SqlQuery += SqlQueryGroup2All;

            var sql2Result = DataTableHelper.LoadDataTableByQuery(DbPathCi, build2SqlQuery, "Sql2Result");

            foreach (var row in sql2Result.Select())
            {
                var detalId = (decimal)row["detal"];
                var detalName = row["detalName"] != DBNull.Value ? ((string)row["detalName"]).Trim() : string.Empty;
                var detalMark = row["detalMark"] != DBNull.Value ? ((string)row["detalMark"]).Trim() : string.Empty;
                var vstk = (decimal)row["vstksum"];
                var rstk = (decimal)row["rstksum"];
                var nadb = (decimal)row["nadb"];
                var prtnorm = (decimal)row["prtnorm"];

                reportResultList.Add(new ComplexityAndSalaryOnUnitByWorkGuild()
                {
                    DetailId = detalId,
                    DetailName = detalName,
                    DetailMark = detalMark,
                    WorkGuildId = 99, // скрыть итоги в отчете по этому признаку
                    AreaId = 99, // скрыть итоги в отчете по этому признаку
                    Vstk = vstk,
                    Rstk = rstk,
                    Nadb = nadb,
                    Prtnorm = prtnorm
                });

            }

            reportResultList.Sort();
            return reportResultList;
        }
    }
}
