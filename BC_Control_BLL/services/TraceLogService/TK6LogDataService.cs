using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_BLL.Extensions;
using BC_Control_BLL.Oprations;
using BC_Control_DAL;
using BC_Control_Models.Log;
using BC_Control_Models;

namespace BC_Control_BLL.services.TraceLogService
{
    public class TK6LogDataService : BaseService<TK6DataLogClass>
    {
        public TK6LogDataService(ILogOpration logOpration) : base(new MapperConfiguration(config =>
        {
            config.AddProfile(new AutoMapperProFile());
        }).CreateMapper(), new SqlSugarHelper<TK6DataLogClass>(logOpration))
        {

        }
    }
}
