//
//文件名：    NewsDetail.aspx.cs
//功能描述：  详细新闻
//创建时间：  2015/12/16
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

namespace M_Zslb.Service.News
{
    public partial class NewsDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var strMessage = Request.Params["message"];
                var str = strMessage.Split('=');
                var str1 = str[1].Split(' ');
                var strNewsId = str1[0];

                string strSql =
                    string.Format(@"select topic,to_char(post_time, 'yyyy-mm-dd hh24:mi:ss') as post_time,message from c_news 
                                    where id = '{0}'", strNewsId);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathNbw).ExecuteTable(strSql);
                var strArray = new Leo.Data.Table(dt).ToArray();

                Json = JsonConvert.SerializeObject(strArray);
            }
            catch (Exception ex)
            {
                LogTool.WriteLog(typeof(NewsDetail), string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message));
            }
        }
        protected string Json;
    }
}