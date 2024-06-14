using Newtonsoft.Json;

namespace Windows_Explorer.Misc
{
    public class ImageTuple
    {
        public ImageTuple(string key, Image value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        [JsonConverter(typeof(ImageConverter))]
        public Image Value { get; set; }
    }
}