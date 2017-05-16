using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FaceAggregator.Utils;

namespace FaceAggregator.Services
{
    public interface IImagesService
    {
        Task<ICollection<Uri>> GetAllPhotos(string containerName);
        Task UploadAsync(HttpFileCollectionBase files, string containerName);
        Task DeleteImage(string filename, string containerName);
        Task DeleteAllImages(string containerName);
        Task UploadAsyncFromUri(IList<Image> foundImages, string containerName);
    }
}