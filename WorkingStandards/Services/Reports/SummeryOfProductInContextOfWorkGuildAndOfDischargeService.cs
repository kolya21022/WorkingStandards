using System;
using System.Collections.Generic;

using WorkingStandards.Util;
using WorkingStandards.Entities.Reports;

namespace WorkingStandards.Services.Reports
{
	/// <summary>
	/// Сервисный класс формирование листа записей отчета [Сводная по изделиям в разрезе цехов и по разрядам]
	/// </summary>
	public class SummeryOfProductInContextOfWorkGuildAndOfDischargeService
	{
	    private static readonly string DbPathTrudnorm = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
        private static readonly string DbPathBase = Properties.Settings.Default.FoxProDbFolder_Base;

	    private static readonly string BodySqlQuery
	        = "SELECT DISTINCT advx03.kizd, " +
	          "advx03.kc, " +
	          "advx03.razr, " +
              "sum(vstk) as vstksum, " +
	          "sum(rstk) as rstksum, " +
	          "sum(prtnorm) as prtnormsum, " +
	          "sum(nadb) as nadbsum, " +
	          "su73.name, " +
	          "su73.obozn " +
	          "FROM advx03 " +
              "JOIN (SELECT DISTINCT detal, pr_pech FROM izd_pech WHERE !empty(pr_pech)) as izd_pech on izd_pech.detal = advx03.kizd " +
	          "LEFT JOIN \"" + DbPathBase + "su73.dbf\" on su73.detal = advx03.kizd " +
              "group by advx03.kizd, advx03.kc, advx03.razr, su73.name, su73.obozn ";

        /// <summary>
        /// Логика формирование листа записей отчета [Сводная по изделиям в разрезе цехов и по разрядам]
        /// </summary>
        public static List<SummeryOfProductInContextOfWorkGuildAndOfDischarge> GetSummeryOfProductInContextOfWorkGuildAndOfDischarge()
		{
			var reportResultList = new List<SummeryOfProductInContextOfWorkGuildAndOfDischarge>();
		    var sqlResult = DataTableHelper.LoadDataTableByQuery(DbPathTrudnorm, BodySqlQuery, "SqlResult");

            foreach (var row in sqlResult.Select())
			{
				var productId = (decimal)row["kizd"];
				var productMark = row["obozn"] != DBNull.Value ? ((string)row["obozn"]).Trim() : string.Empty;
				var productName = row["name"] != DBNull.Value ? ((string)row["name"]).Trim() : string.Empty;
				var razr = (decimal)row["razr"];
				var kc = (decimal)row["kc"];
				var vstk = (decimal)row["vstksum"];
				var rstk = (decimal)row["rstksum"];
				var prtnorm = (decimal)row["prtnormsum"];
				var nadb = (decimal)row["nadbsum"];

				var flag = false;
				foreach (var item in reportResultList)
				{
					if (item.ProductId == productId
						&& item.Kc == kc)
					{
						flag = true;

						if (razr == 1)
						{
							item.Vstk1 = vstk;
							item.Rstk1 = rstk;
							item.Prtnorm1 = prtnorm;
							item.Nadb1 = nadb;

							break;
						}

						if (razr == 2)
						{
							item.Vstk2 = vstk;
							item.Rstk2 = rstk;
							item.Prtnorm2 = prtnorm;
							item.Nadb2 = nadb;

							break;
						}

						if (razr == 3)
						{
							item.Vstk3 = vstk;
							item.Rstk3 = rstk;
							item.Prtnorm3 = prtnorm;
							item.Nadb3 = nadb;

							break;
						}

						if (razr == 4)
						{
							item.Vstk4 = vstk;
							item.Rstk4 = rstk;
							item.Prtnorm4 = prtnorm;
							item.Nadb4 = nadb;

							break;
						}

						if (razr == 5)
						{
							item.Vstk5 = vstk;
							item.Rstk5 = rstk;
							item.Prtnorm5 = prtnorm;
							item.Nadb5 = nadb;

							break;
						}

						if (razr == 6)
						{
							item.Vstk6 = vstk;
							item.Rstk6 = rstk;
							item.Prtnorm6 = prtnorm;
							item.Nadb6 = nadb;

							break;
						}

					}
				}

				if (!flag)
				{
					if (razr == 1)
					{
						reportResultList.Add(new SummeryOfProductInContextOfWorkGuildAndOfDischarge()
						{
							ProductId = productId,
							ProductName = productName,
							ProductMark = productMark,
							Kc = kc,
							Vstk1 = vstk,
							Rstk1 = rstk,
							Prtnorm1 = prtnorm,
							Nadb1 = nadb
						});
					}

					if (razr == 2)
					{
						reportResultList.Add(new SummeryOfProductInContextOfWorkGuildAndOfDischarge()
						{
							ProductId = productId,
							ProductName = productName,
							ProductMark = productMark,
							Kc = kc,
							Vstk2 = vstk,
							Rstk2 = rstk,
							Prtnorm2 = prtnorm,
							Nadb2 = nadb
						});
					}


					if (razr == 3)
					{
						reportResultList.Add(new SummeryOfProductInContextOfWorkGuildAndOfDischarge()
						{
							ProductId = productId,
							ProductName = productName,
							ProductMark = productMark,
							Kc = kc,
							Vstk3 = vstk,
							Rstk3 = rstk,
							Prtnorm3 = prtnorm,
							Nadb3 = nadb
						});
					}

					if (razr == 4)
					{
						reportResultList.Add(new SummeryOfProductInContextOfWorkGuildAndOfDischarge()
						{
							ProductId = productId,
							ProductName = productName,
							ProductMark = productMark,
							Kc = kc,
							Vstk4 = vstk,
							Rstk4 = rstk,
							Prtnorm4 = prtnorm,
							Nadb4 = nadb
						});
					}

					if (razr == 5)
					{
						reportResultList.Add(new SummeryOfProductInContextOfWorkGuildAndOfDischarge()
						{
							ProductId = productId,
							ProductName = productName,
							ProductMark = productMark,
							Kc = kc,
							Vstk5 = vstk,
							Rstk5 = rstk,
							Prtnorm5 = prtnorm,
							Nadb5 = nadb
						});
					}

					if (razr == 6)
					{
						reportResultList.Add(new SummeryOfProductInContextOfWorkGuildAndOfDischarge()
						{
							ProductId = productId,
							ProductName = productName,
							ProductMark = productMark,
							Kc = kc,
							Vstk6 = vstk,
							Rstk6 = rstk,
							Prtnorm6 = prtnorm,
							Nadb6 = nadb
						});
					}

				}
			}
			reportResultList.Sort();
			return reportResultList;
		}
	}
}
