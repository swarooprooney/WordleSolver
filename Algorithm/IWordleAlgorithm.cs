using WordleSolver.Colours;
using WordleSolver.Models;

namespace WordleSolver.Algorithm
{
    public interface IWordleAlgorithm
    {
        AnswersAndGuess RunAlgo(WordleGuess guess, List<string> wordleList);
        Dictionary<char, int> GetStats(List<string> possibleAnswers);

        string GetFirstGuess(List<string> possibleAnswers);
    }
}