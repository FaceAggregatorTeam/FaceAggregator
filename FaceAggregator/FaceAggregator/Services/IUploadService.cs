using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FaceAggregator.Services
{
    public interface IUploadService
    {
        Task<ICollection<Uri>> GetAllPhotos(string containerName);
        Task UploadAsync(HttpFileCollectionBase files, string containerName);
        Task DeleteImage(string filename, string containerName);
        Task DeleteAllImages(string containerName);

    }
}