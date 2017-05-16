using System.Threading.Tasks;

namespace FaceAggregator.Utils
{
    public interface IFaceRecognition
    {
        Task<Image> DetectFaces(string imagePath);
    }
}
