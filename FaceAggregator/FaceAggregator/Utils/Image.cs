using System;
using System.Collections.Generic;

namespace FaceAggregator.Utils
{
    public class Image
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public IList<Face> Faces { get; set; }
    }

    public class Face
    {
        public string FaceId { get; set; }
        public FaceRectangle FaceRectangle { get; set; }
    }

    public class FaceRectangle
    {
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    
    }
}
