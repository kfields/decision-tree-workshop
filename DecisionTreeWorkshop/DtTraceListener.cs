using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace DtWorkshop
{
    public class DtTraceListener : TraceListener
    {
        private TextBoxBase output;

        public DtTraceListener(TextBoxBase output)
        {
            this.Name = "Trace";
            this.output = output;
        }

        public override void Write(string message)
        {

            Action append = delegate()
            {
                output.AppendText(string.Format("[{0}] ", DateTime.Now.ToString()));
                output.AppendText(message);
            };
            if (output.InvokeRequired)
            {
                output.BeginInvoke(append);
            }
            else
            {
                append();
            }

        }

        public override void WriteLine(string message)
        {
            Write(message + Environment.NewLine);
        }
    }
}
