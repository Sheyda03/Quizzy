using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using System.Text.Json;

namespace QuizApp.Services
{
    public class QuizService
    {
        private readonly string _indexFile = Path.Combine("Data", "quizIndex.json");
        private readonly string _quizzesFolder = Path.Combine("Data", "Quizzes");
        private static readonly Random _random = new Random();
        public List<Quiz> GetAllQuizzes()
        {
            if (!File.Exists(_indexFile))
            {
                return new List<Quiz>();
            }

            var json = File.ReadAllText(_indexFile);
            var quizzes = JsonSerializer.Deserialize<List<Quiz>>(json);

            if (quizzes == null)
            {
                quizzes = new List<Quiz>();
            }

            return quizzes;
        }
        public Quiz? GetQuizById(int id)
        {
            var indexList = GetAllQuizzes();
            var quizMeta = indexList.FirstOrDefault(q => q.Id == id);
            if (quizMeta == null)
            {
                return null;
            }                

            var filePath = Path.Combine(_quizzesFolder, quizMeta.File);
            if (!File.Exists(filePath))
            {
                return null;
            }                

            var json = File.ReadAllText(filePath);
            var quiz = JsonSerializer.Deserialize<Quiz>(json);
            return quiz;
        }
        public QuizResult EvaluateQuiz(Quiz quiz, Dictionary<string, string> answers)
        {
            int correctCount = 0;
            var questionResults = new List<QuizResult.QuestionResult>();

            foreach (var question in quiz.Questions)
            {
                answers.TryGetValue($"q_{question.Id}", out string selected);
                var correctOption = question.Options.FirstOrDefault(o => o.IsCorrect);
                bool isCorrect = question.Options.Any(o => o.Text == selected && o.IsCorrect);

                if (isCorrect)
                {
                    correctCount++;
                }                    

                questionResults.Add(new QuizResult.QuestionResult
                {
                    QuestionText = question.QuestionText,
                    SelectedAnswer = selected ?? "(no answer)",
                    CorrectAnswer = correctOption.Text ?? "N/A",
                    IsCorrect = isCorrect
                });
            }

            return new QuizResult
            {
                QuizId = quiz.Id,
                Correct = correctCount,
                Total = quiz.Questions.Count,
                ScorePercent = (int)((double)correctCount / quiz.Questions.Count * 100),
                Questions = questionResults
            };
        }      

        public void ShuffleQuiz(Quiz quiz)
        {
            quiz.Questions = quiz.Questions.OrderBy(q => _random.Next()).ToList();

            foreach (var question in quiz.Questions)
            {
                question.Options = question.Options.OrderBy(o => _random.Next()).ToList();
            }
        }
    }
}
