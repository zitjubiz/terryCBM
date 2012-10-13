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
using NPOI.HSSF.UserModel;
using System.IO;

namespace Terry.CRM.Web.CRM.Excel
{
    /// <summary>
    /// 在开始之前，我们先来补些基础知识。每一个xls都对应一个唯一的HSSFWorkbook，
    /// 每一个HSSFWorkbook会有若干个HSSFSheet，
    /// 而每一个HSSFSheet包含若干HSSFRow（Excel 2003中不得超过65535行）
    /// ，每一个HSSFRow又包含若干个HSSFCell（Excel 2003中不得超过256列）。 
    ///为了遍历所有的单元格，我们就得获得某一个HSSFSheet的所有HSSFRow，
    ///通常可以用HSSFSheet.GetRowEnumerator()。如果要获得某一特定行，
    ///可以直接用HSSFSheet.GetRow(rowIndex)。
    ///另外要遍历我们就必须知道边界，有一些属性我们是可以用的，
    ///比如HSSFSheet.FirstRowNum（工作表中第一个有数据行的行号）、
    ///HSSFSheet.LastRowNum（工作表中最后一个有数据行的行号）、
    ///HSSFRow.FirstCellNum（一行中第一个有数据列的列号）、
    ///HSSFRow.LastCellNum（一行中最后一个有数据列的列号）。
    /// </summary>
    public partial class frmImport : BasePage
    {
        HSSFWorkbook hssfworkbook;
        DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        void InitializeWorkbook(string path)
        {
            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            //book1.xls is an Excel-2007-generated file, so some new unknown BIFF records are added. 
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
        }

        /// <summary>
        /// 把HSSF的数据放到一个DataTable中。
        /// </summary>
        void ConvertToDataTable()
        {
            HSSFSheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            DataTable dt = new DataTable();
            //根据xls文件的列数,生成columns
            for (int j = 0; j < 5; j++)
            {
                dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());
            }

            while (rows.MoveNext())
            {
                HSSFRow row = (HSSFRow)rows.Current;
                DataRow dr = dt.NewRow();

                for (int i = 0; i < row.LastCellNum; i++)
                {
                    HSSFCell cell = row.GetCell(i);


                    if (cell == null)
                    {
                        dr[i] = null;
                    }
                    else
                    {
                        dr[i] = cell.ToString();
                    }
                }
                dt.Rows.Add(dr);
            }
            ds.Tables.Add(dt);
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            InitializeWorkbook(@"C:\Book1.xls");
            ConvertToDataTable();
            this.GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();

        }


    }
}
