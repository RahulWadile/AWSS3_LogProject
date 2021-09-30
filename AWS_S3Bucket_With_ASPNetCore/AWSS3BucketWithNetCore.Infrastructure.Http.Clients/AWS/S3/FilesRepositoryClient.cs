using Amazon.S3.Model;
using AWSS3BucketWithNetCore.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSS3BucketWithNetCore.Infrastructure.Http.Clients.AWS.S3
{
    public class FilesRepositoryClient
    {
        private readonly BucketsRepositoryClient _bucketsRepositoryClient;

        public FilesRepositoryClient(BucketsRepositoryClient bucketsRepositoryClient)
        {
            _bucketsRepositoryClient = bucketsRepositoryClient;
        }

        public async Task<bool> UploadFileAsync(int uploadFileName, string jsonData)
        {
            try
            {
                string fileName = string.Empty;
                byte[] byteArray = Encoding.ASCII.GetBytes(jsonData);
                var seekableStream = new MemoryStream(byteArray);
                seekableStream.Position = 0;

                fileName = $"{uploadFileName}.json";
                return await _bucketsRepositoryClient.UploadFileAsync(seekableStream, fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string>> GetFilesListAsync()
        {
            try
            {
                ListVersionsResponse listVersions = await _bucketsRepositoryClient.GetFilesListAsync();
                return listVersions.Versions.Select(c => c.Key).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Stream> GetFileAsync(string key)
        {
            try
            {
                
                Stream fileStream = await _bucketsRepositoryClient.GetFileAsync(key);
                if (fileStream == null)
                {
                    Exception ex = new Exception("File Not Found");
                    throw ex;
                }
                else
                {
                    return fileStream;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateFileAsync(int uploadFileName, string jsonData)
        {
            try
            {
                string fileName = string.Empty;
                byte[] byteArray = Encoding.ASCII.GetBytes(jsonData);
                var seekableStream = new MemoryStream(byteArray);
                seekableStream.Position = 0;

                fileName = $"{uploadFileName}.json";
                return await _bucketsRepositoryClient.UploadFileAsync(seekableStream, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteFileAsync(string key)
        {
            try
            {
                return await _bucketsRepositoryClient.DeleteFileAsync(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> WriteLog(string logMsg)
        {
            string logfileName = DateTime.Today.ToString("ddMMyyyy") + "_logtest.txt";
            bool isfileExist = await _bucketsRepositoryClient.GetFileExist(logfileName);
            if (!isfileExist)
            {
                //if file not exist a day. create new file

                LogMsg objLog = new LogMsg();

                objLog.LogMessage = logMsg;
                objLog.CreatedDateTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");

                string serializedval = JsonConvert.SerializeObject(objLog);
                byte[] byteArray = Encoding.ASCII.GetBytes(serializedval);
                var seekableStream = new MemoryStream(byteArray);
                seekableStream.Position = 0;

                return await _bucketsRepositoryClient.UploadFileAsync(seekableStream, logfileName);

            }
            else
            {             

                return await _bucketsRepositoryClient.UpdateLogFile(logfileName,logMsg);
               
            }
        }
    }
}
