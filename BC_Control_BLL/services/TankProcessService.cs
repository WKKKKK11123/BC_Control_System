using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BC_Control_BLL.Extensions;
using BC_Control_BLL.Oprations;
using BC_Control_DAL;
using BC_Control_Models;
using BC_Control_Models.Log;

namespace BC_Control_BLL.Services
{
    public class TankProcessService : BaseService<TankProcess>
    {
        public TankProcessService(ILogOpration logOpration) : base(new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProFile())).CreateMapper(),
            new SqlSugarHelper<TankProcess>(logOpration))
        { }
    }
}
