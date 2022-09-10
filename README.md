# WordleSolver

This API aims at helping users to solve wordle efficiently. Just follow the instructions below and you should be able to solve the wordle in 4-5 tries.
If you are not able to solve wordle using this algorithm I would like to know the word so that I can see how best I can improve the algorithm

# How to run the API

You can choose to run the API locally, all you need is .NET 6 installed and a code editor (VS code, Visual Studio or Rider). Once you build and launch the application go to the swagger link http://localhost:5230/swagger/index.html and you should be able to use it from there.

I have also hosted it on azure, so you can visit the link
https://wordlesolver-azure.azurewebsites.net/swagger/index.html and start using it straight away without having to worry about hosting this locally.

# End Points

## Get end points

1. https://wordlesolver-azure.azurewebsites.net/api/v1/Wordle - Gets the stats around the wordle answers. This will list how many times a character has been repeated in all the possible answers. The list is ordered by descending so the highest repeated character appears first.
2. https://wordlesolver-azure.azurewebsites.net/api/v1/Wordle/FirstGuess - Tells you what is the first word you have to enter in wordle. This takes into account the stats in the previous get endpoint and gives you the most appropriate guess.

## Post endpoints

1. https://wordlesolver-azure.azurewebsites.net/api/v1/Wordle -

   I will try to explain how to use this endpoint with the help of an example,I think this will help us understand how we will use the endpoint going forward.
   We use the word that was generated in the second get endpoint which is
   **"alter"** and enter this word into wordle and hit enter the following image is displayed

![FirstGuess](https://github.com/swarooprooney/WordleSolver/blob/main/Images/FirstGuess.png?raw=true)

Here we can see that letters a,e and r are not present and l and t are present but in wrong position, so now we will call the post api endpoint above with the following body

`{ "redCharacters":[ "a", "e", "r" ], "yellowCharacters":[ { "character":"l", "position":1 }, { "character":"t", "position":4 } ] }`

so there are 3 important bits to this API call

- redCharacters - This is an array and we enter the characters which we know to be not present in the final answer. In this example it is a,e and r
- yellowCharacters - This is also an array but with a slightly different format. This represents the characters that are present in the final answer but our guess has it in the wrong position. So we feed the character which turned yellow in wordle along with the position which it turned yellow for. In this example the yellow characters are l and t and they are in position 1 and 4 respectively (remember array is 0 based index)
- greenCharacters - Also an array. This represents the characters that are present both in the final answer and our guess and also they are in the correct position. In this example we have none so we haven't supplied the element at all.

When we execute the post API we get the **"lusty"** as the next guess

![SecondGuess](https://github.com/swarooprooney/WordleSolver/blob/main/Images/SecondGuess.png?raw=true)

we enter lusty into the wordle and then hit enter, following image is displayed

we again call the API with the following body

`{ "redCharacters":[ "a", "e", "r", "u", "s" ], "yellowCharacters":[ { "character":"l", "position":1 }, { "character":"t", "position":4 } ], "greenCharacters":[ { "character":"l", "position":0 }, { "character":"t", "position":3 }, { "character":"y", "position":4 } ] }`

With this second guess we know that u and s are not present in our final answer, hence we add them to redCharacters list. We also know that l,t and y are present in the final answer and are in the correct position in our guess, so we add this information to the greenCharacter array as shown above. Please remember that we do not replace/remove any characters from any of the arrays, we mearly add on the extra information we get from our subsequent guesses.

With this information if we again call the post endpoint we get the guess as **"lofty"**, if we enter this wordle and hit enter our wordle for the day is solved. If it isn't solved you repeat the same steps again till it is solved. Hopefully you can crack it within 6 tries. If you don't let me know the word, I will see why wordle didn't pick it up.

![ThirdGuess](https://github.com/swarooprooney/WordleSolver/blob/main/Images/ThirdGuess.png?raw=true)
