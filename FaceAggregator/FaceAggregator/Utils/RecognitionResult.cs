namespace FaceAggregator.Utils
{
    /// <summary>
    /// Recognition results
    /// </summary>
    class RecognitionResult
    {
        /// <summary>
        /// Gets or sets the face identifier.
        /// </summary>
        /// <value>
        /// The face identifier.
        /// </value>
        public string faceId { get; set; }
        /// <summary>
        /// Gets or sets the confidence.
        /// </summary>
        /// <value>
        /// The confidence.
        /// </value>
        public double confidence { get; set; }
    }
}
