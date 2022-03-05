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
    public partial class FrmCashierNav : Form
    {
        private Form close;
        private Msemployee employee;
        public FrmCashierNav(Form close, Msemployee employee)
        {
            InitializeComponent();
            this.close = close;
            this.employee = employee;
        }

        private void FrmCashierNav_Load(object sender, EventArgs e)
        {
            lblName.Text = employee.Name;
            frmOrder1.employeeid = employee.EmployeeID;
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            SidePanel.Height = btnOrder.Height;
            SidePanel.Top = btnOrder.Top;
            frmOrder1.BringToFront();
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            SidePanel.Height = btnPayment.Height;
            SidePanel.Top = btnPayment.Top;
            frmPayment1.BringToFront();
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
