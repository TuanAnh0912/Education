using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.Core
{
    public class BaseModel : ICloneable
    {
        public object Clone()
        {
            throw new NotImplementedException();
        }
        public string GetTableName()
        {
            var classAttribute = (TableEducationAttribute)GetType().GetCustomAttributes(typeof(TableEducationAttribute), false).FirstOrDefault();
            return classAttribute?.TableName ?? "";
        }
        public string GetPrimaryKey()
        {
            return this.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(KeyAttribute)))?.FirstOrDefault()?.Name ?? "";
        }
        public Type GetTypeOfPriamry()
        {
            return this.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(KeyAttribute)))?.FirstOrDefault()?.PropertyType;
        }
        public bool ContainProperty(string property)
        {
            return this.GetType().GetProperty(property) != null;
        }
        public object GetValue(string property, object entity)
        {
            PropertyInfo infor = this.GetType().GetProperty(property);
            if (infor != null)
            {
                var value = infor.GetValue(entity);
                if (value != null)
                {
                    return value;
                }
            }
            return null;
        }

        //public DateTime? CreatedDate { get; set; }
        //public string CreatedBy { get; set; }
    }
}
