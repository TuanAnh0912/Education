using Education.Core.Model.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Application.Interface
{
    public interface IAuthenService
    {
        Task<LoginResponse> LoginAsync(string username, string password);
       // Task<string> GetTokenByRefreshToken(string rfToken);
    }
}
