using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsExplorer_WPF.Misc;

namespace WindowsExplorer_WPF_NET.Misc.Actions
{
    public abstract class ActionFilters
    {
        public abstract bool CheckFilterTrue(IEnumerable<ActionFilterConditions> conditions);
    }
    public class ActionFilterConditions
    {
        public object KeyCombination { get; set; }
        public string MainViewPath { get; set; }
        public FFBase ClickedIcon { get; set; }
        public object InViewGroup { get; set; }
        public IEnumerable<FFBase> SelectedItems { get; set; } = Enumerable.Empty<FFBase>();
        public Func<string, bool> ExistsList;
        public Func<FFBase, bool> ExistsItemInList;

    }
    public enum ActionTriggerItem
    {
        MainViewAreaSpace, MainViewIcon, MainViewGroupNameHeader, TreeViewIcon, BreadCrumb, ApplicationWide, MainViewSelectedItems
    }

}
