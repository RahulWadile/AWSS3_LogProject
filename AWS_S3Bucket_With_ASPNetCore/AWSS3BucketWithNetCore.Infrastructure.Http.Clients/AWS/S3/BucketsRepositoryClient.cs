using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using AWSS3BucketWithNetCore.Infrastructure.Http.Clients.AWS.S3.Models;
using Amazon;
using Amazon.S3.Model;
using System.IO;
using AWSS3BucketWithNetCore.Domain.Models;
using Newtonsoft.Json;

namespace AWSS3BucketWithNetCore.Infrastructure.Http.Clients.AWS.S3
{
    public class BucketsRepositoryClient
    {
        private readonly IAmazonS3 _clientAmazonS3;
        private readonly AWSS3Bucket _settings;
        public BucketsRepositoryClient(IOptions<AWSS3Bucket> settings)
        {
            _settings = settings.Value;
            _clientAmazonS3 = new AmazonS3Client(_settings.configuration.AccessKey, _settings.configuration.SecretKey, RegionEndpoint.USEast2);
        }

        public async Task<bool> UploadFileAsync(System.IO.Stream inputStream, string fileName)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest()
                {
                    InputStream = inputStream,
                    BucketName = _settings.configuration.BucketName,
                    Key = fileName
                };
                PutObjectResponse response = await _clientAmazonS3.PutObjectAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // Get all file collection from s3 bucket
        public async Task<ListVersionsResponse> GetFilesListAsync()
        {
            return await _clientAmazonS3.ListVersionsAsync(_settings.configuration.BucketName);
        }

        // Get file as per file name form s3 bucket
        public async Task<Stream> GetFileAsync(string key)
        {
            try
            {
                GetObjectResponse response = await _clientAmazonS3.GetObjectAsync(_settings.configuration.BucketName, key);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    return response.ResponseStream;
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Delete file as per filename from s3 bucket
        public async Task<bool> DeleteFileAsync(string key)
        {
            try
            {
                DeleteObjectResponse response = await _clientAmazonS3.DeleteObjectAsync(_settings.configuration.BucketName, key);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Check file already exist or not from s3 bucket
        public async Task<bool> GetFileExist(string key)
        {
            try
            {
                GetObjectResponse response = await _clientAmazonS3.GetObjectAsync(_settings.configuration.BucketName, key);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        // Write log as per day wise basic and store in s3 bucket
        public async Task<bool> UpdateLogFile(string fileName, string logMst)
        {
            try
            {
                StringBuilder objStringBuilder = null;
                LogMsg objLog = null;
                string txtContent = string.Empty;

                GetObjectResponse response = await _clientAmazonS3.GetObjectAsync(_settings.configuration.BucketName, fileName);
                StreamReader reader = new StreamReader(response.ResponseStream);

                txtContent = reader.ReadToEnd();
                objStringBuilder = new StringBuilder();
                objStringBuilder.Append(txtContent);

                //objLog = new LogMsg();

                objLog = new LogMsg
                {
                    LogMessage = logMst,
                    CreatedDateTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss")
                };

                string serializedval = JsonConvert.SerializeObject(objLog);

                objStringBuilder.Append(serializedval);

                byte[] byteArray = Encoding.ASCII.GetBytes(Convert.ToString(objStringBuilder));
                var seekableStream = new MemoryStream(byteArray)
                {
                    Position = 50
                };
                return await UploadFileAsync(seekableStream, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
