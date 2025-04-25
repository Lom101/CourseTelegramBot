using Backend.Dto.Audio.Request;
using Backend.Dto.Audio.Response;
using Backend.Dto.Book.Request;
using Backend.Dto.Book.Response;
using Backend.Dto.Image.Request;
using Backend.Dto.Image.Response;
using Backend.Dto.WordFile.Request;
using Backend.Dto.WordFile.Response;
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
        };

        // Маппинг для AudioContent
        public static GetAudioContentResponse ToAudioDto(AudioContent content) => new GetAudioContentResponse
        {
            Id = content.Id,
            AudioUrl = content.AudioUrl,
            Title = content.Title,
        };

        // Маппинг для WordFileContent
        public static GetWordFileContentResponse ToWordDto(WordFileContent content) => new GetWordFileContentResponse
        {
            Id = content.Id,
            FileUrl = content.FileUrl,
            FileName = content.FileName,
            Title = content.Title,
        };

        // Маппинг для ImageContent
        public static GetImageContentResponse ToImageDto(ImageContent content) => new GetImageContentResponse
        {
            Id = content.Id,
            FileUrl = content.ImageUrl,
            AltText = content.AltText,
            Title = content.Title,
        };
        
        public static WordFileContent ToEntity(CreateWordFileContentRequest request, string fileUrl)
        {
            return new WordFileContent
            {
                TopicId = request.TopicId,
                Title = request.Title,
                FileName = Path.GetFileName(fileUrl),
                FileUrl = fileUrl
            };
        }

        public static BookContent ToEntity(CreateBookContentRequest request, string fileUrl)
        {
            return new BookContent
            {
                TopicId = request.TopicId,
                FileUrl = fileUrl
            };
        }

        public static AudioContent ToEntity(CreateAudioContentRequest request, string audioUrl)
        {
            return new AudioContent
            {
                TopicId = request.TopicId,
                Title = request.Title,
                AudioUrl = audioUrl
            };
        }

        public static ImageContent ToEntity(CreateImageContentRequest request, string imageUrl)
        {
            return new ImageContent
            {
                TopicId = request.TopicId,
                ImageUrl = imageUrl
            };
        }
    }
}