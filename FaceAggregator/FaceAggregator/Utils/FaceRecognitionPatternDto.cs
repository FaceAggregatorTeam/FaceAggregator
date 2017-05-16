using System.Collections.Generic;

namespace FaceAggregator.Utils
{
    public class FaceRecognitionPatternDto
    {
        public string faceId { get; set; }
        public IList<string> faceIds { get; set; }
        public int maxNumOfCandidatesReturned { get; set; }
        public string mode { get; set; }
    }
}
