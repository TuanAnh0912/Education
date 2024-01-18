using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Interface
{
    public interface IGenericRepository<T> where T: class
    {
        Task<T> GetById(object ID);
        Task<IEnumerable<T>> GetAll();
        Task<object> Add(T entity);
        Task<bool> Delete(object ID);
        Task<bool> Update(T entity);
        Task<bool> UpdateCustomColumn(T entity, List<string> columnsToUpdate, string condition, IDbTransaction transaction = null);
        string GetTableName();
        Task<object> MultiInsert(List<BaseModel> data, bool selectKey,IDbTransaction dbTransaction = null);
    }
}
