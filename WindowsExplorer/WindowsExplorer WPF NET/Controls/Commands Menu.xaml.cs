using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WindowsExplorer_WPF;
using WindowsExplorer_WPF_NET.Misc.Data;

namespace WindowsExplorer_WPF_NET.Controls
{
    /// <summary>
    /// Interaction logic for Commands_Menu.xaml
    /// </summary>
    public partial class Commands_Menu : Window
    {
        private Commands_Menu childWindow, parentWindow;
        public CommandsMenuContext CommandsMenuContext { get; set; }
        public bool Locked { get; set; }
        public Commands_Menu Parent
        {
            set
            {
                this.parentWindow = value;
            }
            get
            {
                return this.parentWindow;
            }
        }
        public Commands_Menu ChildWindow
        {
            set
            {
                this.childWindow = value;
                if (value!=null)
                {
                    childWindow.Parent = this;
                }
            }
            get
            {
                return this.childWindow;
            }
        }

        public static Commands_Menu ShowMenu(CommandsMenuContext commandsMenuContext = null, Commands_Menu parent = null)
        {
            Commands_Menu commandsMenu = new Commands_Menu(commandsMenuContext);
            commandsMenu.Show();
            commandsMenu.SetParent(parent);
            return commandsMenu;
        }
        public Commands_Menu(CommandsMenuContext commandsMenuContext = null)
        {
            InitializeComponent();
            CommandsMenuContext = commandsMenuContext == null ? new CommandsMenuContext() : commandsMenuContext;
            DataContext = this;
            Focus();
            LostFocus += new RoutedEventHandler((sender, args) =>
            {
                try
                {
                    //this.Close();
                }
                catch (Exception)
                {

                }
            });
            MouseLeave += new MouseEventHandler((sender, args) =>
            {
                try
                {
                    //this.Close();
                }
                catch (Exception)
                {

                }
            });
        }

        public void SetParent(Commands_Menu parent)
        {
            if (parent == null)
            {
                return;
            }
            this.Parent = parent;
            this.Parent.Locked = true;
            this.Closed += new EventHandler((o, e) => { parent.Locked = false; });
        }

        private void CloseMenu(object sender, MouseEventArgs e)
        {
            if (!Locked)
                Close();
        }

        private void ItemHover(object _sender, MouseEventArgs e)
        {
            var sender = _sender as TextBlock;

            var relativePosition = e.GetPosition(sender);
            var point = sender.PointToScreen(relativePosition);
            if (sender != null)
            {
                var opGroups = sender.DataContext as OpGroups;
                if (ChildWindow != null && !ChildWindow.CommandsMenuContext.OpGroups.Equals(opGroups))
                {
                    ChildWindow.Close();
                }
                else if (ChildWindow != null && ChildWindow.CommandsMenuContext.OpGroups.Equals(opGroups) || opGroups.IsEmpty)
                {
                    return;
                }
                var context = new CommandsMenuContext() { OpGroups = opGroups, };

                this.ChildWindow = ShowMenu(context, this);
                this.ChildWindow.Left = point.X;
                this.ChildWindow.Top = point.Y;
            }
        }
        private void CollapseMenus(OpGroups except)
        {
            CommandsMenuContext.OpGroups.CollapseAll(except);
            CommandsMenuContext.OpGroups.IsExpanded = true;
        }
    }
}
