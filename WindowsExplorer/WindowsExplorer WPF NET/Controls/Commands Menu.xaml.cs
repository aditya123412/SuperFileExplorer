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
using WindowsExplorer_WPF_NET.Misc.Data;

namespace WindowsExplorer_WPF_NET.Controls
{
    /// <summary>
    /// Interaction logic for Commands_Menu.xaml
    /// </summary>
    public partial class Commands_Menu : Window
    {
        public CommandsMenuContext CommandsMenuContext { get; set; }

        public static Commands_Menu ShowMenu(CommandsMenuContext commandsMenuContext = null)
        {
            Commands_Menu commandsMenu = new Commands_Menu(commandsMenuContext);
            return commandsMenu;
        }
        public Commands_Menu(CommandsMenuContext commandsMenuContext = null)
        {
            InitializeComponent();
            CommandsMenuContext = commandsMenuContext ?? new CommandsMenuContext();
            this.DataContext = this;
            CommandsMenuContext.CommandsGroups.Add(new CommandsCategory("View"));
            CommandsMenuContext.CommandsGroups.Add(new CommandsCategory("Group"));
            CommandsMenuContext.CommandsGroups.Add(new CommandsCategory("Folder"));
            this.Focus();
            this.LostFocus += new RoutedEventHandler((sender, args) =>
            {
                try
                {
                    this.Close();
                }
                catch (Exception)
                {

                }
            });
            this.MouseLeave += new MouseEventHandler((sender, args) =>
            {
                try
                {
                    this.Close();
                }
                catch (Exception)
                {

                }
            });
        }
    }
}
