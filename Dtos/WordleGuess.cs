namespace WordleSolver.Colours;

public class WordleGuess
{
    public WordleGuess()
    {
        RedCharacters = new List<char>();
        YellowCharacters = new List<Yellow>();
        GreenCharacters = new List<Green>();
    }

    public List<char> RedCharacters { get; set; }
    public List<Yellow> YellowCharacters { get; set; }
    public List<Green> GreenCharacters { get; set; }
}