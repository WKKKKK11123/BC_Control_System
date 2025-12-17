using BC_Control_Models;
using SqlSugar;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data;

namespace BC_Control_DAL
{
    public interface ISqlSugarHelper<TEntity> where TEntity : class
    {
        ISqlSugarClient Db { get; }
        Task<long> Add(TEntity entity);
        Task<List<long>> AddSplit(TEntity entity);
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression);
        Task<DataTable> QueryDataTable(Expression<Func<TEntity, bool>> whereExpression);
        Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(Expression<Func<T, T2, T3, object[]>> joinExpression, Expression<Func<T, T2, T3, TResult>> selectExpression, Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new();
        Task<List<TEntity>> QuerySplit(Expression<Func<TEntity, bool>> whereExpression, string orderByFields = null);
        Task<List<TEntity>> QueryWithCache(Expression<Func<TEntity, bool>> whereExpression = null);
        Task<bool> Update(TEntity entity);
        Task<bool> Update(Expression<Func<TEntity, TEntity>> setExpression, Expression<Func<TEntity, bool>> whereExpression);
        Task<bool> Delete(Expression<Func<TEntity, bool>> whereExpression);
        Task AddColumn(string columnName);
        Task DelColumn(string columnName);
        Task ModifyColumn(string columnName);
        Task<DataTable> ExecuteStoredProcedure(string procedureName, List<SugarParameter> parameters = null);
        Task<List<TEntity>> ExecuteStoredProcedureGeneric(string procedureName, List<SugarParameter> parameters = null);
        Task<List<TEntity>> QueryView(string viewName);
        
    }
}
