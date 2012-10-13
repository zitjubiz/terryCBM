using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Terry.CRM.Web
{
    public partial class PdfView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["file"] != null)
                {
                    UCViewSwf1.SetFilePath(string.Format("/Upload/swf/{0}.swf", Server.HtmlEncode(Request.QueryString["file"])));
                }
            }
        }

        protected string GetPdfUrl()
        {
            return string.Format("/Upload/pdf/{0}.pdf", Server.HtmlEncode(Request.QueryString["file"]));
        }
    }
}