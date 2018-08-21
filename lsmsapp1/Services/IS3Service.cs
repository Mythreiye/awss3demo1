using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lsmsapp1.Models;

namespace lsmsapp1.Services
{
    public interface IS3Service
    {
        Task<S3Response> CreateBucketAsync(string bucketName);

        Task UploadFileAsync(string bucketName);

        Task GetObjectFromS3Async(string bucketName);
    }
}
