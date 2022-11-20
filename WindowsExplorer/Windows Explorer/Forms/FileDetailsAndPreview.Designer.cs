namespace Windows_Explorer.Forms
{
    partial class FileDetailsAndPreview
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.VisualPreviewPane = new System.Windows.Forms.GroupBox() { AutoSizeMode = AutoSizeMode.GrowOnly, AutoSize = true };
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.AttributePane = new System.Windows.Forms.GroupBox() { AutoSizeMode = AutoSizeMode.GrowOnly, AutoSize = true };
            this.PropValue = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.PropName = new System.Windows.Forms.TextBox();
            this.TagPane = new System.Windows.Forms.GroupBox() { AutoSizeMode = AutoSizeMode.GrowOnly, AutoSize = true };
            this.button2 = new System.Windows.Forms.Button();
            this.TagInput = new System.Windows.Forms.TextBox();
            this.VisualPreviewPane.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.AttributePane.SuspendLayout();
            this.TagPane.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.VisualPreviewPane.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VisualPreviewPane.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.VisualPreviewPane.Controls.Add(this.pictureBox1);
            this.VisualPreviewPane.Location = new System.Drawing.Point(12, 11);
            this.VisualPreviewPane.Name = "groupBox1";
            this.VisualPreviewPane.Size = new System.Drawing.Size(258, 134);
            this.VisualPreviewPane.TabIndex = 0;
            this.VisualPreviewPane.TabStop = false;
            this.VisualPreviewPane.Text = "Preview";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = PreviewBitmap;
            this.pictureBox1.Location = new System.Drawing.Point(6, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(246, 106);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.AttributePane.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AttributePane.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AttributePane.Controls.Add(this.PropValue);
            this.AttributePane.Controls.Add(this.button1);
            this.AttributePane.Controls.Add(this.PropName);
            this.AttributePane.Location = new System.Drawing.Point(12, 151);
            this.AttributePane.Name = "groupBox2";
            this.AttributePane.Size = new System.Drawing.Size(258, 58);
            this.AttributePane.TabIndex = 1;
            this.AttributePane.TabStop = false;
            this.AttributePane.Text = "Properties and Attributes";
            this.Attributes = new Dictionary<string, Object>();
            foreach (var item in this.Attributes)
            {

            }
            // 
            // PropValue
            // 
            this.PropValue.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.PropValue.Location = new System.Drawing.Point(94, 29);
            this.PropValue.Name = "PropValue";
            this.PropValue.Size = new System.Drawing.Size(93, 23);
            this.PropValue.TabIndex = 2;
            // 
            // PropName
            // 
            this.PropName.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.PropName.Location = new System.Drawing.Point(6, 29);
            this.PropName.Name = "PropName";
            this.PropName.Size = new System.Drawing.Size(86, 23);
            this.PropName.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(193, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.TagPane.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TagPane.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.TagPane.Controls.Add(this.button2);
            this.TagPane.Controls.Add(this.TagInput);
            this.TagPane.Location = new System.Drawing.Point(12, 219);
            this.TagPane.Name = "groupBox3";
            this.TagPane.Size = new System.Drawing.Size(258, 75);
            this.TagPane.TabIndex = 2;
            this.TagPane.TabStop = false;
            this.TagPane.Text = "Tags";
            // 
            // TagInput
            // 
            int tagTop = 20;
            this.Tags = this.Tags == null ? new List<string>() : Tags;
            foreach (var item in this.Tags)
            {
                var TagBox = new TextBox() { Parent = TagInput.Parent, Width = TagPane.Width - 40, Visible = true, Text = item, Top = tagTop, Left = TagInput.Left };
                tagTop = TagBox.Bottom + 5;
            }
            this.TagInput.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.TagInput.Location = new System.Drawing.Point(6, 46);
            this.TagInput.Name = "TagInput";
            this.TagInput.Size = new System.Drawing.Size(181, 23);
            this.TagInput.TabIndex = 3;
            this.TagInput.Top = tagTop;
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button2.Location = new System.Drawing.Point(193, 46);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(59, 23);
            this.button2.TabIndex = 4;
            this.button2.Top = tagTop;
            this.button2.Text = "Add";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // FileDetailsAndPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 306);
            this.Controls.Add(this.TagPane);
            this.Controls.Add(this.AttributePane);
            this.Controls.Add(this.VisualPreviewPane);
            this.Name = "FileDetailsAndPreview";
            this.Text = "FileDetailsAndPreview";
            this.Load += new System.EventHandler(this.FileDetailsAndPreview_Load);
            this.VisualPreviewPane.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.AttributePane.ResumeLayout(false);
            this.AttributePane.PerformLayout();
            this.TagPane.ResumeLayout(false);
            this.TagPane.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox VisualPreviewPane;
        private GroupBox AttributePane;
        private GroupBox TagPane;
        private PictureBox pictureBox1;
        private TextBox PropValue;
        private Button button1;
        private TextBox PropName;
        private Button button2;
        private TextBox TagInput;
    }
}