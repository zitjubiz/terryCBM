using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BaseControls;
using Terry.CRM.Entity;
using Terry.CRM.Service;
using System.Globalization;
using Terry.CRM.Web.CommonUtil;

namespace Terry.CRM.Web.CRM
{
    public partial class frmScheduleByWeek : BasePage
    {
        private GTDService svr = new GTDService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Authentication(enumModule.Schedule);
                txtCustOwnerID.BindDropDownList(svr.GetUserByDep(), "UserName", "UserID");
                txtCustOwnerID.Text = base.LoginUserID.ToString();
                //日程任务所有人可见

                BindData();
            }
        }
        //刷新
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            rptWeek.DataSource = new string[] { "8/27 (周一)", "8/28 (周二)", "8/29 (周三)", "8/30 (周四)", "8/31 (周五)" };
            rptWeek.DataBind();
            //BaseCalendar1.DataBind();
        }

        protected void BaseCalendar1_RenderBodyDay(BaseCalendar sender, BaseTagContentDay tag)
        {
            // for every day and if the date is within the current visible month...
            if (!tag.Info.IsOtherMonth)
            {
                // we change the ID of the generated TD (inside TBODY > TR), maybe useful when using javascript
                tag.Id = "x_" + tag.Info.Date.Day;
                // just a demo that you can add/use style values (more than one) for each row
                //tag.Style.Add("cursor:pointer");

                // you can also add custom attributes, in this case "onclick"
                // The actual content of the cell can be anything, like other HTML tags. In this case
                // we render the day number like the calendar would render is we didn't have a DayTag
                // handler by we add an asterisk to show that it's different. 
                tag.Content.Write("<div class=dayheader><a class='day' href='#' onclick=\"EditTask(0," + tag.Info.Date.ToString("yyyyMMdd")
                     + ",'" + txtCustOwnerID.SelectedItem.Text + "');\">" + tag.DefaultText + ChineseCalendar.GetChineseShortDay(tag.Info.Date) + "</a></div>");

                //-----------根据人名，日期取得日程---------
                tag.Content.Write(svr.GetDayTagHtml(tag.Info.Date.ToString("yyyyMMdd"), txtCustOwnerID.SelectedItem.Text));
                //tag.Content.Write(tag.Info.Date.ToString("%d*", sender.CurrentDateTimeFormatInfo));
            }
            else
            {
                // Since we have a DayTag handler (this method) the calendar control doesn't output anything
                // by default (it lets us to our custom rendering). To help us out it puts the content it 
                // would've normally rendered inside DefaultText, but it's still up to us to actually render it;
                // we just don't have to figure out what it would've rendered by default -- note that this is
                // exactly what we did in the other "if" branch...

                tag.Content.Write("");
            }
            // just to show that you can, a dummy "url" attribute with "sweet" inside
            tag.Attributes.Add("url", "sweet");

        }

        protected void txtCustOwnerID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
    }
}
