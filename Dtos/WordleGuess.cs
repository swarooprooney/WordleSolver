namespace WordleSolver.Colours;

public class WordleGuess
{
    public WordleGuess()
    {
        RedCharacters = new List<Red>();
        YellowCharacters = new List<Yellow>();
        GreenCharacters = new List<Green>();
    }

    public List<Red> RedCharacters { get; set; }
    public List<Yellow> YellowCharacters { get; set; }
    public List<Green> GreenCharacters { get; set; }
}