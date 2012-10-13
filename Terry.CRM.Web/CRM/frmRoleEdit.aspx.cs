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
using Terry.CRM.Entity;
using Terry.CRM.Service;
using System.Collections.Generic;

namespace Terry.CRM.Web.CRM
{
    public partial class frmRoleEdit : BasePage
    {
        private BaseService svr = new BaseService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Authentication(enumModule.Role);
                btnDel.Attributes.Add("onclick", "onDel('" + btnDel.UniqueID + "');return false;");
                hidID.Value = Request["id"];

                BindProduct();
                BindProvince();
                BindDepartment();
                BindModuleRight();

                BindData();
            }
        }

        private void BindProvince()
        {
            DataTable dt = svr.SearchByCriteria("CRMProvince", out recordCount, " ", "region");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                dr["Province"] = dr["Province"] + "(" + dr["Region"] + ")";
            }
            DDCLProvince.DataSource = dt;
            DDCLProvince.DataTextField = "Province";
            DDCLProvince.DataValueField = "ProvinceID";
            DDCLProvince.DataBind();
            //bind role relative products
            var Prods = svr.GetRoleRelativeProvince(long.Parse(hidID.Value));
            foreach (var p in Prods)
            {
                foreach (ListItem item in DDCLProvince.Items)
                {
                    if (item.Value == p.ProvinceID.ToString())
                        item.Selected = true;
                }
            }
        }

        private void BindProduct()
        {
            //bind Products(2nd level)
            DataTable dt = svr.SearchByCriteria("CRMProduct",out recordCount, " and len(code)=6", "code");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                dr["Product"] = dr["Product"] + "(" + dr["Code"] + ")";
            }
            DDCLProduct.DataSource = dt;
            DDCLProduct.DataTextField = "Product";
            DDCLProduct.DataValueField = "ProdID";
            DDCLProduct.DataBind();
            //bind role relative products
            var Prods = svr.GetRoleRelativeProducts(long.Parse(hidID.Value));
            foreach (var p in Prods)
            {
                foreach (ListItem item in DDCLProduct.Items)
                {
                    if (item.Value == p.ProdID.ToString())
                        item.Selected = true;
                }
            }
        }

        private void BindDepartment()
        {
            //bind Department
            DataTable dt = svr.SearchByCriteria("CRMDepartment", out recordCount, "", "DepName");

            DDCLDep.DataSource = dt;
            DDCLDep.DataTextField = "DepName";
            DDCLDep.DataValueField = "DepID";
            DDCLDep.DataBind();
            //bind role relative products
            var Deps = svr.GetRoleRelativeDeps(long.Parse(hidID.Value));
            foreach (var p in Deps)
            {
                foreach (ListItem item in DDCLDep.Items)
                {
                    if (item.Value == p.DepID.ToString())
                        item.Selected = true;
                }
            }
        }

        private void BindModuleRight()
        {
            string[] arrModule = new string[] { "客户信息","日程任务","员工信息","部门信息",
                "角色定义","产品信息","公司信息","基础信息","转移客户",
                "登录历史","邮件营销","报表","财务对账","客户系数"};
            //bind  ModuleRight
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Module", typeof(string)));
            dt.Columns.Add(new DataColumn("ModuleID", typeof(int)));

            Array arrEnum =Enum.GetValues(typeof(enumModule));
            foreach (enumModule item in arrEnum)
            {
                if ((int)item <= arrModule.Length)
                {
                    //DDCLModule.Items.Add(new ListItem(item.ToString(),((int)item).ToString()));
                    DataRow dr = dt.NewRow();
                    //dr[0] = item.ToString();
                    dr[0] = arrModule[(int)item - 1];
                    dr[1] = (int)item;
                    dt.Rows.Add(dr);
                }
            }
            //DDCLModule.DataSource = dt;
            //DDCLModule.DataTextField = "Module";
            //DDCLModule.DataValueField = "ModuleID";
            //DDCLModule.DataBind();

            rptMod.DataSource = dt;
            rptMod.DataBind();

            var rights = svr.GetRoleAccessRight(long.Parse(hidID.Value));
            foreach (var p in rights)
            {
                //foreach (ListItem item in DDCLModule.Items)
                //{
                //    if (item.Value == p.ModuleID.ToString())
                //        item.Selected = true;
                //}

                foreach (RepeaterItem item in rptMod.Items)
                {
                    if (((HiddenField)item.FindControl("HidID")).Value == p.ModuleID.ToString())
                    {
                        var cb = (CheckBoxList)item.FindControl("cblRight");
                        cb.Items[0].Selected = p.ReadOnly;
                        cb.Items[1].Selected = p.New;
                        cb.Items[2].Selected = p.Edit;
                        cb.Items[3].Selected = p.Del;
                    }

                }
            }



        }

        private void BindData()
        {
            //如果不是老板,在编辑角色的时候,不显示老板的等级
            if (LoginUserRoleGrade != (int)enumRoleGrade.Boss)
                ddlGrade.Items.RemoveAt(ddlGrade.Items.Count - 1);

            //bind entity
            var entity = (CRMRole)svr.LoadById(typeof(CRMRole), "RoleID", hidID.Value);
            if (entity != null)
            {
                hidID.Value = entity.RoleID.ToString();
                if (entity.Role != null)
                {
                    txtRole.Text = entity.Role.ToString();
                }
                ddlGrade.SelectedByValue(entity.RoleGrade.ToString());
                ddlGrade_SelectedIndexChanged(ddlGrade, null);
            }

        }

        private CRMRole GetSaveEntity()
        {
            var entity = new CRMRole();
            if (string.IsNullOrEmpty(hidID.Value.Trim()) == false)
                entity.RoleID = int.Parse(hidID.Value.Trim());
            if (string.IsNullOrEmpty(txtRole.Text.Trim()) == false)
                entity.Role = txtRole.Text.Trim();
            entity.RoleGrade = int.Parse(ddlGrade.SelectedValue);
            return entity;
        }

        private void CleanFrm()
        {
            hidID.Value = "";
            txtRole.Text = "";
        }

        //Click Save Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = GetSaveEntity();
                //角色对应部门权限
                List<CRMDepartment> DepList = new List<CRMDepartment>();
                string[] arrD = DDCLDep.SelectedValuesToString().Split(',');
                foreach (var ID in arrD)
                {
                    if (!string.IsNullOrEmpty(ID))
                    {
                        var d = new CRMDepartment();
                        d.DepID = long.Parse(ID);
                        DepList.Add(d);

                    }
                }
                //角色对应产品权限
                List<CRMProduct> ProdList = new List<CRMProduct>();
                string[] arrP = DDCLProduct.SelectedValuesToString().Split(',');
                foreach (var ID in arrP)
                {
                    if (!string.IsNullOrEmpty(ID))
                    {
                        var p = new CRMProduct();
                        p.ProdID = long.Parse(ID);
                        ProdList.Add(p);

                    }
                }
                //角色对应区域省份权限
                List<CRMProvince> ProvinceList = new List<CRMProvince>();
                string[] arrPv = DDCLProvince.SelectedValuesToString().Split(',');
                foreach (var ID in arrPv)
                {
                    if (!string.IsNullOrEmpty(ID))
                    {
                        var p = new CRMProvince();
                        p.ProvinceID = int.Parse(ID);
                        ProvinceList.Add(p);

                    }
                }
                //角色对应模块权限
                //string[] arrM = DDCLModule.SelectedValuesToString().Split(',');
                List<CRMRoleModule> ModList = new List<CRMRoleModule>();
                foreach (RepeaterItem item in rptMod.Items)
                {
                    var p = new CRMRoleModule();
                    p.ModuleID = long.Parse(((HiddenField)item.FindControl("HidID")).Value);
                    var cb = (CheckBoxList)item.FindControl("cblRight");
                    p.ReadOnly = cb.Items[0].Selected ;
                    p.New = cb.Items[1].Selected ;
                    p.Edit = cb.Items[2].Selected;
                    p.Del =cb.Items[3].Selected ;
                    ModList.Add(p);

                }

                entity = svr.Save(entity, ProdList, ModList, DepList, ProvinceList);
                hidID.Value = entity.RoleID.ToString();
                this.ShowSaveOK();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }

        //Click Delete Button
        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                //级联删除Role和RoleProds
                svr.DeleteById(typeof(CRMRole), "RoleID", hidID.Value);
                this.ShowDeleteOK();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmRole.aspx");
        }

        protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlGrade.SelectedValue)
            {
                //enumRoleGrade
                case "1": //sales
                    trProd.Visible = false;
                    trDep.Visible = false;
                    break;
                case "2": //prod manager
                    trProd.Visible = true;
                    trDep.Visible = false;
                    break;
                case "3": //dep manager
                    trProd.Visible = false;
                    trDep.Visible = true;
                    break;
                case "4": //sales manager
                    trProd.Visible = false;
                    trDep.Visible = false;
                    break;
                case "8":
                    trProd.Visible = false;
                    trDep.Visible = false;
                    break;
                case "9":
                    break;
                default:
                    break;
            }
        }
    }
}

