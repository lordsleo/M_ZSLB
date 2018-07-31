//
//文件名：    GetMonthOperation.aspx.cs
//功能描述：  获取本月作业情况
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

namespace M_Zslb.Service.Operation
{
    public partial class GetMonthOperation : System.Web.UI.Page
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
                //strStartDate = "2015-11-01";
                //strEndDate = "2015-11-30";
                

                string[,] strArray = null;
                string strSql =
                    string.Format(@"select WORKTYPE,COUNTOFSHIP,COUNTOFDEVICE,SCHEDULOR,WORKDATE 
                                    from LBGSDD.PVW_SCHEDULE
                                    where WORKDATE>='{0}' and WORKDATE<='{1}' 
                                    order by WORKDATE desc",
                                    strStartDate, strEndDate);

                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathIport).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }

                strArray = new string[dt.Rows.Count, 5];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["WORKTYPE"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["COUNTOFSHIP"]);
                    strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["COUNTOFDEVICE"]);
                    strArray[iRow, 3] = Convert.ToString(dt.Rows[iRow]["SCHEDULOR"]);
                    strArray[iRow, 4] = Convert.ToString(dt.Rows[iRow]["WORKDATE"]);
                }

                Json = JsonConvert.SerializeObject(new DicPackage(true, strArray, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}