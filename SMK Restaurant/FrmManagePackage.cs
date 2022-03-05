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
    public partial class FrmManagePackage : UserControl
    {
        Class cls = new Class();
        bool newData;
        public FrmManagePackage()
        {
            InitializeComponent();
        }

        private void AutoNumber()
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                Mspackage p = db.Mspackages.OrderByDescending(s => s.ID).FirstOrDefault();
                if (p != null)
                {
                    string a = p.ID.ToString();
                    int n = Convert.ToInt32(a) + 1;
                    txtID.Text = n.ToString("d1");
                }
                else
                {
                    txtID.Text = "1";
                }
            }
        }

        private void DisplayData()
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                dg1.DataSource = db.Mspackages.Select(s => s);
                dg1.Columns["ID"].HeaderText = "PackageId";
            }
        }

        private void ClearData()
        {
            txtName.Clear();
            txtPrice.Clear();
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
        }

        private void DisabledData()
        {
            txtName.Enabled = false;
            txtPrice.Enabled = false;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            numericUpDown3.Enabled = false;
        }

        private void EnabledData()
        {
            txtName.Enabled = true;
            txtPrice.Enabled = true;
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
            numericUpDown3.Enabled = true;
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

        private void FrmManagePackage_Load(object sender, EventArgs e)
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
                if (txtID.Text == "" || txtName.Text == "" || txtPrice.Text == "" || numericUpDown1.Value == 0 || numericUpDown2.Value == 0 || numericUpDown3.Value == 0)
                {
                    MessageBox.Show("Data Cant Be Empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ClearData();
                }
                else
                {
                    if (MessageBox.Show("Do You Want Delete Data with Name " + txtName.Text + " ?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Mspackage p = db.Mspackages.Where(s => s.ID == Convert.ToInt32(txtID.Text)).FirstOrDefault();
                        db.Mspackages.DeleteOnSubmit(p);
                        db.SubmitChanges();
                        Start();
                        MessageBox.Show("Successfully Deleted Data", "information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    if (txtID.Text == "" || txtName.Text == "" || txtPrice.Text == "" || numericUpDown1.Value == 0 || numericUpDown2.Value == 0 || numericUpDown3.Value == 0)
                    {
                        MessageBox.Show("Data Cant Be Empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ClearData();
                    }
                    else
                    {
                        if (newData == true)
                        {
                            db.Mspackages.InsertOnSubmit(new Mspackage()
                            {
                                ID = Convert.ToInt32(txtID.Text),
                                Name = txtName.Text,
                                Price = Convert.ToInt32(txtPrice.Text),
                                CountMeat = Convert.ToInt32(numericUpDown1.Value),
                                CountVegetable = Convert.ToInt32(numericUpDown2.Value),
                                MaxOrder = Convert.ToInt32(numericUpDown3.Value)
                            });
                            db.SubmitChanges();
                            Start();
                            txtID.Clear();
                            MessageBox.Show("Successfully Added Data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            Mspackage p = db.Mspackages.Where(s => s.ID == Convert.ToInt32(txtID.Text)).FirstOrDefault();
                            p.ID = Convert.ToInt32(txtID.Text);
                            p.Name = txtName.Text;
                            p.Price = Convert.ToInt32(txtPrice.Text);
                            p.CountMeat = Convert.ToInt32(numericUpDown1.Value);
                            p.CountVegetable = Convert.ToInt32(numericUpDown2.Value);
                            p.MaxOrder = Convert.ToInt32(numericUpDown3.Value);
                            db.SubmitChanges();
                            Start();
                            txtID.Clear();
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
                txtID.Text = r.Cells["ID"].Value.ToString();
                txtName.Text = r.Cells["Name"].Value.ToString();
                txtPrice.Text = r.Cells["Price"].Value.ToString();
                numericUpDown1.Value = Convert.ToInt32(r.Cells["CountMeat"].Value.ToString());
                numericUpDown2.Value = Convert.ToInt32(r.Cells["CountVegetable"].Value.ToString());
                numericUpDown3.Value = Convert.ToInt32(r.Cells["MaxOrder"].Value.ToString());
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && (int)e.KeyChar != (int)Keys.Back && (int)e.KeyChar != (int)Keys.Space)
            {
                e.Handled = true;
            }
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (int)e.KeyChar != (int)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}
