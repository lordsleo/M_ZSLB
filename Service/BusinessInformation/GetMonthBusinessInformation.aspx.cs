//
//文件名：    GetMonthBusinessInformation.aspx.cs
//功能描述：  获取本月商务信息
//创建时间：  2015/12/01
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Leo;
using ServiceInterface.Common;

namespace M_Zslb.Service.BusinessInformation
{
    public partial class GetMonthBusinessInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DateTime curDate = DateTime.Now;
                DateTime startDate = new DateTime(curDate.Year, curDate.Month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                string strStartDate = startDate.ToString("yyyy-MM-dd");
                string strEndDate = endDate.ToString("yyyy-MM-dd");
                strStartDate = "2015-11-01";
                strEndDate = "2015-11-30"; ;

                string strSql =
                    string.Format(@"select SUM(COUNTOFINVOICE) as MONTHCOUNTOFINVOICE,SUM(TOTALMONEY) as MONTHTOTALMONEY
                                    from LBGSDD.PVW_INVOICET
                                    where INVOICEDATE>='{0}' and INVOICEDATE<='{1}'",
                                    strStartDate, strEndDate);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathIport).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }

                Dictionary<string, object> info = new Dictionary<string, object>();
                info.Add("张数", Convert.ToString(dt.Rows[0]["MONTHCOUNTOFINVOICE"]));
                info.Add("金额", Convert.ToString(dt.Rows[0]["MONTHTOTALMONEY"]));

                Json = JsonConvert.SerializeObject(new DicPackage(true, info, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}