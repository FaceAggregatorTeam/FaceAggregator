using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FaceAggregator.Utils;

namespace FaceAggregator.Services
{
    /// <summary>
    /// Service managing recognition process
    /// </summary>
    /// <seealso cref="FaceAggregator.Services.IRecognitionService" />
    public class RecognitionService : IRecognitionService
    {
        /// <summary>
        /// The face recognition
        /// </summary>
        private IFaceRecognition _faceRecognition;
        /// <summary>
        /// The face similarity
        /// </summary>
        private IFaceSimilarity _faceSimilarity;
        /// <summary>
        /// Initializes a new instance of the <see cref="RecognitionService"/> class.
        /// </summary>
        /// <param name="faceRecognition">The face recognition.</param>
        /// <param name="faceSimilarity">The face similarity.</param>
        public RecognitionService(IFaceRecognition faceRecognition, IFaceSimilarity faceSimilarity)
        {
            _faceRecognition = faceRecognition;
            _faceSimilarity = faceSimilarity;
        }
        /// <summary>
        /// Finds the similar faces.
        /// </summary>
        /// <param name="facePatternId">The face pattern identifier.</param>
        /// <param name="images">The images.</param>
        /// <returns></returns>
        public Task<IList<Image>> FindSimilar(string facePatternId, IList<Image> images)
        {
            return _faceSimilarity.FindSimilar(facePatternId, images);
        }

        /// <summary>
        /// Detects the faces.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <returns></returns>
        public Task<Image> DetectFaces(string imagePath)
        {
            return _faceRecognition.DetectFaces(imagePath);
        }
    }
}