using System.Threading.Tasks;

namespace FaceAggregator.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFaceRecognition
    {
        /// <summary>
        /// Detects the faces.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <returns></returns>
        Task<Image> DetectFaces(string imagePath);
    }
}
