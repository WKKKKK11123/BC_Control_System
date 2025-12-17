using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BC_Control_Models.Log;

namespace BC_Control_BLL.Extensions
{
    public class AutoMapperProFile : MapperConfigurationExpression
    {
        public AutoMapperProFile()
        {
            CreateMap<LPDLogVo, LPDLog>()
            .ForMember(dest => dest.InsertTime, opt =>
            {
                opt.Ignore();
            })
            .ForAllOtherMembers(opt =>
            {
                opt.MapFrom(src => src.GetType().GetProperty(opt.DestinationMember.Name).GetValue(src, null)
                                   .GetType().GetProperty("Value")
                                   .GetValue(src.GetType().GetProperty(opt.DestinationMember.Name).GetValue(src, null), null)
                );
            });
            

        }
    }
}
