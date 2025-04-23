using Backend.Dto;
using Backend.Dto.Audio.Response;
using Backend.Dto.Book;
using Backend.Dto.Image;
using Backend.Dto.Image.Response;
using Core.Entity.AnyContent;

namespace Backend.Mapper
{
    public static class ContentItemMapper
    {
        // Маппинг для BookContent
        public static GetBookContentResponse ToBookDto(BookContent content) => new GetBookContentResponse
        {
            Id = content.Id,
            FileUrl = content.FileUrl,
            FileName = content.FileName,
            Title = content.Title,
            Description = content.Description
        };

        // Маппинг для AudioContent
        public static GetAudioContentResponse ToAudioDto(AudioContent content) => new GetAudioContentResponse
        {
            Id = content.Id,
            AudioUrl = content.AudioUrl,
            Title = content.Title,
            Description = content.Description
        };

        // Маппинг для WordFileContent
        public static GetWordFileContentResponse ToWordDto(WordFileContent content) => new GetWordFileContentResponse
        {
            Id = content.Id,
            FileUrl = content.FileUrl,
            FileName = content.FileName,
            Title = content.Title,
            Description = content.Description
        };

        // Маппинг для ImageContent
        public static GetImageContentResponse ToImageDto(ImageContent content) => new GetImageContentResponse
        {
            Id = content.Id,
            FileUrl = content.ImageUrl,
            AltText = content.AltText,
            Title = content.Title,
        };
        
        public static WordFileContent ToEntity(CreateWordFileContentRequest request, string fileUrl, int order)
        {
            return new WordFileContent
            {
                TopicId = request.TopicId,
                Order = order,
                Title = request.Title,
                FileName = Path.GetFileName(fileUrl),
                FileUrl = fileUrl
            };
        }

        public static BookContent ToEntity(CreateBookContentRequest request, string fileUrl, int order)
        {
            return new BookContent
            {
                TopicId = request.TopicId,
                Order = order,
                FileUrl = fileUrl
            };
        }

        public static AudioContent ToEntity(CreateAudioContentRequest request, string audioUrl, int order)
        {
            return new AudioContent
            {
                TopicId = request.TopicId,
                Order = order,
                Title = request.Title,
                AudioUrl = audioUrl
            };
        }

        public static ImageContent ToEntity(CreateImageContentRequest request, string imageUrl, int order)
        {
            return new ImageContent
            {
                TopicId = request.TopicId,
                Order = order,
                ImageUrl = imageUrl
            };
        }
    }
}