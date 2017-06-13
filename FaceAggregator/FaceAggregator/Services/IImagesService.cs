using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FaceAggregator.Utils;

namespace FaceAggregator.Services
{
    /// <summary>
    /// Interface for managing images
    /// </summary>
    public interface IImagesService
    {
        /// <summary>
        /// Gets all photos.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <returns></returns>
        Task<ICollection<Uri>> GetAllPhotos(string containerName);
        /// <summary>
        /// Uploads photos the asynchronous.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <returns></returns>
        Task UploadAsync(HttpFileCollectionBase files, string containerName);
        /// <summary>
        /// Deletes the image.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <returns></returns>
        Task DeleteImage(string filename, string containerName);
        /// <summary>
        /// Deletes all images.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <returns></returns>
        Task DeleteAllImages(string containerName);
        /// <summary>
        /// Uploads photos the asynchronous way from URI.
        /// </summary>
        /// <param name="foundImages">The found images.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <returns></returns>
        Task UploadAsyncFromUri(IList<Image> foundImages, string containerName);
    }
}