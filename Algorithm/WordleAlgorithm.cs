using System.Linq;
using WordleSolver.Colours;
using WordleSolver.Models;

namespace WordleSolver.Algorithm
{
    public class WordleAlgorithm : IWordleAlgorithm
    {
        private const int wordLength = 5;

        /// <summary>
        /// Gets the first guess to begin a game of wordle
        /// </summary>
        /// <param name="possibleAnswers">List of possible answers</param>
        /// <returns>The first guess that needs to used in wordle</returns>
        public string GetFirstGuess(List<string> possibleAnswers)
        {
            int guessWordLength = wordLength;
            string? guess = null;
            var stats = GetStats(possibleAnswers);
            while (guess is null)
            {
                var charactersPresentInGuess = string.Empty;
                var charactersPresentInGuessEnumerable = stats.Take(guessWordLength).Select(x => x.Key);

                foreach (var character in charactersPresentInGuessEnumerable)
                {
                    charactersPresentInGuess += character;
                }
                var result = GetUniqueCharacterWordGuess(charactersPresentInGuess, possibleAnswers, stats);
                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }
                guessWordLength++;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets a list of all chacters and number of times they are repeated in the wordle answers.
        /// </summary>
        /// <param name="possibleAnswers">List of all possible answers</param>
        /// <returns>Dictionary of characters and their score</returns>
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

        /// <summary>
        /// The Brains of the operation.
        /// </summary>
        /// <param name="guess">The wordle guess data. This contains details about characters that are not present, characters that are present but not in correct position and characters that are present and in the correct position</param>
        /// <param name="wordleList">List of all possible answers.</param>
        /// <returns>All the possible answers and the guess you should be making next</returns>
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
            var charsPresentInAnswer = String.Concat(greenChars) + String.Concat(yellowChars);
            AnswersAndGuess result;

            //Check if all the letters previous guess is in wordle answer, if yes find the possible guess. 
            if (charsPresentInAnswer.Length == wordLength)
            {
                guessWord = GetUniqueCharacterWordGuess(charsPresentInAnswer, workingCopy, stats);
                result = new AnswersAndGuess(workingCopy, guessWord);
                return result;
            }

            int take = wordLength - charsPresentInAnswer.Length;

            // Find the best guess with unique characters for each letter.
            while (guessWord is null)
            {
                var remainingChars = string.Empty;
                if (stats.Count < take)
                {
                    break;
                }
                var remainingCharsEnumerable = stats.Take(take).Select(x => x.Key);

                foreach (var character in remainingCharsEnumerable)
                {
                    remainingChars += character;
                }
                guessWord = GetUniqueCharacterWordGuess(charsPresentInAnswer + remainingChars, workingCopy, stats);
                if (!string.IsNullOrEmpty(guessWord))
                {
                    return new AnswersAndGuess(workingCopy, guessWord); ;
                }
                take++;
            }

            take = wordLength - charsPresentInAnswer.Length; //Reset take

            // If no unique letter word is found, find the next best possible guess
            while (string.IsNullOrEmpty(guessWord))
            {
                var remainingChars = string.Empty;
                if (stats.Count < take)
                {
                    break;
                }
                var remainingCharsEnumerable = stats.Take(take).Select(x => x.Key);

                foreach (var ch in remainingCharsEnumerable)
                {
                    remainingChars += ch;
                }
                guessWord = GetGuessWithoutUniqueCharacterCheck(charsPresentInAnswer + remainingChars, workingCopy, stats);
                if (!string.IsNullOrEmpty(guessWord))
                {
                    return new AnswersAndGuess(workingCopy, guessWord); ;
                }
                take++;
            }

            result = new AnswersAndGuess(workingCopy, guessWord);
            return result;
        }

        /// <summary>
        /// Gets the guess word for wordle.
        /// </summary>
        /// <param name="chars">all the charcters that are to be present in the guess</param>
        /// <param name="possibleAnswers">A list of possible answers</param>
        /// <param name="characterScore">Score for each character</param>
        /// <returns>Next guess you should be making</returns>
        private string GetUniqueCharacterWordGuess(string chars, List<string> possibleAnswers, Dictionary<char, int> characterScore)
        {
            Dictionary<string, int> wordScore = CalculatePossibleAnswersScore(chars, possibleAnswers, characterScore);
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

        /// <summary>
        /// Gets the guess word for wordle where all the characters in the word are unique.
        /// </summary>
        /// <param name="chars">all the charcters that are to be present in the guess</param>
        /// <param name="possibleAnswers">A list of possible answers</param>
        /// <param name="characterScore">Score for each character</param>
        /// <returns>Next guess you should be making</returns>
        private string GetGuessWithoutUniqueCharacterCheck(string chars, List<string> possibleAnswers, Dictionary<char, int> characterScore)
        {
            Dictionary<string, int> wordScore = CalculatePossibleAnswersScore(chars, possibleAnswers, characterScore);
            var maxValue = wordScore.FirstOrDefault(x => x.Value == wordScore.Values.Max()).Key;
            if (maxValue == null)
            {
                return string.Empty;
            }

            return maxValue;
        }

        /// <summary>
        /// Calculates a score for all the given possible answers
        /// </summary>
        /// <param name="chars">Characters which need to be present in answer</param>
        /// <param name="possibleAnswers">List of possible answers</param>
        /// <param name="characterScore">Score for each character</param>
        /// <returns>Possible answers and their score.</returns>
        private Dictionary<string, int> CalculatePossibleAnswersScore(string chars, List<string> possibleAnswers, Dictionary<char, int> characterScore)
        {
            Dictionary<string, int> wordScore = new Dictionary<string, int>();

            foreach (var answer in possibleAnswers)
            {
                int guessLength = 0;
                foreach (var character in answer)
                {
                    if (!chars.Contains(character))
                    {
                        break;
                    }
                    guessLength++;
                }
                if (guessLength == answer.Length)
                {
                    wordScore.Add(answer, CalculateScore(answer, characterScore));
                }
            }

            return wordScore;
        }

        /// <summary>
        /// Checks if any of the characters in the word is repeated.
        /// </summary>
        /// <param name="word">word which needs to be checked</param>
        /// <returns>True if all the characters are unique, false if it isn't.</returns>
        private bool OnlyOnceCheck(string word)
        {
            return word.Distinct().Count() == word.Length;
        }

        /// <summary>
        /// Calculates score of the word based on stats
        /// </summary>
        /// <param name="answer">word whose score needs to be calculated</param>
        /// <param name="characterScore">stats which says how likely a character will occur in wordle answers.</param>
        /// <returns>Score of how likely this word is the answer for wordle</returns>
        private int CalculateScore(string answer, Dictionary<char, int> characterScore)
        {
            int score = 0;
            foreach (var character in answer)
            {
                score += characterScore[character];
            }
            return score;
        }

    }
}