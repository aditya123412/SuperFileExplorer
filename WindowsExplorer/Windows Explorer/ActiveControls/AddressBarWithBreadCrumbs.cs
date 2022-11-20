using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windows_Explorer.ActiveControls
{
    public partial class AddressBarWithBreadCrumbs : UserControl
    {
        private List<Control> _breadcrumbs = new List<Control>();
        public Func<string, object> Navigate { get; set; }
        public string Text
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }
        public AddressBarWithBreadCrumbs()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Set();
        }

        private void Set()
        {
            _breadcrumbs.ForEach(b => panel1.Controls.Remove(b));
            _breadcrumbs.Clear();
            var breadCrumbs = textBox1.Text.Split(Path.DirectorySeparatorChar);
            textBox1.Visible = false;
            string partialPath = "";
            int left = 0;
            foreach (var breadCrumb in breadCrumbs)
            {
                partialPath = Path.Combine(partialPath, breadCrumb);
                var crumb = new Button() { Visible = true, Text = breadCrumb, Font = textBox1.Font, Left = left, Tag = partialPath, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
                crumb.Refresh();
                crumb.Click += new EventHandler(_Navigate);
                this.panel1.Controls.Add(crumb);
                left += crumb.Width;
                _breadcrumbs.Add(crumb);
            }
        }

        private void _Navigate(object? sender, EventArgs e)
        {
            Navigate((sender as Button).Tag as string);
        }

        private void Unset()
        {
            _breadcrumbs.ForEach(b => panel1.Controls.Remove(b));
            _breadcrumbs = new List<Control> { };
            textBox1.Visible = true;
            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Navigate(textBox1.Text);
        }

        private void textBox1_LostFocus(object sender, EventArgs e)
        {
            Set();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Navigate("");
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AddressBarWithBreadCrumbs_Load(object sender, EventArgs e)
        {

        }
    }
}
