using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExecuteCommands
{
    public partial class Form1 : Form
    {
        Boolean isActive;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            timer1.Interval = Int32.Parse(txtInterval.Text) * 1000;

            isActive = !isActive;
            btnRun.Text = isActive ? "Stop" : "Run";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isActive)
            {
                grdResults.Rows.Add(runScript(txtCommand.Text));
            }
        }

        private string runScript(string script)
        {
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(script);
            pipeline.Commands.Add("Out-String");
            Collection<PSObject> results = pipeline.Invoke();
            runspace.Close();
            StringBuilder stringBuilder = new StringBuilder(); 
            foreach (PSObject obj in results)
                stringBuilder.Append(obj.ToString());
            return stringBuilder.ToString();
        }
    }
}
