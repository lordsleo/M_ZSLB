//
//文件名：    GetMonthOperationDetails.aspx.cs
//功能描述：  获取本月作业详情
//创建时间：  2015/12/08
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
    public partial class GetMonthOperationDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //数据起始行
            var startRow = Request.Params["StartRow"];
            //行数
            var count = Request.Params["Count"];

           try
            {
                if (startRow == null || count == null)
                {
                    string warning = string.Format("参数StartRow，Count不能为null！举例：http://218.92.115.55/M_Zslb/Service/Operation/GetMonthOperationDetails.aspx?StartRow=1&Count=14");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                DateTime curDate = DateTime.Now;
                DateTime startDate = new DateTime(curDate.Year, curDate.Month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                string strStartDate = startDate.ToString("yyyy-MM-dd");
                string strEndDate = endDate.ToString("yyyy-MM-dd");
                //strStartDate = "2015-11-01";
                //strEndDate = "2015-11-30";
                

                string[,] strArray = null;
                string strSql =
                     string.Format(@"select * from (
                                     select a.DEVICE,a.TIMEPAI,a.TIMEWAN,a.DQBW,b.DATANAME,a.CHI_VESSEL,a.ENG_VESSEL,a.LOA,a.TON_DEAD,a.NAME,a.TRADENAME,a.GUOJI,a.YHY,TO_CHAR(a.DATEOFCREATE, 'YYYY-MM-DD') as DATEOFCREATE 
                                     from LBGSDD.CVW_SCHEDULE_MOBILE a, LBGSDD.LVW_WORKTYPE2 b 
                                     where a.WORKTYPE=b.DATACODE(+)) 
                                     where DATEOFCREATE>='{0}' and DATEOFCREATE<='{1}' 
                                     order by DATEOFCREATE desc",
                                     strStartDate, strEndDate);

                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathIport).ExecuteTable(strSql, Convert.ToInt32(startRow), Convert.ToInt32(startRow) + Convert.ToInt32(count) - 1);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }

                strArray = new string[dt.Rows.Count, 14];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["DEVICE"]);
                    strArray[iRow, 1] = StringTool.ToDayNightForSql(Convert.ToString(dt.Rows[0]["TIMEPAI"]));
                    strArray[iRow, 2] = StringTool.ToDayNightForSql(Convert.ToString(dt.Rows[0]["TIMEWAN"]));
                    strArray[iRow, 3] = Convert.ToString(dt.Rows[iRow]["DQBW"]);
                    strArray[iRow, 4] = Convert.ToString(dt.Rows[iRow]["DATANAME"]);
                    strArray[iRow, 5] = Convert.ToString(dt.Rows[iRow]["CHI_VESSEL"]);
                    strArray[iRow, 6] = Convert.ToString(dt.Rows[iRow]["ENG_VESSEL"]);
                    strArray[iRow, 7] = Convert.ToString(dt.Rows[iRow]["LOA"]);
                    strArray[iRow, 8] = Convert.ToString(dt.Rows[iRow]["TON_DEAD"]);
                    strArray[iRow, 9] = Convert.ToString(dt.Rows[iRow]["NAME"]);
                    strArray[iRow, 10] = Convert.ToString(dt.Rows[iRow]["TRADENAME"]);
                    strArray[iRow, 11] = Convert.ToString(dt.Rows[iRow]["GUOJI"]);
                    strArray[iRow, 12] = Convert.ToString(dt.Rows[iRow]["YHY"]);
                    strArray[iRow, 13] = Convert.ToString(dt.Rows[iRow]["DATEOFCREATE"]);  
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
   