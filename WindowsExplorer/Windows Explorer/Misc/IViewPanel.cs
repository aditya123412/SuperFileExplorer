using Windows_Explorer.ActiveControls;

namespace Windows_Explorer.Misc
{
    public abstract class IViewPanel : Panel
    {
        public bool CtrlKeyPressed, CtrlKeyReleased;
        public bool AltKeyPressed, AltKeyReleased;
        public bool ShiftKeyPressed, ShiftKeyReleased;

        public bool MultiSelect { get; set; }
        public ViewType ViewType { get; set; }

        public int CursorPosition { get; set; }
        public abstract IViewPanel Get(PanelConfig panelConfig);

        public abstract List<ClickableItemBase> GetSelected();
        public List<FileAndFolder.FFBase> FSItems;
        public abstract void UnselectAll();
        public abstract void SelectAll();
        public abstract int GetCursorPosition();
        public abstract void SetCursorPosition(int position);
    }

    public class PanelConfig
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Margin { get; set; }

        public string Title { get; set; }
        public Control ParentView { get; set; }
        public bool ScrollBars { get; set; }
        public List<IconBox> Items { get; set; }
        public FFBaseCollection Files { get; set; }
        public int Gap { get; set; }
    }
}
