using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using DiagNexusApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DiagNexusApi.Services
{
    public interface IReportServices
    {
        public Task<Stream?> FetchReportAsync(string key);
        public Task<string> UploadReportAsync(IFormFile fileStream);

    }
    public class ReportServices: IReportServices
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public ReportServices(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
            _bucketName = "We will bucket name here";
        }

        // Fetch a single report from S3 by key (path)
        public async Task<Stream?> FetchReportAsync(string key)
        {
            try
            {
                var response = await _s3Client.GetObjectAsync(_bucketName, key);
                return response.ResponseStream;
            }
            catch (AmazonS3Exception)
            {
                return null;
            }
        }

        // Upload a report to S3
        public async Task<string> UploadReportAsync(IFormFile file)
        {
            var fileTransferUtility = new TransferUtility(_s3Client);

            using (var stream = file.OpenReadStream())
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = stream,
                    Key = file.FileName,
                    BucketName = _bucketName,
                    ContentType = file.ContentType
                };

                await fileTransferUtility.UploadAsync(uploadRequest);
                return "file uploaded successfully";
            }
        }

    }
}
