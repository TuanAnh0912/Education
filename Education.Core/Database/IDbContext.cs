using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Database
{
    public interface IDbContext<T> where T:class
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<bool> Add(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Update(T entity);
        IDbTransaction GetDbTransaction();
        IDbConnection CreateConnection();
        Task<IEnumerable<X>> QueryUsingStore<X>(Dictionary<string, object> dicParams, string storeName, IDbTransaction? transaction = null, CommandType commandType = CommandType.StoredProcedure);
        Task<IEnumerable<T>> QueryUsingStore(Dictionary<string, object> dicParams, string storeName, IDbTransaction? transaction = null, CommandType commandType = CommandType.StoredProcedure);
        Task<int> ExcuseUsingStore(Dictionary<string, object> dicParams, string storeName, IDbTransaction? transaction = null, CommandType commandType = CommandType.StoredProcedure);
        Task<object> ExecuteScalarUsingStore(Dictionary<string, object> dicParams, string storeName, IDbTransaction? transaction = null, CommandType commandType = CommandType.StoredProcedure);
    }
}
