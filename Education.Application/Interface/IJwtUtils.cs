using Education.Core.Model;
using Education.Core.Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Application.Interface
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(UserDto user);
        public Task<string> GenerateRefreshToken();
        string GenerateResetLink();
    }
}
