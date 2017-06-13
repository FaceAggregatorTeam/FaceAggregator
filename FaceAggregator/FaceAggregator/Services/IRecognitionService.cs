using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaceAggregator.Utils;

namespace FaceAggregator.Services
{
    /// <summary>
    /// Intreface for recognition services
    /// </summary>
    /// <seealso cref="FaceAggregator.Utils.IFaceSimilarity" />
    /// <seealso cref="FaceAggregator.Utils.IFaceRecognition" />
    public interface IRecognitionService : IFaceSimilarity, IFaceRecognition
    {
    }
}