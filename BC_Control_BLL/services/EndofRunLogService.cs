using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_BLL.Extensions;
using BC_Control_BLL.Oprations;
using BC_Control_DAL;
using BC_Control_Models.ClassK.SQLService;
using BC_Control_Models.Log;
using BC_Control_Models;

namespace BC_Control_BLL.Services
{
    public class EndofRunLogService : BaseService<EndofRunData>
    {
        public EndofRunLogService(ILogOpration logOpration)
            : base(new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProFile());
            }).CreateMapper(), new SqlSugarHelper<EndofRunData>(logOpration))
        {

        }


    }
}
