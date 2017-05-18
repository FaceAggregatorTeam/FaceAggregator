using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using FaceAggregator.Utils;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace FaceAggregator.Services
{
    public class ImagesService : IImagesService
    {
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient _blobClient;

        public ImagesService(IConfiguration configuration)
        {
            _storageAccount = CloudStorageAccount.Parse(configuration.GetValue("StorageConnectionString"));
            _blobClient = _storageAccount.CreateCloudBlobClient();
        }
        
        public async Task<ICollection<Uri>> GetAllPhotos(string containerName)
        {
            
            CloudBlobContainer blobContainer = _blobClient.GetContainerReference(containerName);
            bool containerExists = blobContainer.Exists();
            if (!containerExists)
            {
                await blobContainer.CreateAsync();
                await blobContainer.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }
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
        public async Task UploadAsyncFromUri(IList<Image> foundImages, string containerName)
        {
            int fileCount = foundImages.Count;
            CloudBlobContainer blobContainer = _blobClient.GetContainerReference(containerName);
            blobContainer.CreateIfNotExists();
            if (fileCount > 0)
            {
                for (int i = 0; i < fileCount; i++)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(foundImages[i].Path);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream inputStream = response.GetResponseStream();
                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(GetRandomBlobName(foundImages[i].Path));
                    await blob.UploadFromStreamAsync(inputStream);
                }
            }
        }
    }
}