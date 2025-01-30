using Microsoft.AspNetCore.Identity;
using QuizApp.Models;

namespace QuizApp.Entity
{
    public class Report
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public string ReporterId { get; set; }
        public AppUser Reporter { get; set; }
        public string Reason { get; set; }
        public ReportStatus Status { get; set; } = ReportStatus.Open;
        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;
    }
}
