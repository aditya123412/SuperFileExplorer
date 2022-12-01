using WindowsExplorer_WPF.Misc;

namespace WindowsExplorer_WPF.Misc;
public class Folder : FFBase
{
    public System.Collections.Generic.List<FFBase> Items { get; set; }
    public Folder()
    {
        //this.DataActions.Add("Open", (FFBase f) =>
        //{
        //    Context.Navigate(f.FullPath);
        //    return f;
        //});
    }

}
