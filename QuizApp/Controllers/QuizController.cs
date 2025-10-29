using Microsoft.AspNetCore.Mvc;
using QuizApp.Services;

namespace QuizApp.Controllers
{
    public class QuizController : Controller
    {
        private readonly QuizService _quizService;
        public QuizController(QuizService quizService)
        {
            _quizService = quizService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var quizzes = _quizService.GetAllQuizzes();
            return View(quizzes);
        }
        [HttpGet]
        public IActionResult Start(int id)
        {
            var quiz = _quizService.GetQuizById(id);
            if (quiz == null)
            {
                return NotFound();
            }             

            _quizService.ShuffleQuiz(quiz);
            return View(quiz);
        }

        [HttpPost]
        public IActionResult Submit(int id, Dictionary<string, string> answers)
        {
            var quiz = _quizService.GetQuizById(id);
            if (quiz == null)
            {
                return NotFound();
            }

            var result = _quizService.EvaluateQuiz(quiz, answers);
            return View("Result", result);
        }

    }
}
