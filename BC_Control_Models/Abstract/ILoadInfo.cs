using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    public interface ILoadInfo
    {
        /// <summary>
        /// 获取工程公共文件路径
        /// </summary>
        /// <returns></returns>
        string GetProjectBoundPath();
        /// <summary>
        /// 获取Recipe路径
        /// </summary>
        string GetRecipeBoundPath();
        /// <summary>
        /// 加载控制器
        /// </summary>
        /// <param name="path"></param>
        void LoadDeviceInfo(string path);       
       List<string> LoadStationInfo();
       
    }
}
