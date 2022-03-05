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
    public partial class FrmOrder : UserControl
    {
        menuu selected;
        public string employeeid;
        string kodeid;
        List<menuu> list = new List<menuu>();

        public FrmOrder()
        {
            InitializeComponent();
        }

        private void DisplayData()
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                dg1.DataSource = db.Msmenus.Select(s => s);
                dg1.Columns["MenuID"].Visible = false;
                dg1.Columns["Photo"].Visible = false;
                dg1.Columns["Name"].HeaderText = "Menu";
                cbMember.DataSource = db.Msmembers.Select(s => s.Name);
            }
        }

        private void ClearData()
        {
            txtMenu.Clear();
            txtQty.Clear();
            txtPrice.Clear();
            dg2.Rows.Clear();
            lblTotal.Text = "Total : 0";
            dg2.DataSource = null;
            pictureBox1.ImageLocation = null;
            pictureBox1.BackgroundImage = null;
        }

        private void Start()
        {
            DisplayData();
            ClearData();
            txtMenu.Enabled = false;
            txtPrice.Enabled = false;
            cbMember.Enabled = true;
            dg2.Refresh();
        }

        private void RefreshLabel()
        {
            int tot = 0;
            foreach (menuu i in list)
            {
                tot += i.Total;
                lblTotal.Text = "Total : " + tot.ToString();
            }
            if (list.Count == 0)
            {
                lblTotal.Text = "Total : 0";
            }
        }

        private void RefreshData()
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = list;
            dg2.DataSource = bs;
            if (list.Count != null)
            {
                dg2.Columns["kodemenu"].Visible = false;
                dg2.Columns["namamember"].HeaderText = "Nama Member";
                dg2.Columns["namamenu"].HeaderText = "Menu";
            }
            else
            {
                dg2.DataSource = null;
            }
        }

        string c;
        private void AutoNumber()
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                Detailorder d = db.Detailorders.OrderByDescending(s => s.DetailID).FirstOrDefault();
                if (d != null)
                {
                    string a = d.DetailID.ToString().Substring(6, 4);
                    int n = Convert.ToInt32(a) + 1;
                    c = DateTime.Today.ToString("yyyyMM") + n.ToString("d4");
                }
                else
                {
                    c = DateTime.Today.ToString("yyyyMM") + "0001";
                }
            }
        }

        private void FrmOrder_Load(object sender, EventArgs e)
        {
            Start();
        }

        private void cbMember_SelectedValueChanged(object sender, EventArgs e)
        {
            cbMember.Enabled = false;
        }

        private void dg1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewRow r = dg1.Rows[e.RowIndex];
                kodeid = r.Cells["MenuID"].Value.ToString();
                txtMenu.Text = r.Cells["Name"].Value.ToString();
                txtPrice.Text = r.Cells["Price"].Value.ToString();
                string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\Images\" + r.Cells["Photo"].Value.ToString();
                pictureBox1.BackgroundImage = Image.FromFile(appPath);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (DataClassesDataContext db = new DataClassesDataContext())
                {
                    if (txtMenu.Text == "" || txtPrice.Text == "" || txtQty.Text == "" || cbMember.SelectedItem == null)
                    {
                        MessageBox.Show("Data Cant Be Empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ClearData();
                    }
                    else
                    {
                        menuu m = list.Where(s => s.kodemenu == kodeid).FirstOrDefault();
                        if (m != null)
                        {
                            m.Qty += Convert.ToInt32(txtQty.Text);
                        }
                        else
                        {
                            list.Add(new menuu
                            {
                                kodemenu = kodeid,
                                namamember = cbMember.SelectedItem.ToString(),
                                namamenu = txtMenu.Text,
                                Price = Convert.ToInt32(txtPrice.Text),
                                Qty = Convert.ToInt32(txtQty.Text)
                            });
                        }
                        RefreshLabel();
                        RefreshData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Add : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Please Select Data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (selected.Qty > 1)
                {
                    selected.Qty--;
                }
                else
                {
                    list.Remove(selected);
                }
                RefreshLabel();
                RefreshData();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want to Cancel ?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Start();
                cbMember.Enabled = true;
            }
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (int)e.KeyChar != (int)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void dg2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                selected = new menuu();
                selected = list[e.RowIndex];
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            try
            {
                using (DataClassesDataContext db = new DataClassesDataContext())
                {
                    if (dg2.DataSource == null)
                    {
                        MessageBox.Show("Data Cant Be Empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Start();
                    }
                    else
                    {
                        Headerorder d = db.Headerorders.OrderByDescending(s => s.OrderID).FirstOrDefault();
                        if (d != null)
                        {
                            string auto = d.OrderID.ToString().Substring(6, 4);
                            int n = Convert.ToInt32(auto) + 1;
                            c = DateTime.Today.ToString("yyyyMM") + n.ToString("d4");
                        }
                        else
                        {
                            c = DateTime.Today.ToString("yyyyMM") + "0001";
                        }
                        int dd;
                        Detailorder ddd = db.Detailorders.OrderByDescending(s => s.DetailID).FirstOrDefault();
                        if (ddd != null)
                        {
                            string autonumber = ddd.DetailID.ToString();
                            int n = Convert.ToInt32(autonumber) + 1;
                            dd = Convert.ToInt32(n.ToString("d1"));
                        }
                        else
                        {
                            dd = 1;
                        }
                        var a = c;
                        List<Msmember> m = db.Msmembers.Select(s => s).ToList();
                        db.Headerorders.InsertOnSubmit(new Headerorder
                        {
                            OrderID = a,
                            EmployeeID = employeeid,
                            Date = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd")),
                            MemberID = m[cbMember.SelectedIndex].MemberID
                        });
                        db.SubmitChanges();
                        foreach (menuu i in list)
                        {
                            db.Detailorders.InsertOnSubmit(new Detailorder
                            {
                                DetailID = dd,
                                OrderID = a,
                                MenuID = Convert.ToInt32(i.kodemenu),
                                Qty = i.Qty,
                                Price = i.Price,
                                Status = "Pending"
                            });
                            db.SubmitChanges();
                        }
                        MessageBox.Show("Successfully Order Data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Try Again, Please Contact Admin" + "\n" + ex.ToString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Start();
            }
        }

        class menuu
        {
            public string namamember { get; set; }
            public string namamenu { get; set; }
            public string kodemenu { get; set; }
            public int Qty { get; set; }
            public int Price { get; set; }
            public int Total { get { return Price * Qty; } }
        }
    }
}
