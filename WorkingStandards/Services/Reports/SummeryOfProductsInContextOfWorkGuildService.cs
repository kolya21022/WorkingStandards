using System;
using System.Collections.Generic;

using WorkingStandards.Util;
using WorkingStandards.Entities.Reports;

namespace WorkingStandards.Services.Reports
{
	/// <summary>
	/// Сервисный класс формирование листа записей отчета [Сводки по изделиям в разрезе цехов]
	/// </summary>
	public class SummeryOfProductsInContextOfWorkGuildService
	{
		private static readonly string DbPathTrudnorm = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
		private static readonly string DbPathBase = Properties.Settings.Default.FoxProDbFolder_Base;

		private static readonly string BodySqlQuery 
		    = "SELECT DISTINCT advx03.kizd, " +
		      "advx03.kc, " +
		      "sum(vstk) as vstksum, " +
		      "sum(rstk) as rstksum, " +
		      "sum(prtnorm) as prtnormsum, " +
		      "sum(nadb) as nadbsum, " +
		      "su73.name, " +
		      "su73.obozn " +
		      "FROM advx03 " +
		      "JOIN (SELECT DISTINCT detal, pr_pech FROM izd_pech WHERE !empty(pr_pech)) as izd_pech on izd_pech.detal = advx03.kizd " +
		      "LEFT JOIN \"" + DbPathBase + "su73.dbf\" on su73.detal = advx03.kizd " +
              "group by advx03.kizd, advx03.kc, su73.name, su73.obozn ";

		/// <summary>
		/// Логика формирование листа записей отчета [Сводки по изделиям в разрезе цехов]
		/// </summary>
		public static List<SummeryOfProductsInContextOfWorkGuild> GetSummeryOfProductsInContextOfWorkGuild ()
		{
			var reportResultList = new List<SummeryOfProductsInContextOfWorkGuild>();
			var sqlResult = DataTableHelper.LoadDataTableByQuery(DbPathTrudnorm, BodySqlQuery, "SqlResult");

			foreach (var row in sqlResult.Select())
			{
				var productId = (decimal) row["kizd"];
				var productMark = row["obozn"] != DBNull.Value ? ((string) row["obozn"]).Trim() : string.Empty;
				var productName = row["name"] != DBNull.Value ? ((string) row["name"]).Trim() : string.Empty;
				var kc = (decimal) row["kc"];
				var vstk = (decimal) row["vstksum"];
				var rstk = (decimal) row["rstksum"];
				var prtnorm = (decimal) row["prtnormsum"];
				var nadb = (decimal) row["nadbsum"];

				var flag = false;
				foreach (var item in reportResultList)
				{
					if (item.ProductId == productId)
					{
						flag = true;

						if (kc == 2)
						{
							item.Vstk2 = vstk;
							item.Rstk2 = rstk;
							item.Prtnorm2 = prtnorm;
							item.Nadb2 = nadb;
							
						}

						if (kc == 3)
						{
							item.Vstk3 = vstk;
							item.Rstk3 = rstk;
							item.Prtnorm3 = prtnorm;
							item.Nadb3 = nadb;
							
						}

						if (kc == 4)
						{
							item.Vstk4 = vstk;
							item.Rstk4 = rstk;
							item.Prtnorm4 = prtnorm;
							item.Nadb4 = nadb;
							
						}

						if (kc == 5)
						{
							item.Vstk5 = vstk;
							item.Rstk5 = rstk;
							item.Prtnorm5 = prtnorm;
							item.Nadb5 = nadb;
							
						}

						if (kc == 21 || kc == 24 || kc == 25)
						{
							item.Vstk21 += vstk;
							item.Rstk21 += rstk;
							item.Prtnorm21 += prtnorm;
							item.Nadb21 += nadb;
							
						}
						
						item.VstkZavod += vstk;
						item.RstkZavod += rstk;
						item.PrtnormZavod += prtnorm;
						item.NadbZavod += nadb;

					}	

				}

				
				if (!flag)
				{
					if (kc == 2)
					{
						reportResultList.Add(new SummeryOfProductsInContextOfWorkGuild()
						{
							ProductId = productId,
							ProductMark = productMark,
							ProductName = productName,
							Vstk2 = vstk,
							Rstk2 = rstk,
							Prtnorm2 = prtnorm,
							Nadb2 = nadb,
							VstkZavod = vstk,
							RstkZavod = rstk,
							PrtnormZavod = prtnorm,
							NadbZavod = nadb
						});
                        continue;
					}

					if (kc == 3)
					{
						reportResultList.Add(new SummeryOfProductsInContextOfWorkGuild()
						{
							ProductId = productId,
							ProductMark = productMark,
							ProductName = productName,
							Vstk3 = vstk,
							Rstk3 = rstk,
							Prtnorm3 = prtnorm,
							Nadb3 = nadb,
							VstkZavod = vstk,
							RstkZavod = rstk,
							PrtnormZavod = prtnorm,
							NadbZavod = nadb
						});
					    continue;
                    }


					if (kc == 4)
					{
						reportResultList.Add(new SummeryOfProductsInContextOfWorkGuild()
						{
							ProductId = productId,
							ProductMark = productMark,
							ProductName = productName,
							Vstk4 = vstk,
							Rstk4 = rstk,
							Prtnorm4 = prtnorm,
							Nadb4 = nadb,
							VstkZavod = vstk,
							RstkZavod = rstk,
							PrtnormZavod = prtnorm,
							NadbZavod = nadb
						});
					    continue;
                    }

					if (kc == 5)
					{
						reportResultList.Add(new SummeryOfProductsInContextOfWorkGuild()
						{
							ProductId = productId,
							ProductMark = productMark,
							ProductName = productName,
							Vstk5 = vstk,
							Rstk5 = rstk,
							Prtnorm5 = prtnorm,
							Nadb5 = nadb,
							VstkZavod = vstk,
							RstkZavod = rstk,
							PrtnormZavod = prtnorm,
							NadbZavod = nadb
						});
					    continue;
                    }

					if (kc == 21 || kc == 24 || kc == 25)
					{
						reportResultList.Add(new SummeryOfProductsInContextOfWorkGuild()
						{
							ProductId = productId,
							ProductMark = productMark,
							ProductName = productName,
							Vstk21 = vstk,
							Rstk21 = rstk,
							Prtnorm21 = prtnorm,
							Nadb21 = nadb,
							VstkZavod = vstk,
							RstkZavod = rstk,
							PrtnormZavod = prtnorm,
							NadbZavod = nadb
						});
                    }
				}
			}

			reportResultList.Sort();
			return reportResultList;
		}
	}
}