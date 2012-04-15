using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

using DtWorkshop.ID3;

namespace DtWorkshop.GUI
{
    public class DtBuildPage : DtPage
    {
        DtBuildPanel Control;
        CheckBox PrepruneCheckBox;
        TraceListener TraceListener;
        //
        public DtBuildPage(DtDocument document, DtBuildPanel control)
            : base(document, "Report")
        {
            document.TreeBuilder.BuildStarted += OnBuildStarted;
            document.TreeBuilder.BuildCanceled += OnBuildCanceled;
            document.TreeBuilder.BuildFinished += OnBuildFinished;
            //
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
            switch (kind)
            {
                case RefreshKind.Initial:
                    break;
                case RefreshKind.Load:
                    string[] heuristics = Enum.GetNames(typeof(DtHeuristicKind));
                    Control.HeuristicCombo.Items.AddRange(heuristics);
                    Control.HeuristicCombo.SelectedItem = heuristics[0];
                    //
                    string[] attributes = Document.DataTable.GetAttributeNames();
                    Control.TargetAttributeCombo.Items.AddRange(attributes);
                    Control.TargetAttributeCombo.SelectedItem = attributes[attributes.Length - 1];
                    //
                    CheckBox cb = PrepruneCheckBox = new CheckBox();
                    cb.Text = "Preprune";
                    cb.CheckState = CheckState.Checked;
                    ToolStripControlHost host = new ToolStripControlHost(cb);
                    Control.BuildStrip.Items.Insert(Control.BuildStrip.Items.IndexOf(Control.OptionsSeparator), host);

                    cb.CheckStateChanged += (s, ex) =>
                    {
                        CheckState cs = cb.CheckState;
                        if(cs == CheckState.Checked)
                            Document.TreeBuilder.Prepruning = true;
                        else
                            Document.TreeBuilder.Prepruning = false;
                    };
                    //
                    break;
                case RefreshKind.Final:
                    Trace.Listeners.Remove(TraceListener);
                    break;
            }
        }
        //
        
        private void InvokeEnableControls(bool enabled)
        {
            Control.BeginInvoke(new InvokeDelegate(() => { EnableControls(enabled); }));
        }
        private void EnableControls(bool enabled)
        {
            foreach (Control c in Control.BuildStrip.Controls)
            {
                c.Enabled = enabled;
            }
            if (enabled)
                Control.BuildButton.Enabled = enabled;
        }
        //Events
        private void OnBuildStarted()
        {
            InvokeEnableControls(false); 
        }
        private void OnBuildCanceled()
        {
            InvokeEnableControls(true);
        }
        private void OnBuildFinished()
        {
            InvokeEnableControls(true);
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
}
