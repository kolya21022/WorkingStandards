using System.Data.OleDb;
using System.IO;

using WorkingStandards.Db;

namespace WorkingStandards.Services
{
	/// <summary>
	/// Обработчик сервисного слоя для класса расчёта трудовых норм - [CalculationWorkingStandarts]
	/// </summary>
	public class CalculationWorkingStandarts
    {
        private static readonly string DbfPathCi = Properties.Settings.Default.FoxProDbFolder_Foxpro_CI;
        private static readonly string DbfPathTrudnorm = Properties.Settings.Default.FoxProDbFolder_Foxpro_Trudnorm;
        private static readonly string DbfPathArmBase = Properties.Settings.Default.FoxProDbFolder_Fox60_arm_Base;
        private static readonly string DbfPathTemp = Properties.Settings.Default.FoxProDbFolder_Temp;
        private static readonly string Nu67Dbf = Properties.Settings.Default.Nu67Dbf;

        private static readonly string QueryAdvx01 = "INSERT INTO \"" + DbfPathTrudnorm + "ADVX01.dbf\" (" +
                                            "kizd, detal, kol, operac, kc, " +
                                            "uch, lin, grobor, tehoper, prof, " +
                                            "razr, vidnorm, razmpart, vstk, tshtok, " +
                                            "rstk, rascok, nomizv, dataizv, vrgr, " +
                                            "rez, prtnorm, nadb, premper, prin, dat_spec) " +
                                            "SELECT ki as kizd, chto as detal, koli as kol, operac, ceh as kc, " +
                                            "uch, lin, grobor, tehoper, prof, " +
                                            "razr, vidnorm, 0 as razmpart, (vstk* koli) as VstkKoli, 0.00 as tshtok, " +
                                            "(rstk* koli) as RstkKoli, 0.00 as rascok, nomizv, dataizv, " +
                                            "iif(tehoper<> 205 and tehoper <> 401 and tehoper <> 0, round((vst/ednorm + tpz/razmpart) * koli, 3), 0) as vrgr, " +
                                            "0 as rez, iif(vidnorm = 11, round(0.05 * rstk * koli,3), " +
                                            "iif(vidnorm = 0 or vidnorm = 40 or vidnorm = 90, 0, " +
                                            "iif(vidnorm = 10, round(0.05 * rstk * koli,3), " +
                                            "iif(vidnorm = 12 or vidnorm = 13 or vidnorm = 14, 0, " +
                                            "iif(vidnorm = 15, round(0.05 * rstk * koli,3), " +
                                            "iif(vidnorm = 16, round(0.15 * rstk * koli,3), round(0.05 * rstk * koli,3))))))) as prtnorm, " +
                                            "iif(vidnorm = 11, round(cast(koli as numeric(5,6)) * cast(rstk as numeric(5,6)) * 0.05,3),0.00000) as nadb, " +
                                            "0.00 as premper, prin, {..} as dat_spec " +
                                            "FROM " +
                                            $"(SELECT acux01.ki, acux01.chto, acux01.koli, {Nu67Dbf}.operac, {Nu67Dbf}.Ceh, " +
                                            $"{Nu67Dbf}.Uch, {Nu67Dbf}.Lin, {Nu67Dbf}.grobor, {Nu67Dbf}.tehoper, {Nu67Dbf}.razr, " +
                                            $"{Nu67Dbf}.prof, {Nu67Dbf}.vidnorm, {Nu67Dbf}.vstk, " +
                                            $"{Nu67Dbf}.rstk, {Nu67Dbf}.Nomizv, {Nu67Dbf}.dataizv, " +
                                            $"{Nu67Dbf}.vst, {Nu67Dbf}.ednorm, {Nu67Dbf}.tpz, {Nu67Dbf}.razmpart, acux01.prin " +
                                            "FROM \"" + DbfPathArmBase + $"acux01.dbf\", {Nu67Dbf}.dbf " +
                                            $"WHERE acux01.chto = {Nu67Dbf}.detal " +
                                            "and acux01.ki in (SELECT DISTINCT izd_rasc.detal from \"" + DbfPathTrudnorm + "izd_rasc.dbf\" " +
                                            "where pr_rasc = '+' and detal != 0) " +
                                            "and (acux01.prin = 1 or acux01.prin = 31 or acux01.prin = 34 or " +
                                            "acux01.prin = 32 or acux01.prin = 41 or acux01.prin = 42 or " +
                                            "acux01.prin = 44 or acux01.prin = 51 or acux01.prin = 52 or " +
                                            "acux01.prin = 61 or acux01.prin = 62 or acux01.prin = 81)) as link";

