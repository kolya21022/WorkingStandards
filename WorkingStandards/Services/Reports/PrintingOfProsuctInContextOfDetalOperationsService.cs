using System;
using System.Collections.Generic;
using WorkingStandards.Entities.External;
using WorkingStandards.Util;
using WorkingStandards.Entities.Reports;


namespace WorkingStandards.Services.Reports
{
	/// <summary>
	/// Сервисный класс формирование листа записей отчета [Печать по изделиям в разрезе детале-операций(сжатая)]
	/// </summary>
	public class PrintingOfProsuctInContextOfDetalOperationsService
	{
		private static readonly string DbPathTrudnorm = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
		private static readonly string DbPathBase = Properties.Settings.Default.FoxProDbFolder_Base;

		private static readonly string BodySqlQuery = "SELECT DISTINCT advx01.kizd, " +
		                                                        "advx01.detal, advx01.kc, advx01.operac, advx01.tehoper, advx01.kol, " +
		                                                        "sum(vstk) as vstksum, " +
		                                                        "sum(rstk) as rstksum, " +
		                                                        "su73a.name as detalName, " +
		                                                        "su73a.obozn as detalMark, " +
		                                                        "su73b.name as izdName, su73b.obozn as izdMark, tehnoper.naim as tehnoperName " +
		                                                        "FROM advx01 " +
		                                                        "LEFT JOIN \"" + DbPathBase + "su73.dbf\"  as su73a on su73a.detal = advx01.detal " +
		                                                        "LEFT JOIN \"" + DbPathBase + "su73.dbf\"  as su73b on su73b.detal = advx01.kizd " +
		                                                        "LEFT JOIN \"" + DbPathBase + "tehnoper.dbf\" on tehnoper.kod = advx01.tehoper " +
		                                                        "WHERE advx01.kizd = {0} ";

	    private const string SqlQueryKc = "advx01.kc = {0} ";
	    private const string SqlQueryGroup = "group by advx01.kizd, advx01.detal, advx01.kc, advx01.operac, advx01.tehoper, advx01.kol, " +
	                                         "su73a.name, su73a.obozn, su73b.name, su73b.obozn, tehnoper.naim";

        /// <summary>
        /// Логика формирование листа записей отчета [Печать по изделиям в разрезе детале-операций(сжатая)]
        /// </summary>
        public static List<PrintingOfProsuctInContextOfDetalOperations> GetPrintingOfProsuctInContextOfDetalOperations(
	        decimal code, WorkGuild workGuild)
	    {
	        var reportResultList = new List<PrintingOfProsuctInContextOfDetalOperations>();

	        var buildSqlQuery = string.Format(BodySqlQuery, code);
       
	        if (workGuild != null)
	        {
	            buildSqlQuery += "AND " + string.Format(SqlQueryKc, workGuild.Id);
	        }
	        buildSqlQuery += SqlQueryGroup;

	        var sqlResult = DataTableHelper.LoadDataTableByQuery(DbPathTrudnorm, buildSqlQuery, "SqlResult");

	        foreach (var row in sqlResult.Select())
	        {
	            var productId = (decimal) row["kizd"];
	            var productMark = row["izdMark"] != DBNull.Value ? ((string) row["izdMark"]).Trim() : string.Empty;
	            var productName = row["izdName"] != DBNull.Value ? ((string) row["izdName"]).Trim() : string.Empty;
	            var detalId = (decimal) row["detal"];
	            var detalName = row["detalName"] != DBNull.Value ? ((string) row["detalName"]).Trim() : string.Empty;
	            var detalMark = row["detalMark"] != DBNull.Value ? ((string) row["detalMark"]).Trim() : string.Empty;
	            var kc = (decimal) row["kc"];
	            var vstk = (decimal) row["vstksum"];
	            var rstk = (decimal) row["rstksum"];
	            var kol = (decimal) row["kol"];
	            var operac = (decimal) row["operac"];
	            var tehnoper = (decimal) row["tehoper"];

	            var operationName = row["tehnoperName"] != DBNull.Value
	                ? ((string) row["tehnoperName"]).Trim()
	                : string.Empty;

	            reportResultList.Add(new PrintingOfProsuctInContextOfDetalOperations()
	            {
	                ProductId = productId,
	                ProductName = productName,
	                ProductMark = productMark,
	                DetalId = detalId,
	                DetalName = detalName,
	                DetalMark = detalMark,
	                Kc = kc,
	                Kol = kol,
	                Operac = operac,
	                Tehoper = tehnoper,
	                OperationName = operationName,
	                Vstk = vstk,
	                Rstk = rstk,
	            });
	        }

	        reportResultList.Sort();
	        return reportResultList;
	    }
	}
}

