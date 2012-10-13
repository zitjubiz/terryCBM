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

namespace Terry.CRM.Web.UserControl
{
    public partial class TextArea : System.Web.UI.UserControl
    {
        public int MaxLength { get; set; }

        public string Text {
            get 
            {
                return mytext.Value;
            }
            set
            {
                mytext.Value = value;
            }
        }

        private string _width="450px";

        public string Width
        {
            get
            {
                if (mytext.Attributes["width"] == "")
                    return _width;
                else
                    return mytext.Attributes["width"].ToString();
            }
            set
            {
                mytext.Attributes.Remove("width");
                mytext.Attributes.Add("width", value);
            }
        }
    }
}