        private static readonly string QueryAdvx03 = "INSERT INTO ADVX03 (" +
                                            "kizd, kc, uch, prof, razr, vstk, " +
                                            "rstk, prtnorm, nadb, premper, protdpl, vrgr) " +
                                            "SELECT kizd, kc, uch, prof, razr, round(sum(vstk)/60,3) as vstk, " +
                                            "round(sum(rstk)/100,3) as rstk, round(sum(prtnorm)/100,3) as prtnorm, " +
                                            "round(sum(nadb)/100,3) as nadb, round(sum(premper)/100,3) as premper, " +
                                            "1 as protdpl, round(sum(vrgr)/60,3) as vrgr " +
                                            "FROM advx01 group by kizd, kc, uch, prof, razr";

        public static void Calculation()
        {
            var filePathDeleteAdvx01 = Path.Combine(DbfPathTrudnorm, "ADVX01.dbf");
            var filePathDeleteAdvx01Cdx = Path.Combine(DbfPathTrudnorm, "ADVX01.cdx");
            var filePathDeleteAdvx03 = Path.Combine(DbfPathTrudnorm, "ADVX03.dbf");
            var filePathDeleteAdvx03Cdx = Path.Combine(DbfPathTrudnorm, "ADVX03.cdx");
            var filePathCopyAdvx01 = Path.Combine(DbfPathTemp, "ADVX01.dbf");
            var filePathCopyAdvx01Cdx = Path.Combine(DbfPathTemp, "ADVX01.cdx");
            var filePathCopyAdvx03 = Path.Combine(DbfPathTemp, "ADVX03.dbf");
            var filePathCopyAdvx03Cdx = Path.Combine(DbfPathTemp, "ADVX03.cdx");
            var filePathCopy2Advx01 = Path.Combine(DbfPathTrudnorm, "ADVX01.dbf");
            var filePathCopy2Advx01Cdx = Path.Combine(DbfPathTrudnorm, "ADVX01.cdx");
            var filePathCopy2Advx03 = Path.Combine(DbfPathTrudnorm, "ADVX03.dbf");
            var filePathCopy2Advx03Cdx = Path.Combine(DbfPathTrudnorm, "ADVX03.cdx");

            var filePathDeleteAdvx01gr = Path.Combine(DbfPathTrudnorm, "ADVX01GR.dbf");
            var filePathDeleteAdvx01grCdx = Path.Combine(DbfPathTrudnorm, "ADVX01GR.cdx");
            var filePathCopyAdvx01gr = Path.Combine(DbfPathTrudnorm, "ADVX01.dbf");
            var filePathCopyAdvx01grCdx = Path.Combine(DbfPathTrudnorm, "ADVX01.cdx");
            var filePathCopy2Advx01gr = Path.Combine(DbfPathTrudnorm, "ADVX01GR.dbf");
            var filePathCopy2Advx01grCdx = Path.Combine(DbfPathTrudnorm, "ADVX01GR.cdx");

            File.Delete(filePathDeleteAdvx01);
            File.Delete(filePathDeleteAdvx01Cdx);
            File.Copy(filePathCopyAdvx01, filePathCopy2Advx01);
            File.Copy(filePathCopyAdvx01Cdx, filePathCopy2Advx01Cdx);

            using (var oleDbConnection = DbControl.GetConnection(DbfPathCi))
            {
                oleDbConnection.TryConnectOpen();

                using (var oleDbCommand = new OleDbCommand(QueryAdvx01, oleDbConnection))
                {
                    oleDbCommand.ExecuteNonQuery();
                }
            }

            File.Delete(filePathDeleteAdvx03);
            File.Delete(filePathDeleteAdvx03Cdx);
            File.Copy(filePathCopyAdvx03, filePathCopy2Advx03);
            File.Copy(filePathCopyAdvx03Cdx, filePathCopy2Advx03Cdx);

            using (var oleDbConnection = DbControl.GetConnection(DbfPathTrudnorm))
            {
                oleDbConnection.TryConnectOpen();

                using (var oleDbCommand = new OleDbCommand(QueryAdvx03, oleDbConnection))
                {
                    oleDbCommand.ExecuteNonQuery();
                }
            }

            File.Delete(filePathDeleteAdvx01gr);
            File.Delete(filePathDeleteAdvx01grCdx);
            File.Copy(filePathCopyAdvx01gr, filePathCopy2Advx01gr);
            File.Copy(filePathCopyAdvx01grCdx, filePathCopy2Advx01grCdx);
        }
    }
}

