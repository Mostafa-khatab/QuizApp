namespace QuizApp.Dtos
{
    public class UserDto
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public bool IsBanned { get; set; }
        public IList<string>? Roles { get; set; }
    }
}
