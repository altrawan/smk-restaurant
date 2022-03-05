using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMK_Restaurant
{
    public partial class FrmChefNav : Form
    {
        private Form close;
        private Msemployee employee;
        public FrmChefNav(Form close, Msemployee employee)
        {
            InitializeComponent();
            this.close = close;
            this.employee = employee;
        }

        private void FrmChefNav_Load(object sender, EventArgs e)
        {
            lblName.Text = employee.Name;
        }

        private void btnViewOrder_Click(object sender, EventArgs e)
        {
            SidePanel.Height = btnViewOrder.Height;
            SidePanel.Top = btnViewOrder.Top;
            frmViewOrder1.BringToFront();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            SidePanel.Height = btnLogout.Height;
            SidePanel.Top = btnLogout.Top;
            if (MessageBox.Show("Do You Want To Logout?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                close.Show();
                this.Hide();
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            (new FrmChangePass(employee)).Show();
        }
    }
}
