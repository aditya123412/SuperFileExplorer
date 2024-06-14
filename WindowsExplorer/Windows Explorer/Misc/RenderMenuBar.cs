using System;
using System.Collections.Generic;
using System.Drawing;

namespace Windows_Explorer.Misc
{
    public class RenderMenuBar
    {
        public static System.Windows.Forms.Panel Create(System.Windows.Forms.Control parent,
            Dictionary<string, Delegate> menuItems,
            MenuOrientation orientation,
            int buttonWidth,
            int buttonHeight,
            Color menuBodyColor,
            Color buttonColor,
            Color buttonTextColor,
            int cornerRadius = 20,
            int margin = 20)
        {
            var menuBody = new System.Windows.Forms.Panel();
            menuBody.Padding = new System.Windows.Forms.Padding(margin);
            menuBody.BackColor = menuBodyColor;
            menuBody.Width = buttonWidth + 2 * margin;
            menuBody.Height = (buttonHeight + margin) * menuItems.Count;
            int i = 0;
            int drawTop = margin;
            foreach (var item in menuItems)
            {
                var button = new System.Windows.Forms.Button() { Parent = menuBody };
                menuBody.Controls.Add(button);
                button.BackColor = buttonColor;
                button.ForeColor = buttonTextColor;
                button.Top = drawTop;
                button.Left = margin;
                button.Width = buttonWidth;
                button.Height = buttonHeight;
                button.Click += (EventHandler)item.Value;

                RectangleRounder.RectangleRound(button, margin);

                i++;
                drawTop += buttonHeight + margin;
            }
            RectangleRounder.RectangleRound(menuBody, margin);
       
            menuBody.Visible = true;
            menuBody.Parent = parent;
            parent.Controls.Add(menuBody);
            parent.Invalidate();
            return menuBody;
        }
    }
    public enum MenuOrientation
    {
        Vertical, Horizontal
    }
}
