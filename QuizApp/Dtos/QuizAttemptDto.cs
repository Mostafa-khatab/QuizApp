namespace QuizApp.Dtos
{
    public class QuizAttemptDto
    {
        public int QuizId { get; set; }
        public List<AnswerSubmissionDto> Answers { get; set; }
        public int SessionId { get; set; }
    }
}
