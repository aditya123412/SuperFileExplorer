namespace Windows_Explorer.Forms
{
    partial class ActionsMenu
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
            this.PropertiesBtn = new System.Windows.Forms.Button();
            this.DeleteBtn = new System.Windows.Forms.Button();
            this.RenameBtn = new System.Windows.Forms.Button();
            this.AddToClipBoardBtn = new System.Windows.Forms.Button();
            this.SetAsDestinationBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PropertiesBtn
            // 
            this.PropertiesBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.PropertiesBtn.Location = new System.Drawing.Point(0, 157);
            this.PropertiesBtn.Margin = new System.Windows.Forms.Padding(0);
            this.PropertiesBtn.Name = "PropertiesBtn";
            this.PropertiesBtn.Size = new System.Drawing.Size(145, 35);
            this.PropertiesBtn.TabIndex = 7;
            this.PropertiesBtn.Text = "Properties";
            this.PropertiesBtn.UseVisualStyleBackColor = true;
            this.PropertiesBtn.Click += new System.EventHandler(this.button4_Click);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.DeleteBtn.Location = new System.Drawing.Point(0, 122);
            this.DeleteBtn.Margin = new System.Windows.Forms.Padding(0);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(145, 35);
            this.DeleteBtn.TabIndex = 6;
            this.DeleteBtn.Text = "Delete";
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.button3_Click);
            // 
            // RenameBtn
            // 
            this.RenameBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.RenameBtn.Location = new System.Drawing.Point(0, 87);
            this.RenameBtn.Margin = new System.Windows.Forms.Padding(0);
            this.RenameBtn.Name = "RenameBtn";
            this.RenameBtn.Size = new System.Drawing.Size(145, 35);
            this.RenameBtn.TabIndex = 5;
            this.RenameBtn.Text = "Rename";
            this.RenameBtn.UseVisualStyleBackColor = true;
            this.RenameBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // AddToClipBoardBtn
            // 
            this.AddToClipBoardBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.AddToClipBoardBtn.Location = new System.Drawing.Point(0, 0);
            this.AddToClipBoardBtn.Margin = new System.Windows.Forms.Padding(0);
            this.AddToClipBoardBtn.Name = "AddToClipBoardBtn";
            this.AddToClipBoardBtn.Size = new System.Drawing.Size(145, 35);
            this.AddToClipBoardBtn.TabIndex = 4;
            this.AddToClipBoardBtn.Text = "Copy to Clipboard";
            this.AddToClipBoardBtn.UseVisualStyleBackColor = true;
            this.AddToClipBoardBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // SetAsDestinationBtn
            // 
            this.SetAsDestinationBtn.Location = new System.Drawing.Point(0, 207);
            this.SetAsDestinationBtn.Name = "SetAsDestinationBtn";
            this.SetAsDestinationBtn.Size = new System.Drawing.Size(145, 33);
            this.SetAsDestinationBtn.TabIndex = 8;
            this.SetAsDestinationBtn.Text = "Set as Destination";
            this.SetAsDestinationBtn.UseVisualStyleBackColor = true;
            this.SetAsDestinationBtn.Click += new System.EventHandler(this.SetAsDestinationBtn_Click);
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button1.Location = new System.Drawing.Point(0, 35);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 35);
            this.button1.TabIndex = 9;
            this.button1.Text = "Copy to a List...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // ActionsMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(233, 272);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.SetAsDestinationBtn);
            this.Controls.Add(this.PropertiesBtn);
            this.Controls.Add(this.DeleteBtn);
            this.Controls.Add(this.RenameBtn);
            this.Controls.Add(this.AddToClipBoardBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ActionsMenu";
            this.Text = "FileName";
            this.TopMost = true;
            this.Leave += new System.EventHandler(this.ActionsMenu_Leave);
            this.ResumeLayout(false);

        }

        #endregion

        public Button PropertiesBtn;
        public Button DeleteBtn;
        public Button RenameBtn;
        public Button AddToClipBoardBtn;
        private Button SetAsDestinationBtn;
        public Button button1;
    }
}