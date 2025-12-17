using AutoMapper;
using BC_Control_BLL.Extensions;
using BC_Control_BLL.Oprations;
using BC_Control_DAL;
using BC_Control_Models;
using BC_Control_Models.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_BLL.services.TraceLogService
{
    public class TK7LogDataService : BaseService<TK7DataLogClass>
    {
        public TK7LogDataService(ILogOpration logOpration) : base(new MapperConfiguration(config =>
        {
            config.AddProfile(new AutoMapperProFile());
        }).CreateMapper(), new SqlSugarHelper<TK7DataLogClass>(logOpration))
        {
        
        }
    }
}
