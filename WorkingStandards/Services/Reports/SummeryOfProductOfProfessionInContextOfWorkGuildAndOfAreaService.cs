using System;
using System.Collections.Generic;

using WorkingStandards.Util;
using WorkingStandards.Entities.Reports;

namespace WorkingStandards.Services.Reports
{
	/// <summary>
	/// Сервисный класс формирование листа записей отчета [Сводная по изделиям по профессиям в разрезе цехов, участков]
	/// </summary>
	public class SummeryOfProductOfProfessionInContextOfWorkGuildAndOfAreaService
	{
	    private static readonly string DbPathTrudnorm = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
	    private static readonly string DbPathBase = Properties.Settings.Default.FoxProDbFolder_Base;

	    private static readonly string BodySqlQuery
	        = "SELECT DISTINCT advx03.kizd, " +
	          "advx03.kc, " +
	          "advx03.uch, " +
              "advx03.prof, " +
	          "sum(vstk) as vstksum, " +
	          "sum(rstk) as rstksum, " +
	          "sum(prtnorm) as prtnormsum, " +
	          "sum(premper) as premper, " +
	          "sum(nadb) as nadbsum, " +
	          "su73.name as izdName, " +
	          "su73.obozn, " +
	          "fsprof.name as professionName " +
	          "FROM advx03 " +
	          "JOIN (SELECT DISTINCT detal, pr_rasc FROM izd_rasc WHERE !empty(pr_rasc)) as izd_rasc on izd_rasc.detal = advx03.kizd " +
	          "LEFT JOIN \"" + DbPathBase + "su73.dbf\" on su73.detal = advx03.kizd " +
	          "LEFT JOIN \"" + DbPathBase + "fsprof.dbf\" on fsprof.prof = advx03.prof " +
              "WHERE kc = {0} and uch = {1} " +
	          "group by advx03.kizd, advx03.kc, advx03.uch, advx03.prof, su73.name, su73.obozn, fsprof.name";

        /// <summary>
        /// Логика формирование листа записей отчета [Сводная по изделиям по профессиям в разрезе цехов, участков]
        /// </summary>
        public static List<SummeryOfProductOfProfessionInContextOfWorkGuildAndOfArea>
			GetSummeryOfProductOfProfessionInContextOfWorkGuildAndOfArea(decimal ceh, decimal area)
		{
			var reportResultList = new List<SummeryOfProductOfProfessionInContextOfWorkGuildAndOfArea>();
		    var sqlResult = DataTableHelper.LoadDataTableByQuery(DbPathTrudnorm, 
		        string.Format(BodySqlQuery, ceh, area), "SqlResult");

            foreach (var row in sqlResult.Select())
			{
				var productId = (decimal) row["kizd"];
				var productMark = row["obozn"] != DBNull.Value ? ((string) row["obozn"]).Trim() : string.Empty;
				var productName = row["izdName"] != DBNull.Value ? ((string) row["izdName"]).Trim() : string.Empty;
				var professionId = (decimal) row["prof"];
				var professionName = row["professionName"] != DBNull.Value ? ((string) row["professionName"]).Trim() : string.Empty;
				var kc = (decimal) row["kc"];
				var uch = (decimal)row["uch"];
				var vstk = (decimal) row["vstksum"];
				var rstk = (decimal) row["rstksum"];
				var prtnorm = (decimal) row["prtnormsum"];
				var nadb = (decimal) row["nadbsum"];

				var flag = false;
				foreach (var item in reportResultList)
				{
					if (item.ProductId == productId && item.ProfessionId == professionId
					    && item.Kc == kc && item.Uch == uch)
					{
						item.Vstk = vstk;
						item.Rstk = rstk;
						item.Prtnorm = prtnorm;
						item.Nadb = nadb;
						flag = true;
					}
				}

				if (!flag)
				{
					reportResultList.Add(new SummeryOfProductOfProfessionInContextOfWorkGuildAndOfArea()
					{
						ProductId = productId,
						ProductName = productName,
						ProductMark = productMark,
						ProfessionId = professionId,
						ProfessionName = professionName,
						Kc = kc,
						Uch = uch,
						Vstk = vstk,
						Rstk = rstk,
						Prtnorm = prtnorm,
						Nadb = nadb
					});
				}


			}
			reportResultList.Sort();
			return reportResultList;		
		}
	}
}
