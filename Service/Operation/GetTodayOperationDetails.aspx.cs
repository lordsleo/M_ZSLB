//
//文件名：    GetTodayOperationDetails.aspx.cs
//功能描述：  获取今日作业详情
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
    public partial class GetTodayOperationDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strOperationDate = DateTime.Now.ToString("yyyy-MM-dd");
                strOperationDate = "2015-12-07";

                string strSql =
                    string.Format(@"select * from (
                                    select a.DEVICE,a.TIMEPAI,a.TIMEWAN,a.DQBW,b.DATANAME,a.CHI_VESSEL,a.ENG_VESSEL,a.LOA,a.TON_DEAD,a.NAME,a.TRADENAME,a.GUOJI,a.YHY,TO_CHAR(a.DATEOFCREATE, 'YYYY-MM-DD') as DATEOFCREATE 
                                    from LBGSDD.CVW_SCHEDULE_MOBILE a, LBGSDD.LVW_WORKTYPE2 b 
                                    where a.WORKTYPE=b.DATACODE(+) 
                                    order by DATEOFCREATE desc) 
                                    where DATEOFCREATE='{0}'",
                                    strOperationDate);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathIport).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }

                string[] strNameArray = { "拖轮", "开始时间", "结束时间", "泊位", "作业类型", "中文船名", "英文船名",
                                       "船长", "载重吨", "船代", "内外贸", "国籍", "引航员", "创建时间"};
                //排序字符串
                string strOrder = string.Empty;
                strOrder = strNameArray[0] + "+" + strNameArray[1] + "+" + strNameArray[2] + "+" + strNameArray[3] + "+" + strNameArray[4] + "+" + strNameArray[5] + "+" + strNameArray[6] + "+" +
                        strNameArray[7] + "+" + strNameArray[8] + "+" + strNameArray[9] + "+" + strNameArray[10] + "+" + strNameArray[11] + "+" + strNameArray[12] + "+" + strNameArray[13];
                
                Dictionary<string, string> info = new Dictionary<string, string>();
                info.Add(strNameArray[0], Convert.ToString(dt.Rows[0]["DEVICE"]));
                info.Add(strNameArray[1], StringTool.ToDayNightForSql(Convert.ToString(dt.Rows[0]["TIMEPAI"])));
                info.Add(strNameArray[2], StringTool.ToDayNightForSql(Convert.ToString(dt.Rows[0]["TIMEWAN"])));
                info.Add(strNameArray[3], Convert.ToString(dt.Rows[0]["DQBW"]));
                info.Add(strNameArray[4], Convert.ToString(dt.Rows[0]["DATANAME"]));
                info.Add(strNameArray[5], Convert.ToString(dt.Rows[0]["CHI_VESSEL"]));
                info.Add(strNameArray[6], Convert.ToString(dt.Rows[0]["ENG_VESSEL"]));
                info.Add(strNameArray[7], Convert.ToString(dt.Rows[0]["LOA"]));
                info.Add(strNameArray[8], Convert.ToString(dt.Rows[0]["TON_DEAD"]));
                info.Add(strNameArray[9], Convert.ToString(dt.Rows[0]["NAME"]));
                info.Add(strNameArray[10], Convert.ToString(dt.Rows[0]["TRADENAME"]));
                info.Add(strNameArray[11], Convert.ToString(dt.Rows[0]["GUOJI"]));
                info.Add(strNameArray[12], Convert.ToString(dt.Rows[0]["YHY"]));
                info.Add(strNameArray[13], Convert.ToString(dt.Rows[0]["DATEOFCREATE"]));
                info.Add("Order", strOrder);

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