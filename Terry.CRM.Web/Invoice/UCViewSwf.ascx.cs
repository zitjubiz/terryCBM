using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Terry.CRM.Web
{
    public partial class UCViewSwf : System.Web.UI.UserControl
    {
        protected string FilePath = "";


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetFilePath(string filePath)
        {
            this.Visible = !string.IsNullOrEmpty(filePath);
            FilePath = filePath;            
        }
    }
}