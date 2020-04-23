using System;
using System.Collections.Generic;
using WorkingStandards.Entities.External;
using WorkingStandards.Util;
using WorkingStandards.Entities.Reports;

namespace WorkingStandards.Services.Reports
{
	/// <summary>
	/// Сервисный класс формирование листа записей отчета [Печать по изделиям в разрезе деталей]
	/// </summary>
	public class PrintingOfProsuctInContextOfDetailsService
	{
		private static readonly string DbPathTrudnorm = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
		private static readonly string DbPathBase = Properties.Settings.Default.FoxProDbFolder_Base;

		private static readonly string BodySqlQuery = "SELECT DISTINCT advx01.kizd, " +
		                                                        "advx01.detal, advx01.kc, advx01.kol, " +
		                                                        "sum(vstk) as vstksum, " +
		                                                        "sum(rstk) as rstksum, " +
		                                                        "su73a.name as detalName, " +
		                                                        "su73a.obozn as detalMark, " +
		                                                        "su73b.name as izdName, su73b.obozn as izdMark " +
		                                                        "FROM advx01 " +
		                                                        "LEFT JOIN \"" + DbPathBase + "su73.dbf\"  as su73a on su73a.detal = advx01.detal " +
		                                                        "LEFT JOIN \"" + DbPathBase + "su73.dbf\"  as su73b on su73b.detal = advx01.kizd ";

	    private const string SqlQueryKizd = "advx01.kizd = {0} ";
	    private const string SqlQueryKc = "advx01.kc = {0} ";
	    private const string SqlQueryGroup = "group by advx01.kizd, advx01.detal, advx01.kc, advx01.kol, " +
	                                         "su73a.name, su73a.obozn, su73b.name, su73b.obozn";

        /// <summary>
        /// Логика формирование листа записей отчета [Печать по изделиям в разрезе деталей]
        /// </summary>
        public static List<PrintingOfProsuctInContextOfDetails> GetPrintingOfProsuctInContextOfDetails(Product product,
			WorkGuild workGuild)
		{
			var reportResultList = new List<PrintingOfProsuctInContextOfDetails>();

		    var buildSqlQuery = BodySqlQuery;

            if (product != null)
		    {
		        buildSqlQuery += "WHERE " + string.Format(SqlQueryKizd, product.Id);
		    }
		    if (workGuild != null && product == null)
		    {
		        buildSqlQuery += "WHERE " + string.Format(SqlQueryKc, workGuild.Id);
		    }
		    else if (workGuild != null)
		    {
		        buildSqlQuery += "AND " + string.Format(SqlQueryKc, workGuild.Id);
            }
		    buildSqlQuery += SqlQueryGroup;

            var sqlResult = DataTableHelper.LoadDataTableByQuery(DbPathTrudnorm, buildSqlQuery, "SqlResult");

			foreach (var row in sqlResult.Select())
			{
				var productId = (decimal)row["kizd"];
				var productMark = row["izdMark"] != DBNull.Value ? ((string)row["izdMark"]).Trim() : string.Empty;
				var productName = row["izdName"] != DBNull.Value ? ((string)row["izdName"]).Trim() : string.Empty;
				var detalId = (decimal)row["detal"];
				var detalName = row["detalName"] != DBNull.Value ? ((string)row["detalName"]).Trim() : string.Empty;
				var detalMark = row["detalMark"] != DBNull.Value ? ((string)row["detalMark"]).Trim() : string.Empty;
				var kc = (decimal)row["kc"];
				var vstk = (decimal)row["vstksum"];
				var rstk = (decimal)row["rstksum"];
				var kol = (decimal)row["kol"];
		
				var flag = false;

                foreach (var item in reportResultList)
				{
					if (item.ProductId == productId && item.DetalId == detalId
					                                && item.Kc == kc)
					{

						item.Vstk += vstk;
						item.Rstk += rstk;
						flag = true;
					}
				}

				if (!flag)
				{
					reportResultList.Add(new PrintingOfProsuctInContextOfDetails()
					{
						ProductId = productId,
						ProductName = productName,
						ProductMark = productMark,
						DetalId = detalId,
						DetalName = detalName,
						DetalMark = detalMark,
						Kc = kc,
						Kol = kol,
						Vstk = vstk,
						Rstk = rstk
					});
				}
			}
			reportResultList.Sort();
			return reportResultList;
		}
	}
}
