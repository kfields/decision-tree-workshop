using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace DtWorkshop.GUI
{
    delegate void InvokeDelegate();

    public enum RefreshKind
    {
        Initial,
        Default,
        Load,
        Total,
        Final
    }
    public class DtPage
    {
        public DtDocument Document;
        public DtPage(DtDocument document, string name)
        {
            Document = document;
        }
        public virtual void Refresh(RefreshKind kind)
        {
        }
    }
    public class DtDataPage : DtPage
    {
        public DtDataPanel Control;
        public DtDataPage(DtDocument document, DtDataPanel control)
            : base(document, "Data")
        {
            Control = control;
            DataGridView dataView = Control.DataGridView;
            dataView.DataSource = Document.DataTable;
        }
        public override void Refresh(RefreshKind kind)
        {
        }
    }
    public class DtGraphPage : DtPage
    {
        DtGraphPanel Control;
        DtTreeCanvas Canvas;
        ScrollViewer CanvasViewer;
        Slider CanvasZoomer;
        //
        public DtGraphPage(DtDocument document, DtGraphPanel control)
            : base(document, "Graph")
        {
            //Document.Tree.RootChanged += OnRootChanged;
            Document.TreeBuilder.BuildFinished += OnBuildFinished;
            Control = control;
            DtWpfGraphPanel panel = new DtWpfGraphPanel();
            Canvas = (DtTreeCanvas)panel.FindName("treeCanvas");
            Canvas.Document = Document;
            Control.WpfHost.Child = panel;
            CanvasViewer = (ScrollViewer)panel.FindName("canvasViewer");
            CanvasZoomer = (Slider)panel.FindName("canvasZoomer");
            CanvasZoomer.ValueChanged += OnCanvasZoomerValue;
        }
        public override void Refresh(RefreshKind kind)
        {
            switch (kind)
            {
                case RefreshKind.Initial:
                case RefreshKind.Total:
                    Canvas.Clear();
                    Canvas.Build();
                    break;
            }
        }
        private void OnBuildFinished()
        {
            Control.BeginInvoke(new InvokeDelegate(() => { Refresh(RefreshKind.Total); }));
        }
        private void OnRootChanged()
        {
            //Control.BeginInvoke(new InvokeDelegate(() => { Refresh(RefreshKind.Total); }));
        }
        private void OnCanvasZoomerValue(object sender, EventArgs e)
        {
            Canvas.LayoutTransform = new ScaleTransform(CanvasZoomer.Value, CanvasZoomer.Value);
        }
    }
}
