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
    /// <summary>
    /// Service for managing images
    /// </summary>
    /// <seealso cref="FaceAggregator.Services.IImagesService" />
    public class ImagesService : IImagesService
    {
        /// <summary>
        /// The storage account
        /// </summary>
        private CloudStorageAccount _storageAccount;
        /// <summary>
        /// The BLOB client
        /// </summary>
        private CloudBlobClient _blobClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagesService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ImagesService(IConfiguration configuration)
        {
            _storageAccount = CloudStorageAccount.Parse(configuration.GetValue("StorageConnectionString"));
            _blobClient = _storageAccount.CreateCloudBlobClient();
        }

        /// <summary>
        /// Gets all photos.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Uploads photos the asynchronous way.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the random name of the BLOB.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        private string GetRandomBlobName(string filename)
        {
            string ext = Path.GetExtension(filename);
            return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
        }



        /// <summary>
        /// Deletes the image.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <returns></returns>
        public async Task DeleteImage(string filename, string containerName)
        {
            CloudBlobContainer blobContainer = _blobClient.GetContainerReference(containerName);
            var blob = blobContainer.GetBlockBlobReference(filename);
            await blob.DeleteIfExistsAsync();
        }

        /// <summary>
        /// Deletes all images.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Uploads photos the asynchronous way from URI.
        /// </summary>
        /// <param name="foundImages">The found images.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <returns></returns>
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