using Dapper;
using Education.Core.Model;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Database
{
    public class MySQLContext<T> : IDbContext<T> where T : class
    {
        private DbSettings _dbSettings;
        public MySQLContext(IOptions<DbSettings> dbsetting)
        {
            _dbSettings = dbsetting.Value;
        }
        public Task<bool> Add(T entity)
        {
            throw new NotImplementedException();
        }

        public IDbConnection CreateConnection()
        {
            var connectionString = _dbSettings.ConnectionString;
            return new MySqlConnection(connectionString);
        }
        public Task<bool> Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> ExcuseUsingStore(Dictionary<string, object> dicParams, string storeName, IDbTransaction? transaction = null, CommandType commandType = CommandType.StoredProcedure)
        {
            if (string.IsNullOrEmpty(storeName))
            {
                throw (new Exception("Không tồn tại store"));
            }
            dicParams ??= new Dictionary<string, object>();
            if (transaction != null)
            {
                var rs = await transaction.Connection.ExecuteAsync(storeName, param: dicParams, commandType: commandType, transaction: transaction);
                return rs;
            }
            using (var db = CreateConnection())
            {
                var rs = await db.ExecuteAsync(storeName, param: dicParams, commandType: commandType, transaction: transaction);
                return rs;
            }
        }

        public async Task<object> ExecuteScalarUsingStore(Dictionary<string, object> dicParams, string storeName, IDbTransaction? transaction = null, CommandType commandType = CommandType.StoredProcedure)
        {
            if (string.IsNullOrEmpty(storeName))
            {
                throw (new Exception("Không tồn tại store"));
            }
            dicParams ??= new Dictionary<string, object>();
            if (transaction != null)
            {
                var rs = await transaction.Connection.ExecuteScalarAsync(storeName, param: dicParams, commandType: commandType, transaction: transaction);
                return rs;
            }
            using (var db = CreateConnection())
            {
                var rs = await db.ExecuteScalarAsync(storeName, param: dicParams, commandType: commandType, transaction: transaction);
                return rs;
            }
        }

        public Task<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IDbTransaction GetDbTransaction()
        {
            var db = CreateConnection();
            db.Open();
            return db.BeginTransaction();
        }

        public async Task<IEnumerable<T>> QueryUsingStore(Dictionary<string, object> dicParams, string storeName, IDbTransaction? transaction = null, CommandType commandType = CommandType.StoredProcedure)
        {
            if (string.IsNullOrEmpty(storeName))
            {
                throw (new Exception("Không tồn tại store"));
            }
            dicParams ??= new Dictionary<string, object>();
            if (transaction != null)
            {
                return await transaction.Connection.QueryAsync<T>(storeName, param: dicParams, commandType: commandType, transaction: transaction);
            }
            using (var db = CreateConnection())
            {
                return await db.QueryAsync<T>(storeName, param: dicParams, commandType: commandType, transaction: transaction);
            }
        }
        public async Task<IEnumerable<X>> QueryUsingStore<X>(Dictionary<string, object> dicParams, string storeName, IDbTransaction? transaction = null, CommandType commandType = CommandType.StoredProcedure)
        {
            if (string.IsNullOrEmpty(storeName))
            {
                throw (new Exception("Không tồn tại store"));
            }
            dicParams ??= new Dictionary<string, object>();
            if (transaction != null)
            {
                return await transaction.Connection.QueryAsync<X>(storeName, param: dicParams, commandType: commandType, transaction: transaction);
            }
            using (var db = CreateConnection())
            {
                return await db.QueryAsync<X>(storeName, param: dicParams, commandType: commandType, transaction: transaction);
            }
        }

        public Task<bool> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
