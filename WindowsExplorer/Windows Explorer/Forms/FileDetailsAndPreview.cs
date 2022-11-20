using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windows_Explorer.Forms
{
    public partial class FileDetailsAndPreview : Form
    {
        public Image PreviewBitmap { get; set; }
        public List<string> Tags{ get; set; }
        public Dictionary<string, Object> Attributes { get; set; }
        private FileDetailsAndPreview()
        {
            InitializeComponent();
        }
        public FileDetailsAndPreview(Image preview, List<string> tags, Dictionary<string, Object> props)
        {
            PreviewBitmap = preview;
            Tags = tags;
            Attributes = props;
            InitializeComponent();
        }

        private void FileDetailsAndPreview_Load(object sender, EventArgs e)
        {

        }
    }
}
