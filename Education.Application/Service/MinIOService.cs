using Amazon.S3;
using Amazon.S3.Model;
using Education.Application.Interface;
using Education.Core.Model.AWS_S3;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Education.Application.Service
{
    public class MinIOService : IStorageService
    {
        private IAmazonS3 _s3Client;
        private AwsCredentials _awsCredentials;
        public MinIOService(IAmazonS3 s3Client, IOptions<AwsCredentials> awsCredentials)
        {
            _s3Client = s3Client;
            _awsCredentials = awsCredentials.Value;
        }
        protected virtual IAmazonS3 GetServiceAWS(AwsCredentials awsConfig)
        {
            var config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast1,
                ServiceURL = awsConfig.ServiceURL,
            };
            return new AmazonS3Client(awsConfig.AwsKey, awsConfig.AwsSecretKey, config);
        }
        public async Task SaveFile(string name, Stream content)
        {
            var contentType = GetContentType(name);
            var requestParam = new PutObjectRequest()
            {
                BucketName = "test",
                Key = name,
                InputStream = content,
                ContentType = (string)contentType
            };
            var rs = await _s3Client.PutObjectAsync(requestParam);
        }
        public async Task<MemoryStream> GetAsync(string name)
        {
            var request = new GetObjectRequest()
            {
                BucketName = "test",
                Key = name
            };
            MemoryStream data = null;
            data = await GetFile(request);
            return data;
        }

        public async Task<MemoryStream?> GetFile(GetObjectRequest request)
        {
            var rs = await _s3Client.GetObjectAsync(request);
            if(rs.HttpStatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            MemoryStream meStream = new MemoryStream();
            rs.ResponseStream.CopyTo(meStream);
            rs.ResponseStream.Dispose();
            return meStream;
        }

        private object GetContentType(string name)
        {
            var extention = Path.GetExtension(name).ToLower();
            var type = "";
            switch (extention)
            {
                case ".xls":
                case ".xlsx":
                    type = "application/vnd.ms-excel";
                    break;
                case ".doc":
                case ".docx":
                    type = "application/msword";
                    break;
                case ".pdf":
                    type = "application/msword";
                    break; 
                case ".zip":
                    type = "application/x-zip-compressed";
                    break;
                case ".png":
                    type = "image/png";
                    break; 
                case ".gif":
                    type = "image/gif";
                    break;
                default:
                    break;
            }
            return type;
        }
    }
}
