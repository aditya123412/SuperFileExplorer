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
        private void InitializeComponent()
        {
            this.ActiveArea = new System.Windows.Forms.Panel();
            this.Label = new System.Windows.Forms.Label();
            this.Display = new System.Windows.Forms.PictureBox();
            this.ActiveArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Display)).BeginInit();
            this.SuspendLayout();
            // 
            // ActiveArea
            // 
            this.ActiveArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ActiveArea.AutoSize = true;
            this.ActiveArea.Controls.Add(this.Label);
            this.ActiveArea.Controls.Add(this.Display);
            this.ActiveArea.Location = new System.Drawing.Point(5, 5);
            this.ActiveArea.Margin = new System.Windows.Forms.Padding(5);
            this.ActiveArea.Name = "ActiveArea";
            this.ActiveArea.Padding = new System.Windows.Forms.Padding(5);
            this.ActiveArea.Size = new System.Drawing.Size(103, 150);
            this.ActiveArea.TabIndex = 0;
            // 
            // Label
            // 
            this.Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label.BackColor = System.Drawing.Color.Transparent;
            this.Label.Enabled = false;
            this.Label.Location = new System.Drawing.Point(1, 101);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(104, 44);
            this.Label.TabIndex = 1;
            this.Label.Text = "label1";
            this.Label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Display
            // 
            this.Display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Display.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Display.Location = new System.Drawing.Point(1, 1);
            this.Display.Name = "Display";
            this.Display.Size = new System.Drawing.Size(102, 97);
            this.Display.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Display.TabIndex = 0;
            this.Display.TabStop = false;
            this.Display.Click += new System.EventHandler(this.IconBox_Click);
            this.Display.DoubleClick += new System.EventHandler(this.IconBox_DoubleClick);
            // 
            // IconBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ActiveArea);
            this.Name = "IconBox";
            this.Size = new System.Drawing.Size(113, 184);
            this.SizeChanged += new System.EventHandler(this.IconBox_SizeChanged);
            this.Click += new System.EventHandler(this.IconBox_Click);
            this.DoubleClick += new System.EventHandler(this.IconBox_DoubleClick);
            this.ActiveArea.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Display)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel ActiveArea;
        private System.Windows.Forms.Label Label;
        public System.Windows.Forms.PictureBox Display;
    }
}
