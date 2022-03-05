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
    public partial class FrmLogin : Form
    {
        Class cls = new Class();
        public FrmLogin()
        {
            InitializeComponent();
            txtEmail.Focus();
        }

        private void ClearData()
        {
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtEmail.Focus();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            txtEmail.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtEmail.Text == "" || txtPassword.Text == "")
                {
                    MessageBox.Show("Data Can't Be Empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (cls.IsValidEmail(txtEmail.Text) == false)
                {
                    MessageBox.Show("Invalid Address Email", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ClearData();
                }
                else if (cls.IsValidPassword(txtPassword.Text) == false)
                {
                    MessageBox.Show("Password Must Contains Uppercase, Lowercase, Number with Total 6 - 9 Character", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ClearData();
                }
                else
                {
                    using (DataClassesDataContext db = new DataClassesDataContext())
                    {
                        Msemployee employee = db.Msemployees.Where(s => s.Email == txtEmail.Text && s.Password == cls.Hash(txtPassword.Text)).FirstOrDefault();
                        if (employee != null)
                        {
                            if (employee.Position == "Admin")
                            {
                                (new FrmAdminNav(this, employee)).Show();
                                this.Hide();
                                ClearData();
                            }
                            else if (employee.Position == "Chef")
                            {
                                (new FrmChefNav(this, employee)).Show();
                                this.Hide();
                                ClearData();
                            }
                            else if (employee.Position == "Cashier")
                            {
                                (new FrmCashierNav(this, employee)).Show();
                                this.Hide();
                                ClearData();
                            }
                        }
                        else
                        {
                            MessageBox.Show("No Such User", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ClearData();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Message : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Exit This Application?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPassword.Focus();
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }
    }
}
