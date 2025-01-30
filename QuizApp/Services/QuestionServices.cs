using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Dtos;
using QuizApp.Entity;
using QuizApp.Interfaces;
using QuizApp.Models;

namespace QuizApp.Services
{
    public class QuestionServices : IQuestionServices
    {

        private readonly AppDbContext _context;

        public QuestionServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddQuestion(QuestionDto dto)
        {
            var quiz = await _context.Quizzes.FirstOrDefaultAsync(x => x.Id == dto.QuizId);
            if (quiz == null)
            {
                 throw new ArgumentException("Quiz not found"); 
            }
            var question = new Question
            {
                Text = dto.Text,
                Type = dto.Type,
                QuizId = dto.QuizId,
            };

            ValidateQuestion(dto, dto.Type);

            foreach (var answerDto in dto.Answers)
            {
                question.Answers.Add(new Answer
                {
                    Text = answerDto.Text,
                    IsCorrect = answerDto.IsCorrect
                });
            }

            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();

            
        }

        public async Task RemoveQuestion(int id)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == id);

            if (question == null)
            {
                throw new ArgumentException("Question not found");
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

           
        }

        public async Task UpdateQuestion(int id, QuestionDto dto)
        {
            var question = _context.Questions.FirstOrDefault(x => x.Id == id);

            if (question == null)
            {
                throw new ArgumentException("Question not found");
            }
            var quiz = await _context.Quizzes.FirstOrDefaultAsync(x => x.Id == dto.QuizId);
            if (quiz == null)
            {
                throw new ArgumentException("Quiz not found");
            }
            question.Text = dto.Text;
            question.Type = dto.Type;
            question.QuizId = dto.QuizId;

            ValidateQuestion(dto, dto.Type);

            var answersToRemove = _context.Answers.Where(x => x.QuestionId == id).ToList();

            _context.Answers.RemoveRange(answersToRemove);

            await _context.SaveChangesAsync();


            foreach (var answerDto in dto.Answers)
            {
                question.Answers.Add(new Answer
                {
                    Text = answerDto.Text,
                    IsCorrect = answerDto.IsCorrect
                });
            }

            await _context.SaveChangesAsync();

        }

        public void ValidateQuestion(QuestionDto dto, QuestionType type)
        {
            switch (type)
            {
                case QuestionType.TrueFalse:
                    if (dto.Answers.Count != 2)
                        throw new ArgumentException("True/False questions require exactly 2 answers");
                    if (dto.Answers.Count(a => a.IsCorrect) != 1)
                        throw new ArgumentException("True/False questions must have exactly 1 correct answer");
                    break;

                case QuestionType.MultipleChoice:
                    if (dto.Answers.Count < 2)
                        throw new ArgumentException("Multiple choice questions require at least 2 answers");
                    if (dto.Answers.Count(a => a.IsCorrect) < 1)
                        throw new ArgumentException("Multiple choice questions require at least 1 correct answer");
                    break;
            }
        }
        public async Task<Question> GetQuestionWithAnswers(int questionId)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == questionId);

            if(question == null)
            {
                throw new ArgumentException("Question Not Found");
            }

            return await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == questionId);
        }
        

    }
}
