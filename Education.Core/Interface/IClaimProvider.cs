using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Interface
{
    public interface IClaimProvider
    {
        Guid GetUserID();
        void SetUserID(string value);
        string GetEmail();
        void SetEmail(string value);
        string GetUserName();
        void SetUserName(string value);
    }
}
