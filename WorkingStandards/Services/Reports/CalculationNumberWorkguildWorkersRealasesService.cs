using System;
using System.Collections.Generic;
using WorkingStandards.Entities.External;
using WorkingStandards.Entities.Reports;
using WorkingStandards.Util;

namespace WorkingStandards.Services.Reports
{
    /// <summary>
    /// Сервисный класс формирование листа записей отчета [Расчет кол-ва рабочих по цехам на выпуск]
    /// </summary>
    public class CalculationNumberWorkguildWorkersRealasesService
    {
        private static readonly string DbPathTrudnorm = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
        private static readonly string DbPathBase = Properties.Settings.Default.FoxProDbFolder_Base;

        private static readonly string BodySqlQuery
            = "SELECT advx03.kizd, advx03.kc, advx03.uch, advx03.prof, " +
              "sum(vstk) as vstksum, " +
              "sum(rstk) as rstksum, " +
              "sum(prtnorm) as prtnormsum, " +
              "sum(nadb) as nadbsum, " +
              "su73.name, su73.obozn, izd_rasc.vypusk, " +
              "fsprof.name as professionName " +
              "FROM advx03 " +
              "JOIN (SELECT detal, obozn, naim, vypusk FROM izd_rasc " +
              "WHERE !Empty(vypusk) and vypusk <> 0) as izd_rasc on izd_rasc.detal = advx03.kizd " +
              "LEFT JOIN \"" + DbPathBase + "su73.dbf\" on advx03.kizd = su73.detal " +
              "LEFT JOIN \"" + DbPathBase + "fsprof.dbf\" on advx03.prof = fsprof.prof " +
              "WHERE advx03.prof <> 0 ";

        private const string SqlQueryWorkGuild = "and advx03.kc = {0} ";
        private const string SqlQueryArea = "and advx03.uch = {0} ";

        private const string SqlGroupQuery =
            "group by advx03.kizd, advx03.kc, advx03.uch, advx03.prof, su73.name, su73.obozn, izd_rasc.vypusk, fsprof.name";

        /// <summary>
        /// Логика формирование листа записей отчета [Расчет кол-ва рабочих по цехам на выпуск]
        /// </summary>
        public static List<CalculationNumberWorkguildWorkersRealase> GetCalculationNumberWorkguildWorkersRealases
            (WorkGuild workGuild, Area area)
        {
            var reportResultList = new List<CalculationNumberWorkguildWorkersRealase>();
            var buildSqlQuery = BodySqlQuery;
            if (workGuild != null)
            {
                buildSqlQuery += string.Format(SqlQueryWorkGuild, workGuild.Id);
            }
            if (workGuild != null && area != null)
            {
                buildSqlQuery += string.Format(SqlQueryArea, area.Id);
            }

            buildSqlQuery += string.Format(SqlGroupQuery);

            var sqlResult = DataTableHelper.LoadDataTableByQuery(DbPathTrudnorm, buildSqlQuery, "SqlResult");

            foreach (var row in sqlResult.Select())
            {
                var productId = (decimal) row["kizd"];
                var productMark = row["obozn"] != DBNull.Value ? ((string) row["obozn"]).Trim() : string.Empty;
                var productName = row["name"] != DBNull.Value ? ((string) row["name"]).Trim() : string.Empty;
                var professionId = (decimal) row["prof"];
                var professionName = row["professionName"] != DBNull.Value
                    ? ((string) row["professionName"]).Trim()
                    : string.Empty;
                var vstk = (decimal) row["vstksum"];
                var rstk = (decimal) row["rstksum"];
                var prtnorm = (decimal) row["prtnormsum"];
                var nadb = (decimal) row["nadbsum"];
                var vypusk = (decimal) row["vypusk"];

                reportResultList.Add(new CalculationNumberWorkguildWorkersRealase()
                {
                    ProfessionId = professionId,
                    ProfessionName = professionName,
                    ProductId = productId,
                    ProductMark = productMark,
                    ProductName = productName,
                    Vstk = vstk,
                    Rstk = rstk,
                    Prtnorm = prtnorm,
                    Nadb = nadb,
                    Vypusk = vypusk
                });
            }

            reportResultList.Sort();
            return reportResultList;
        }
    }
}