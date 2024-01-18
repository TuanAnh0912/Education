using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Core.Model.AWS_S3
{
    public class AwsCredentials
    {
        public string AwsKey { get; set; }
        public string AwsSecretKey { get; set; }
        public string ServiceURL { get; set; }
    }
}
