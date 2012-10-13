using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Terry.CRM.Web
{
    public static class DropDownListExtension
    {
        public static void SelectedByText(this DropDownList ddl, string text)
        {
            if (text == null)
            {
                ddl.SelectedIndex = -1;
                return;
            }
            ListItem item = ddl.Items.FindByText(text);
            // ddl.Items.FindByText(text.Trim)
            if (item == null)
            {
                ddl.SelectedIndex = -1;
            }
            else
            {
                //ddl.SelectedIndex = -1;
                item.Selected = true;
            }
        }

        public static void SelectedByValue(this DropDownList ddl, string value)
        {
            if (value == null)
            {
                ddl.SelectedIndex = -1;
                return;
            }
            ListItem item = ddl.Items.FindByValue(value);
            // ddl.Items.FindByText(text.Trim)
            if (item == null)
            {
                ddl.SelectedIndex = -1;
            }
            else
            {
                //ddl.SelectedIndex = -1;
                item.Selected = true;
            }
        }


        public static void BindDropDownList(this DropDownList ddl, Object list, string textField, string valueField)
        {
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataSource = list;
            ddl.DataBind();
        }
        
        public static void BindDropDownListAndSelect(this DropDownList ddl, Object list, string textField, string valueField)
        {
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataSource = list;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(System.Web.HttpContext.GetGlobalResourceObject("re", "DDLSelected").ToString(), ""));
        }

        //CheckBoxList extension
        public static void BindCheckBoxList(this CheckBoxList cbl, Object list, string textField, string valueField)
        {
            cbl.DataTextField = textField;
            cbl.DataValueField = valueField;
            cbl.DataSource = list;
            cbl.DataBind();
        }
        /// <summary>
        /// 初始化CheckBoxList中哪些是选中了的
        /// <param name="checkList">CheckBoxList</param>
        /// <param name="selval">选中了的值串例如："0,1,1,2,1"</param>
        /// <param name="separator">值串中使用的分割符例如"0,1,1,2,1"中的逗号</param>
        ///</summary>
        public static void SetChecked(this CheckBoxList checkList, string selval, string separator)
        {
            if (selval == null) return;

            string[] arrSep = {separator};
            string[] arrVal = selval.Split(arrSep,StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < checkList.Items.Count; i++)
            {
                checkList.Items[i].Selected = false;
                foreach (string val in arrVal)
                {
                    if (checkList.Items[i].Value == val)
                    {
                        checkList.Items[i].Selected = true;
                        break;
                    }
                }
            }
 
        }
        /// <summary>
        /// 得到CheckBoxList中选中了的值
        /// </summary>
        /// <param name="checkList">CheckBoxList</param>
        /// <param name="separator">分割符号</param>
        /// <returns></returns>
        public static string GetChecked(this CheckBoxList checkList, string separator)
        {
            string selval = "";
            for (int i = 0; i < checkList.Items.Count; i++)
            {
                if (checkList.Items[i].Selected)
                {
                    selval += checkList.Items[i].Value + separator;
                }
            }
            return selval;
        }
    }

    public static class PageExtensionMethods
    {
        public static Control FindControlRecursive(this Control ctrl, string controlID)
        {
            if (string.Compare(ctrl.ID, controlID, true) == 0)
            {
                // We found the control!
                return ctrl;
            }
            else
            {
                // Recurse through ctrl's Controls collections
                foreach (Control child in ctrl.Controls)
                {
                    Control lookFor = FindControlRecursive(child, controlID);

                    if (lookFor != null)
                        return lookFor;  // We found the control
                }

                // If we reach here, control was not found
                return null;
            }
        }
    }
}

