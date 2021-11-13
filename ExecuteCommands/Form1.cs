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
using System;
using Excel = Microsoft.Office.Interop.Excel;
using IronXL;

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

        private void button1_Click(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            var formatedDate = now.Year + "-" + now.Month + "-" + now.Day;
            var nameFile = txtCommand.Text.Length > 20 ? txtCommand.Text.Substring(0, 19) : txtCommand.Text;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = nameFile + " " + formatedDate + ".xls";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                WorkBook workbook = WorkBook.Create(ExcelFileFormat.XLSX);
                var sheet = workbook.CreateWorkSheet("results");

                foreach (DataGridViewRow row in grdResults.Rows)
                {
                    sheet["A" + row.Index + 1].Value = row?.Cells[0]?.Value;
                }

                workbook.SaveAs(saveFileDialog1.FileName);
            }
        }
    }
}
