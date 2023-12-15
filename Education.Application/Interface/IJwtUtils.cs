using Education.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Application.Interface
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(User user);
        public Task<string> GenerateRefreshToken();
        string GenerateResetLink();
    }
}
