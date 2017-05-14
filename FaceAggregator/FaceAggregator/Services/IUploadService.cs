using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FaceAggregator.Services
{
    public interface IUploadService
    {
        Task<ICollection<Uri>> GetAllBlobs(string emailAddress);
        Task UploadAsync(HttpFileCollectionBase files, string emailAddress);
        Task DeleteImage(string filename, string emailAddress);
        Task DeleteAllImages(string emailAddress);
    }
}