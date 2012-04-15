namespace DtWorkshop.GUI
{
    partial class DtDocForm
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
            if (disposing)
            {
                Document.Close();
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.dataPage = new System.Windows.Forms.TabPage();
            this.dataControl = new DtWorkshop.GUI.DtDataPanel();
            this.buildPage = new System.Windows.Forms.TabPage();
            this.reportControl = new DtWorkshop.GUI.DtBuildPanel();
            this.graphPage = new System.Windows.Forms.TabPage();
            this.graphControl = new DtWorkshop.GUI.DtGraphPanel();
            this.tabControl.SuspendLayout();
            this.dataPage.SuspendLayout();
            this.buildPage.SuspendLayout();
            this.graphPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.dataPage);
            this.tabControl.Controls.Add(this.buildPage);
            this.tabControl.Controls.Add(this.graphPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(701, 523);
            this.tabControl.TabIndex = 0;
            // 
            // dataPage
            // 
            this.dataPage.Controls.Add(this.dataControl);
            this.dataPage.Location = new System.Drawing.Point(4, 22);
            this.dataPage.Name = "dataPage";
            this.dataPage.Padding = new System.Windows.Forms.Padding(3);
            this.dataPage.Size = new System.Drawing.Size(693, 497);
            this.dataPage.TabIndex = 0;
            this.dataPage.Text = "Data";
            this.dataPage.UseVisualStyleBackColor = true;
            // 
            // dataControl
            // 
            this.dataControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataControl.Location = new System.Drawing.Point(3, 3);
            this.dataControl.Name = "dataControl";
            this.dataControl.Size = new System.Drawing.Size(687, 491);
            this.dataControl.TabIndex = 0;
            // 
            // buildPage
            // 
            this.buildPage.Controls.Add(this.reportControl);
            this.buildPage.Location = new System.Drawing.Point(4, 22);
            this.buildPage.Name = "buildPage";
            this.buildPage.Padding = new System.Windows.Forms.Padding(3);
            this.buildPage.Size = new System.Drawing.Size(603, 421);
            this.buildPage.TabIndex = 2;
            this.buildPage.Text = "Build";
            this.buildPage.UseVisualStyleBackColor = true;
            // 
            // reportControl
            // 
            this.reportControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportControl.Location = new System.Drawing.Point(3, 3);
            this.reportControl.Name = "reportControl";
            this.reportControl.Size = new System.Drawing.Size(597, 415);
            this.reportControl.TabIndex = 0;
            // 
            // graphPage
            // 
            this.graphPage.Controls.Add(this.graphControl);
            this.graphPage.Location = new System.Drawing.Point(4, 22);
            this.graphPage.Name = "graphPage";
            this.graphPage.Padding = new System.Windows.Forms.Padding(3);
            this.graphPage.Size = new System.Drawing.Size(603, 421);
            this.graphPage.TabIndex = 1;
            this.graphPage.Text = "Graph";
            this.graphPage.UseVisualStyleBackColor = true;
            // 
            // graphControl
            // 
            this.graphControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphControl.Location = new System.Drawing.Point(3, 3);
            this.graphControl.Name = "graphControl";
            this.graphControl.Size = new System.Drawing.Size(597, 415);
            this.graphControl.TabIndex = 0;
            // 
            // DtDocForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 523);
            this.Controls.Add(this.tabControl);
            this.Name = "DtDocForm";
            this.Text = "DtDocForm";
            this.tabControl.ResumeLayout(false);
            this.dataPage.ResumeLayout(false);
            this.buildPage.ResumeLayout(false);
            this.graphPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        public System.Windows.Forms.TabPage dataPage;
        public System.Windows.Forms.TabPage graphPage;
        public System.Windows.Forms.TabPage buildPage;
        public DtDataPanel dataControl;
        public DtGraphPanel graphControl;
        public DtBuildPanel reportControl;

    }
}