namespace ReportGeneratorApp
{
    partial class Form1
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
            this.btnGeneratePdf = new System.Windows.Forms.Button();
            this.panelDynamicInputs = new System.Windows.Forms.Panel();
            this.flowPanelSubjects = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // btnGeneratePdf
            // 
            this.btnGeneratePdf.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnGeneratePdf.Location = new System.Drawing.Point(740, 643);
            this.btnGeneratePdf.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnGeneratePdf.Name = "btnGeneratePdf";
            this.btnGeneratePdf.Size = new System.Drawing.Size(168, 51);
            this.btnGeneratePdf.TabIndex = 4;
            this.btnGeneratePdf.Text = "حفظ ملف PDF";
            this.btnGeneratePdf.UseVisualStyleBackColor = false;
            this.btnGeneratePdf.Click += new System.EventHandler(this.btnGeneratePdf_Click);
            // 
            // panelDynamicInputs
            // 
            this.panelDynamicInputs.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelDynamicInputs.Location = new System.Drawing.Point(290, 54);
            this.panelDynamicInputs.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panelDynamicInputs.Name = "panelDynamicInputs";
            this.panelDynamicInputs.Size = new System.Drawing.Size(624, 502);
            this.panelDynamicInputs.TabIndex = 5;
            // 
            // flowPanelSubjects
            // 
            this.flowPanelSubjects.AutoScroll = true;
            this.flowPanelSubjects.Location = new System.Drawing.Point(26, 54);
            this.flowPanelSubjects.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.flowPanelSubjects.Name = "flowPanelSubjects";
            this.flowPanelSubjects.Size = new System.Drawing.Size(207, 502);
            this.flowPanelSubjects.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 748);
            this.Controls.Add(this.panelDynamicInputs);
            this.Controls.Add(this.flowPanelSubjects);
            this.Controls.Add(this.btnGeneratePdf);
            this.Font = new System.Drawing.Font("Arial Narrow", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "Form1";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "طباعة قرارات مجلس الكلية";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnGeneratePdf;
        private System.Windows.Forms.Panel panelDynamicInputs;
        private System.Windows.Forms.FlowLayoutPanel flowPanelSubjects;
    }
}

