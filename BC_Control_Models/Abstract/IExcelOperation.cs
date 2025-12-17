using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    /// <summary>
    /// Excel 操作通用接口
    /// </summary>
    public interface IExcelOperation
    {
        /// <summary>
        /// 读取 Excel 文件到 DataTable
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="sheetName">工作表名称(为空时读取第一个工作表)</param>
        /// <param name="hasHeader">是否包含表头</param>
        /// <returns>DataTable 对象</returns>
        DataTable ReadExcelToDataTable(string filePath, string sheetName = null, bool hasHeader = true);       
        /// <summary>
        /// 读取 Excel 文件到对象列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <param name="sheetName">工作表名称</param>
        /// <returns>对象列表</returns>
        IEnumerable<T> ReadExcelToObjects<T>(string filePath, string sheetName = null) where T : class, new();
        /// <summary>
        /// 将 DataTable 写入 Excel 文件
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="sheetName">工作表名称</param>
        /// <param name="overwrite">是否覆盖已存在文件</param>
        void WriteDataTableToExcel(DataTable dataTable, string filePath, string sheetName = "Sheet1", bool overwrite = true);
        /// <summary>
        /// 将对象列表写入 Excel 文件
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="objects">对象列表</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="sheetName">工作表名称</param>
        /// <param name="overwrite">是否覆盖已存在文件</param>
        void WriteObjectsToExcel<T>(IEnumerable<T> objects, string filePath, string sheetName = "Sheet1", bool overwrite = true) where T : class;
        /// <summary>
        /// 获取 Excel 文件的工作表名称列表
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>工作表名称列表</returns>
        IEnumerable<string> GetSheetNames(string filePath);

        /// <summary>
        /// 异步获取 Excel 文件的工作表名称列表
        /// </summary>
        IEnumerable<dynamic> QueryExcel(string filePath, string sheetName = null, string startCell = "A1", string endCell = null);
    }
}