﻿using Backend.Dto.UserProgress;
using Core.Entity;
using Core.Model;

namespace Backend.Mapper;

public static class UserProgressMapper
{
    public static GetUserProgressResponse ToGetUserProgressResponse(this UserProgressDetails progressDetails, User user)
    {
        return new GetUserProgressResponse
        {
            UserId = progressDetails.UserId,
            FullName = user.FullName,    // Имя пользователя
            Email = user.Email,          // Email пользователя
            BlockProgresses = progressDetails.BlockCompletionProgresses.Select(bcp => new GetBlockProgressResponse
            {
                BlockId = bcp.BlockId,
                BlockTitle = bcp.Block.Title,
                IsBlockCompleted = bcp.IsBlockCompleted,
                TopicProgresses = progressDetails.CompletedTopics
                    .Where(tp => tp.BlockId == bcp.BlockId)
                    .Select(tp => new GetTopicProgressResponse
                    {
                        TopicId = tp.TopicId,
                        IsTopicCompleted = tp.IsCompleted
                    }).ToList(),
                FinalTestProgress = progressDetails.FinalTestProgresses
                    .Where(f => f.BlockId == bcp.BlockId)
                    .Select(f => new GetFinalTestProgressResponse
                    {
                        IsPassed = f.IsPassed,
                        CorrectAnswersCount = f.CorrectAnswersCount,
                        PassedAt = f.PassedAt
                    }).FirstOrDefault()
            }).ToList()
        };
    }
}
