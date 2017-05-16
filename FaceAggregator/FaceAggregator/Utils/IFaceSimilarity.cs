using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaceAggregator.Utils
{
    public interface IFaceSimilarity
    {
        Task<IList<Image>> FindSimilar(string facePatternId, IList<Image> images);
    }
}
