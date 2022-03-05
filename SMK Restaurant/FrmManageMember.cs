using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMK_Restaurant
{
    public partial class FrmManageMember : UserControl
    {
        Class cls = new Class();
        bool newData;
        public FrmManageMember()
        {
            InitializeComponent();
        }

        private void AutoNumber()
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                Msmember m = db.Msmembers.OrderByDescending(s => s.MemberID).FirstOrDefault();
                if (m != null)
                {
                    string a = m.MemberID.ToString().Substring(4, 4);
                    int n = Convert.ToInt32(a) + 1;
                    txtID.Text = DateTime.Today.ToString("yyyy") + n.ToString("d4");
                }
                else
                {
                    txtID.Text = DateTime.Today.ToString("yyyy") + "0001";
                }
            }
        }

        private void DisplayData()
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                dg1.DataSource = db.Msmembers.Select(s => s);
                dg1.Columns["MemberID"].HeaderText = "MemberId";
            }
        }

        private void ClearData()
        {
            txtName.Clear();
            txtEmail.Clear();
            txtHandphone.Clear();
        }

        private void DisabledData()
        {
            txtName.Enabled = false;
            txtEmail.Enabled = false;
            txtHandphone.Enabled = false;
        }

        private void EnabledData()
        {
            txtName.Enabled = true;
            txtEmail.Enabled = true;
            txtHandphone.Enabled = true;
        }

        private void Button1()
        {
            btnInsert.Visible = true;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            btnSave.Visible = false;
            btnCancel.Visible = false;
        }

        private void Button2()
        {
            btnInsert.Visible = false;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnSave.Visible = true;
            btnCancel.Visible = true;
        }

        private void Start()
        {
            DisplayData();
            ClearData();
            txtID.Enabled = false;
            DisabledData();
            Button1();
        }

        private void FrmManageMember_Load(object sender, EventArgs e)
        {
            Start();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            AutoNumber();
            ClearData();
            EnabledData();
            Button2();
            newData = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            EnabledData();
            Button2();
            newData = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                if (txtID.Text == "" || txtName.Text == "" || txtEmail.Text == "" || txtHandphone.Text == "")
                {
                    MessageBox.Show("Data Cant Be Empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ClearData();
                }
                else
                {
                    if (MessageBox.Show("Do You Want Delete Data with Name " + txtName.Text + " ?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Msmember m = db.Msmembers.Where(s => s.MemberID == txtID.Text).FirstOrDefault();
                        db.Msmembers.DeleteOnSubmit(m);
                        db.SubmitChanges();
                        Start();
                        MessageBox.Show("Successfully Deleted Data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (DataClassesDataContext db = new DataClassesDataContext())
                {
                    if (txtID.Text == "" || txtName.Text == "" || txtEmail.Text == "" || txtHandphone.Text == "")
                    {
                        MessageBox.Show("Data Cant Be Empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ClearData();
                    }
                    else if (txtName.Text.Length < 3 || txtName.Text.Length > 20)
                    {
                        MessageBox.Show("Ensure name have between 3 and 20 character", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (cls.IsValidEmail(txtEmail.Text) == false)
                    {
                        MessageBox.Show("Invalid Address Email", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (txtHandphone.Text.Length < 11 || txtHandphone.Text.Length > 13 && !txtHandphone.Text.StartsWith("08"))
                    {
                        MessageBox.Show("Ensure phone number must be 11 – 12 Digit and start with 08", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (newData == true)
                        {
                            db.Msmembers.InsertOnSubmit(new Msmember()
                            {
                                MemberID = txtID.Text,
                                Name = txtName.Text,
                                Email = txtEmail.Text,
                                Handphone = txtHandphone.Text,
                                JoinDate = DateTime.Now
                            });
                            db.SubmitChanges();
                            Start();
                            MessageBox.Show("Successfully Added Data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            Msmember m = db.Msmembers.Where(s => s.MemberID == txtID.Text).FirstOrDefault();
                            m.MemberID = txtID.Text;
                            m.Name = txtName.Text;
                            m.Email = txtEmail.Text;
                            m.Handphone = txtHandphone.Text;
                            m.JoinDate = DateTime.Now;
                            db.SubmitChanges();
                            Start();
                            MessageBox.Show("Successfully Saved Data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Try Again, Please Contact Admin" + "\n" + ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want to Cancel ?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Start();
                txtID.Clear();
            }
        }

        private void dg1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewRow r = dg1.Rows[e.RowIndex];
                txtID.Text = r.Cells["MemberID"].Value.ToString();
                txtName.Text = r.Cells["Name"].Value.ToString();
                txtEmail.Text = r.Cells["Email"].Value.ToString();
                txtHandphone.Text = r.Cells["Handphone"].Value.ToString();
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && (int)e.KeyChar != (int)Keys.Back && (int)e.KeyChar != (int)Keys.Space)
            {
                e.Handled = true;
            }
        }

        private void txtHandphone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (int)e.KeyChar != (int)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}
