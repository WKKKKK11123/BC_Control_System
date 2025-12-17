using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_BLL.Extensions;
using BC_Control_BLL.Oprations;
using BC_Control_DAL;
using BC_Control_Models;
using BC_Control_Models.Log;

namespace BC_Control_BLL.Services
{
    public class SysAdminService : BaseService<SysAdmin>
    {
        public SysAdminService(ILogOpration logOpration) : base(new MapperConfiguration(configure => new AutoMapperProFile()).CreateMapper(), new SqlSugarHelper<SysAdmin>(logOpration))
        {
        }

        // 封装添加方法
        public async Task<bool> Insert(SysAdmin admin)
        {
            var result = await Add(admin); // 调用 BaseService.Add()
            return result > 0;
        }

        // 封装更新方法
        public async Task<bool> Update(SysAdmin admin)
        {
            return await UpdateEntity(admin); // 调用 BaseService.UpdateEntity()
        }

        // 获取全部管理员
        public async Task<List<SysAdmin>> GetList()
        {
            return await GetAll();
        }

        // 删除用户
        public async Task<bool> Delete(SysAdmin admin)
        {
            return await Delete(x => x.LoginId == admin.LoginId);
        }
    }
}
