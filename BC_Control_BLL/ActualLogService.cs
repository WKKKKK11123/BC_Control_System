using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BC_Control_BLL.Oprations;
using BC_Control_BLL.Services;
using BC_Control_Helper;
using BC_Control_Models;
using BC_Control_Models.Log;
using BC_Control_Models.LogVo;

namespace BC_Control_DAL
{
    public static class ActualLogService
    {
        public static async Task SaveActualDataLog<T>(this IBaseService<T> baseService,List<StatusClass> statusClasses) where T : class, IInsertTime, new()
        {
			try
			{
                var list = statusClasses;               
                PropertyInfo[] propertyInfos = typeof(T).GetProperties();
                list.UpdateStatus();
                T actualDataClass = new T();
                actualDataClass.InsertTime = DateTime.Now;
                foreach (var item in list)
                {
                    var propertyInfo = propertyInfos.FirstOrDefault(para =>
                        para.Name == item.ParameterName
                    );
                    if (propertyInfo == null)
                    {
                        continue;
                    }
                    if (item.ActualValue == "")
                    {
                        propertyInfo.SetValue(actualDataClass, "0");
                    }                  
                    else
                    {
                        propertyInfo.SetValue(actualDataClass, item.ActualValue);
                    }
                }
                await baseService.Add(actualDataClass);
                await Task.Delay(50);

            }
			catch (Exception ee)
			{

				throw;
			}
        }
    }
}
