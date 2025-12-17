using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using BC_Control_Helper;
using BC_Control_Models;

namespace BC_Control_Helper
{
    /// <summary>
    /// MiniExcel 操作实现类
    /// </summary>
    public class MiniExcelOperation : IExcelOperation
    {
        /// <summary>
        /// 读取 Excel 文件到 DataTable
        /// </summary>
       
        public DataTable ReadExcelToDataTable(string filePath, string sheetName = null, bool hasHeader = true)
        {
            ValidateFilePath(filePath);

            try
            {
                using (var stream = File.OpenRead(filePath))
                {
                    return stream.QueryAsDataTable(useHeaderRow: hasHeader, sheetName: sheetName);
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception($"读取Excel文件失败: {ex.Message}", ex);
            }
        }       

        /// <summary>
        /// 读取 Excel 文件到对象列表
        /// </summary>
        public IEnumerable<T> ReadExcelToObjects<T>(string filePath, string sheetName = null) where T : class, new()
        {
            ValidateFilePath(filePath);

            try
            {
                return MiniExcel.Query<T>(filePath, sheetName: sheetName);
            }
            catch (Exception ex)
            {
                throw new Exception($"读取Excel到对象列表失败: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// 将 DataTable 写入 Excel 文件
        /// </summary>
        public void WriteDataTableToExcel(DataTable dataTable, string filePath, string sheetName = "Sheet1", bool overwrite = true)
        {
            ValidateDataTable(dataTable);
            ValidateFilePath(filePath, checkExists: false);

            try
            {
                if (overwrite && File.Exists(filePath))
                    File.Delete(filePath);

                MiniExcel.SaveAs(filePath, dataTable, sheetName: sheetName, printHeader: true);
            }
            catch (Exception ex)
            {
                throw new Exception($"写入DataTable到Excel失败: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// 将对象列表写入 Excel 文件
        /// </summary>
        public void WriteObjectsToExcel<T>(IEnumerable<T> objects, string filePath, string sheetName = "Sheet1", bool overwrite = true) where T : class
        {
            ValidateObjects(objects);
            ValidateFilePath(filePath, checkExists: false);

            try
            {
                if (overwrite && File.Exists(filePath))
                    File.Delete(filePath);

                MiniExcel.SaveAs(filePath, objects, sheetName: sheetName, printHeader: true);
            }
            catch (Exception ex)
            {
                throw new Exception($"写入对象列表到Excel失败: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// 获取 Excel 文件的工作表名称列表
        /// </summary>
        public IEnumerable<string> GetSheetNames(string filePath)
        {
            ValidateFilePath(filePath);

            try
            {
                return MiniExcel.GetSheetNames(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"获取工作表名称失败: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// 查询 Excel 文件
        /// </summary>
        public IEnumerable<dynamic> QueryExcel(string filePath, string sheetName = null, string startCell = "A1", string endCell = null)
        {
            ValidateFilePath(filePath);

            try
            {
                return MiniExcel.Query(filePath, sheetName: sheetName, startCell: startCell);
            }
            catch (Exception ex)
            {
                throw new Exception($"查询Excel失败: {ex.Message}", ex);
            }
        }

        #region 私有方法

        private void ValidateFilePath(string filePath, bool checkExists = true)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath), "文件路径不能为空");

            if (checkExists && !File.Exists(filePath))
                throw new FileNotFoundException($"文件不存在: {filePath}");

            string extension = Path.GetExtension(filePath).ToLower();
            if (extension != ".xlsx" && extension != ".xls" && extension != ".csv")
                throw new ArgumentException("不支持的文件格式，仅支持.xlsx、.xls和.csv文件");
        }

        private void ValidateDataTable(DataTable dataTable)
        {
            if (dataTable == null)
                throw new ArgumentNullException(nameof(dataTable), "DataTable不能为空");

            if (dataTable.Columns.Count == 0)
                throw new ArgumentException("DataTable必须包含列定义");
        }

        private void ValidateObjects<T>(IEnumerable<T> objects) where T : class
        {
            if (objects == null)
                throw new ArgumentNullException(nameof(objects), "对象列表不能为空");
        }

        #endregion
    }
}