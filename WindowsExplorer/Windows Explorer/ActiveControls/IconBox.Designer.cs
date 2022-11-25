using Windows_Explorer.FileAndFolder;
using Windows_Explorer.Misc;
using DataObject = Classes.Operations.DataObject;

namespace Windows_Explorer.ActiveControls
{
    partial class IconBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void CreateNormalIcon()
        {
            this.Label = new System.Windows.Forms.Label();
            this.Display = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Display)).BeginInit();
            this.SuspendLayout();
            // 
            // Label
            // 
            this.Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label.BackColor = System.Drawing.Color.Transparent;
            this.Label.Enabled = false;
            this.Label.Location = new System.Drawing.Point(0, 0);
            this.Label.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(125, 77);
            this.Label.TabIndex = 3;
            this.Label.Text = (this.fileItem as FFBase).Name;
            this.Label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Label.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyUpEventHandler);
            this.Label.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownEventHandler);
            this.Label.KeyUp += new System.Windows.Forms.KeyEventHandler(Context.KeyUpEventHandler);
            this.Label.KeyDown += new System.Windows.Forms.KeyEventHandler(Context.KeyDownEventHandler);
            //this.Label.MouseClick += new System.Windows.Forms.MouseEventHandler(this.IconBox_Click);
            //this.Label.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IconBox_DoubleClick);
            // 
            // Display
            // 
            this.Display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Display.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Display.Location = new System.Drawing.Point(0, 0);
            this.Display.Margin = new System.Windows.Forms.Padding(0);
            this.Display.Name = "Display";
            this.Display.Size = new System.Drawing.Size(125, 125);
            this.Display.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Display.TabIndex = 2;
            this.Display.TabStop = false;
            this.Display.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyUpEventHandler);
            this.Display.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownEventHandler);
            this.Display.KeyUp += new System.Windows.Forms.KeyEventHandler(Context.KeyUpEventHandler);
            this.Display.KeyDown += new System.Windows.Forms.KeyEventHandler(Context.KeyDownEventHandler);
            //this.Display.MouseClick += new System.Windows.Forms.MouseEventHandler(this.IconBox_Click);
            //this.Display.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IconBox_DoubleClick);
            if (fileItem.Type != FileAndFolder.Type.Folder && fileItem.DefaultIcon != null)
            {
                Display.Image = fileItem.DefaultIcon;
            }
            if (fileItem.Thumbnail != null)
            {
                Display.Image = fileItem.Thumbnail;
            }
            // 
            // IconBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.Label);
            this.Controls.Add(this.Display);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "IconBox";
            this.Size = new System.Drawing.Size(125, 202);
            this.SizeChanged += new System.EventHandler(this.IconBox_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownEventHandler);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyUpEventHandler);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(Context.KeyUpEventHandler);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(Context.KeyDownEventHandler);
            this.TabStop = true;
            //this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.IconBox_Click);
            //this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IconBox_DoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.Display)).EndInit();
            this.ResumeLayout(false);

        }

        private void IconBox_SizeChanged(object sender, EventArgs e)
        {
        }

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void CreateTextOnlyIcon()
        {
            this.Label = new System.Windows.Forms.Label();
            this.Display = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Display)).BeginInit();
            this.SuspendLayout();
            // 
            // Label
            // 
            this.Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Label.BackColor = System.Drawing.Color.Transparent;
            this.Label.Enabled = false;
            this.Label.Location = new System.Drawing.Point(0, 0);
            this.Label.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(125, 77);
            this.Label.TabIndex = 3;
            this.Label.Text = (this.fileItem as FFBase).Name;
            this.Label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Label.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyUpEventHandler);
            this.Label.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownEventHandler);
            this.Label.KeyUp += new System.Windows.Forms.KeyEventHandler(Context.KeyUpEventHandler);
            this.Label.KeyDown += new System.Windows.Forms.KeyEventHandler(Context.KeyDownEventHandler);
            this.Label.Click += new System.EventHandler(this.IconBox_Click);
            this.Label.DoubleClick += new System.EventHandler(this.IconBox_DoubleClick);
            this.Label.MouseClick += new System.Windows.Forms.MouseEventHandler(this.IconBox_Click);
            this.Label.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IconBox_DoubleClick);

            // 
            // IconBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.Label);
            this.Controls.Add(this.Display);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "IconBox";
            this.Size = new System.Drawing.Size(125, 202);
            this.SizeChanged += new System.EventHandler(this.IconBox_SizeChanged);
            this.Click += new System.EventHandler(this.IconBox_Click);
            this.DoubleClick += new System.EventHandler(this.IconBox_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownEventHandler);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(Context.KeyUpEventHandler);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(Context.KeyDownEventHandler);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyUpEventHandler);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.IconBox_Click);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IconBox_DoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.Display)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void CreateImageOnlyIcon()
        {
            this.Label = new System.Windows.Forms.Label();
            this.Display = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Display)).BeginInit();
            // 
            // Display
            // 
            this.Display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Display.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Display.Location = new System.Drawing.Point(0, 0);
            this.Display.Margin = new System.Windows.Forms.Padding(0);
            this.Display.Name = "Display";
            this.Display.Size = new System.Drawing.Size(125, 125);
            this.Display.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Display.TabIndex = 2;
            this.Display.TabStop = false;
            this.Display.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyUpEventHandler);
            this.Display.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownEventHandler);
            this.Display.Click += new System.EventHandler(this.IconBox_Click);
            this.Display.DoubleClick += new System.EventHandler(this.IconBox_DoubleClick);
            this.Display.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IconBox_DoubleClick);
            if (fileItem.Type != FileAndFolder.Type.Folder)
            {
                Display.Image = fileItem.DefaultIcon;
            }
            if (fileItem.DefaultIcon != null)
            {
                Display.Image = fileItem.DefaultIcon;
            }
            this.SuspendLayout();
            // 
            // IconBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.Label);
            this.Controls.Add(this.Display);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "IconBox";
            this.Size = new System.Drawing.Size(125, 202);
            this.SizeChanged += new System.EventHandler(this.IconBox_SizeChanged);
            this.Click += new System.EventHandler(this.IconBox_Click);
            this.DoubleClick += new System.EventHandler(this.IconBox_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownEventHandler);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyUpEventHandler);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.IconBox_Click);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IconBox_DoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.Display)).EndInit();
            this.ResumeLayout(false);

        }

        public Point PointAt(int v1, int v2)
        {
            return new System.Drawing.Point(v1, v2);
        }

        #endregion

        private Label Label;
        public PictureBox Display;
    }
}
