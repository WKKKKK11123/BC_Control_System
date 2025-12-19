using NPOI.XWPF.UserModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using thinger.DataConvertLib;
using BC_Control_Models;
using NPOI.HSSF.Record;

namespace BC_Control_Helper
{
    public static class DataClassHelper
    {
        private static IPLCHelper _plcHelper;

        /// <summary>
        /// 初始化PLC服务接口
        /// </summary>
        public static void Initialize(IPLCHelper plcService)
        {
            _plcHelper = plcService ?? throw new ArgumentNullException(nameof(plcService));
        }

        public static void UpdateDataClasses(this List<DataClass> dataClasses)
        {
            if (_plcHelper == null)
                throw new InvalidOperationException("PLC服务未初始化，请先调用Initialize方法");

            try
            {
                foreach (var item in dataClasses)
                {
                    UpdateDataClass(item);
                }
            }
            catch (Exception ex)
            {
                // 记录日志或处理异常
                // 注意：这里与原代码不同，不吞掉异常
                throw new Exception($"批量更新数据类时发生错误: {ex.Message}", ex);
            }
        }

       
        public static T ToEntity<T>(this List<DataClass> dataClasses) where T : class, new()
        {
            try
            {
                T tEntity = new T();
                PropertyInfo[] propertyInfos = typeof(T).GetProperties();
                Parallel.ForEach(dataClasses, (item) =>
                {
                    var propertyInfo = propertyInfos.FirstOrDefault(para =>
                        GetPropertyDescription(para) == item.ParameterName
                    );
                    if (propertyInfo == null)
                    {
                        return;
                    }
                    propertyInfo.SetValue(tEntity, item);
                });                              
                return tEntity;
            }
            catch (Exception)
            {

                return default(T);
            }
        }
        private static void UpdateDataClass(DataClass item)
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
                    item.SettingValue = falseValue;
                    return;
                }

                UpdateItemValues(item, variable, dataType);
            }
            catch (Exception ex)
            {
                throw new Exception($"{item.ValueAddress},{item.PLC},{item.ParameterName}:{ex.Message}");
            }
        }

        /// <summary>
        /// 根据数据类型获取默认值
        /// </summary>
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

        /// <summary>
        /// 更新项目的值和设置值
        /// </summary>
        private static void UpdateItemValues(DataClass item, Variable variable, DataType dataType)
        {
            try
            {
                if (variable.VarValue==null)
                {
                    return;
                }
                if (!string.IsNullOrEmpty(item.ValueAddress))
                {
                    item.Value = dataType == DataType.Float
                        ? Convert.ToDouble(variable.VarValue).ToString("F2")
                        : variable.VarValue.ToString()!;
                }

                if (!string.IsNullOrEmpty(item.SettingValueAddress))
                {
                    var SettingValue = _plcHelper.FindVariable(item.SettingValueAddress, item.PLC);
                    if (SettingValue == null)
                    {
                        item.SettingValue = "";
                        return;
                    }
                    item.SettingValue = dataType == DataType.Float
                        ? Convert.ToDouble(SettingValue.VarValue).ToString("F2")
                        : variable.VarValue.ToString()!;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
        private static string GetPropertyDescription(PropertyInfo propertyInfo)
        {
            try
            {
                if (propertyInfo == null)
                    return string.Empty;

                // 获取属性的 Description 特性
                var attributes = propertyInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                    return ((DescriptionAttribute)attributes[0]).Description;
                else
                    return propertyInfo.Name; // 返回属性名作为默认值
            }
            catch (Exception ex)
            {
                return propertyInfo.Name; // 返回属性名作为默认值
            }
            
        }

    }
}
