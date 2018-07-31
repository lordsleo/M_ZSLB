//
//文件名：    GetTodayOperation.aspx.cs
//功能描述：  获取今日作业汇总
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
    public partial class GetTodayOperation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //作业日期
            //string strOperationDat = Request.Params["OperationDate"];

            try
            {
                string strOperationDate = DateTime.Now.ToString("yyyy-MM-dd");
                //strOperationDate = "2015-11-30";

                string[,] strArray = null;
                string strSql =
                    string.Format(@"select WORKTYPE,COUNTOFSHIP,COUNTOFDEVICE,SCHEDULOR 
                                    from LBGSDD.PVW_SCHEDULE
                                    where WORKDATE='{0}'
                                    order by WORKDATE desc", 
                                    strOperationDate);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathIport).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }

                strArray = new string[dt.Rows.Count, 4];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["WORKTYPE"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["COUNTOFSHIP"]);
                    strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["COUNTOFDEVICE"]);
                    strArray[iRow, 3] = Convert.ToString(dt.Rows[iRow]["SCHEDULOR"]);
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