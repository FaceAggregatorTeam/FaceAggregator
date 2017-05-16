using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaceAggregator.Utils;

namespace FaceAggregator.Services
{
    public interface IRecognitionService : IFaceSimilarity, IFaceRecognition
    {
    }
}