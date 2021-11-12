using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            isActive = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isActive)
            {
                grdResults.Rows.Add("Teste");
            }
        }
    }
}
