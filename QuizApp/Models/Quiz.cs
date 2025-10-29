namespace QuizApp.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? File { get; set; }
        public List<Question> Questions { get; set; }
    }
}
