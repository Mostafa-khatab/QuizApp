namespace QuizApp.Entity
{
    public class QuizSession
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int QuizId { get; set; }
        public DateTime StartTime { get; set; }
        public int TimeRemaining { get; set; }
        public bool IsCompleted { get; set; }
    }
}
