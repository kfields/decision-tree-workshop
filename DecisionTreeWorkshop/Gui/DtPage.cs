using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

using DtWorkshop.ID3;

namespace DtWorkshop
{
    public enum RefreshKind
    {
        Initial,
        Default,
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
    public class DtBuildPage : DtPage
    {
        DtBuildPanel Control;
        TraceListener TraceListener;
        //
        public DtBuildPage(DtDocument document, DtBuildPanel control)
            : base(document, "Report")
        {
            Control = control;
            Control.BuildButton.Click += OnBuildButtonClicked;
            Control.CancelButton.Click += OnCancelButtonClicked;
            //
            TraceListener = new DtTraceListener(Control.RichText);
            //Debug.Listeners.Add(TraceListener);
            Trace.Listeners.Add(TraceListener);
        }
        ~DtBuildPage()
        {
        }
        public override void Refresh(RefreshKind kind)
        {
            switch(kind){
                case RefreshKind.Initial:
                    string[] heuristics = Enum.GetNames(typeof(DtHeuristicKind));
                    Control.HeuristicCombo.Items.AddRange(heuristics);
                    Control.HeuristicCombo.SelectedItem = heuristics[0];
                    //
                    string[] attributes = Document.DataTable.GetAttributeNames();
                    Control.TargetAttributeCombo.Items.AddRange(attributes);
                    Control.TargetAttributeCombo.SelectedItem = attributes[attributes.Length - 1];
                    break;
                case RefreshKind.Final:
                    Trace.Listeners.Remove(TraceListener);
                    break;
            }
        }
        private void OnBuildButtonClicked(object sender, EventArgs e)
        {
            Document.Build(
                (DtHeuristicKind)Enum.Parse(typeof(DtHeuristicKind), (string)Control.HeuristicCombo.SelectedItem),
                (string)Control.TargetAttributeCombo.SelectedItem
                );
        }
        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            Document.CancelBuild();
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
            Document.Tree.RootChanged += OnRootChanged;
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
        private delegate void InvokeDelegate();
        private void OnRootChanged()
        {
            Control.BeginInvoke(new InvokeDelegate(() => { Refresh(RefreshKind.Total); }));
        }
        private void OnCanvasZoomerValue(object sender, EventArgs e)
        {
            Canvas.LayoutTransform = new ScaleTransform(CanvasZoomer.Value, CanvasZoomer.Value);
        }
    }
}
