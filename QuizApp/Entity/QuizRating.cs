using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Entity
{
    public class QuizRating
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; } 
        public DateTime RatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(QuizId))]
        public Quiz Quiz { get; set; }
    }
}
