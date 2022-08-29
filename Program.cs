// See https://aka.ms/new-console-template for more information

using WordleSolver;
using WordleSolver.Colours;

Console.WriteLine("Hello, World!");
var wordList = Answers.AnswersList;
 WordleGuess guess1 =  new WordleGuess
{
    RedChars = new List<Red>
    {
        new Red
        {
            Ch = 'a'
        },
        new Red
        {
            Ch = 'r'
        },
        new Red
        {
            Ch = 's'
        }
        ,
        new Red
        {
            Ch = 'o'
        }
        ,
        new Red
        {
            Ch = 'b'
        }
        
    },
    YellowChars = new List<Yellow>
    {
        new Yellow
        {
            Ch = 'e', Pos = 4
        },
        new Yellow
        {
            Ch = 'e', Pos = 1
        },
        new Yellow
        {
            Ch = 'e', Pos = 2
        }
        ,
        new Yellow
        {
            Ch = 'c', Pos = 3
        },
        new Yellow
        {
            Ch = 'h', Pos = 4
        }
    },
    GreenChars = new List<Green>()
    // {
    //     new Green
    //     {
    //         Ch = 'r',
    //         Pos = 1
    //     }
    // }
};


wordList = WordleSolverAlgo.RunAlgo(guess1,wordList);
// WordleGuess guess2 = new WordleGuess
// {
//     RedChars = new List<Red>
//     {
//         new Red
//         {
//             Ch = 'g'
//         },
//         new Red
//         {
//             Ch = 'm'
//         }
//     },
//     YellowChars = new List<Yellow>(),
//     // {
//     //     new Yellow
//     //     {
//     //         Ch = 'e', Pos = 3
//     //     }
//     // },
//     GreenChars = new List<Green>
//     {
//         new Green
//         {
//             Ch = 'a',
//             Pos = 2
//         }
//     }
// };
//
// wordList = WordleSolverAlgo.RunAlgo(guess2,wordList);
// WordleGuess guess3 = new WordleGuess
// {
//     RedChars = new List<Red>
//     {
//         new Red
//         {
//             Ch = 'p'
//         },
//         new Red
//         {
//             Ch = 't'
//         }
//     },
//     YellowChars = new List<Yellow>
//     {
//         new Yellow
//         {
//             Ch = 'y',
//             Pos = 0
//         }
//     },
//     GreenChars = new List<Green>()
//     // {
//     //     new Green
//     //     {
//     //         Ch = 'g',
//     //         Pos = 0
//     //     },
//     //     new Green
//     //     {
//     //         Ch = 'a',
//     //         Pos = 1
//     //     },
//     //     new Green
//     //     {
//     //         Ch = 'e',
//     //         Pos = 4
//     //     }
//     // }
// };
//
// wordList = WordleSolverAlgo.RunAlgo(guess3,wordList);
// WordleGuess guess4 = new WordleGuess
// {
//     RedChars = new List<Red>
//     {
//         new Red
//         {
//             Ch = 'o'
//         }
//     },
//     YellowChars = new List<Yellow>
//     {
//         new Yellow
//         {
//             Ch = 'c',
//             Pos = 3
//         }
//     },
//     GreenChars = new List<Green>
//     {
//         new Green
//         {
//             Ch = 'y',
//             Pos = 4
//         }
//     }
//     // {
//     //     new Green
//     //     {
//     //         Ch = 'g',
//     //         Pos = 0
//     //     },
//     //     new Green
//     //     {
//     //         Ch = 'a',
//     //         Pos = 1
//     //     },
//     //     new Green
//     //     {
//     //         Ch = 'e',
//     //         Pos = 4
//     //     }
//     // }
// };
//
// wordList = WordleSolverAlgo.RunAlgo(guess4,wordList);
foreach (var remainingWord in wordList)
{
    Console.WriteLine(remainingWord + Environment.NewLine);
}


public static class WordleSolverAlgo
{

    private static List<string> _seedWords = new List<string>
    {
        "arles", 
        "rates",
        "arose", 
        "reais",
        "dares", 
        "roate",
        "lares", 
        "soare",
        "lores", 
        "stare",
        "nares", 
        "tales",
        "raile", 
        "tares",
        "raise", 
        "tores",
        "rales",
    };

    public static void RunAlgo(string answer)
    {
        Random rnd = new Random();
        var index = rnd.Next(0, 16);
        string myFirstGuess = _seedWords[index];

        var wordList = Guess.GuessWords;
        var workingCopy = new List<string>(wordList);

        foreach (var word in wordList)
        {
            for (int i = 0; i < myFirstGuess.Length; i++)
            {

                if (!word.Contains(myFirstGuess[i] /*and yellow or green*/))
                {
                    var removeIndex = workingCopy.FindIndex(x=>x.Equals(word));
                    if (removeIndex != -1)
                    {
                        workingCopy.RemoveAt(removeIndex);
                    }
                }
            }
        }
    }

    public static List<string> RunAlgo(WordleGuess guess, List<string> wordleList)
    {
        var redChars = guess.RedChars;
        var greenChars = guess.GreenChars;
        var yellowChars = guess.YellowChars;
        var wordList = wordleList;
        var workingCopy = new List<string>(wordList);
        
        //Red
        foreach (var word in wordList)
        {
            foreach (var redChar in redChars)
            {
                //Red
                if (word.Contains(redChar.Ch))
                {
                    var removeIndex = workingCopy.FindIndex(x=>x.Equals(word));
                    if (removeIndex != -1)
                    {
                        workingCopy.RemoveAt(removeIndex);
                    }
                }
            }
            
            //Green
            foreach (var greenChar in greenChars)
            {
                if (!word.Contains(greenChar.Ch) || word[greenChar.Pos] != greenChar.Ch)
                {
                    var removeIndex = workingCopy.FindIndex(x=>x.Equals(word));
                    if (removeIndex != -1)
                    {
                        workingCopy.RemoveAt(removeIndex);
                    }
                }
            }
            
            //Yellow
            foreach (var yellowChar in yellowChars)
            {
                if (!word.Contains(yellowChar.Ch) || word[yellowChar.Pos]==yellowChar.Ch)
                {
                    var removeIndex = workingCopy.FindIndex(x=>x.Equals(word));
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
