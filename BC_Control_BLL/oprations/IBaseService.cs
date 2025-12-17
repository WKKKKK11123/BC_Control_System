using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_BLL.Oprations
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        Task<long> Add(TEntity entity);
        Task<List<long>> AddSplit(TEntity entity);
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression = null);
        Task<List<TEntity>> QuerySplit(Expression<Func<TEntity, bool>> whereExpression, string orderByFields = null);
        Task<List<TEntity>> QueryWithCache(Expression<Func<TEntity, bool>> whereExpression = null);
    }
}
