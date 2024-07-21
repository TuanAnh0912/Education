using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.DataModel;
using Education.Core.Model.ResponseModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Interface
{
    public interface IUserRepository:IGenericRepository<User>
    {
        Task<User?> CheckLogin(string username, string password);
        Task<object> InitLogin(string userID);
        Task<User> CheckByUserNameAndEmail(string userName);
        Task<PagingResponse> GetPagingUserExamByID(Guid userID, bool isTearcher, int pageSize, int pageIndex);
        Task<Role> GetRoleUserByID(Guid userID);
        //Task<User?> GetUserByRefreshToken(string resfreshToken);
    }
}
