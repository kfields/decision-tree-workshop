namespace DtWorkshop
{
    partial class DtBuildPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DtBuildPanel));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.RichText = new System.Windows.Forms.RichTextBox();
            this.BuildStrip = new System.Windows.Forms.ToolStrip();
            this.heuristicLabel = new System.Windows.Forms.ToolStripLabel();
            this.HeuristicCombo = new System.Windows.Forms.ToolStripComboBox();
            this.targetAttributeLabel = new System.Windows.Forms.ToolStripLabel();
            this.TargetAttributeCombo = new System.Windows.Forms.ToolStripComboBox();
            this.BuildButton = new System.Windows.Forms.ToolStripButton();
            this.CancelButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.BuildStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.RichText);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(737, 135);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(737, 160);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.BuildStrip);
            // 
            // RichText
            // 
            this.RichText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichText.Location = new System.Drawing.Point(0, 0);
            this.RichText.Name = "RichText";
            this.RichText.Size = new System.Drawing.Size(737, 135);
            this.RichText.TabIndex = 1;
            this.RichText.Text = "";
            // 
            // BuildStrip
            // 
            this.BuildStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.BuildStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.heuristicLabel,
            this.HeuristicCombo,
            this.targetAttributeLabel,
            this.TargetAttributeCombo,
            this.BuildButton,
            this.CancelButton});
            this.BuildStrip.Location = new System.Drawing.Point(3, 0);
            this.BuildStrip.Name = "BuildStrip";
            this.BuildStrip.Size = new System.Drawing.Size(468, 25);
            this.BuildStrip.TabIndex = 0;
            // 
            // heuristicLabel
            // 
            this.heuristicLabel.Name = "heuristicLabel";
            this.heuristicLabel.Size = new System.Drawing.Size(48, 22);
            this.heuristicLabel.Text = "Heuristic";
            // 
            // HeuristicCombo
            // 
            this.HeuristicCombo.Name = "HeuristicCombo";
            this.HeuristicCombo.Size = new System.Drawing.Size(121, 25);
            // 
            // targetAttributeLabel
            // 
            this.targetAttributeLabel.Name = "targetAttributeLabel";
            this.targetAttributeLabel.Size = new System.Drawing.Size(85, 22);
            this.targetAttributeLabel.Text = "Target Attribute";
            // 
            // TargetAttributeCombo
            // 
            this.TargetAttributeCombo.Name = "TargetAttributeCombo";
            this.TargetAttributeCombo.Size = new System.Drawing.Size(121, 25);
            // 
            // BuildButton
            // 
            this.BuildButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BuildButton.Image = ((System.Drawing.Image)(resources.GetObject("BuildButton.Image")));
            this.BuildButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BuildButton.Name = "BuildButton";
            this.BuildButton.Size = new System.Drawing.Size(23, 22);
            this.BuildButton.Text = "Build";
            // 
            // CancelButton
            // 
            this.CancelButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CancelButton.Image = ((System.Drawing.Image)(resources.GetObject("CancelButton.Image")));
            this.CancelButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(23, 22);
            this.CancelButton.Text = "toolStripButton1";
            // 
            // DtBuildPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "DtBuildPanel";
            this.Size = new System.Drawing.Size(737, 160);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.BuildStrip.ResumeLayout(false);
            this.BuildStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        public System.Windows.Forms.RichTextBox RichText;
        private System.Windows.Forms.ToolStrip BuildStrip;
        private System.Windows.Forms.ToolStripLabel targetAttributeLabel;
        public System.Windows.Forms.ToolStripComboBox TargetAttributeCombo;
        public System.Windows.Forms.ToolStripButton BuildButton;
        private System.Windows.Forms.ToolStripLabel heuristicLabel;
        public System.Windows.Forms.ToolStripComboBox HeuristicCombo;
        public System.Windows.Forms.ToolStripButton CancelButton;


    }
}
