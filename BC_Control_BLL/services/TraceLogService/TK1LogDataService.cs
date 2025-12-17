using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BC_Control_BLL.Extensions;
using BC_Control_BLL.Oprations;
using BC_Control_DAL;
using BC_Control_Models;
using BC_Control_Models.Log;

namespace BC_Control_BLL.services.TraceLogService
{
    public class TK1LogDataService : BaseService<TK1DataLogClass>
    {
        public TK1LogDataService(ILogOpration logOpration) : base(new MapperConfiguration(config =>
        {
            config.AddProfile(new AutoMapperProFile());
        }).CreateMapper(), new SqlSugarHelper<TK1DataLogClass>(logOpration))
        {

        }
       
    }
}
