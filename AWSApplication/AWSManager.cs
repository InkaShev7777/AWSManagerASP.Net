using System;
using System.Security.AccessControl;
using System.Xml.Linq;
using Amazon.S3;
using Amazon.S3.Model;
using AWSApplication.Model;

namespace AWSApplication
{
	public class AWSManager
	{
		public string accessKey { get; private set; }
		public string secretKey { get; private set; }
		AmazonS3Client s3Client;

        public AWSManager()
		{
			this.accessKey = "AKIASGPBJ3IHQHE3MKNF";
			this.secretKey = "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";
			this.s3Client = new AmazonS3Client(accessKey,secretKey,Amazon.RegionEndpoint.EUWest2);
		}
        public async Task<IResult> Download(string nameFile)
        {
            var request = new GetObjectRequest
            {
                BucketName = "inkashevbucket",
                Key = nameFile,
            };
            using GetObjectResponse response = await this.s3Client.GetObjectAsync(request);
            try
            {
                await response.WriteResponseStreamToFileAsync($"/Users/ilyaschevchenko/Desktop/TestAWS/{nameFile}", true, System.Threading.CancellationToken.None);
                return Results.Ok();
            }
            catch (AmazonS3Exception ex)
            {
                return Results.NotFound(ex.Message);
            }
        }
        public async Task<IResult> Upload(IFormFile file)
        {
            await using var memoryStr = new MemoryStream();
            await file.CopyToAsync(memoryStr);
            var fileextention = Path.GetExtension(file.FileName);
            string name = $"{Guid.NewGuid()}.{fileextention}";

            var request = new PutObjectRequest
            {
                BucketName = "inkashevbucket",
                Key = file.FileName,
                InputStream = memoryStr
            };
            try
            {
                var response = await this.s3Client.PutObjectAsync(request);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
        public async Task<IResult> Delete(string nameFile)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = "inkashevbucket",
                    Key = nameFile
                }; Console.WriteLine("Deleting an object");
                await this.s3Client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception e)
            {
                return Results.Problem();
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
            return Results.Ok();
        }
        public async Task<List<FileModel>> List()
        {
            List<FileModel> models = new List<FileModel>();
            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = "inkashevbucket",
                    MaxKeys = 5,
                };
                var response = new ListObjectsV2Response();
                do
                {
                    response = await this.s3Client.ListObjectsV2Async(request);
                    response.S3Objects.ForEach(obj => models.Add(new FileModel(obj.Key, obj.LastModified.ToString(), obj.Size.ToString())));
                    request.ContinuationToken = response.NextContinuationToken;
                }
                while (response.IsTruncated);
                return models;
            }
            catch (AmazonS3Exception ex)
            {
                return models;
            }
        }
    }
}

