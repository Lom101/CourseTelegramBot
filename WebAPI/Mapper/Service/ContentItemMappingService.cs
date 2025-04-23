using Core.Entity.AnyContent;

namespace Backend.Mapper.Service;

public static class ContentItemMappingService
{
    public static object MapToDto(ContentItem contentItem)
    {
        return contentItem switch
        {
            BookContent book => ContentItemMapper.ToBookDto(book),
            AudioContent audio => ContentItemMapper.ToAudioDto(audio),
            WordFileContent word => ContentItemMapper.ToWordDto(word),
            ImageContent image => ContentItemMapper.ToImageDto(image),
            _ => throw new InvalidOperationException("Unknown content type")
        };
    }
}
