using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class TableEducationAttribute: Attribute
    {
        public string TableName { get; }
        public string PermissionName { get; }

        public TableEducationAttribute(string tableName, string permissionName)
        {
            TableName = tableName;
            PermissionName = permissionName;    
        }
        public TableEducationAttribute(string tableName)
        {
            TableName = tableName;
            PermissionName = "";
        }
    }
}
