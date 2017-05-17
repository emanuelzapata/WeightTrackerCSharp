using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace Weight_Tracker_with_DB
{
    public partial class popUp : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32
                SendMessage(
                                IntPtr hWnd,
                                int msg,
                                int wParam,
                                [MarshalAs(UnmanagedType.LPWStr)]string lParam
                            );
        private const int EM_SETCUEBANNER = 0x1501;
      
        public popUp()
        {
            InitializeComponent();
            SendMessage(textBox1.Handle, EM_SETCUEBANNER, 0, "Initial Weight");
            SendMessage(textBox2.Handle, EM_SETCUEBANNER, 0, "Goal Weight");
        }

        private void submitInitial_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null || textBox2.Text == null)
            {
                //do nothing;
            }
            else
            {
                this.Close();
            }
        }
    }
}
