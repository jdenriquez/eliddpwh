namespace ReportUtility
{
    partial class Maitenance
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Maitenance));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtReportTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtApproverPos = new System.Windows.Forms.TextBox();
            this.txtApproverDept = new System.Windows.Forms.TextBox();
            this.txtApproverName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtCertifierPos = new System.Windows.Forms.TextBox();
            this.txtCertifierDept = new System.Windows.Forms.TextBox();
            this.txtCertifierName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtReportTitle);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(544, 85);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // txtReportTitle
            // 
            this.txtReportTitle.Location = new System.Drawing.Point(130, 37);
            this.txtReportTitle.Name = "txtReportTitle";
            this.txtReportTitle.Size = new System.Drawing.Size(396, 20);
            this.txtReportTitle.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Report Title:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnClose);
            this.groupBox3.Controls.Add(this.btnSave);
            this.groupBox3.Controls.Add(this.txtApproverPos);
            this.groupBox3.Controls.Add(this.txtApproverDept);
            this.groupBox3.Controls.Add(this.txtApproverName);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox3.Location = new System.Drawing.Point(0, 236);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(544, 149);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Approver";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(451, 120);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(370, 120);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtApproverPos
            // 
            this.txtApproverPos.Location = new System.Drawing.Point(130, 84);
            this.txtApproverPos.Name = "txtApproverPos";
            this.txtApproverPos.Size = new System.Drawing.Size(396, 20);
            this.txtApproverPos.TabIndex = 9;
            // 
            // txtApproverDept
            // 
            this.txtApproverDept.Location = new System.Drawing.Point(130, 53);
            this.txtApproverDept.Name = "txtApproverDept";
            this.txtApproverDept.Size = new System.Drawing.Size(396, 20);
            this.txtApproverDept.TabIndex = 8;
            // 
            // txtApproverName
            // 
            this.txtApproverName.Location = new System.Drawing.Point(130, 23);
            this.txtApproverName.Name = "txtApproverName";
            this.txtApproverName.Size = new System.Drawing.Size(396, 20);
            this.txtApproverName.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(13, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(111, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Approver Position:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(13, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Approver Department:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Approver Name:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtCertifierPos);
            this.groupBox2.Controls.Add(this.txtCertifierDept);
            this.groupBox2.Controls.Add(this.txtCertifierName);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 85);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(544, 151);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Certifier";
            // 
            // txtCertifierPos
            // 
            this.txtCertifierPos.Location = new System.Drawing.Point(130, 104);
            this.txtCertifierPos.Name = "txtCertifierPos";
            this.txtCertifierPos.Size = new System.Drawing.Size(396, 20);
            this.txtCertifierPos.TabIndex = 6;
            // 
            // txtCertifierDept
            // 
            this.txtCertifierDept.Location = new System.Drawing.Point(130, 70);
            this.txtCertifierDept.Name = "txtCertifierDept";
            this.txtCertifierDept.Size = new System.Drawing.Size(396, 20);
            this.txtCertifierDept.TabIndex = 5;
            // 
            // txtCertifierName
            // 
            this.txtCertifierName.Location = new System.Drawing.Point(130, 40);
            this.txtCertifierName.Name = "txtCertifierName";
            this.txtCertifierName.Size = new System.Drawing.Size(396, 20);
            this.txtCertifierName.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Certifier Position:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 18);
            this.label3.TabIndex = 3;
            this.label3.Text = "Certifier Department:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Certifier Name:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Maitenance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 385);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Maitenance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report Maitenance";
            this.Load += new System.EventHandler(this.Maitenance_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtReportTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtCertifierName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtApproverPos;
        private System.Windows.Forms.TextBox txtApproverDept;
        private System.Windows.Forms.TextBox txtApproverName;
        private System.Windows.Forms.TextBox txtCertifierPos;
        private System.Windows.Forms.TextBox txtCertifierDept;
    }
}