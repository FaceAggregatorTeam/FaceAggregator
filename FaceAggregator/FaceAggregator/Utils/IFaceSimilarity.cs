using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaceAggregator.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFaceSimilarity
    {
        /// <summary>
        /// Finds the similar faces.
        /// </summary>
        /// <param name="facePatternId">The face pattern identifier.</param>
        /// <param name="images">The images.</param>
        /// <returns></returns>
        Task<IList<Image>> FindSimilar(string facePatternId, IList<Image> images);
    }
}