/////////// SQL запрос повторяющий логику старой программы ///////////
//////////////////////////////////////////////////////////////////////

//SELECT ki as kizd, 
//       chto as detal, 
//       koli as kol, 
//       operac, 
//       ceh as kc, 
//       uch, 
//       lin, 
//       grobor, 
//       tehoper, 
//       prof, 
//       razr, 
//       vidnorm, 
//       0 as razmpart, 
//       (vstk* koli) as VstkKoli, 
//       0.00 as tshtok, 
//       (rstk* koli) as RstkKoli, 
//       0.00 as rascok, 
//       nomizv, 
//       dataizv, 
//       iif(tehoper<> 205 and tehoper <> 401 and tehoper <> 0, 
//           (round(vst/ednorm + tpz/razmpart) * koli,3),                                   //true
//           0) as vrgr,                                                                    //false
//           0 as rez, 
//           iif(vidnorm = 11, 
//               round(0.05 * rstk * koli,3),                                              //true
//               iif(vidnorm = 0 or vidnorm = 40 or vidnorm = 90,                           //false if
//                   0,                                                                     //true
//                   iif(vidnorm = 10,                                                      //false if
//                       round(0.05 * (rstk* koli),3),                                      //true
//                       iif(vidnorm = 12 or vidnorm = 13 or vidnorm = 14,                  //false if
//                           0,                                                             //true
//                           iif(vidnorm = 15,                                              //false if
//                               round(0.05 * (rstk* koli),3),                              //true
//                               iif(vidnorm = 16,                                          //false if
//                                   round(0.15 * (rstk* koli),3),                          //true
//                                   round(0.05 * (rstk* koli),3))))))) as prtnorm,         //false
//       iif(vidnorm = 11,  
//           round(0.05 * rstk * koli,3),                                                  //true
//           0) as nadb,                                                                    //false
//       0.00 as premper, 
//       prin, 
//       {..} as dat_spec
//FROM
//(SELECT acux01.ki, 
//        acux01.chto, 
//        acux01.koli, 
//        nu67.operac, 
//        nu67.Ceh, 
//        nu67.Uch, 
//        nu67.Lin, 
//        nu67.grobor, 
//        nu67.tehoper, 
//        nu67.prof, 
//        nu67.razr, 
//        nu67.vidnorm, 
//        nu67.vstk, 
//        nu67.rstk, 
//        nu67.Nomizv, 
//        nu67.dataizv, 
//        nu67.vst, 
//        nu67.ednorm, 
//        nu67.tpz, 
//        nu67.razmpart, 
//        acux01.prin
//FROM "d:\VirtualMashinFiles\trudnorm\SERVER\V\FOX60\arm\BASE\acux01.dbf", nu67
//WHERE acux01.chto = nu67.detal
//      and acux01.ki in 
//        (SELECT DISTINCT izd_rasc.detal from "d:\VirtualMashinFiles\trudnorm\SERVER\V\FOXPRO\TRUDNORM\izd_rasc.dbf" 
//        WHERE pr_rasc = '+' and detal != 0)
//        and (acux01.prin = 1 or acux01.prin = 31 or acux01.prin = 34 or acux01.prin = 32 
//             or acux01.prin = 41 or acux01.prin = 42 or acux01.prin = 44 or acux01.prin = 51 
//             or acux01.prin = 52 or acux01.prin = 61 or acux01.prin = 62 or acux01.prin = 81)) as link


