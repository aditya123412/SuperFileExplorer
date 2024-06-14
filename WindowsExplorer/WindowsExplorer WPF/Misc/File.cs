using System.Diagnostics;

namespace WindowsExplorer_WPF.Misc;

public class File : FFBase
{
    public string Extension { get; set; }
    public MediaType MediaType { get; set; }
    public File()
    {
        //this.DataActions.Add("Open", (FFBase f) =>
        //{
        //    Process.Start("explorer", "\"" + f.FullPath + "\"");
        //    return f;
        //});
    }
}
public enum MediaType { Text, Image, Video, Audio, Document, Executable, Link }
