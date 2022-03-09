using System;
using System.Collections.Generic;
using System.Linq;
using Google.Cloud.Storage.V1;
using System.IO;
using Flurl.Http;
using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;
using Android.Content.Res;

namespace ShikkhanobishStudentApp.ViewModel
{
    public class UploadImage
    {
        private string content;

        public AssetManager Assets { get; private set; }
    

        public object AuthExplicit(string projectId, string jsonPath)
        {
            // Explicitly use service account credentials by specifying
            // the private key file.
            var credential = GoogleCredential.FromJson(jsonPath);
            var storage = StorageClient.Create(credential);
            // Make an authenticated API request.
            var buckets = storage.ListBuckets(projectId);

            return null;
        }

        public async Task<string> UploadFileSample(string bucketName, FileStream fs)
        {
            string link = "";
            try
            {
                var json = await "https://test.shikkhanobish.com/enhanced-emblem-234505-8ede9bde9f07.json".GetStringAsync();
                //System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", json);
                var credential = GoogleCredential.FromJson(json);
                var storage = StorageClient.Create(credential);
                string objectName = generateID();
                using FileStream fileStream = fs;
                storage.UploadObject(bucketName, objectName, null, fileStream);
                link = "https://storage.cloud.google.com/" + bucketName + "/" + objectName;
            }
            catch (Exception ex)
            {
                var gg = ex.Message;
            }
            
            return link;
        }
        public string generateID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
