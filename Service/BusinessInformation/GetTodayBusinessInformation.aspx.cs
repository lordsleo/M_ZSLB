//
//文件名：    GetTodayBusinessInformation.aspx.cs
//功能描述：  获取今日商务信息
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
    public partial class GetTodayBusinessInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strInvoiceDate = DateTime.Now.ToString("yyyy-MM-dd");
                strInvoiceDate = "2015-11-05";

                string strSql =
                    string.Format(@"select COUNTOFINVOICE,TOTALMONEY
                                    from LBGSDD.PVW_INVOICET
                                    where INVOICEDATE='{0}'",
                                    strInvoiceDate);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathIport).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }

                Dictionary<string, object> info = new Dictionary<string, object>();
                info.Add("张数", Convert.ToString(dt.Rows[0]["COUNTOFINVOICE"]));
                info.Add("金额", Convert.ToString(dt.Rows[0]["TOTALMONEY"]));

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