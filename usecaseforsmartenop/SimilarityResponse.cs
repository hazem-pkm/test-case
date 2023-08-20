using System;
namespace usecaseforsmartenop
{
    public class SimilarityResponse
    {
        public int Time { get; set; }
        public double Similarity { get; set; }
        public string Lang { get; set; }
        public double LangConfidence { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

