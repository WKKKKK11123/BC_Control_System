using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models;
using BC_Control_Helper;
using thinger.DataConvertLib;

namespace BC_Control_Helper
{
    public static class StatusClassHelper
    {
        private static IPLCHelper _plcHelper;

        /// <summary>
        /// 初始化PLC服务接口
        /// </summary>
        public static void Initialize(IPLCHelper plcService)
        {
            _plcHelper = plcService ?? throw new ArgumentNullException(nameof(plcService));
        }
        public static void AnalysisStatusAttribute(this List<StatusClass> basestatusClass)
        {
            try
            {
                if (basestatusClass.Count == 0)
                {
                    return;
                }
                foreach (var statusClass in basestatusClass)
                {
                    if (string.IsNullOrEmpty(statusClass.StatusArributeString))
                    {
                        continue;
                    }
                    string temp = statusClass.StatusArributeString;
                    List<string> strings = temp.Split(',').ToList();
                    statusClass.StatusArribute = strings
                        .Where(prop => prop.Contains('='))
                        .Select(prop => new
                        {
                            key = Convert.ToInt32(prop.Split('=')[1]),
                            Value = prop.Split('=')[0]
                        })
                        .GroupBy(prop => prop.key)
                        .ToDictionary(g => g.Key, g => g.Select(prop => prop.Value).First());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void UpdateStatus(this List<StatusClass> basestatusClass)
        {
            try
            {
                if (_plcHelper == null)
                    throw new InvalidOperationException("PLC服务未初始化，请先调用Initialize方法");

                try
                {
                    foreach (var item in basestatusClass)
                    {
                        UpdateStatusClass(item);
                    }
                }
                catch (Exception ex)
                {
                    // 记录日志或处理异常
                    // 注意：这里与原代码不同，不吞掉异常
                    throw new Exception($"批量更新数据类时发生错误: {ex.Message}", ex);
                }
                //foreach (var item in basestatusClass)
                //{
                //    Device tempDevice = PLCSelect.Instance.SelectDevice(item.PLC);
                //    if (tempDevice==null)
                //    {
                //        item.Value = "0";
                //        continue;
                //    }
                //    if (!PLCSelect.Instance.SelectDevice(item.PLC).IsConnected)
                //    {
                //        item.Value = "0";
                //        continue;
                //    }
                //    //Device tempDevice = PLCSelect.Instance.SelectDevice(item.PLC);
                //    string value = PLCSelect.Instance.GetValue(item);
                //    int value1 = 0;
                //    if (
                //        !int.TryParse(value, out value1)
                //        || item.StatusArribute.Count() == 0
                //        || !item.StatusArribute.ContainsKey(value1)
                //    ) //如果无特性或者不是数字则可直接跳出
                //    {
                //        item.Value = value;
                //        continue;
                //    }
                //    item.Value = item.StatusArribute[value1];
                //}
            }
            catch (Exception ee)
            {

            }
        }
        private static void UpdateStatusClass(StatusClass item)
        {
            if (item == null) return;

            try
            {
                // 使用接口方法替换原来的静态实例调用
                var tempDevice = _plcHelper.SelectDevice(item.PLC);
                if (tempDevice == null) return;

                var variable = _plcHelper.FindVariable(item.ValueAddress, item.PLC);
                if (variable == default(Variable)) return;

                var dataType = (DataType)Enum.Parse(typeof(DataType), variable.DataType, true);
                string falseValue = GetDefaultValue(dataType);

                if (!tempDevice.IsConnected)
                {
                    item.Value = falseValue;
                    return;
                }

                UpdateItemValues(item, variable, dataType);
                string value=item.ActualValue;
                int value1 = 0;
                if (
                    !int.TryParse(item.ActualValue, out value1)
                    || item.StatusArribute.Count() == 0
                    || !item.StatusArribute.ContainsKey(value1)
                ) //如果无特性或者不是数字则可直接跳出
                {
                    item.Value = value;
                    return;
                }
                item.Value = item.StatusArribute[value1];
                

            }
            catch (Exception ex)
            {
                throw new Exception($"{item.ValueAddress},{item.PLC},{item.ParameterName}:{ex.Message}");
            }
        }
        private static string GetDefaultValue(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Bool:
                    return false.ToString();
                case DataType.Float:
                    return "0.0";
                default:
                    return "0";
            }
        }
        private static void UpdateItemValues(StatusClass item, Variable variable, DataType dataType)
        {
            try
            {
                if (variable.VarValue == null)
                {
                    return;
                }
                if (!string.IsNullOrEmpty(item.ValueAddress))
                {
                    item.ActualValue = dataType == DataType.Float
                        ? Convert.ToDouble(variable.VarValue).ToString("F2")
                        : variable.VarValue.ToString()!;
                    
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
