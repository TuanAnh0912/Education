using Education.Core.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Application.Interface
{
    public interface IBaseService<T> where T:class
    {
        Task<ServiceResponse> GetById(object ID);
        Task<ServiceResponse> GetAll();
        Task<ServiceResponse> Add(T entity);
        Task<ServiceResponse> Delete(object ID);
        Task<ServiceResponse> Update(T entity);
        Task<bool> CheckRoleAccess(List<string> listRole);
    }
}
