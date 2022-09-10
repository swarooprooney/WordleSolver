using System.Linq;
using WordleSolver.Colours;
using WordleSolver.Models;

namespace WordleSolver.Algorithm
{
    public class WordleAlgorithm : IWordleAlgorithm
    {
        public string GetFirstGuess(List<string> possibleAnswers)
        {
            int i = 5;
            string? guess = null;
            var stats = GetStats(possibleAnswers);
            var chars = string.Empty;
            while (guess is null)
            {
                var test = stats.Take(i).Select(x => x.Key);

                foreach (var ch in test)
                {
                    chars = chars + ch;
                }
                var result = GetGuess(chars, possibleAnswers, stats);
                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }
                i++;
            }

            return string.Empty;
        }

        private string GetGuess(string chars, List<string> possibleAnswers, Dictionary<char, int> characterScore)
        {
            Dictionary<string, int> wordScore = new Dictionary<string, int>();

            foreach (var answer in possibleAnswers)
            {
                int i = 0;
                foreach (var character in answer)
                {
                    if (!chars.Contains(character))
                    {
                        break;
                    }
                    i++;
                }
                if (i == answer.Length)
                {
                    wordScore.Add(answer, CalculateScore(answer, characterScore));
                }
            }

            var uniqueCharaterWords = new Dictionary<string, int>();
            foreach (var words in wordScore)
            {
                if (OnlyOnceCheck(words.Key))
                {
                    uniqueCharaterWords.Add(words.Key, words.Value);
                }
            }

            var maxValue = uniqueCharaterWords.FirstOrDefault(x => x.Value == uniqueCharaterWords.Values.Max()).Key;
            return maxValue;
        }

        private string GetGuessWithoutUniqueCharacterCheck(string chars, List<string> possibleAnswers, Dictionary<char, int> characterScore)
        {
            Dictionary<string, int> wordScore = new Dictionary<string, int>();

            foreach (var answer in possibleAnswers)
            {
                int i = 0;
                foreach (var character in answer)
                {
                    if (!chars.Contains(character))
                    {
                        break;
                    }
                    i++;
                }
                if (i == answer.Length)
                {
                    wordScore.Add(answer, CalculateScore(answer, characterScore));
                }
            }

            var maxValue = wordScore.FirstOrDefault(x => x.Value == wordScore.Values.Max()).Key;
            if (maxValue == null)
            {
                return string.Empty;
            }


            return maxValue;
        }

        private bool OnlyOnceCheck(string input)
        {
            return input.Distinct().Count() == input.Length;
        }

        private int CalculateScore(string answer, Dictionary<char, int> characterScore)
        {
            int score = 0;
            foreach (var character in answer)
            {
                score = score + characterScore[character];
            }
            return score;
        }

        public Dictionary<char, int> GetStats(List<string> possibleAnswers)
        {
            Dictionary<char, int> results = new Dictionary<char, int>();
            foreach (var answer in possibleAnswers)
            {
                foreach (var character in answer)
                {
                    if (results.ContainsKey(character))
                    {
                        results[character] = results[character] + 1;
                    }
                    else
                    {
                        results.Add(character, 1);
                    }
                }
            }

            return results.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        public AnswersAndGuess RunAlgo(WordleGuess guess, List<string> wordleList)
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
                    if (word.Contains(redChar))
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

            var stats = GetStats(workingCopy);
            string? guessWord = null;
            var finalChars = String.Concat(greenChars) + String.Concat(yellowChars);
            AnswersAndGuess result;
            if (finalChars.Length == 5)
            {
                guessWord = GetGuess(finalChars, workingCopy, stats);
                result = new AnswersAndGuess(workingCopy, guessWord);
                return result;
            }
            int take = 5 - finalChars.Length;
            var chars = string.Empty;
            while (guessWord is null)
            {
                if (stats.Count < take)
                {
                    break;
                }
                var test = stats.Take(take).Select(x => x.Key);

                foreach (var ch in test)
                {
                    chars = chars + ch;
                }
                guessWord = GetGuess(finalChars + chars, workingCopy, stats);
                if (!string.IsNullOrEmpty(guessWord))
                {
                    return new AnswersAndGuess(workingCopy, guessWord); ;
                }
                take++;
            }
            if (guessWord is null)
            {
                while (guessWord is null)
                {
                    if (stats.Count < take)
                    {
                        break;
                    }
                    var test = stats.Take(take).Select(x => x.Key);

                    foreach (var ch in test)
                    {
                        chars = chars + ch;
                    }
                    guessWord = GetGuessWithoutUniqueCharacterCheck(finalChars + chars, workingCopy, stats);
                    if (!string.IsNullOrEmpty(guessWord))
                    {
                        return new AnswersAndGuess(workingCopy, guessWord); ;
                    }
                    take++;
                }
            }
            result = new AnswersAndGuess(workingCopy, guessWord);
            return result;
        }
    }
}