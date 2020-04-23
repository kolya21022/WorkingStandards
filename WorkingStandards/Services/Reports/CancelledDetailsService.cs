using System;
using System.Collections.Generic;

using WorkingStandards.Entities.Reports;
using WorkingStandards.Util;

namespace WorkingStandards.Services.Reports
{
    /// <summary>
    /// Сервисный класс формирование листа записей отчета [Аннулируемые детали]
    /// </summary>
    public class CancelledDetailsService
    {
        private static readonly string DbPathCi = Properties.Settings.Default.FoxProDbFolder_Foxpro_CI;
        private static readonly string DbPathBase = Properties.Settings.Default.FoxProDbFolder_Base;

        private static readonly string BodySqlQuery
            = "SELECT anul67.detal, " +
              "su73.name as nameDetal, " +
              "su73.obozn, " +
              "anul67.operac, " +
              "anul67.ceh, " +
              "anul67.uch, " +
              "anul67.tehoper, " +
              "tehnoper.naim as nameTehnoper, " +
              "anul67.koefvr, " +
              "anul67.prof, " +
              "fsprof.name as nameProf, " +
              "anul67.kolrab, " +
              "anul67.razr, " +
              "anul67.koldet, " +
              "anul67.ednorm, " +
              "anul67.tarset, " +
              "anul67.vidnorm, " +
              "anul67.razmpart, " +
              "anul67.tpz, " +
              "anul67.vst, " +
              "anul67.koefneos, " +
              "anul67.vstk, " +
              "anul67.rstk, " +
              "anul67.nomizv, " +
              "anul67.dataizv " +
              "FROM anul67 " +
              "LEFT JOIN \"" + DbPathBase + "su73.dbf\" on anul67.detal = su73.detal " +
              "LEFT JOIN \"" + DbPathBase + "fsprof.dbf\" on anul67.prof = fsprof.prof " +
              "LEFT JOIN \"" + DbPathBase + "tehnoper.dbf\" on anul67.tehoper = tehnoper.kod " +
              "WHERE anul67.dataizv >= ctod( '{0}' ) and anul67.dataizv <= ctod( '{1}' ) and anul67.detal <> 0";

        private static readonly string BodySqlQuery2
            = "SELECT anul67.detal, " +
              "su73.name as nameDetal, " +
              "su73.obozn, " +
              "anul67.operac, " +
              "anul67.ceh, " +
              "anul67.uch, " +
              "anul67.tehoper, " +
              "tehnoper.naim as nameTehnoper, " +
              "anul67.koefvr, " +
              "anul67.prof, " +
              "fsprof.name as nameProf, " +
              "anul67.kolrab, " +
              "anul67.razr, " +
              "anul67.koldet, " +
              "anul67.ednorm, " +
              "anul67.tarset, " +
              "anul67.vidnorm, " +
              "anul67.razmpart, " +
              "anul67.tpz, " +
              "anul67.vst, " +
              "anul67.koefneos, " +
              "anul67.vstk, " +
              "anul67.rstk, " +
              "anul67.nomizv, " +
              "anul67.dataizv " +
              "FROM anul67 " +
              "LEFT JOIN \"" + DbPathBase + "su73.dbf\" on anul67.detal = su73.detal " +
              "LEFT JOIN \"" + DbPathBase + "fsprof.dbf\" on anul67.prof = fsprof.prof " +
              "LEFT JOIN \"" + DbPathBase + "tehnoper.dbf\" on anul67.tehoper = tehnoper.kod " +
              "WHERE anul67.detal = {0}";

