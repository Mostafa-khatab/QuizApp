using QuizApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Entity
{
    public class QuizAttempt
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; }
        public int QuizId { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public DateTime AttemptDate { get; set; }
        public TimeSpan Duration { get; set; }
        public Quiz Quiz { get; set; }
        public string ShareCode { get; set; } = Guid.NewGuid().ToString("N");
        public bool AllowSharing { get; set; } = true;
    }
}
