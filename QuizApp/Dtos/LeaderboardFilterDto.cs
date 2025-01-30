namespace QuizApp.Dtos
{
    public class LeaderboardFilterDto
    {
        public int? QuizId { get; set; }
        public int TopN { get; set; } = 10;
        public TimeSpan? TimeRange { get; set; }
    }
}
