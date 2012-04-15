using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DtWorkshop.GUI
{
    public partial class AppForm : Form
    {
        public AppForm()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            DtDocForm form = CreateChildForm();
            form.LoadDocument("");
            DockChildForm(form);
        }

        private void OpenFile(object sender, EventArgs e)
        {
            /*
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            */
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open Decision Tree File";
            openFileDialog.Filter = "Decision Tree Files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = @"..\..\Data";
            if (openFileDialog.ShowDialog(this) != DialogResult.OK)
                return;
            string filename = openFileDialog.FileName.ToString();
            //
            DtDocForm form = CreateChildForm();
            form.LoadDocument(filename);
            DockChildForm(form);
        }
        public DtDocForm CreateChildForm()
        {
            DtDocForm childForm = new DtDocForm();
            childForm.MdiParent = this;
            return childForm;
        }
        public void DockChildForm(Form childForm)
        {
            childForm.Show();
            //DockableFormInfo childFormInfo = docker.Add(childForm, zAllowedDock.All, new Guid("a6402b80-2ebd-4fd3-8930-024a6201d001"));
            //docker.DockForm(childFormInfo, DockStyle.Fill, zDockMode.Inner);
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
    }
}
