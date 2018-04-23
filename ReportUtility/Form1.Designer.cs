namespace ReportUtility
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menu1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.strip1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.group2 = new System.Windows.Forms.GroupBox();
            this.lvFile = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.crv = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu1.SuspendLayout();
            this.group2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu1
            // 
            this.menu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.generateReportToolStripMenuItem});
            this.menu1.Location = new System.Drawing.Point(0, 0);
            this.menu1.Name = "menu1";
            this.menu1.Size = new System.Drawing.Size(645, 24);
            this.menu1.TabIndex = 7;
            this.menu1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.strip1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // strip1
            // 
            this.strip1.Name = "strip1";
            this.strip1.Size = new System.Drawing.Size(181, 22);
            this.strip1.Text = "Report Maintenance";
            this.strip1.Click += new System.EventHandler(this.signatoriesToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // generateReportToolStripMenuItem
            // 
            this.generateReportToolStripMenuItem.Name = "generateReportToolStripMenuItem";
            this.generateReportToolStripMenuItem.Size = new System.Drawing.Size(104, 20);
            this.generateReportToolStripMenuItem.Text = "Generate Report";
            this.generateReportToolStripMenuItem.Click += new System.EventHandler(this.generateReportToolStripMenuItem_Click);
            // 
            // group2
            // 
            this.group2.Controls.Add(this.lvFile);
            this.group2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.group2.Location = new System.Drawing.Point(3, 19);
            this.group2.Name = "group2";
            this.group2.Size = new System.Drawing.Size(639, 407);
            this.group2.TabIndex = 6;
            this.group2.TabStop = false;
            this.group2.Enter += new System.EventHandler(this.group2_Enter);
            // 
            // lvFile
            // 
            this.lvFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFile.FullRowSelect = true;
            this.lvFile.GridLines = true;
            this.lvFile.HoverSelection = true;
            this.lvFile.Location = new System.Drawing.Point(3, 16);
            this.lvFile.Name = "lvFile";
            this.lvFile.Size = new System.Drawing.Size(633, 388);
            this.lvFile.TabIndex = 0;
            this.lvFile.UseCompatibleStateImageBehavior = false;
            this.lvFile.View = System.Windows.Forms.View.Details;
            this.lvFile.SelectedIndexChanged += new System.EventHandler(this.lvFile_SelectedIndexChanged);
            this.lvFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvFile_DragDrop);
            this.lvFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvFile_DragEnter);
            this.lvFile.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvFile_KeyDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.crv);
            this.groupBox1.Controls.Add(this.group2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(645, 429);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // crv
            // 
            this.crv.ActiveViewIndex = -1;
            this.crv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crv.Cursor = System.Windows.Forms.Cursors.Default;
            this.crv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crv.Location = new System.Drawing.Point(3, 16);
            this.crv.Name = "crv";
            this.crv.Size = new System.Drawing.Size(639, 3);
            this.crv.TabIndex = 7;
            this.crv.Visible = false;
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
            this.editToolStripMenuItem.Text = "Delete CSV";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 456);
            this.Controls.Add(this.menu1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report Utility";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menu1.ResumeLayout(false);
            this.menu1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menu1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem strip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateReportToolStripMenuItem;
        private System.Windows.Forms.GroupBox group2;
        private System.Windows.Forms.ListView lvFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crv;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    }
}

