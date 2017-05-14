using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace FaceAggregator.Services
{
    public class UploadService : IUploadService
    {
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient _blobClient;

        public UploadService()
        {
            _storageAccount = CloudStorageAccount.Parse(WebConfigurationManager.AppSettings["StorageConnectionString"]);
            _blobClient = _storageAccount.CreateCloudBlobClient();
        }
        
        public async Task<ICollection<Uri>> GetAllPhotos(string containerName)
        {
            
            CloudBlobContainer blobContainer = _blobClient.GetContainerReference(containerName);
            await blobContainer.CreateIfNotExistsAsync();

            await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            List<Uri> allBlobs = new List<Uri>();
            foreach (IListBlobItem blob in blobContainer.ListBlobs())
            {
                if (blob.GetType() == typeof(CloudBlockBlob))
                    allBlobs.Add(blob.Uri);
            }

            return allBlobs;
        }

        public async Task UploadAsync(HttpFileCollectionBase files, string containerName)
        {
            int fileCount = files.Count;
            CloudBlobContainer blobContainer = _blobClient.GetContainerReference(containerName);
            if (fileCount > 0)
            {
                for (int i = 0; i < fileCount; i++)
                {
                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(GetRandomBlobName(files[i].FileName));
                    blob.Properties.ContentType = files[i].ContentType;
                    await blob.UploadFromStreamAsync(files[i].InputStream);
                }
            }
        }

        private string GetRandomBlobName(string filename)
        {
            string ext = Path.GetExtension(filename);
            return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
        }

        

        public async Task DeleteImage(string filename, string containerName)
        {
            CloudBlobContainer blobContainer = _blobClient.GetContainerReference(containerName);
            var blob = blobContainer.GetBlockBlobReference(filename);
            await blob.DeleteIfExistsAsync();
        }

        public async Task DeleteAllImages(string containerName)
        {
            CloudBlobContainer blobContainer = _blobClient.GetContainerReference(containerName);
            foreach (var blob in blobContainer.ListBlobs())
            {
                if (blob.GetType() == typeof(CloudBlockBlob))
                {
                    await((CloudBlockBlob)blob).DeleteIfExistsAsync();
                }
            }
        }
        
    }
}