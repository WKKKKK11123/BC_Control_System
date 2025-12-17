
//using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models;
using NPOI.SS.Formula.Functions;

namespace BC_Control_DAL
{
    public class SqlSugarHelper<TEntity> : ISqlSugarHelper<TEntity> where TEntity : class, new()
    {
        //private readonly SqlSugarScope _dbBase;
        public ISqlSugarClient Db => _db;
        

        private ISqlSugarClient _db;

        public SqlSugarHelper()
        {
            
            string temp = ConfigurationManager.ConnectionStrings["connString"].ToString();
            _db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = temp,
                DbType = SqlSugar.DbType.SqlServer, // 选择数据库类型
                //IsAutoCloseConnection = true, // 自动关闭数据库连接         
            });
            _db.Aop.OnLogExecuting = (sql, pars) =>
            {
                //Console.WriteLine($"执行 SQL: {sql}"); // 输出到控制台
                // 你也可以将其写入到文件或其他日志系统
            };
            _db.Aop.OnError = (sql) => { };

        }
        public SqlSugarHelper(ILogOpration logOpration)
        {
            string temp = ConfigurationManager.ConnectionStrings["connString"].ToString();
            _db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = temp,
                DbType = SqlSugar.DbType.SqlServer, // 选择数据库类型
                //IsAutoCloseConnection = true, // 自动关闭数据库连接         
            });
            _db.Aop.OnLogExecuting = (sql, pars) =>
            {
               
            };
            _db.Aop.OnError = (sql) => { logOpration.WriteError($"执行 SQL: {sql}"); };
            
        }
        public async Task<long> Add(TEntity entity)
        {
            try
            {
                var insert = _db.Insertable(entity);
                return await insert.ExecuteCommandAsync(); // 执行插入
            }
            catch (Exception ex)
            {
                await Task.CompletedTask;
               
                return 0;
            }

        }

        public async Task<long> AddAndReturnId(TEntity entity)
        {
            try
            {
                return await _db.Insertable(entity).ExecuteReturnIdentityAsync();
            }
            catch (Exception ex)
            {
               
                return 0;
            }

        }

        public async Task<List<long>> AddSplit(TEntity entity)
        {
            var insert = _db.Insertable(entity).SplitTable();
            return await insert.ExecuteReturnSnowflakeIdListAsync();
        }
        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">需要更新的实体</param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity entity)
        {
            try
            {
                var update = _db.Updateable(entity);
                return await update.ExecuteCommandAsync() > 0; // 返回受影响的行数
            }
            catch (Exception ex)
            {
               
                return false;
            }

        }

        /// <summary>
        /// 根据条件删除实体数据
        /// </summary>
        /// <param name="whereExpression">删除条件</param>
        /// <returns></returns>
        public async Task<bool> Delete(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await _db.Deleteable<TEntity>().Where(whereExpression).ExecuteCommandAsync() > 0; // 返回受影响的行数
        }
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            try
            {
                List<TEntity> list = await _db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
               
                return default(List<TEntity>);
            }

        }
        public async Task<DataTable> QueryDataTable(Expression<Func<TEntity, bool>> whereExpression)
        {
            try
            {
                return await _db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).ToDataTableAsync();
            }
            catch (Exception ex)
            {
               
                return default(DataTable);
            }

        }
        public Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(Expression<Func<T, T2, T3, object[]>> joinExpression, Expression<Func<T, T2, T3, TResult>> selectExpression, Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public async Task<List<TEntity>> QuerySplit(Expression<Func<TEntity, bool>> whereExpression, string orderByFields = null)
        {

            try
            {
                return await _db.Queryable<TEntity>()
               .SplitTable()
               .OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields)
               .WhereIF(whereExpression != null, whereExpression)
               .ToListAsync();
            }
            catch (Exception ex)
            {
               
                return default;
            }
        }

        public async Task<List<TEntity>> QueryWithCache(Expression<Func<TEntity, bool>> whereExpression = null)
        {
            try
            {
                return await _db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).WithCache().ToListAsync();
            }
            catch (Exception ex)
            {
               
                return default;
            }

        }

        public Task AddColumn(string columnName)
        {
            throw new NotImplementedException();
        }

        public Task DelColumn(string columnName)
        {
            throw new NotImplementedException();
        }

        public Task ModifyColumn(string columnName)
        {
            throw new NotImplementedException();
        }
        // 调用存储过程并返回 DataTable
        public async Task<DataTable> ExecuteStoredProcedure(string procedureName, List<SugarParameter> parameters = null)
        {
            try
            {
                return await _db.Ado.UseStoredProcedure().GetDataTableAsync(procedureName, parameters);
            }
            catch (Exception ex)
            {
               
                throw;
            }
        }

        // 调用存储过程并返回泛型列表
        public async Task<List<TEntity>> ExecuteStoredProcedureGeneric(string procedureName, List<SugarParameter> parameters = null)
        {
            try
            {
                return await _db.Ado.UseStoredProcedure().SqlQueryAsync<TEntity>(procedureName, parameters);
            }
            catch (Exception ex)
            {
               
                throw;
            }
        }
        public async Task<List<TEntity>> QueryView(string viewName)
        {
            try
            {
                return await _db.Queryable<TEntity>().AS(viewName).ToListAsync();

            }
            catch (Exception ex)
            {
               
                throw;
            }
        }

        public async Task<bool> Update(Expression<Func<TEntity, TEntity>> setExpression, Expression<Func<TEntity, bool>> whereExpression)
        {
            try
            {
                var result = await _db.Updateable<TEntity>()
                     .SetColumns(setExpression)
                     .Where(whereExpression)
                     .ExecuteCommandAsync();
                return result > 0;
            }
            catch (Exception ee)
            {
                throw;
            }

        }

        #region 存储过程调用方法

        // 1. 执行无返回值的存储过程
        public async Task<int> ExecuteProcedureNonQueryAsync(string procedureName, params SugarParameter[] parameters)
        {
            try
            {
                return await _db.Ado.ExecuteCommandAsync(procedureName,  parameters);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        // 2. 执行返回单个值的存储过程
        public async Task<object> ExecuteProcedureScalarAsync(string procedureName, params SugarParameter[] parameters)
        {
            try
            {
                return await _db.Ado.GetScalarAsync(procedureName, parameters);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        // 3. 执行返回DataTable的存储过程（你已有的方法）
        public async Task<DataTable> ExecuteProcedureDataTableAsync(string procedureName, params SugarParameter[] parameters)
        {
            try
            {
                return await _db.Ado.UseStoredProcedure().GetDataTableAsync(procedureName, parameters);
            }
            catch (Exception ex)
            {
               
                throw;
            }
        }

        // 4. 执行返回实体列表的存储过程
        public async Task<List<TEntity>> ExecuteProcedureListAsync(string procedureName, params SugarParameter[] parameters)
        {
            try
            {
                return await _db.Ado.UseStoredProcedure().SqlQueryAsync<TEntity>(procedureName, parameters);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        // 5. 执行返回动态类型的存储过程
        public async Task<List<dynamic>> ExecuteProcedureDynamicAsync(string procedureName, params SugarParameter[] parameters)
        {
            try
            {
                return await _db.Ado.UseStoredProcedure().SqlQueryAsync<dynamic>(procedureName, parameters);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        // 6. 执行返回多个结果集的存储过程
        public async Task<List<DataTable>> ExecuteProcedureMultipleResultAsync(string procedureName, params SugarParameter[] parameters)
        {
            try
            {
                var result = new List<DataTable>();
                using (var reader = await _db.Ado.UseStoredProcedure().GetDataReaderAsync(procedureName, parameters))
                {
                    do
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(reader);
                        result.Add(dataTable);
                    }
                    while (reader.NextResult());
                }
                return result;
            }
            catch (Exception ex)
            {
               
                throw;
            }
        }

        #endregion



    }
}
