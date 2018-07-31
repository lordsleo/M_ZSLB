//
//文件名：    GetAlarmEnquiriesInformation.aspx.cs
//功能描述：  获取报警查询数据
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


namespace M_Zslb.Service.SafetyProduction
{
    public partial class GetAlarmEnquiriesInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //数据起始行
            string strStartRow = Request.Params["StartRow"];
            //行数
            string strCount = Request.Params["Count"];

            try
            {
                if (strStartRow == null || strCount == null)
                {
                    string warning = string.Format("参数StartRow，Count不能为null！举例：http://218.92.115.55/M_Zslb/Service/SafetyProduction/GetAlarmEnquiriesInformation.aspx?StartRow=1&Count=14");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                string[,] strArray = null;
                string strSql =
                    string.Format(@"select DEVICE,CONDITION,REPORTLEVEL,REPORTTIME 
                                    from LBGSDD.PVW_SECURITY");
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathIport).ExecuteTable(strSql, Convert.ToInt32(strStartRow), Convert.ToInt32(strStartRow) + Convert.ToInt32(strCount) - 1);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无更多数据！").DicInfo());
                    return;
                }

                strArray = new string[dt.Rows.Count, 4];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["DEVICE"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["CONDITION"]);
                    strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["REPORTLEVEL"]);
                    strArray[iRow, 3] = Convert.ToString(dt.Rows[iRow]["REPORTTIME"]);
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