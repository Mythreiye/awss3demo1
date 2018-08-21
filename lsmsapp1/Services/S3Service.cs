using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using lsmsapp1.Models;

namespace lsmsapp1.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _client;

        public S3Service(IAmazonS3 client)
        {
            _client = client;
        }

        public async Task<S3Response> CreateBucketAsync(string bucketName)
        {
            try
            {
                if(await AmazonS3Util.DoesS3BucketExistAsync(_client, bucketName) == false)
                {
                    var putBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };

                    var response = await _client.PutBucketAsync(putBucketRequest);

                    return new S3Response
                    {
                        Message = response.ResponseMetadata.RequestId,
                        status = response.HttpStatusCode
                    };
                }
            }
            catch(AmazonS3Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    status = e.StatusCode
                };
            }
            catch (Exception e)
            {
                return new S3Response
                {
                    status = HttpStatusCode.InternalServerError,
                    Message = e.Message
                };
            }

            return new S3Response
            {
                Message = "Internal error",
                status = HttpStatusCode.InternalServerError
            };
        }

        private const string FilePath = "C:\\Users\\mythr.krishnamoorthy\\holidays\\Calendario 2018 (003).pdf";
        private const string UploadWithKeyName = "UploadWithKeyName";
        private const string FileStreamUpload = "FileStreamUpload";
        private const string AdvancedUpload = "AdvancedUpload";

        public async Task UploadFileAsync(string bucketName)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_client);

                await fileTransferUtility.UploadAsync(FilePath, bucketName);

                await fileTransferUtility.UploadAsync(FilePath, bucketName, UploadWithKeyName);

                using (var fileToUpload = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    await fileTransferUtility.UploadAsync(fileToUpload, bucketName, FileStreamUpload);
                }

                var fileTransferUtilityRequest = new TransferUtilityUploadRequest()
                {
                    BucketName = bucketName,
                    FilePath = FilePath,
                    StorageClass = S3StorageClass.Standard,
                    PartSize = 6291456,
                    Key = AdvancedUpload,
                    CannedACL = S3CannedACL.NoACL
                };

                fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
                fileTransferUtilityRequest.Metadata.Add("param2", "Value2");

                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);

            }
            catch(AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
          
        }

        public async Task GetObjectFromS3Async(string bucketName)
        {
            const string keyName = "Calendario 2018 (003).pdf";

            try
            {
                var request = new Amazon.S3.Model.GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };

                string responseBody;

                using (var response = await _client.GetObjectAsync(request))
                using (var responseStream = response.ResponseStream)
                using (var reader = new StreamReader(responseStream))
                {
                    responseBody = reader.ReadToEnd();
                }

                var pathAndFileName = $"C:\\S3Temp\\{keyName}";

                var createText = responseBody;

                File.WriteAllText(pathAndFileName, createText);

            }
            catch(AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when reading an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when reading an object", e.Message);
            }
        }


    }

}
