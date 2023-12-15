using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.ResponseModel
{
    public class LoginResponse
    {
        public LoginResponse(bool success, string message, string? refreshToken = null, string toKen = null)
        {
            Success = success;
            Message = message;
            RefreshToken = refreshToken;
            ToKen = toKen;
        }
        public string ToKen { get; set; }
        public bool Success { get; set; }
        public string? RefreshToken { get; set; }
        public string? Message { get; set; }
    }
}
