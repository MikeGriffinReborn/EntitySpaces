namespace EntitySpaces.TemplateUI
{
    partial class GeneratedMaster_ProxyStub
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
            this.chkProxyStub = new System.Windows.Forms.CheckBox();
            this.chkManageState = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkWCFSupport = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDataContract = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkWCFEmitDefault = new System.Windows.Forms.CheckBox();
            this.chkWCFIsRequired = new System.Windows.Forms.CheckBox();
            this.chkWCFOrder = new System.Windows.Forms.CheckBox();
            this.chkCompactXML = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkProxyStub
            // 
            this.chkProxyStub.AutoSize = true;
            this.chkProxyStub.Location = new System.Drawing.Point(19, 19);
            this.chkProxyStub.Name = "chkProxyStub";
            this.chkProxyStub.Size = new System.Drawing.Size(144, 17);
            this.chkProxyStub.TabIndex = 0;
            this.chkProxyStub.Text = "Generate the Proxy/Stub";
            this.chkProxyStub.UseVisualStyleBackColor = true;
            this.chkProxyStub.CheckStateChanged += new System.EventHandler(this.chkProxyStub_CheckStateChanged);
            // 
            // chkManageState
            // 
            this.chkManageState.AutoSize = true;
            this.chkManageState.Checked = true;
            this.chkManageState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkManageState.Enabled = false;
            this.chkManageState.Location = new System.Drawing.Point(19, 43);
            this.chkManageState.Name = "chkManageState";
            this.chkManageState.Size = new System.Drawing.Size(265, 17);
            this.chkManageState.TabIndex = 1;
            this.chkManageState.Text = "Include Added/Modified/Delete state in the XML ?";
            this.chkManageState.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(310, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "---------  Windows Communication Foundation  -----------";
            // 
            // chkWCFSupport
            // 
            this.chkWCFSupport.AutoSize = true;
            this.chkWCFSupport.Enabled = false;
            this.chkWCFSupport.Location = new System.Drawing.Point(19, 92);
            this.chkWCFSupport.Name = "chkWCFSupport";
            this.chkWCFSupport.Size = new System.Drawing.Size(223, 17);
            this.chkWCFSupport.TabIndex = 3;
            this.chkWCFSupport.Text = "Enable [DataContract] requires .NET 3.0+";
            this.chkWCFSupport.UseVisualStyleBackColor = true;
            this.chkWCFSupport.CheckStateChanged += new System.EventHandler(this.chkWCFSupport_CheckStateChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "[ DataContract ]";
            // 
            // txtDataContract
            // 
            this.txtDataContract.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDataContract.Enabled = false;
            this.txtDataContract.Location = new System.Drawing.Point(19, 148);
            this.txtDataContract.Name = "txtDataContract";
            this.txtDataContract.Size = new System.Drawing.Size(307, 20);
            this.txtDataContract.TabIndex = 4;
            this.txtDataContract.Text = "http://tempuri.org/";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 183);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "[ DataMember ]  attribute(s) :";
            // 
            // chkWCFEmitDefault
            // 
            this.chkWCFEmitDefault.AutoSize = true;
            this.chkWCFEmitDefault.Checked = true;
            this.chkWCFEmitDefault.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWCFEmitDefault.Enabled = false;
            this.chkWCFEmitDefault.Location = new System.Drawing.Point(19, 211);
            this.chkWCFEmitDefault.Name = "chkWCFEmitDefault";
            this.chkWCFEmitDefault.Size = new System.Drawing.Size(135, 17);
            this.chkWCFEmitDefault.TabIndex = 5;
            this.chkWCFEmitDefault.Text = "EmitDefaultValue=false";
            this.chkWCFEmitDefault.UseVisualStyleBackColor = true;
            // 
            // chkWCFIsRequired
            // 
            this.chkWCFIsRequired.AutoSize = true;
            this.chkWCFIsRequired.Enabled = false;
            this.chkWCFIsRequired.Location = new System.Drawing.Point(19, 235);
            this.chkWCFIsRequired.Name = "chkWCFIsRequired";
            this.chkWCFIsRequired.Size = new System.Drawing.Size(101, 17);
            this.chkWCFIsRequired.TabIndex = 6;
            this.chkWCFIsRequired.Text = "IsRequired=true";
            this.chkWCFIsRequired.UseVisualStyleBackColor = true;
            // 
            // chkWCFOrder
            // 
            this.chkWCFOrder.AutoSize = true;
            this.chkWCFOrder.Checked = true;
            this.chkWCFOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWCFOrder.Enabled = false;
            this.chkWCFOrder.Location = new System.Drawing.Point(19, 259);
            this.chkWCFOrder.Name = "chkWCFOrder";
            this.chkWCFOrder.Size = new System.Drawing.Size(58, 17);
            this.chkWCFOrder.TabIndex = 7;
            this.chkWCFOrder.Text = "Order=";
            this.chkWCFOrder.UseVisualStyleBackColor = true;
            // 
            // chkCompactXML
            // 
            this.chkCompactXML.AutoSize = true;
            this.chkCompactXML.Enabled = false;
            this.chkCompactXML.Location = new System.Drawing.Point(19, 295);
            this.chkCompactXML.Name = "chkCompactXML";
            this.chkCompactXML.Size = new System.Drawing.Size(93, 17);
            this.chkCompactXML.TabIndex = 10;
            this.chkCompactXML.Text = "Compact XML";
            this.chkCompactXML.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(16, 279);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(315, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "-----------------------------------------------------------------------------";
            // 
            // GeneratedMaster_ProxyStub
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkCompactXML);
            this.Controls.Add(this.chkWCFOrder);
            this.Controls.Add(this.chkWCFIsRequired);
            this.Controls.Add(this.chkWCFEmitDefault);
            this.Controls.Add(this.txtDataContract);
            this.Controls.Add(this.chkWCFSupport);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkManageState);
            this.Controls.Add(this.chkProxyStub);
            this.Name = "GeneratedMaster_ProxyStub";
            this.Size = new System.Drawing.Size(339, 483);
            this.Load += new System.EventHandler(this.GeneratedMaster_ProxyStub_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkProxyStub;
        private System.Windows.Forms.CheckBox chkManageState;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkWCFSupport;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDataContract;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkWCFEmitDefault;
        private System.Windows.Forms.CheckBox chkWCFIsRequired;
        private System.Windows.Forms.CheckBox chkWCFOrder;
        private System.Windows.Forms.CheckBox chkCompactXML;
        private System.Windows.Forms.Label label5;
    }
}
