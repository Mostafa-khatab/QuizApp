namespace QuizApp.Dtos
{
    public class LeaderboardEntryDto
    {
        public string Username { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public double Percentage { get; set; }
        public DateTime AttemptDate { get; set; }
    }
}
