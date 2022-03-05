using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SMK_Restaurant
{
    public partial class FrmManageMenu : UserControl
    {
        Class cls = new Class();
        bool newData;
        public FrmManageMenu()
        {
            InitializeComponent();
        }

        private void AutoNumber()
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                Msmenu m = db.Msmenus.OrderByDescending(s => s.MenuID).FirstOrDefault();
                if (m != null)
                {
                    string a = m.MenuID.ToString();
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
                dg1.DataSource = db.Msmenus.Select(s => s);
                dg1.Columns["MenuID"].HeaderText = "MenuId";
                dg1.Columns["Photo"].Visible = false;
            }
        }

        private void ClearData()
        {
            txtName.Clear();
            txtPrice.Clear();
            txtPhoto.Clear();
            pictureBox1.ImageLocation = null;
            pictureBox1.BackgroundImage = null;
        }

        private void DisabledData()
        {
            txtName.Enabled = false;
            txtPrice.Enabled = false;
            BtnBrowse.Enabled = false;
        }

        private void EnabledData()
        {
            txtName.Enabled = true;
            txtPrice.Enabled = true;
            BtnBrowse.Enabled = true;
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
            txtPhoto.ReadOnly = true;
            DisabledData();
            Button1();
        }

        private void FrmManageMenu_Load(object sender, EventArgs e)
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
                if (txtID.Text == "" || txtName.Text == "" || txtPrice.Text == "" || txtPhoto.Text == "")
                {
                    MessageBox.Show("Data Cant Be Empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ClearData();
                }
                else
                {
                    if (MessageBox.Show("Do You Want Delete Data with Name " + txtName.Text + " ?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Msmenu m = db.Msmenus.Where(s => s.MenuID == Convert.ToInt32(txtID.Text)).FirstOrDefault();
                        db.Msmenus.DeleteOnSubmit(m);
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
                    if (txtID.Text == "" || txtName.Text == "" || txtPrice.Text == "" || txtPhoto.Text == "")
                    {
                        MessageBox.Show("Data Cant Be Empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ClearData();
                    }
                    else
                    {
                        if (newData == true)
                        {
                            db.Msmenus.InsertOnSubmit(new Msmenu()
                            {
                                MenuID = Convert.ToInt32(txtID.Text),
                                Name = txtName.Text,
                                Price = Convert.ToInt32(txtPrice.Text),
                                Photo = txtPhoto.Text,
                            });
                            db.SubmitChanges();
                            Start();
                            txtID.Clear();
                            MessageBox.Show("Successfully Added Data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            Msmenu m = db.Msmenus.Where(s => s.MenuID == Convert.ToInt32(txtID.Text)).FirstOrDefault();
                            m.MenuID = Convert.ToInt32(txtID.Text);
                            m.Name = txtName.Text;
                            m.Price = Convert.ToInt32(txtPrice.Text);
                            m.Photo = txtPhoto.Text;
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

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            //open file dialog
            OpenFileDialog opFile = new OpenFileDialog();
            //title
            opFile.Title = "Select a Image";
            //image filters
            opFile.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp";

            string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\Images\"; 
            if (Directory.Exists(appPath) == false)                                              
            {                                                                                   
                Directory.CreateDirectory(appPath);                                              
            }                                                                                    

            if (opFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string iName = opFile.SafeFileName;   
                    string filepath = opFile.FileName;    
                    File.Copy(filepath, appPath + iName);
                    // display image in picture box  
                    pictureBox1.BackgroundImage = Image.FromFile(opFile.FileName);
                    // name file  
                    txtPhoto.Text = iName;
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Unable to open file " + exp.Message);
                }
            }
            else
            {
                opFile.Dispose();
            }
        }

        private void dg1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            pictureBox1.ImageLocation = null;
            pictureBox1.BackgroundImage = null;
            if (e.RowIndex > -1)
            {
                DataGridViewRow r = dg1.Rows[e.RowIndex];
                txtID.Text = r.Cells["MenuID"].Value.ToString();
                txtName.Text = r.Cells["Name"].Value.ToString();
                txtPrice.Text = r.Cells["Price"].Value.ToString();
                txtPhoto.Text = r.Cells["Photo"].Value.ToString();
                string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\Images\" + r.Cells["Photo"].Value.ToString();
                pictureBox1.BackgroundImage = Image.FromFile(appPath);
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
