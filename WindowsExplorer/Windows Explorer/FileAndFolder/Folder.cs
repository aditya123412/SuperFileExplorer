using Windows_Explorer.Misc;

namespace Windows_Explorer.FileAndFolder
{
    public class Folder : FFBase
    {
        public List<FFBase> Items { get; set; }
        public Folder()
        {
            this.DataActions.Add("Open", (FFBase f) =>
            {
                Context.Navigate(f.FullPath);
                return f;
            });
        }
    }
}
