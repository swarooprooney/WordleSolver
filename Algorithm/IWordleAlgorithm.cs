using WordleSolver.Colours;

namespace WordleSolver.Algorithm
{
    public interface IWordleAlgorithm
    {
        List<string> RunAlgo(WordleGuess guess, List<string> wordleList);
        Dictionary<char, int> GetStats(List<string> possibleAnswers);

        string GetFirstGuess(List<string> possibleAnswers);
    }
}