namespace WordleSolver.Colours;

public class Red
{
    public char Ch { get; set; }
}

public class Yellow
{
    public char Ch { get; set; }
    public int Pos { get; set; }
}

public class Green
{
     public char Ch { get; set; }
        public int Pos { get; set; }
}

public class WordleGuess
{
    public List<Red> RedChars { get; set; }
    public List<Yellow> YellowChars { get; set; }
    public List<Green> GreenChars { get; set; }
}