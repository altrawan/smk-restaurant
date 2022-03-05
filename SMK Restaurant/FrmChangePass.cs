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
    public partial class FrmChangePass : Form
    {
        private Msemployee emp;
        Class cls = new Class();

        public FrmChangePass(Msemployee emp)
        {
            InitializeComponent();
            this.emp = emp;
            emp = new Msemployee();
        }

        private void ClearData()
        {
            txtOld.Clear();
            txtNew.Clear();
            txtConfirm.Clear();
            txtOld.Focus();
        }

        private void DisabledData()
        {
            txtNew.Enabled = false;
            txtConfirm.Enabled = false;
            checkBox1.Checked = false;
            checkBox2.Enabled = false;
            checkBox2.Checked = false;
            checkBox3.Enabled = false;
            checkBox3.Checked = false;

        }

        private void Start()
        {
            DisabledData();
            ClearData();
        }

        private void FrmChangePass_Load(object sender, EventArgs e)
        {
            Start();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (DataClassesDataContext db = new DataClassesDataContext())
                {
                    Msemployee employee = db.Msemployees.Where(s => s.EmployeeID == emp.EmployeeID).FirstOrDefault();
                    if (txtOld.Text == "" || txtNew.Text == "" || txtConfirm.Text == "")
                    {
                        MessageBox.Show("Data Cant Be Empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Start();
                    }
                    else if (cls.IsValidPassword(txtNew.Text) == false || cls.IsValidPassword(txtConfirm.Text) == false)
                    {
                        MessageBox.Show("Password Must Contains Uppercase, Lowercase, Number with Total 6 - 9 Character", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (txtNew.Text != txtConfirm.Text)
                    {
                        MessageBox.Show("New Password And Confirm Password Are Not The Same", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        employee.Password = cls.Hash(txtConfirm.Text);
                        db.SubmitChanges();
                        MessageBox.Show("Successfully Change Password", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Start();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Try Again, Please Contact Admin" + "\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel Change Password ?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                txtOld.UseSystemPasswordChar = false;
            }
            else
            {
                txtOld.UseSystemPasswordChar = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                txtNew.UseSystemPasswordChar = false;
            }
            else
            {
                txtNew.UseSystemPasswordChar = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                txtConfirm.UseSystemPasswordChar = false;
            }
            else
            {
                txtConfirm.UseSystemPasswordChar = true;
            }
        }

        private void txtOld_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtOld_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                using (DataClassesDataContext db = new DataClassesDataContext())
                {
                    Msemployee employee = db.Msemployees.Where(s => s.EmployeeID == emp.EmployeeID).FirstOrDefault();
                    if (cls.Hash(txtOld.Text) == employee.Password)
                    {
                        txtNew.Enabled = true;
                        checkBox2.Enabled = true;
                        txtNew.Focus();
                    }
                    else
                    {
                        txtNew.Enabled = false;
                        checkBox2.Enabled = false;
                        txtOld.Focus();
                        MessageBox.Show("Your Password Doesn't Match", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void txtNew_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (cls.IsValidPassword(txtNew.Text) == false)
                {
                    txtConfirm.Enabled = false;
                    checkBox3.Enabled = false;
                    txtNew.Focus();
                    MessageBox.Show("Password Must Contains Uppercase, Lowercase, Number with Total 6 - 9 Character", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    txtConfirm.Enabled = true;
                    checkBox3.Enabled = true;
                    txtConfirm.Focus();
                }
            }
        }

        private void txtConfirm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (txtNew.Text != txtConfirm.Text)
                {
                    btnSave.Enabled = false;
                    txtConfirm.Focus();
                    MessageBox.Show("New Password And Confirm Password Are Not The Same", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    btnSave.Enabled = true;
                    btnSave.PerformClick();
                }
            }
        }
    }
}
