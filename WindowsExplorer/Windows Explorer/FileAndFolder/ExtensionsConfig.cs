using System.Text.Json;
using static Windows_Explorer.FileAndFolder.File;

namespace Windows_Explorer.FileAndFolder
{
    public class ExtensionsConfig
    {
        private Dictionary<string, MediaType> MediaTypes;
        private Dictionary<string, bool> hasTags;
        private Dictionary<string, bool> supportsThumbnails;

        public Dictionary<MediaType, Image> DefaultIcons;
        public (MediaType type, Image DefaultIcon) GetConfig(string extension)
        {
            var mediaType = MediaTypes.GetValueOrDefault(extension, MediaType.Document);
            return (mediaType, default);
        }

        public void Save(string path)
        {
            System.IO.File.WriteAllText(path, JsonSerializer.Serialize(this));
        }
        public static ExtensionsConfig Load(string path)
        {
            return JsonSerializer.Deserialize<ExtensionsConfig>(path);
        }

        public MediaType GetMediaType(string extension)
        {
            return MediaTypes.GetValueOrDefault(extension, MediaType.Document);
        }

        public bool SupportsThumbnails(string extension)
        {
            return supportsThumbnails.GetValueOrDefault(extension, false);
        }

        public bool HasTags(string extension)
        {
            return hasTags.GetValueOrDefault(extension, false);
        }
    }
}
