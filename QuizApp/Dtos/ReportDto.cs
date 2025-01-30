namespace QuizApp.Dtos
{
    public class ReportDto
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string QuizTitle { get; set; }
        public string Reason { get; set; }
        public DateTime ReportedAt { get; set; }
        public string ReporterName { get; set; }
    }
}
