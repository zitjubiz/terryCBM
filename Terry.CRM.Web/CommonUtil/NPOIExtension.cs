using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;

namespace Terry.CRM.Web
{
    public static class NPOIExtension
    {
        public static string GetStringCellValue(this HSSFSheet sheet, int row, string AlphaBet)
        {
            HSSFCell cell = sheet.GetRow(row).GetCell(Col(AlphaBet));

            if (cell == null)
                return "";
            else
                return cell.ToString().Trim();
        }
        public static double GetDoubleCellValue(this HSSFSheet sheet, int row, string AlphaBet)
        {
            HSSFCell cell = sheet.GetRow(row).GetCell(Col(AlphaBet));

            if (cell.CellType == HSSFCell.CELL_TYPE_NUMERIC)
                return cell.NumericCellValue;
            else
                return 0;

        }
        public static DateTime? GetDateCellValue(this HSSFSheet sheet, int row, string AlphaBet)
        {
            HSSFCell cell = sheet.GetRow(row).GetCell(Col(AlphaBet));

            if (cell == null)
                return new DateTime(1900, 1, 1);
            else
            {
                if (cell.StringCellValue == "")
                    return null;
                else
                    return cell.DateCellValue;
            }
        }
        //用excel生成报表,行列不定时用
        public static HSSFCell Items(this HSSFSheet sheet, int row, int column)
        {

            HSSFRow hssfRow;
            if (row <= sheet.LastRowNum)
                hssfRow = sheet.GetRow(row);
            else
                hssfRow = sheet.CreateRow(row);

            HSSFCell hssfCell;
            hssfCell = hssfRow.GetCell(column);

            if (hssfCell == null)
                hssfCell = hssfRow.CreateCell(column);

            return hssfCell;
        }
        public static HSSFCell Items(this HSSFSheet sheet, int row, string ColumnAlphaBet)
        {
            return Items(sheet, row, Col(ColumnAlphaBet));
        }

        public static HSSFCell GetCell(this HSSFRow row, string ColumnAlphaBet)
        {
            try 
	        {	        
		        return row.GetCell(Col(ColumnAlphaBet));
	        }
	        catch (NullReferenceException e)
	        {		
		        throw new NullReferenceException("row="+ row.RowNum + ",col="+ ColumnAlphaBet,e);
	        }
            
        }

        public static HSSFCell CreateCell(this HSSFRow row, string ColumnAlphaBet)
        {
            return row.CreateCell(Col(ColumnAlphaBet));
        }
        public static void Hide(this HSSFRow row)
        {
            row.ZeroHeight = true;
        }
        public static void Show(this HSSFRow row)
        {
            row.ZeroHeight = false;
        }
        //NPOI是从0开始
        private static int Col(char Column)
        {
            char A = 'A';
            return (int)Column - (int)A;
        }
        //AA,BA...
        private static int Col(string Column)
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
    }
}