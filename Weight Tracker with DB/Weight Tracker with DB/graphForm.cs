using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Weight_Tracker_with_DB
{
    public partial class graphForm : Form
    {
        public graphForm()
        {
            InitializeComponent();
        }

        private void graphForm_Load(object sender, EventArgs e)
        {

        }

        public void getPoints(List<string>[] list)
        {
            for (int x = 0; x < list[0].Count();x++)
            {
                double temp = Convert.ToDouble(list[0][x]);
                chart1.Series["Weight"].Points.AddXY(x, list[0][x]);
            }

        }
    }
}
