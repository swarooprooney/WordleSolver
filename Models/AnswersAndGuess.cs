namespace WordleSolver.Models
{
    public class AnswersAndGuess
    {
        public AnswersAndGuess(List<string> possibleAnswers, string guess)
        {
            PossibleAnswers = possibleAnswers;
            Guess = guess;
        }
        public List<string> PossibleAnswers { get; private set; }

        public string Guess { get; private set; }
    }
}