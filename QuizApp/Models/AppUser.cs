using Microsoft.AspNetCore.Identity;

namespace QuizApp.Models
{
    public class AppUser : IdentityUser
    {
        public int? Score { get; set; }
    }
}
