namespace EntitySpaces.TemplateUI
{
    partial class WCFServiceJson_Advanced
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
            this.chkHierarchicalLazyLoad = new System.Windows.Forms.CheckBox();
            this.chkHierarchicalSelectedOnly = new System.Windows.Forms.CheckBox();
            this.chkHierarchical = new System.Windows.Forms.CheckBox();
            this.chkSingleFile = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkHierarchicalLazyLoad
            // 
            this.chkHierarchicalLazyLoad.AutoSize = true;
            this.chkHierarchicalLazyLoad.Checked = true;
            this.chkHierarchicalLazyLoad.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHierarchicalLazyLoad.Location = new System.Drawing.Point(41, 81);
            this.chkHierarchicalLazyLoad.Name = "chkHierarchicalLazyLoad";
            this.chkHierarchicalLazyLoad.Size = new System.Drawing.Size(111, 17);
            this.chkHierarchicalLazyLoad.TabIndex = 8;
            this.chkHierarchicalLazyLoad.Text = "Enable Lazy Load";
            this.chkHierarchicalLazyLoad.UseVisualStyleBackColor = true;
            // 
            // chkHierarchicalSelectedOnly
            // 
            this.chkHierarchicalSelectedOnly.AutoSize = true;
            this.chkHierarchicalSelectedOnly.Location = new System.Drawing.Point(41, 59);
            this.chkHierarchicalSelectedOnly.Name = "chkHierarchicalSelectedOnly";
            this.chkHierarchicalSelectedOnly.Size = new System.Drawing.Size(127, 17);
            this.chkHierarchicalSelectedOnly.TabIndex = 9;
            this.chkHierarchicalSelectedOnly.Text = "Selected Tables Only";
            this.chkHierarchicalSelectedOnly.UseVisualStyleBackColor = true;
            // 
            // chkHierarchical
            // 
            this.chkHierarchical.AutoSize = true;
            this.chkHierarchical.Checked = true;
            this.chkHierarchical.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHierarchical.Location = new System.Drawing.Point(19, 38);
            this.chkHierarchical.Name = "chkHierarchical";
            this.chkHierarchical.Size = new System.Drawing.Size(161, 17);
            this.chkHierarchical.TabIndex = 10;
            this.chkHierarchical.Text = "Generate Hierarchical Model";
            this.chkHierarchical.UseVisualStyleBackColor = true;
            this.chkHierarchical.CheckedChanged += new System.EventHandler(this.chkHierarchical_CheckedChanged);
            // 
            // chkSingleFile
            // 
            this.chkSingleFile.AutoSize = true;
            this.chkSingleFile.Checked = true;
            this.chkSingleFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSingleFile.Enabled = false;
            this.chkSingleFile.Location = new System.Drawing.Point(19, 15);
            this.chkSingleFile.Name = "chkSingleFile";
            this.chkSingleFile.Size = new System.Drawing.Size(130, 17);
            this.chkSingleFile.TabIndex = 7;
            this.chkSingleFile.Text = "Generate a Single File";
            this.chkSingleFile.UseVisualStyleBackColor = true;
            // 
            // WCFServiceJson_Advanced
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.chkHierarchicalLazyLoad);
            this.Controls.Add(this.chkHierarchicalSelectedOnly);
            this.Controls.Add(this.chkHierarchical);
            this.Controls.Add(this.chkSingleFile);
            this.Name = "WCFServiceJson_Advanced";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Size = new System.Drawing.Size(404, 471);
            this.Load += new System.EventHandler(this.WCFServiceJson_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkHierarchicalLazyLoad;
        private System.Windows.Forms.CheckBox chkHierarchicalSelectedOnly;
        private System.Windows.Forms.CheckBox chkHierarchical;
        private System.Windows.Forms.CheckBox chkSingleFile;
    }
}
