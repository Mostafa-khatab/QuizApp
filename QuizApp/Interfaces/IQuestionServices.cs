using Microsoft.AspNetCore.Mvc;
using QuizApp.Dtos;
using QuizApp.Entity;
using QuizApp.Models;

namespace QuizApp.Interfaces
{
    public interface IQuestionServices
    {
        public void ValidateQuestion(QuestionDto dto, QuestionType type);
        public Task AddQuestion(QuestionDto dto);
        public Task RemoveQuestion(int id);
        public Task UpdateQuestion(int id, QuestionDto dto);
        public Task<Question> GetQuestionWithAnswers(int questionId);



    }
}
