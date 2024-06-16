namespace english_learning_server.Interfaces
{
    public interface ILevenshteinDistanceService
    {
        public double CalculateNormalizedLevenshteinDistance(string text1, string text2);
    }
}
