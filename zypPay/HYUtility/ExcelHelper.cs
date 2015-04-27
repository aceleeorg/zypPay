using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using System.IO;
using System.Data;
using System.Text;
using NPOI.XSSF.UserModel;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using System.Collections;
using System.Collections.Generic;
using System;

namespace HYUtility
{
    public class ExcelHelper
    {

        /// <summary>   
        /// 由DataSet导出Excel    
        /// </summary>    
        /// <param name="sourceTable">要导出数据的DataTable</param>    
        /// <param name="sheetName">工作表名称</param>    
        /// <returns>Excel工作表</returns>    
        private static Stream ExportDataSetToExcel(DataSet sourceDs, string sheetName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            string[] sheetNames = sheetName.Split(',');
            for (int i = 0; i < sheetNames.Length; i++)
            {
                ISheet sheet = workbook.CreateSheet(sheetNames[i]);
                IRow headerRow = sheet.CreateRow(0);
                //      
                foreach (DataColumn column in sourceDs.Tables[i].Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                //             
                int rowIndex = 1;
                foreach (DataRow row in sourceDs.Tables[i].Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in sourceDs.Tables[i].Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                    rowIndex++;
                }
            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            workbook = null;
            return ms;
        }

       


        public static byte[] ExportDataSetToBytes(DataSet sourceDs, string sheetName)
        {
            MemoryStream ms = ExportDataSetToExcel(sourceDs, sheetName) as MemoryStream;

            return ms.ToArray();
            //HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            //HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            //HttpContext.Current.Response.End();
            //ms.Close();
            //ms = null;
        }

        /// <summary>   
        ///  由DataTable导出Excel   
        /// </summary>   
        /// <param name="sourceTable">要导出数据的DataTable</param>    
        /// <returns>Excel工作表</returns>    
        private static Stream ExportDataTableToXls(DataTable sourceTable, string sheetName,bool ifProtect)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet(sheetName);
            IRow headerRow = sheet.CreateRow(0);
            //          
            foreach (DataColumn column in sourceTable.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            //          
            int rowIndex = 1;
            foreach (DataRow row in sourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                if (ifProtect)
                {
                    ICellStyle locked = workbook.CreateCellStyle();
                    locked.IsLocked = true;//确定当前单元格被设置保护
                    dataRow.RowStyle = locked;
                }
                foreach (DataColumn column in sourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    //if (!ifProtect)
                    //{
                       
                    //}
                    //else
                    //{ 
                    ////dataRow.CreateCell
                    //}
                }
                rowIndex++;
            }
            sheet.ProtectSheet("hypay");
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            headerRow = null;
            workbook = null;
            return ms;
        }

        /// <summary>   
        ///  由DataTable导出Excel   
        /// </summary>   
        /// <param name="sourceTable">要导出数据的DataTable</param>    
        /// <returns>Excel工作表</returns>    
        private static Stream ExportDataTableToXlsx(DataTable sourceTable, string sheetName, bool ifProtect)
        {
            IWorkbook workbook = new XSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet(sheetName);
            IRow headerRow = sheet.CreateRow(0);
            //          
            foreach (DataColumn column in sourceTable.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            //          
            int rowIndex = 1;
            foreach (DataRow row in sourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                if (ifProtect)
                {
                    ICellStyle locked = workbook.CreateCellStyle();
                    locked.IsLocked = true;//确定当前单元格被设置保护
                    dataRow.RowStyle = locked;
                }

                foreach (DataColumn column in sourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }
                rowIndex++;
            }
            sheet.ProtectSheet("hypay");
            workbook.Write(ms);
            ms.Flush();
            //ms.Position = 0;
            sheet = null;
            headerRow = null;
            workbook = null;
            return ms;
        }

        private static Stream ExportDataTablesToExcel(DataTable[] sourceTables, string[] sheetNames)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            for (int i = 0; i < sourceTables.Length; i++)
            {
                DataTable sourceTable = sourceTables[i];
                string sheetName = sheetNames[i];
                ISheet sheet = workbook.CreateSheet(sheetName);
                IRow headerRow = sheet.CreateRow(0);
                //          
                foreach (DataColumn column in sourceTable.Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                //          
                int rowIndex = 1;
                foreach (DataRow row in sourceTable.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in sourceTable.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                    rowIndex++;
                }
                sheet = null;
                headerRow = null;
            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            workbook = null;
            return ms;
        }

        private static Stream ExportDataTablesToStyleExcel(DataTable[] sourceTables, string[] sheetNames, bool titleBold, bool firstColBold, List<int>[] indexs1, List<int>[] indexs2)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            for (int i = 0; i < sourceTables.Length; i++)
            {
                if (sourceTables[i] == null) continue;
                DataTable sourceTable = sourceTables[i];
                string sheetName = sheetNames[i];
                ISheet sheet = workbook.CreateSheet(sheetName);
                IRow headerRow = sheet.CreateRow(0);
                //          
                foreach (DataColumn column in sourceTable.Columns)
                {
                    if (!titleBold)
                        headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                    else
                    {
                        ICellStyle style = workbook.CreateCellStyle();
                        style.BorderBottom = BorderStyle.THIN;// CellBorderType.THIN;
                        style.BorderLeft = BorderStyle.THIN;// CellBorderType.THIN;
                        style.BorderRight = BorderStyle.THIN;
                        style.BorderTop = BorderStyle.THIN;
                        style.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.BROWN.index;
                        style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.CORAL.index;
                        style.FillPattern = FillPatternType.SOLID_FOREGROUND;
                        IFont font = workbook.CreateFont();
                        font.Boldweight = (short)FontBoldWeight.BOLD;//加粗
                        //font.FontHeightInPoints = 14;
                        style.SetFont(font);

                        ICell colCell = headerRow.CreateCell(column.Ordinal);
                        colCell.SetCellValue(column.ColumnName);
                        colCell.CellStyle = style;
                    }
                }
                //          
                int rowIndex = 1;

                foreach (DataRow row in sourceTable.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    for (int j = 0; j < sourceTable.Columns.Count; j++)
                    {
                        DataColumn column = sourceTable.Columns[j];
                        ICell drCell = dataRow.CreateCell(column.Ordinal);
                        drCell.SetCellValue(row[column].ToString());

                        if (firstColBold && j == 0)
                        {
                            ICellStyle style = workbook.CreateCellStyle();
                            style.BorderBottom = BorderStyle.THIN;
                            style.BorderLeft = BorderStyle.THIN;
                            style.BorderRight = BorderStyle.THIN;
                            style.BorderTop = BorderStyle.THIN;
                            IFont font = workbook.CreateFont();
                            font.Boldweight = (short)FontBoldWeight.BOLD;//加粗
                            //font.FontHeightInPoints = 14;
                            style.SetFont(font);
                            style.Alignment = HorizontalAlignment.CENTER;
                            style.VerticalAlignment = VerticalAlignment.CENTER;
                            drCell.CellStyle = style;
                        }

                    }
                    //foreach (DataColumn column in sourceTable.Columns)
                    //{

                    //    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    //}
                    rowIndex++;
                }
                List<int> index1 = indexs1[i];
                List<int> index2 = indexs2[i];
                if (index1 != null && index2 != null)
                {
                    for (int k = 0; k < index1.Count; k++)
                    {
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(index1[k], index2[k], 0, 0));
                    }
                }
                //sheet .AddMergedRegion (new NPOI.SS.Util.CellRangeAddress (
                sheet = null;
                headerRow = null;
            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            workbook = null;
            return ms;
        }

        /// <summary>    
        /// 由DataTable导出Excel
        /// </summary>   
        /// <param name="sourceTable">要导出数据的DataTable</param>   
        /// <param name="fileName">Excel名称</param>    
        /// <param name="sheetName">Excel工作区名称</param>
        /// <returns>Excel工作表</returns>    
        public static void ExportDataTableToExcel(DataTable sourceTable, string fileName, string sheetName,bool ifProtect)
        {
            //MemoryStream ms = ExportDataTableToXls(sourceTable, sheetName) as MemoryStream;
       
            //HttpContext curContext = HttpContext.Current;

            //// 设置编码和附件格式   
            //curContext.Response.ContentType = "application/vnd.ms-excel";
            //curContext.Response.ContentEncoding = Encoding.UTF8;
            //curContext.Response.Charset = "UTF8";
            //curContext.Response.AppendHeader("Content-Disposition",
            //    "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + ".xls");

            //curContext.Response.BinaryWrite(ms.ToArray());
            //curContext.Response.End();

         

            using (MemoryStream ms = ExportDataTableToXls(sourceTable, sheetName,ifProtect ) as MemoryStream)
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }


        /// <summary>    
        /// 由DataTable导出Excel
        /// </summary>   
        /// <param name="sourceTable">要导出数据的DataTable</param>   
        /// <param name="fileName">Excel名称</param>    
        /// <param name="sheetName">Excel工作区名称</param>
        /// <returns>Excel工作表</returns>    
        public static void ExportDataTableToXlsx(DataTable sourceTable, string fileName, string sheetName, bool ifProtect)
        {
            //MemoryStream ms = ExportDataTableToXlsx(sourceTable, sheetName) as MemoryStream;
          
            //HttpContext curContext = HttpContext.Current;

            //// 设置编码和附件格式   
            //curContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //curContext.Response.ContentEncoding = Encoding.UTF8;
            //curContext.Response.Charset = "UTF8";
            //curContext.Response.AppendHeader("Content-Disposition",
            //    "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + ".xlsx");

            //curContext.Response.BinaryWrite(ms.ToArray());
            //curContext.Response.End();

            //ms.Close();
            //ms = null;

            using (MemoryStream ms = ExportDataTableToXlsx(sourceTable, sheetName,ifProtect ) as MemoryStream)
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

      
       

        public static byte[] ExportDataTableToBytes(DataTable sourceTable, string sheetName,bool ifProtect)
        {
            MemoryStream ms = ExportDataTableToXlsx(sourceTable, sheetName,ifProtect ) as MemoryStream;

            return ms.ToArray();
            //HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            //HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            //HttpContext.Current.Response.End();
            //ms.Close();
            //ms = null;
        }


        private static Stream ExportDataTableToExcelFromTemplate(DataTable sourceTable, string sheetName, string templatepath)
        {

            FileStream fileTemplate = new FileStream(templatepath, FileMode.Open, FileAccess.Read);
            HSSFWorkbook workbook = new HSSFWorkbook(fileTemplate);
            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.GetSheet(sheetName);
            IRow headerRow = sheet.CreateRow(0);
            //         
            foreach (DataColumn column in sourceTable.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            //        
            int rowIndex = 1;
            foreach (DataRow row in sourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in sourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }
                rowIndex++;
            }

            sheet.ForceFormulaRecalculation = true;
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            headerRow = null;
            workbook = null;
            return ms;
        }




        /// <summary>
        /// 导出数据到EXCEL 多个表的
        /// </summary>
        /// <param name="ds">数据集</param>
        /// <param name="AbosultedFilePath">导出的 EXCEL 路径</param>
        /// <param name="name">EXCEL 工作簿的名字</param>
        /// <param name="title">表头</param>
        /// <returns>返回文件路径</returns>
        public static string ExportToExcels(System.Data.DataSet ds, string AbosultedFilePath, string path, string[] name, string title)
        {
            try
            {

                //string path = System.Configuration.cong;
                //判断路径是否存在
                if (Directory.Exists(path))
                {
                    //删除文件夹及文件
                    foreach (string d in Directory.GetFileSystemEntries(path))
                    {
                        if (File.Exists(d))
                        { File.Delete(d); }
                    }
                    Directory.Delete(path, true);
                }
                int PageIndex = 0;
                if (ds.Tables.Count <= 0)
                    return string.Empty;
                for (int t = 0; t < ds.Tables.Count; t++)
                {
                    System.Data.DataTable dt = ds.Tables[t];
                    int count = dt.Rows.Count;//获取datatable内数据量
                    int pagecount = 50000; //每页的数据
                    PageIndex = Pagount(count, pagecount); //获取分页数
                    string filename = t.ToString() == "0" ? "Area_Statistics" : "IP_statistics";
                    //存在分页时 创建新目录保存新execl文件
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    for (int i = 1; i <= PageIndex; i++)
                    {
                        //将模板文件复制到新目录下
                        string fileName = path + "/" + filename + i + ".xls";
                        //根据页码获取DataTable内的数据
                        System.Data.DataTable execlDT = GetPagedTable(dt, i, pagecount);
                        //将DataTable内的数据写入execl
                        RenderDataTableToExcel(execlDT, fileName);
                    }
                }
                //完成写入后 压缩文件
                ZipDir(path, path, 2, title);
                return path + title + ".zip";
            }
            catch (Exception ex)
            {
                // Logger.Error("DataTable转execl失败" + ex.Message);
                return string.Empty;
            }
        }


        /// <summary>
        /// 导出数据到EXCEL 多个表的
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="path">导出的 EXCEL 路径</param>
        /// <param name="excelName">EXCEL 工作簿的名字</param>
        /// <param name="zipTitle">表头</param>
        /// <returns>返回文件路径</returns>
        public static string ExportToExcels(DataTable dt, string path, string excelName, string zipTitle)
        {
            try
            {

                //string path = System.Configuration.cong;
                //判断路径是否存在
                if (Directory.Exists(path))
                {
                    //删除文件夹及文件
                    foreach (string d in Directory.GetFileSystemEntries(path))
                    {
                        if (File.Exists(d))
                        { File.Delete(d); }
                    }
                    Directory.Delete(path, true);
                }
                int PageIndex = 0;


                //  System.Data.DataTable dt = ds.Tables[t];
                int count = dt.Rows.Count;//获取datatable内数据量
                int pagecount = 50000; //每页的数据
                PageIndex = Pagount(count, pagecount); //获取分页数
                string filename = excelName;// t.ToString() == "0" ? "Area_Statistics" : "IP_statistics";
                //存在分页时 创建新目录保存新execl文件
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                for (int i = 1; i <= PageIndex; i++)
                {
                    //将模板文件复制到新目录下
                    string fileName = path + "/" + filename + i + ".xls";
                    //根据页码获取DataTable内的数据
                    System.Data.DataTable execlDT = GetPagedTable(dt, i, pagecount);
                    //将DataTable内的数据写入execl
                    RenderDataTableToExcel(execlDT, fileName);
                }

                //完成写入后 压缩文件
                ZipDir(path, path, 2, zipTitle);
                return path + zipTitle + ".zip";
            }
            catch (Exception ex)
            {
                // Logger.Error("DataTable转execl失败" + ex.Message);
                return string.Empty;
            }
        }
        #region 压缩文件
        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="DirToZip">文件夹路径</param>
        /// <param name="ZipedFile">输出文件路径</param>
        /// <param name="CompressionLevel">设置缓存大小</param>
        ///<param name="fileName">压缩后的文件名称</param>
        public static void ZipDir(string DirToZip, string ZipedFile, int CompressionLevel, string fileName)
        {
            try
            {
                //压缩文件为空时默认与压缩文件夹同一级目录   
                if (ZipedFile == string.Empty)
                {
                    ZipedFile = DirToZip.Substring(DirToZip.LastIndexOf("\\") + 1);
                    ZipedFile = DirToZip.Substring(0, DirToZip.LastIndexOf("\\")) + "\\" + ZipedFile + ".zip";
                }
                if (System.IO.Path.GetExtension(ZipedFile) != ".zip")
                {
                    ZipedFile = ZipedFile + fileName + ".zip";
                }
                using (ZipOutputStream zipoutputstream = new ZipOutputStream(System.IO.File.Create(ZipedFile)))
                {
                    zipoutputstream.SetLevel(CompressionLevel);
                    Crc32 crc = new Crc32();
                    System.IO.DirectoryInfo myDir = new DirectoryInfo(DirToZip);
                    List<DictionaryEntry> fileList = GetAllFiles(DirToZip);
                    foreach (DictionaryEntry item in fileList)
                    {
                        //可能存在文件夹无法访问情况 需捕捉异常，根据实际情况返回
                        try
                        {
                            System.IO.FileStream fs = System.IO.File.OpenRead(item.Key.ToString());
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            ZipEntry entry = new ZipEntry(item.Key.ToString().Substring(DirToZip.Length + 1));
                            entry.DateTime = (DateTime)item.Value;
                            entry.Size = fs.Length;
                            fs.Flush();
                            fs.Close();
                            crc.Reset();
                            crc.Update(buffer);
                            entry.Crc = crc.Value;
                            zipoutputstream.PutNextEntry(entry);
                            zipoutputstream.Write(buffer, 0, buffer.Length);
                        }
                        catch (Exception ex)
                        {
                            //Logger.Error("压缩文件夹：" + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Logger.Error("压缩execl文件夹：" + ex.Message);
            }
        }

        /// <summary>   
        /// 获取所有文件   
        /// </summary>   
        /// <returns></returns>   
        private static List<DictionaryEntry> GetAllFiles(string dir)
        {
            try
            {
                List<DictionaryEntry> dictonary = new List<DictionaryEntry>();
                if (!System.IO.Directory.Exists(dir))
                {
                    return dictonary;
                }
                else
                {
                    System.IO.DirectoryInfo root = new System.IO.DirectoryInfo(dir);
                    System.IO.FileSystemInfo[] arrary = root.GetFileSystemInfos();
                    for (int i = 0; i < arrary.Length; i++)
                    {
                        dictonary.Add(new DictionaryEntry(arrary[i].FullName, arrary[i].LastWriteTime));
                    }
                }
                return dictonary;
            }
            catch (Exception ex)
            {
                // Logger.Error("获取文件夹下的所有文件" + ex.Message);
                return null;
            }
        }
        #endregion

        #region DataTable分页
        /// <summary>
        /// DataTable分页
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="PageIndex">页索引,注意：从1开始</param>
        /// <param name="PageSize">每页大小</param>
        /// <returns>分好页的DataTable数据</returns>              第1页        每页10条
        public static System.Data.DataTable GetPagedTable(System.Data.DataTable dt, int PageIndex, int PageSize)
        {
            if (PageIndex == 0) { return dt; }
            System.Data.DataTable newdt = dt.Copy();
            newdt.Clear();
            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
            { return newdt; }

            if (rowend > dt.Rows.Count)
            { rowend = dt.Rows.Count; }
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }

        /// <summary>
        /// 返回分页的页数
        /// </summary>
        /// <param name="count">总条数</param>
        /// <param name="pageye">每页显示多少条</param>
        /// <returns>如果 结尾为0：则返回1</returns>
        public static int Pagount(int count, int pageye)
        {
            int page = 0;
            int sesepage = pageye;
            if (count % sesepage == 0) { page = count / sesepage; }
            else { page = (count / sesepage) + 1; }
            if (page == 0) { page += 1; }
            return page;
        }
        #endregion

        #region Datatable转Execl
        /// <summary>
        /// 把Datatable中的数据保存成指定的Excel文件
        /// </summary>
        /// <param name="SourceTable">需要转成execl的DateTable</param>
        /// <param name="FileName">详细的文件路径带文件名与格式</param>
        public static void RenderDataTableToExcel(System.Data.DataTable SourceTable, string FileName)
        {
            //Logger.Info("进入方法RenderDataTableToExcel 文件名：" + FileName);
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream _ms = new MemoryStream();
            // 创建Excel文件的Sheet
            ISheet sheet = workbook.CreateSheet("Sheet1");
            sheet.SetColumnWidth(0, 30 * 256); //设置单元格的宽度
            sheet.SetColumnWidth(1, 20 * 256);//设置单元格的宽度
            sheet.SetColumnWidth(2, 20 * 256);//设置单元格的宽度
            // 创建行
            IRow headerRow = sheet.CreateRow(0);
            // 把Datatable中的列名添加Sheet中第一列中作为表头
            foreach (DataColumn column in SourceTable.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            int rowIndex = 1;
            // 循环Datatable中的行和列添加数据到Excel中
            foreach (DataRow row in SourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in SourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }
                rowIndex++;
            }
            try
            {
                MemoryStream ms = _ms as MemoryStream;
                workbook.Write(ms);
                _ms.Flush();
                _ms.Position = 0;
                FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite);
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
                ms.Flush();
                ms.Close();
                data = null;
                ms = null;
                fs = null;
            }
            catch (Exception ex)
            {
                //Logger.Error("把Datatable中的数据保存成指定的Excel文件:" + ex.Message);
            }
        }
        #endregion




    }

    public class TemplateExcelExportHelper
    {
        public void SetDataTableValue(ISheet sheet, int rowIndex, int columnIndex, DataTable dt)
        {
            IRow row = null;
            ICell cell = null;
            foreach (DataRow dataRow in dt.Rows)
            {
                //row = sheet.GetRow(rowIndex);//模板中行数固定时，使用 sheet.GetRow(rowIndex)
                //动态添加行时使用：
                row = sheet.CreateRow(rowIndex);
                columnIndex = 0;
                foreach (DataColumn column in dt.Columns)
                {
                    //模板中行数固定时，使用row.GetCell(columnIndex);
                    //cell = row.GetCell(columnIndex);
                    //动态添加时使用：
                    cell = row.CreateCell(columnIndex);
                    string drValue = dataRow[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String":
                            cell.SetCellValue(drValue);
                            break;
                        case "System.DateTime":
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            cell.SetCellValue(dateV);

                            break;
                        case "System.Boolean":
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            cell.SetCellValue(boolV);
                            break;
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            cell.SetCellValue(intV);
                            break;
                        case "System.Decimal":
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            cell.SetCellValue(doubV);
                            break;
                        case "System.DBNull":
                            cell.SetCellValue("");
                            break;
                        default:
                            cell.SetCellValue("");
                            break;
                    }
                    columnIndex++;
                }

                rowIndex++;
            }
        }

        //public void ExportDataTables2TemplateExcel(DataTable sourceTable, string fileName, string templateName, int rowIndex, int colIndex)
        //{
        //    TemplateExcelHelper hepler = new TemplateExcelHelper(templateName, fileName);
        //    StartCol = colIndex;
        //    StartRow = rowIndex;
        //    tempTable = sourceTable;
        //    hepler.ExportDataToExcel(SetPurchaseOrder);

        //}

        //public void ExportDataTables2TemplateExcel2(HttpContext context, DataTable sourceTable, string fileName, string templateName, int rowIndex, int colIndex)
        //{
        //    TemplateExcelHelper hepler = new TemplateExcelHelper(templateName, fileName);
        //    StartCol = colIndex;
        //    StartRow = rowIndex;
        //    tempTable = sourceTable;
        //    hepler.ExportDataToExcel2(SetPurchaseOrder, context);

        //}
        DataTable tempTable = null;
        int StartRow = 0;
        int StartCol = 0;
        private void SetPurchaseOrder(ISheet sheet)
        {
            //HSSFRow row = null;
            //HSSFCell cell = null;

            //DataTable itemDT = PrepareItemDTForTest();

            SetDataTableValue(sheet, StartRow, StartCol, tempTable);

        }
    }

    //public class TemplateExcelHelper
    //{
    //    private string templatePath;
    //    private string newFileName;
    //    private string templdateName;

    //    private string sheetName;

    //    public string SheetName
    //    {
    //        get { return sheetName; }
    //        set { sheetName = value; }
    //    }

    //    public TemplateExcelHelper(string templdateName, string newFileName)
    //    {
    //        this.sheetName = "sheet1";
    //        templatePath = HttpContext.Current.Server.MapPath("/") + "/Template/";
    //        this.templdateName = string.Format("{0}{1}", templatePath, templdateName);
    //        this.newFileName = newFileName;
    //    }

    //    public void ExportDataToExcel(Action<ISheet> actionMethod)
    //    {

    //        using (MemoryStream ms = SetDataToExcel(actionMethod))
    //        {
    //            byte[] data = ms.ToArray();

    //            #region response to the client
    //            HttpResponse response = System.Web.HttpContext.Current.Response;
    //            response.Clear();
    //            response.Charset = "UTF-8";
    //            response.ContentType = "application/vnd-excel";//"application/vnd.ms-excel";
    //            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + newFileName));
    //            System.Web.HttpContext.Current.Response.BinaryWrite(data);
    //            #endregion
    //        }
    //    }
    //    public void ExportDataToExcel2(Action<ISheet> actionMethod, HttpContext context)
    //    {

    //        using (MemoryStream ms = SetDataToExcel(actionMethod))
    //        {
    //            byte[] data = ms.ToArray();

    //            #region response to the client
    //            HttpResponse response = context.Response;
    //            response.Clear();
    //            response.Charset = "UTF-8";
    //            response.ContentType = "application/vnd-excel";//"application/vnd.ms-excel";
    //            context.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + newFileName));
    //            context.Response.BinaryWrite(data);
    //            //return data;
    //            #endregion
    //        }
    //    }

    //    private MemoryStream SetDataToExcel(Action<ISheet> actionMethod)
    //    {
    //        //Load template file
    //        FileStream file = new FileStream(templdateName, FileMode.Open, FileAccess.Read);
    //        HSSFWorkbook workbook = new HSSFWorkbook(file);
    //        ISheet sheet = workbook.GetSheet(SheetName);

    //        if (actionMethod != null) actionMethod(sheet);

    //        sheet.ForceFormulaRecalculation = true;
    //        using (MemoryStream ms = new MemoryStream())
    //        {
    //            workbook.Write(ms);
    //            ms.Flush();
    //            ms.Position = 0;
    //            sheet = null;
    //            workbook = null;

    //            return ms;
    //        }
    //    }
    //}
}
