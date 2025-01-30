namespace QuizApp.Dtos
{
    public class QuizReviewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int QuestionCount { get; set; }
        public int ReportCount { get; set; }
    }
}
