using WordleSolver.Colours;

namespace WordleSolver.Algorithm
{
    public class WordleAlgorithm : IWordleAlgorithm
    {
        public List<string> RunAlgo(WordleGuess guess, List<string> wordleList)
        {
            var redChars = guess.RedCharacters;
            var greenChars = guess.GreenCharacters;
            var yellowChars = guess.YellowCharacters;
            var wordList = wordleList;
            var workingCopy = new List<string>(wordList);

            //Red
            foreach (var word in wordList)
            {
                foreach (var redChar in redChars)
                {
                    //Red
                    if (word.Contains(redChar.Character))
                    {
                        var removeIndex = workingCopy.FindIndex(x => x.Equals(word));
                        if (removeIndex != -1)
                        {
                            workingCopy.RemoveAt(removeIndex);
                        }
                    }
                }

                //Green
                foreach (var greenChar in greenChars)
                {
                    if (!word.Contains(greenChar.Character) || word[greenChar.Position] != greenChar.Character)
                    {
                        var removeIndex = workingCopy.FindIndex(x => x.Equals(word));
                        if (removeIndex != -1)
                        {
                            workingCopy.RemoveAt(removeIndex);
                        }
                    }
                }

                //Yellow
                foreach (var yellowChar in yellowChars)
                {
                    if (!word.Contains(yellowChar.Character) || word[yellowChar.Position] == yellowChar.Character)
                    {
                        var removeIndex = workingCopy.FindIndex(x => x.Equals(word));
                        if (removeIndex != -1)
                        {
                            workingCopy.RemoveAt(removeIndex);
                        }
                    }
                }
            }


            return workingCopy;
        }
    }
}