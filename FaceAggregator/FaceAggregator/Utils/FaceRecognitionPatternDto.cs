using System.Collections.Generic;

namespace FaceAggregator.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class FaceRecognitionPatternDto
    {
        /// <summary>
        /// Gets or sets the face identifier.
        /// </summary>
        /// <value>
        /// The face identifier.
        /// </value>
        public string faceId { get; set; }
        /// <summary>
        /// Gets or sets the face ids.
        /// </summary>
        /// <value>
        /// The face ids.
        /// </value>
        public IList<string> faceIds { get; set; }
        /// <summary>
        /// Gets or sets the maximum number of candidates returned.
        /// </summary>
        /// <value>
        /// The maximum number of candidates returned.
        /// </value>
        public int maxNumOfCandidatesReturned { get; set; }
        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>
        /// The mode.
        /// </value>
        public string mode { get; set; }
    }
}
