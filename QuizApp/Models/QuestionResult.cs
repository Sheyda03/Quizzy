namespace QuizApp.Models
{
    public class QuizResult
    {
        public int QuizId { get; set; }
        public int Correct { get; set; }
        public int Total { get; set; }
        public int ScorePercent { get; set; }
        public List<QuestionResult> Questions { get; set; }

        public class QuestionResult
        {
            public string QuestionText { get; set; }
            public string SelectedAnswer { get; set; }
            public string CorrectAnswer { get; set; }
            public bool IsCorrect { get; set; }
        }
    }
}
