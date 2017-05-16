using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FaceAggregator.Utils;

namespace FaceAggregator.Services
{
    public class RecognitionService : IRecognitionService
    {
        private IFaceRecognition _faceRecognition;
        private IFaceSimilarity _faceSimilarity;
        public RecognitionService(IFaceRecognition faceRecognition, IFaceSimilarity faceSimilarity)
        {
            _faceRecognition = faceRecognition;
            _faceSimilarity = faceSimilarity;
        }
        public Task<IList<Image>> FindSimilar(string facePatternId, IList<Image> images)
        {
            return _faceSimilarity.FindSimilar(facePatternId, images);
        }

        public Task<Image> DetectFaces(string imagePath)
        {
            return _faceRecognition.DetectFaces(imagePath);
        }
    }
}