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

        public async Task<ListVersionsResponse> GetFilesListAsync()
        {
            return await _clientAmazonS3.ListVersionsAsync(_settings.configuration.BucketName);
        }

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
        public async Task<bool> UpdateLogFile(string fileName, string logMst)
        {
            try
            {

                GetObjectResponse response = await _clientAmazonS3.GetObjectAsync(_settings.configuration.BucketName, fileName);
                StreamReader reader = new StreamReader(response.ResponseStream);

                StringBuilder stringBuilder = new StringBuilder();


                string content = reader.ReadToEnd();


                StringBuilder stringBuild = new StringBuilder();
                stringBuild.Append(content);

                LogMsg objLog = new LogMsg();

                objLog.LogMessage = logMst;
                objLog.CreatedDateTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");

                //  dese.Add(objLog);

                string serializedval = JsonConvert.SerializeObject(objLog);

                stringBuild.Append(serializedval);
                //List<LogMsg> dese = JsonConvert.DeserializeObject<List<LogMsg>>(content);

                //LogMsg objLog = new LogMsg();

                //objLog.LogMsgstring = logMst;
                //objLog.CreatedDateTime = DateTime.Now.ToString("yyyy:MM:dd:HH:mm:ss");

                //dese.Add(objLog);

                //string serializedval = JsonConvert.SerializeObject(dese);

                byte[] byteArray = Encoding.ASCII.GetBytes(Convert.ToString(stringBuild));
                var seekableStream = new MemoryStream(byteArray);
                seekableStream.Position = 50;

                return await UploadFileAsync(seekableStream, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
