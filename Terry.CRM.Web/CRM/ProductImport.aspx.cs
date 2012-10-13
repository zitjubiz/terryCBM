using System;
using System.Collections;
using System.Collections.Generic;
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
using Terry.ECommerce.Persistence;
using Terry.ECommerce.DomainModel;
using Terry.ECommerce.DomainModel.ValueObject;
using System.Diagnostics;
using System.IO;
using NPOI.HSSF.UserModel;


namespace Terry.ECommerce.Web
{
    public partial class ProductImport : BasePage
    {
        ProductRepositoryImp pr = new ProductRepositoryImp();
        CategoryRepositoryImp cr = new CategoryRepositoryImp();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                base.Authentication(Module.BaseInformation);
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.PostedFile != null)
            {
                string filename = FileUpload1.FileName;
                string fileExt = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
                if (fileExt != "xls")
                {
                    ClientScript.RegisterClientScriptBlock(typeof(string), "xls", "<script>alert('Please select Excel file!');</script>");
                    return;
                }
                filename = Server.MapPath("~/Upload/CSV/") + "Product" + Session.SessionID.Substring(0, 2) + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                FileUpload1.SaveAs(filename);
                ExtractExcelData(filename);
                //取完数据之后删除
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                Response.Write("<script>alert('Import Succeed!');</script>");
                //this.ShowMessage("Import Succeed!");
            }

        }
        private void ExtractExcelData(string filename)
        {
            Product p;
            FileStream file = new FileStream(filename, FileMode.Open);
            HSSFWorkbook wb = new HSSFWorkbook(file);
            HSSFSheet sht;
            sht = wb.GetSheetAt(0); //取第一个sheet
            //取行Excel的最大行数

            int rowsCount = sht.PhysicalNumberOfRows;
            //第1行是header,不是数据
            for (int i = 1; i < rowsCount; i++)
            {
                //如果ProductID是空,跳过
                if (sht.GetRow(i).GetCell(Col('A')) == null)
                    continue;

                p = new Product();
                p.CreateDate = DateTime.Now;
                p.ID = sht.GetRow(i).GetCell(Col('A')).ToString().Trim();
                p.UPC = sht.GetRow(i).GetCell(Col('B')).ToString().Trim();
                p.StyleNum = sht.GetRow(i).GetCell(Col('C')).ToString().Trim();
                
                //D,E,F,G是保存在ProductAttributes里面
                p.HTSCode = sht.GetRow(i).GetCell(Col('H')).ToString().Trim();
                p.EUHTSCode = sht.GetRow(i).GetCell(Col('I')).ToString().Trim();
                p.PackingType = sht.GetRow(i).GetCell(Col('J')).ToString().Trim();
                //K,L是保存在ProductAttributes里面
                p.Cost = sht.GetRow(i).GetCell(Col('M')).ToString().ToDecimal();
                if (sht.GetRow(i).GetCell(Col('N')) != null)
                    p._Brand = new Brand() { supplierCode = sht.GetRow(i).GetCell(Col('N')).ToString() };
                else
                    p._Brand = null;
                //-------add categories----------
                if (sht.GetRow(i).GetCell(Col('O')) != null)
                {
                    p.Categories = new List<Category>();
                    string[] Cats = sht.GetRow(i).GetCell(Col('O')).ToString().Split(',');
                    for (int j = 0; j < Cats.Length; j++)
                    {
                        if (Cats[j].Trim() != "")
                            p.Categories.Add(new Category() { categoryCode = Cats[j].Trim() });
                    }
                }
                else
                {
                    p.Categories = null;
                }
                if (sht.GetRow(i).GetCell(Col('P')) != null)
                    p._Supplier = new Supplier() { ID = sht.GetRow(i).GetCell(Col('P')).ToString() };
                else
                    p._Supplier = null;
                //Column Q=>V   On Hand(GZ)/On Hand(DG)/On Hand(HK)/On Hand(US)/
                //On Order(PO to Factory)/On Sales Order(PI from Customer)"

                p.PrePack = sht.GetRow(i).GetCell(Col('W')).ToString().ToInt();
                p.GrossWeight = sht.GetRow(i).GetCell(Col('X')).ToString().ToDecimal();
                p.NetWeight = sht.GetRow(i).GetCell(Col('Y')).ToString().ToDecimal();
                p.Length = sht.GetRow(i).GetCell(Col('Z')).ToString().ToDecimal();
                p.Width = sht.GetRow(i).GetCell(Col("AA")).ToString().ToDecimal();
                p.Height = sht.GetRow(i).GetCell(Col("AB")).ToString().ToDecimal();
                p.IsActive = string2bool(sht.GetRow(i).GetCell(Col("AC")).ToString());
                p.SequenceInCat = sht.GetRow(i).GetCell(Col("AD")).ToString().ToInt();
                //现在取StyleNumFE
                p.StyleNumFE = sht.GetRow(i).GetCell(Col("AE")).ToString();
                p.IsNewArrival = string2bool(sht.GetRow(i).GetCell(Col("AF")).ToString());
                
                //add attributes,因为在hbm里设置了inverse=true,
                //所以用Product.ProductAttributes.add 是不会保存到数据庫里的.
                //D,E,F,G,K,L是保存在ProductAttributes里面
                List<ProductAttribute> PA = new List<ProductAttribute>();
                PA.Add(pr.BuildAttribute(p, this.Region, "ProductName", sht.GetRow(i).GetCell(Col('D')).ToString()));
                PA.Add(pr.BuildAttribute(p, this.Region, "ShipDesc", sht.GetRow(i).GetCell(Col('E')).ToString()));
                PA.Add(pr.BuildAttribute(p, this.Region, "FabricComp", sht.GetRow(i).GetCell(Col('F')).ToString()));
                PA.Add(pr.BuildAttribute(p, this.Region, "ProdDesc", sht.GetRow(i).GetCell(Col('G')).ToString()));
                PA.Add(pr.BuildAttribute(p, this.Region, "Color", sht.GetRow(i).GetCell(Col('K')).ToString()));
                PA.Add(pr.BuildAttribute(p, this.Region, "Size", sht.GetRow(i).GetCell(Col('L')).ToString()));

                //部分字段没有值,等待客户新资料
                PA.Add(pr.BuildAttribute(p, this.Region, "Display", "True"));
                PA.Add(pr.BuildAttribute(p, this.Region, "IsLowPrice", "True"));
                PA.Add(pr.BuildAttribute(p, this.Region, "ShowHomePage", "False"));
                PA.Add(pr.BuildAttribute(p, this.Region, "WholeSalePrice", "0"));
                PA.Add(pr.BuildAttribute(p, this.Region, "ProductPrice", "0"));
                try
                {
                    pr.BatchImport(p, PA);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + p.ID + " ");
                    this.ShowMessage("Please check " + p.ID + " " + ex.Message);
                }
            }
            file.Close();



        }

        #region old csv code
        //private void ExtractData(string filename)
        //{
        //    //throw new NotImplementedException();
        //    CsvStreamReader csv = new CsvStreamReader(filename, System.Text.Encoding.UTF8);
        //    //first row is header,not data, start from 2, <= RowCount
        //    for (int i = 2; i <= csv.RowCount; i++)
        //    {
        //        //选择styleNum 不为空的行
        //        if (csv[i, Col('C')] != "")
        //        {
        //            Product p = new Product();
        //            //p.IsVisible = string2bool(csv[i, Col('O')]);
        //            //p.IsLowest = string2bool(csv[i, Col('P')]);
        //            //p.IsHot = string2bool(csv[i, Col('Q')]);

        //            //int.TryParse(csv[i, Col('R')],out qtyStock);
        //            //p.QtyInStock = qtyStock;
        //            p.CreateDate = DateTime.Now;
        //            p.ID = csv[i, Col('A')].Trim();
        //            p.UPC = csv[i, Col('B')];
        //            p.StyleNum = csv[i, Col('C')].Trim();
        //            //D,E,F,G是保存在ProductAttributes里面
        //            p.HTSCode = csv[i, Col('H')].Trim();
        //            p.EUHTSCode = csv[i, Col('I')].Trim();
        //            p.PackingType = csv[i, Col('J')].Trim();
        //            //K,L是保存在ProductAttributes里面
        //            p.Cost = csv[i, Col('M')].ToDecimal();
        //            if (csv[i, Col('N')] != "")
        //                p._Brand = new Brand() { supplierCode = csv[i, Col('N')] };
        //            else
        //                p._Brand = null;
        //            //-------add categories----------
        //            if (csv[i, Col('O')] != null)
        //            {
        //                p.Categories = new List<Category>();
        //                string[] Cats = csv[i, Col('O')].Split(',');
        //                for (int j = 0; j < Cats.Length; j++)
        //                {
        //                    if (Cats[j].Trim() != "")
        //                        p.Categories.Add(new Category() { categoryCode = Cats[j].Trim() });
        //                }
        //            }
        //            else
        //            {
        //                p.Categories = null;
        //            }
        //            if (csv[i, Col('P')] != "")
        //                p._Supplier = new Supplier() { ID = csv[i, Col('P')] };
        //            else
        //                p._Supplier = null;
        //            //Column Q=>V   On Hand(GZ)/On Hand(DG)/On Hand(HK)/On Hand(US)/
        //            //On Order(PO to Factory)/On Sales Order(PI from Customer)"

        //            p.PrePack = csv[i, Col('W')].ToInt();
        //            p.GrossWeight = csv[i, Col('X')].ToDecimal();
        //            p.NetWeight = csv[i, Col('Y')].ToDecimal();
        //            p.Length = csv[i, Col('Z')].ToDecimal();
        //            p.Width = csv[i, Col("AA")].ToDecimal();
        //            p.Height = csv[i, Col("AB")].ToDecimal();
        //            p.IsActive = string2bool(csv[i, Col("AC")]);
        //            p.SequenceInCat = csv[i, Col("AD")].ToInt();

        //            //add attributes,因为在hbm里设置了inverse=true,
        //            //所以用Product.ProductAttributes.add 是不会保存到数据庫里的.
        //            List<ProductAttribute> PA = new List<ProductAttribute>();
        //            PA.Add(pr.BuildAttribute(p, this.Region, "ProductName", csv[i, Col('D')]));
        //            PA.Add(pr.BuildAttribute(p, this.Region, "ShipDesc", csv[i, Col('E')]));
        //            PA.Add(pr.BuildAttribute(p, this.Region, "FabricComp", csv[i, Col('F')]));
        //            PA.Add(pr.BuildAttribute(p, this.Region, "ProdDesc", csv[i, Col('G')]));
        //            PA.Add(pr.BuildAttribute(p, this.Region, "Color", csv[i, Col('K')]));
        //            PA.Add(pr.BuildAttribute(p, this.Region, "Size", csv[i, Col('L')]));

        //            //部分字段没有值,等待客户新资料
        //            PA.Add(pr.BuildAttribute(p, this.Region, "Display", "True"));
        //            PA.Add(pr.BuildAttribute(p, this.Region, "IsLowPrice", "True"));
        //            PA.Add(pr.BuildAttribute(p, this.Region, "ShowHomePage", "False"));
        //            PA.Add(pr.BuildAttribute(p, this.Region, "WholeSalePrice", "0"));
        //            PA.Add(pr.BuildAttribute(p, this.Region, "ProductPrice", "0"));

        //            try
        //            {
        //                pr.BatchImport(p, PA);

        //            }
        //            catch (Exception ex)
        //            {
        //                Debug.WriteLine(ex.Message + p.ID + " ");
        //                this.ShowMessage("Please check " + p.ID + " " + ex.Message);
        //            }

        //        }
        //    }
        //}

        //NPOI是从0开始
        private int Col(char Column)
        {
            char A = 'A';
            return (int)Column - (int)A;
        }
        //AA,BA...
        private int Col(string Column)
        {
            if (Column.Length > 1)
            {
                var chr = Column.ToCharArray();
                char First = chr[0];
                char Second = chr[1];
                char A = 'A';
                return ((int)First - (int)A + 1) * 26 + (int)Second - (int)A;
            }
            else
                return Col(Column.ToCharArray(0, 1)[0]);

        }
        #endregion



        private bool string2bool(string TrueOrFalse)
        {
            if (TrueOrFalse.ToLower() == "true")
                return true;
            else
                return false;
        }


    }
}
