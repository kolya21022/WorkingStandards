using System;
using System.Collections.Generic;

using WorkingStandards.Util;
using WorkingStandards.Entities.Reports;

namespace WorkingStandards.Services.Reports
{
	/// <summary>
	/// Сервисный класс формирование листа записей отчета [Сводная по изделиям в разрезе цехов, участков(для цехов)]
	/// </summary>
	public class SummeryOfProductInContextOfWorkGuildAndAreaForWorkGuildService
	{
	    private static readonly string DbPathTrudnorm = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
	    private static readonly string DbPathBase = Properties.Settings.Default.FoxProDbFolder_Base;

	    private static readonly string BodySqlQuery
	        = "SELECT DISTINCT advx03.kizd, " +
	          "advx03.kc, " +
	          "advx03.uch, " +
	          "sum(vstk) as vstksum, " +
	          "sum(rstk) as rstksum, " +
	          "sum(prtnorm) as prtnormsum, " +
	          "sum(premper) as premper, " +
              "sum(nadb) as nadbsum, " +
	          "su73.name, " +
	          "su73.obozn " +
	          "FROM advx03 " +
	          "JOIN " +
	          "(SELECT DISTINCT detal, pr_pech FROM izd_pech WHERE !empty(pr0{0})) as izd_pech " +
	          "on izd_pech.detal = advx03.kizd " +
	          "LEFT JOIN \"" + DbPathBase + "su73.dbf\" on su73.detal = advx03.kizd " +
              "WHERE kc = {0} group by advx03.kizd, advx03.kc, advx03.uch, su73.name, su73.obozn ";

        /// <summary>
        /// Логика формирование листа записей отчета [Сводная по изделиям в разрезе цехов, участков(для цехов)]
        /// </summary>
        public static List<SummeryOfProductInContextOfWorkGuildAndAreaForWorkGuild> 
            GetSummeryOfProductInContextOfWorkGuildAndAreaForWorkGuildService(decimal ceh)
		{
			var reportResultList = new List<SummeryOfProductInContextOfWorkGuildAndAreaForWorkGuild>();
			var sqlResult = DataTableHelper.LoadDataTableByQuery(DbPathTrudnorm,
		        query: string.Format(BodySqlQuery, ceh),
		        tableName: "SqlResult");

			foreach (var row in sqlResult.Select())
			{
				var productId = (decimal)row["kizd"];
				var productMark = row["obozn"] != DBNull.Value ? ((string)row["obozn"]).Trim() : string.Empty;
				var productName = row["name"] != DBNull.Value ? ((string)row["name"]).Trim() : string.Empty;
				var kc = (decimal)row["kc"];
				var uch = (decimal)row["uch"];
				var vstk = (decimal)row["vstksum"];
				var rstk = (decimal)row["rstksum"];
				var premper = (decimal)row["premper"];
				var prtnorm = (decimal)row["prtnormsum"];
				var nadb = (decimal)row["nadbsum"];


				var flag = false;
				foreach (var item in reportResultList)
				{
					if (item.ProductId == productId
						&& item.Kc == kc && item.Uch == uch)
					{
							item.Vstk = vstk;
							item.Rstk = rstk;
							item.Premper = premper;
							item.Prtnorm = prtnorm;
							item.Nadb = nadb;
						flag = true;
					}
				}

				if (!flag)
				{
					reportResultList.Add(new SummeryOfProductInContextOfWorkGuildAndAreaForWorkGuild()
					{
						ProductId = productId,
						ProductName = productName,
						ProductMark = productMark,
						Kc = kc,
						Uch = uch,
						Vstk = vstk,
						Rstk = rstk,
						Premper = premper,
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
