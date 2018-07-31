//
//文件名：    GetTodayMaterialOutStorage.aspx.cs
//功能描述：  获取今日物料领料
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

namespace M_Zslb.Service.MaterialConsumption
{
    public partial class GetTodayMaterialOutStorage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strOutdateDate = DateTime.Now.ToString("yyyy-MM-dd");
                strOutdateDate = "2015-11-27";

                string[,] strArray = null;
                string strSql =
                    string.Format(@"select ITEM_NAME,TOTALMONEY,DEPT
                                    from MATERIAL.PVW_STOREOUTBILLGROUP
                                    where OUTDATE='{0}'
                                    order by OUTDATE desc",
                                    strOutdateDate);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathIport).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }

                strArray = new string[dt.Rows.Count, 3];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["ITEM_NAME"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["TOTALMONEY"]);
                    strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["DEPT"]);
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