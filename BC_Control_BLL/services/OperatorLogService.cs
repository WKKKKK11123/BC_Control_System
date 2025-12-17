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
using BC_Control_Models.ClassK.SQLService;

namespace BC_Control_BLL.Services
{
    public class OperatorLogService:BaseService<OperatorLog>
    {
        public OperatorLogService(ILogOpration logOpration) : base (new MapperConfiguration(config =>
        {
            config.AddProfile(new AutoMapperProFile());
        }).CreateMapper(), new SqlSugarHelper<OperatorLog>()) { }

        public async Task Add(EndofRunData endofRunData)
        {
            throw new NotImplementedException();
        }
    }
}