        /// <summary>
        /// Логика формирование листа записей отчета [Аннулируемые детали за месяц]
        /// </summary>
        public static List<CancelledDetail> GetCancelledDetailsOnDate
            (DateTime startDate, DateTime endDate)
        {
            var reportResultList = new List<CancelledDetail>();  
            var sqlResult = DataTableHelper.LoadDataTableByQuery(DbPathCi, 
                string.Format(BodySqlQuery, startDate.ToString("MM/dd/yyyy"), endDate.ToString("MM/dd/yyyy")), "SqlResult");

            foreach (var row in sqlResult.Select())
            {
                var detal = (decimal)row["detal"];
                var nameDetal = row["nameDetal"] != DBNull.Value ? ((string)row["nameDetal"]).Trim() : string.Empty;
                var obozn = row["obozn"] != DBNull.Value ? ((string)row["obozn"]).Trim() : string.Empty;
                var operac = (decimal)row["operac"];
                var workGuild = (decimal)row["ceh"];
                var uch = (decimal)row["uch"];
                var tehoper = (decimal)row["tehoper"];
                var nameTehnoper = row["nameTehnoper"] != DBNull.Value ? ((string)row["nameTehnoper"]).Trim() : string.Empty;
                var koefvr = (decimal)row["koefvr"];
                var prof = (decimal)row["prof"];
                var nameProf = row["nameProf"] != DBNull.Value ? ((string)row["nameProf"]).Trim() : string.Empty;
                var kolrab = (decimal)row["kolrab"];
                var razr = (decimal)row["razr"];
                var koldet = (decimal)row["koldet"];
                var ednorm = (decimal)row["ednorm"];
                var tarset = (decimal)row["tarset"];
                var vidnorm = (decimal)row["vidnorm"];
                var razmpart = (decimal)row["razmpart"];
                var tpz = (decimal)row["tpz"];
                var koefneos = (decimal)row["koefneos"];
                var vstk = (decimal)row["vstk"];
                var rstk = (decimal)row["rstk"];
                var nomizv = row["nomizv"] != DBNull.Value ? ((string)row["nomizv"]).Trim() : string.Empty;
                var dateIzv = (DateTime)row["dataizv"];
                var vst = (decimal)row["vst"];

                reportResultList.Add(new CancelledDetail()
                {
                    CodeDetail = detal,
                    NameDetail = nameDetal,
                    OboznDetail = obozn,
                    Operac = operac,
                    WorkGuildId = workGuild,
                    AreaId = uch,
                    TehnoperId = tehoper,
                    NameTehnoper = nameTehnoper,
                    Koefvr = koefvr,
                    ProfId = prof,
                    NameProf = nameProf,
                    Kolrab = kolrab,
                    Razr = razr,
                    Koldet = koldet,
                    Ednorm = ednorm,
                    Tarset = tarset,
                    Vidnorm = vidnorm,
                    Razmpart = razmpart,
                    Tpz = tpz,
                    Koefneos = koefneos,
                    Vstk = vstk,
                    Rstk = rstk,
                    Nomizv = nomizv,
                    DateIzv = dateIzv,
                    Vst = vst
                });
            }
            reportResultList.Sort();
            return reportResultList;
        }

        /// <summary>
        /// Логика формирование листа записей отчета [Аннулируемые детали]
        /// </summary>
        public static List<CancelledDetail> GetCancelledDetails(decimal codeDetal)
        {
            var reportResultList = new List<CancelledDetail>();
            var sqlResult = DataTableHelper.LoadDataTableByQuery(DbPathCi, string.Format(BodySqlQuery2, codeDetal), "SqlResult");

            foreach (var row in sqlResult.Select())
            {
                var detal = (decimal)row["detal"];
                var nameDetal = row["nameDetal"] != DBNull.Value ? ((string)row["nameDetal"]).Trim() : string.Empty;
                var obozn = row["obozn"] != DBNull.Value ? ((string)row["obozn"]).Trim() : string.Empty;
                var operac = (decimal)row["operac"];
                var workGuild = (decimal)row["ceh"];
                var uch = (decimal)row["uch"];
                var tehoper = (decimal)row["tehoper"];
                var nameTehnoper = row["nameTehnoper"] != DBNull.Value ? ((string)row["nameTehnoper"]).Trim() : string.Empty;
                var koefvr = (decimal)row["koefvr"];
                var prof = (decimal)row["prof"];
                var nameProf = row["nameProf"] != DBNull.Value ? ((string)row["nameProf"]).Trim() : string.Empty;
                var kolrab = (decimal)row["kolrab"];
                var razr = (decimal)row["razr"];
                var koldet = (decimal)row["koldet"];
                var ednorm = (decimal)row["ednorm"];
                var tarset = (decimal)row["tarset"];
                var vidnorm = (decimal)row["vidnorm"];
                var razmpart = (decimal)row["razmpart"];
                var tpz = (decimal)row["tpz"];
                var koefneos = (decimal)row["koefneos"];
                var vstk = (decimal)row["vstk"];
                var rstk = (decimal)row["rstk"];
                var nomizv = row["nomizv"] != DBNull.Value ? ((string)row["nomizv"]).Trim() : string.Empty;
                var dateIzv = (DateTime)row["dataizv"];
                var vst = (decimal)row["vst"];

                reportResultList.Add(new CancelledDetail()
                {
                    CodeDetail = detal,
                    NameDetail = nameDetal,
                    OboznDetail = obozn,
                    Operac = operac,
                    WorkGuildId = workGuild,
                    AreaId = uch,
                    TehnoperId = tehoper,
                    NameTehnoper = nameTehnoper,
                    Koefvr = koefvr,
                    ProfId = prof,
                    NameProf = nameProf,
                    Kolrab = kolrab,
                    Razr = razr,
                    Koldet = koldet,
                    Ednorm = ednorm,
                    Tarset = tarset,
                    Vidnorm = vidnorm,
                    Razmpart = razmpart,
                    Tpz = tpz,
                    Koefneos = koefneos,
                    Vstk = vstk,
                    Rstk = rstk,
                    Nomizv = nomizv,
                    DateIzv = dateIzv,
                    Vst = vst
                });
            }
            reportResultList.Sort();
            return reportResultList;
        }
    }
}

