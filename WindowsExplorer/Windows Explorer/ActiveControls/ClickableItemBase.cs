using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows_Explorer.ActiveControls
{
    public class ClickableItemBase : UserControl
    {
        public const string CLICKED = "Click";
        public const string DOUBLECLICKED = "DoubleClick";
        public const string KEYDOWN = "KeyDown";
        public const string KEYUP = "KeyUp";

        public Func<ClickableItemBase, object> DblClicked;
        public Func<ClickableItemBase, object> Clicked;
        public Func<KeyEventArgs, object> Key_Down;
        public Func<KeyEventArgs, object> Key_Up;
        public string Data;

        //Names of FunctionDelegates that run with itself as the parameter
        public Dictionary<string, Func<ClickableItemBase, object>> ClickableBaseActions = new Dictionary<string, Func<ClickableItemBase, object>>
        {
            {CLICKED , (ClickableItemBase cb)=>{return cb; }},
            {DOUBLECLICKED , (ClickableItemBase cb)=>{return cb; }},
            {KEYDOWN , (ClickableItemBase cb)=>{return cb; }},
            {KEYUP , (ClickableItemBase cb)=>{return cb; }}
        };
        public bool FullWidth { get; set; }
        public bool FullHeight { get; set; }
    }
}
