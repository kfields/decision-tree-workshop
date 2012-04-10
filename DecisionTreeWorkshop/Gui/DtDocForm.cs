using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DtWorkshop
{
    public partial class DtDocForm : Form
    {
        public DtDocument Document;
        public DtDocForm()
        {
            InitializeComponent();
        }
        public void LoadDocument(string filename)
        {
            Document = new DtDocument();
            if (filename != "")
                Document.Load(filename);
            Text = Document.FileName;
            //
            DtPage page;
            page = new DtDataPage(Document, dataControl);
            Document.AddPage(page);
            page = new DtGraphPage(Document, graphControl);
            Document.AddPage(page);
            page = new DtBuildPage(Document, reportControl);
            Document.AddPage(page);
            //
            Document.RefreshPages(RefreshKind.Initial);
            if (filename != "")
                Document.RefreshPages(RefreshKind.Load);
        }
        public void CreateNewDocument()
        {
            Document = new DtDocument();
        }
    }
}
