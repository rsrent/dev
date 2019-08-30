/*

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace ModuleLibraryiOS.Storage
{
    public class AmazonStorage
    {
        //string bucketName = "";

        private readonly AmazonCredentials Credentials;

        public AmazonStorage(AmazonCredentials credentials)
        {
            Credentials = credentials;
        }

        public async Task<string[]> multipartUpload(string bucketName, string filePath, string keyName)
        {

            IAmazonS3 s3Client = new AmazonS3Client(Credentials.AccessKeyId, Credentials.SecretAccessKey, Credentials.SessionToken, Amazon.RegionEndpoint.EUCentral1);

            //string keyName = AppDelegate.model.signedInUser._id + "/" + up.upload._id + ".mov";

            // List to store upload part responses.
            List<UploadPartResponse> uploadResponses = new List<UploadPartResponse>();

            // 1. Initialize.
            InitiateMultipartUploadRequest initiateRequest = new InitiateMultipartUploadRequest
            {
                BucketName = bucketName,
                Key = keyName
            };
            InitiateMultipartUploadResponse initResponse = null;
            try
            {
                initResponse = await s3Client.InitiateMultipartUploadAsync(initiateRequest);
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
            }


            // 2. Upload Parts.
            long contentLength = new FileInfo(filePath).Length;
            long partSize = 5 * (long)Math.Pow(2, 20); // 5 MB

            try
            {
                long filePosition = 0;
                for (int i = 1; filePosition < contentLength; i++)
                {
                    UploadPartRequest uploadRequest = new UploadPartRequest
                    {
                        BucketName = bucketName,
                        Key = keyName,
                        UploadId = initResponse.UploadId,
                        PartNumber = i,
                        PartSize = partSize,
                        FilePosition = filePosition,
                        FilePath = filePath
                    };

                    // Upload part and add response to our list.
                    uploadResponses.Add(await s3Client.UploadPartAsync(uploadRequest));

                    filePosition += partSize;
                }

                // Step 3: complete.
                CompleteMultipartUploadRequest completeRequest = new CompleteMultipartUploadRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    UploadId = initResponse.UploadId
                };
                completeRequest.AddPartETags(uploadResponses);

                CompleteMultipartUploadResponse completeUploadResponse =
                    await s3Client.CompleteMultipartUploadAsync(completeRequest);

                return new string[] { completeUploadResponse.Location, completeUploadResponse.Key };
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception occurred: {0}", exception.Message);
                AbortMultipartUploadRequest abortMPURequest = new AbortMultipartUploadRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    UploadId = initResponse.UploadId
                };
                await s3Client.AbortMultipartUploadAsync(abortMPURequest);
                return null;
            }
            return null;
        }
    }
}


*/