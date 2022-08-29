using Microsoft.AspNetCore.Mvc;
using WordleSolver.Colours;
using WordleSolver.Algorithm;
using WordleSolver.StaticData;

namespace WordleSolver.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WordleController : ControllerBase
    {
        private readonly IWordleAlgorithm _wordleAlgorithm;

        public WordleController(IWordleAlgorithm wordleAlgorithm)
        {
            _wordleAlgorithm = wordleAlgorithm;

        }

        [HttpPost]
        public ActionResult Post(WordleGuess guess)
        {
            var remainingWords = _wordleAlgorithm.RunAlgo(guess, Guess.GuessWords);
            return Ok(remainingWords);
        }
    }
}