using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BC_Control_DAL;

namespace BC_Control_BLL.Oprations
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
        private readonly IMapper _mapper;
        private readonly ISqlSugarHelper<TEntity> _baseRepository;
        public BaseService(IMapper mapper, ISqlSugarHelper<TEntity> baseRepository)
        {
            _mapper = mapper;
            _baseRepository = baseRepository;
        }
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression = null)
        {
            var entities = await _baseRepository.Query(whereExpression);
            //var llout = _mapper.Map<List<TVo>>(entities);
            return entities;
        }
        public async Task<bool> UpdateEntity(TEntity entity)
        {
            // 假设 _baseRepository.Update 返回受影响的行数
            var rows = await _baseRepository.Update(entity);
            return rows;
        }
        public async Task<bool> UpdateEntity(Expression<Func<TEntity, TEntity>> setExpression, Expression<Func<TEntity, bool>> whereExpression)
        {
            // 更新满足 whereExpression 条件的记录，根据 setExpression 设置的字段进行更新
            var rows = await _baseRepository.Update(setExpression, whereExpression);
            return rows;
        }
        public async Task<List<TEntity>> QueryWithCache(Expression<Func<TEntity, bool>> whereExpression = null)
        {
            var entities = await _baseRepository.QueryWithCache(whereExpression);
            //var llout = _mapper.Map<List<TVo>>(entities);
            return entities;
        }

        public async Task<long> Add(TEntity entity)
        {
            return await _baseRepository.Add(entity);
        }
            
        public async Task<List<TEntity>> QuerySplit(Expression<Func<TEntity, bool>> whereExpression, string orderByFields = null)
        {
            return await _baseRepository.QuerySplit(whereExpression, orderByFields);
        }

        public async Task<List<long>> AddSplit(TEntity entity)
        {
            return await _baseRepository.AddSplit(entity);
        }

        public async Task<TEntity> GetFirst(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await _baseRepository.Db.Queryable<TEntity>()
                .Where(whereExpression)
                .FirstAsync(); // or FirstOrDefaultAsync()
        }

        public async Task<int> AddReturnId(TEntity entity)
        {
            return await _baseRepository.Db.Insertable(entity).ExecuteReturnIdentityAsync();
        }
        public async Task<bool> Delete(Expression<Func<TEntity, bool>> whereExpression)
        {
            var result = await _baseRepository.Db.Deleteable<TEntity>()
                            .Where(whereExpression)
                            .ExecuteCommandAsync();
            return result > 0;
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _baseRepository.Db.Queryable<TEntity>().ToListAsync();
        }
    }
}
