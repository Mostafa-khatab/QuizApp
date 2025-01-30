using Microsoft.AspNetCore.Identity;
using QuizApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Entity
{
    public class Quiz
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public string? Description { get; set; }
        public CategoryType CategoryType { get; set; }
        public DifficultyType DifficultyType { get; set; }
        public int ViewCount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public double AverageRating { get; set; }
        public int TimeLimitSeconds { get; set; }
        public List<Question> Questions { get; set; } = new();
        public QuizStatus Status { get; set; } = QuizStatus.Pending;
        public DateTime? ReviewedAt { get; set; }
        public string? ReviewerId { get; set; }        
        public string? RejectionReason { get; set; }
        public List<Report> Reports { get; set; } = new();

    }
}
