using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Linq;
using System.Reflection;
using NPOI.SS.Util;
using BC_Control_Models;


namespace BC_Control_Helper.Excel
{
    public class NPIOExcelOpration : IExcelOperation
    {
        public IEnumerable<string> GetSheetNames(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("文件不存在", filePath);

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = WorkbookFactory.Create(fs);
                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    yield return workbook.GetSheetName(i);
                }
            }
        }

        public IEnumerable<dynamic> QueryExcel(string filePath, string sheetName = null, string startCell = "A1", string endCell = null)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("文件不存在", filePath);

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = WorkbookFactory.Create(fs);
                ISheet sheet = workbook.GetSheet(sheetName) ?? workbook.GetSheetAt(0);

                CellAddress startAddress = new CellAddress(startCell);
                CellAddress endAddress = endCell != null ?
                    new CellAddress(endCell) :
                    new CellAddress(sheet.LastRowNum, sheet.GetRow(sheet.LastRowNum)?.LastCellNum ?? 0);

                for (int rowNum = startAddress.Row; rowNum <= endAddress.Row; rowNum++)
                {
                    IRow row = sheet.GetRow(rowNum);
                    if (row == null) continue;

                    dynamic expando = new System.Dynamic.ExpandoObject();
                    var dict = expando as IDictionary<string, object>;

                    for (int colNum = startAddress.Column; colNum <= endAddress.Column; colNum++)
                    {
                        ICell cell = row.GetCell(colNum);
                        string columnName = GetColumnName(colNum);
                        dict[columnName] = GetCellValue(cell);
                    }
                    yield return expando;
                }
            }
        }

        public DataTable ReadExcelToDataTable(string filePath, string sheetName = null, bool hasHeader = true)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("文件不存在", filePath);

            DataTable dt = new DataTable();
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = WorkbookFactory.Create(fs);
                ISheet sheet = workbook.GetSheet(sheetName) ?? workbook.GetSheetAt(0);

                IRow headerRow = sheet.GetRow(sheet.FirstRowNum);
                int cellCount = headerRow.LastCellNum;

                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(hasHeader ?
                        GetCellValue(headerRow.GetCell(i))?.ToString() :
                        $"Column{i}");
                    dt.Columns.Add(column);
                }

                int startRow = hasHeader ? sheet.FirstRowNum + 1 : sheet.FirstRowNum;
                for (int rowNum = startRow; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    IRow row = sheet.GetRow(rowNum);
                    if (row == null) continue;

                    DataRow dataRow = dt.NewRow();
                    for (int i = row.FirstCellNum; i < cellCount; i++)
                    {
                        dataRow[i] = GetCellValue(row.GetCell(i));
                    }
                    dt.Rows.Add(dataRow);
                }
            }
            return dt;
        }

        public IEnumerable<T> ReadExcelToObjects<T>(string filePath, string sheetName = null) where T : class, new()
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("文件不存在", filePath);

            var properties = typeof(T).GetProperties();
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = WorkbookFactory.Create(fs);
                ISheet sheet = workbook.GetSheet(sheetName) ?? workbook.GetSheetAt(0);
                IRow headerRow = sheet.GetRow(0);

                var columnMappings = new Dictionary<int, PropertyInfo>();
                for (int i = headerRow.FirstCellNum; i < headerRow.LastCellNum; i++)
                {
                    string columnName = GetCellValue(headerRow.GetCell(i))?.ToString();
                    var prop = properties.FirstOrDefault(p =>
                        p.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));
                    if (prop != null)
                        columnMappings[i] = prop;
                }

                for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    IRow row = sheet.GetRow(rowNum);
                    if (row == null) continue;

                    T obj = new T();
                    foreach (var mapping in columnMappings)
                    {
                        ICell cell = row.GetCell(mapping.Key);
                        object value = GetCellValue(cell);
                        if (value != null)
                        {
                            var propType = mapping.Value.PropertyType;
                            if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                propType = Nullable.GetUnderlyingType(propType);

                            mapping.Value.SetValue(obj, Convert.ChangeType(value, propType));
                        }
                    }
                    yield return obj;
                }
            }
        }

        public void WriteDataTableToExcel(DataTable dataTable, string filePath, string sheetName = "Sheet1", bool overwrite = true)
        {
            IWorkbook workbook = File.Exists(filePath) ?
                WorkbookFactory.Create(filePath) :
                new HSSFWorkbook();

            if (workbook.GetSheet(sheetName) != null)
            {
                if (overwrite) workbook.RemoveSheetAt(workbook.GetSheetIndex(sheetName));
                else throw new ArgumentException("工作表已存在");
            }

            ISheet sheet = workbook.CreateSheet(sheetName);
            IRow headerRow = sheet.CreateRow(0);

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                headerRow.CreateCell(i).SetCellValue(dataTable.Columns[i].ColumnName);
            }

            for (int rowNum = 0; rowNum < dataTable.Rows.Count; rowNum++)
            {
                IRow row = sheet.CreateRow(rowNum + 1);
                for (int colNum = 0; colNum < dataTable.Columns.Count; colNum++)
                {
                    row.CreateCell(colNum).SetCellValue(dataTable.Rows[rowNum][colNum]?.ToString());
                }
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                workbook.Write(fs);
            }
        }

        public void WriteObjectsToExcel<T>(IEnumerable<T> objects, string filePath, string sheetName = "Sheet1", bool overwrite = true) where T : class
        {
            IWorkbook workbook = File.Exists(filePath) ?
                WorkbookFactory.Create(filePath) :
                new HSSFWorkbook();

            if (workbook.GetSheet(sheetName) != null)
            {
                if (overwrite) workbook.RemoveSheetAt(workbook.GetSheetIndex(sheetName));
                else throw new ArgumentException("工作表已存在");
            }

            ISheet sheet = workbook.CreateSheet(sheetName);
            var properties = typeof(T).GetProperties();
            IRow headerRow = sheet.CreateRow(0);

            for (int i = 0; i < properties.Length; i++)
            {
                headerRow.CreateCell(i).SetCellValue(properties[i].Name);
            }

            int rowNum = 1;
            foreach (var obj in objects)
            {
                IRow row = sheet.CreateRow(rowNum++);
                for (int colNum = 0; colNum < properties.Length; colNum++)
                {
                    object value = properties[colNum].GetValue(obj);
                    row.CreateCell(colNum).SetCellValue(value?.ToString());
                }
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                workbook.Write(fs);
            }
        }

        private object GetCellValue(ICell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.Blank: //BLANK:
                    return null;

                case CellType.Boolean: //BOOLEAN:
                    return cell.BooleanCellValue;

                case CellType.Numeric: //NUMERIC:
                    return cell.NumericCellValue;

                case CellType.String: //STRING:
                    return cell.StringCellValue;

                case CellType.Error: //ERROR:
                    return cell.ErrorCellValue;

                case CellType.Formula: //FORMULA:
                default:
                    return "=" + cell.CellFormula;
            }
        }

        private string GetColumnName(int index)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string columnName = "";
            while (index >= 0)
            {
                columnName = letters[index % 26] + columnName;
                index = (index / 26) - 1;
            }
            return columnName;
        }
    }
}
