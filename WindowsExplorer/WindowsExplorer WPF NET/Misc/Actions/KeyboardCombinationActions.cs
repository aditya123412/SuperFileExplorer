using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WindowsExplorer_WPF_NET.Misc.Actions
{
    public class KeyboardCombinationActions
    {
        public List<KeyboardCombination> KeyboardCombinations { get; set; } = new List<KeyboardCombination>();
        public async void HandleEvent(object sender, KeyEventArgs e)
        {
            foreach (var keyCombo in KeyboardCombinations)
            {
                if (e.Key == keyCombo.Key && e.KeyboardDevice.Modifiers == keyCombo.ModifierKeys)
                {
                    keyCombo.Action();
                }
            }
        }
        public void AddKeyCombination(Action action, Key key, ModifierKeys modifierKeys)
        {
            KeyboardCombinations.Add(new KeyboardCombination()
            {
                Action = action,
                ModifierKeys = modifierKeys,
                Key = key
            });
        }
    }
    public class KeyboardCombination
    {
        public Key Key { get; set; }
        public ModifierKeys ModifierKeys { get; set; }
        public object Sender { get; set; }
        public Action Action { get; set; }
    }
}
