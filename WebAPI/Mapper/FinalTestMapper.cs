using Backend.Dto.FinalTest.Request;
using Backend.Dto.FinalTest.Response;
using Core.Entity.Test;

namespace Backend.Mapper;

public static class FinalTestMapper
{
    public static FinalTest ToEntity(CreateFinalTestRequest request)
    {
        return new FinalTest
        {
            Title = request.Title,
            Questions = request.Questions.Select(q => new TestQuestion
            {
                QuestionText = q.QuestionText,
                CorrectIndex = q.CorrectIndex,
                Options = q.Options.Select(o => new TestOption
                {
                    OptionText = o.OptionText
                }).ToList(),
            }).ToList()
        };
    }

    public static GetFinalTestResponse ToDto(FinalTest entity)
    {
        return new GetFinalTestResponse
        {
            Id = entity.Id,
            Title = entity.Title,
            Questions = entity.Questions.Select(q => new GetTestQuestionResponse
            {
                Id = q.Id,
                QuestionText = q.QuestionText,
                CorrectIndex = q.CorrectIndex,
                Options = q.Options.Select(o => new GetTestOptionResponse
                {
                    Id = o.Id,
                    OptionText = o.OptionText
                }).ToList()
            }).ToList()
        };
    }
}