/////////// Логика старой программы переведенная на язык C# ///////////
///////////////////////////////////////////////////////////////////////

//foreach (var izdrascItem in izdrasc)
//{
//var productId = izdrascItem.Detal;
//var acux01 = Acux01Services.GetAcux01ByProductId(productId, oleDbConnectionBase);
//    foreach (var acux01Item in acux01)
//    {
//    var detal = acux01Item.Chto;
//    var nu67 = Nu67Services.GetNu67ByDetal(detal, oleDbConnectionCi); <= prin = 1 or prin = 31 or prin = 34 or prin = 32 
//                                                                         or prin = 41 or prin = 42 or acux01.prin = 44 
//                                                                         or acux01.prin = 51 or acux01.prin = 52 
//                                                                         or acux01.prin = 61 or acux01.prin = 62 
//                                                                         or acux01.prin = 81
//    foreach (var nu67Item in nu67)
//    {
//        var count = acux01Item.Koli;
//        var prin = acux01Item.Prin;
//        var operac = nu67Item.Operac;
//        var workGuildId = nu67Item.Ceh;
//        var uch = nu67Item.Uch;
//        var lin = nu67Item.Lin;
//        var grobor = nu67Item.Grobor;
//        var tehoper = nu67Item.Tehoper;
//        var prof = nu67Item.Prof;
//        var razr = nu67Item.Razr;
//        var typeNorm = nu67Item.Vidnorm;
//        var vstk = nu67Item.Vstk * count;
//        var rstk = nu67Item.Rstk * count;
//        var numIzv = nu67Item.NomIzv;
//        var dateIzv = nu67Item.DataIzv;

//        var vrgr = 0M;
//        decimal nadb;
//        decimal prtNorm;
//        if (tehoper != 205M && tehoper != 401M && tehoper != 0M)
//        {
//            vrgr = (nu67Item.Vst / nu67Item.Ednorm + nu67Item.Tpz / nu67Item.Razmpart) * count;
//        }
//        if (typeNorm == 11M)
//        {
//            nadb = decimal.Round(0.05M * rstk, 3); // c 01.11.2003
//            prtNorm = decimal.Round(0.05M * rstk, 3); // c 01.05.2005
//        }
//        else
//        {
//            if (typeNorm == 0M || typeNorm == 40M || typeNorm == 90M)
//            {
//                nadb = 0M;
//                prtNorm = 0M;
//            }
//            else
//            {
//                nadb = 0M;

//                if (typeNorm == 10M)
//                {
//                    prtNorm = decimal.Round(0.05M * rstk, 3); // c 01.05.2005

//                }
//                else
//                {
//                    if (typeNorm == 12M || typeNorm == 13M || typeNorm == 14M)
//                    {
//                        prtNorm = 0M;

//                    }
//                    else
//                    {
//                        if (typeNorm == 15M)
//                        {
//                            prtNorm = decimal.Round(0.05M * rstk, 3);
//                        }
//                        else
//                        {
//                            if (typeNorm == 16M)
//                            {
//                                prtNorm = decimal.Round(0.15M * rstk, 3);
//                            }
//                            else
//                            {
//                                prtNorm = decimal.Round(0.05M * rstk, 3); // с 01.11.2003
//                            }
//                        }
//                    }
//                }
//            }
//        }
//        advx01.Add(new Advx01()
//        {
//            Kizd = productId,
//            Detal = detal,
//            Kol = count,
//            Operac = operac,
//            Kc = workGuildId,
//            Uch = uch,
//            Lin = lin,
//            Grobor = grobor,
//            Tehoper = tehoper,
//            Prof = prof,
//            Razr = razr,
//            Vidnorm = typeNorm,
//            Vstk = vstk,
//            Rstk = rstk,
//            NomIzv = numIzv,
//            DataIzv = dateIzv,
//            Vrgr = vrgr,
//            Prtnorm = prtNorm,
//            Prin = prin,
//            Nadb = nadb
//        });
//    }
//}