using Education.Core.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Repositories
{
    public class ClaimProvider:IClaimProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimProvider(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        public Guid GetUserID()
        {
            return Guid.Parse(_httpContextAccessor.HttpContext.Request.Headers["UserID"].ToString());
        }

        public void SetUserID(string value)
        {
            _httpContextAccessor.HttpContext.Request.Headers["UserID"] = value;
        }
        public string GetEmail()
        {
            return _httpContextAccessor.HttpContext.Request.Headers["Email"].ToString();
        }
        public void SetEmail(string value)
        {
            _httpContextAccessor.HttpContext.Request.Headers["Email"] = value;
        }
        public string GetUserName()
        {
            return _httpContextAccessor.HttpContext.Request.Headers["UserName"].ToString();
        }
        public void SetUserName(string value)
        {
            _httpContextAccessor.HttpContext.Request.Headers["UserName"] = value;
        }
    }
}
