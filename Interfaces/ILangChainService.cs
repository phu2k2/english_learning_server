namespace english_learning_server.Interfaces
{
    public interface ILangChainService
    {
        public Task<string> GenerateSentenceByTopic(string text);   

        // public Task<string> GeneratePictureChoice(string text);
    }
}