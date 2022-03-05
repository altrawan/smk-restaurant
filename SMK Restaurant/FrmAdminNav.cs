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
    public partial class FrmAdminNav : Form
    {
        private Form close;
        private Msemployee employee;

        public FrmAdminNav(Form close, Msemployee employee)
        {
            InitializeComponent();
            this.close = close;
            this.employee = employee;
        }

        private void FrmAdminNav_Load(object sender, EventArgs e)
        {
            lblName.Text = employee.Name;
        }

        private void btnManageEmployee_Click(object sender, EventArgs e)
        {
            SidePanel.Height = btnManageEmployee.Height;
            SidePanel.Top = btnManageEmployee.Top;
            frmManageEmployee1.BringToFront();
        }

        private void btnManageMenu_Click(object sender, EventArgs e)
        {
            SidePanel.Height = BtnManageMenu.Height;
            SidePanel.Top = BtnManageMenu.Top;
            frmManageMenu1.BringToFront();
        }

        private void btnManageMember_Click(object sender, EventArgs e)
        {
            SidePanel.Height = btnManageMember.Height;
            SidePanel.Top = btnManageMember.Top;
            frmManageMember1.BringToFront();
        }

        private void btnManagePackage_Click(object sender, EventArgs e)
        {
            SidePanel.Height = btnManagePackage.Height;
            SidePanel.Top = btnManagePackage.Top;
            frmManagePackage1.BringToFront();
        }

        private void btnViewReport_Click_1(object sender, EventArgs e)
        {
            SidePanel.Height = btnViewReport.Height;
            SidePanel.Top = btnViewReport.Top;
            frmReport1.BringToFront();
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

        private void frmReport1_Load(object sender, EventArgs e)
        {

        }
    }
}
