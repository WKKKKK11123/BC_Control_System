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

namespace BC_Control_BLL.services
{
    public class TK10LogDataService : BaseService<TK10DataLogClass>
    {
        public TK10LogDataService(ILogOpration logOpration) : base(new MapperConfiguration(config =>
        {
            config.AddProfile(new AutoMapperProFile());
        }).CreateMapper(), new SqlSugarHelper<TK10DataLogClass>(logOpration))
        { 
        
        }
    }
}
