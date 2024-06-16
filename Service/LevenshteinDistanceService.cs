using DuoVia.FuzzyStrings;
using english_learning_server.Interfaces;

namespace english_learning_server.Service
{
    public class LevenshteinDistanceService : ILevenshteinDistanceService
    {
        public double CalculateNormalizedLevenshteinDistance(string text1, string text2)
        {
            int distance = text1.LevenshteinDistance(text2);
            int maxLength = Math.Max(text1.Length, text2.Length);
            return 1.0 - (double)distance / maxLength;
        }
    }
}
