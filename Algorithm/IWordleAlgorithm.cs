using WordleSolver.Colours;

namespace WordleSolver.Algorithm
{
    public interface IWordleAlgorithm
    {
        List<string> RunAlgo(WordleGuess guess, List<string> wordleList);

    }
}