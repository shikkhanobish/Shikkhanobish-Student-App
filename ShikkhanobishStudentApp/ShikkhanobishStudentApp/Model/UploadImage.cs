using System;
using System.Collections.Generic;
using System.Linq;
using Google.Cloud.Storage.V1;
using System.IO;
using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;

namespace ShikkhanobishStudentApp.Model
{
    public class UploadImage
    {
        // Some APIs, like Storage, accept a credential in their Create()
        // method.
        public object AuthExplicit(string projectId, string jsonPath)
        {
            // Explicitly use service account credentials by specifying
            // the private key file.
            var credential = GoogleCredential.FromFile(jsonPath);
            var storage = StorageClient.Create(credential);
            // Make an authenticated API request.
            var buckets = storage.ListBuckets(projectId);

            return null;
        }

        public string UploadFileSample(string bucketName, FileStream fs)
        {
            AuthExplicit("enhanced-emblem-234505", "wwwroot/GoogleKeyFile/enhanced-emblem-234505-8ede9bde9f07.json");
            string objectName = generateID();
            var storage = StorageClient.Create();
            using FileStream fileStream = fs;
            storage.UploadObject(bucketName, objectName, null, fileStream);
            return "https://storage.cloud.google.com/" + bucketName + "/" + objectName;
        }
        public string generateID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
