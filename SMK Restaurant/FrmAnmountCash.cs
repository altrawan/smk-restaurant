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
    public partial class FrmAnmountCash : Form
    {
        public FrmAnmountCash()
        {
            InitializeComponent();
        }

        private void FrmAnmountCash_Load(object sender, EventArgs e)
        {
            lblTotal.Text = "Rp. " + Class.total.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int a = Convert.ToInt32(txtMoney.Text);
            int b = Convert.ToInt32(Class.total);
            if (txtMoney.Text == "")
            {
                MessageBox.Show("Data Cant Be Empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMoney.Clear();
                txtMoney.Focus();
            }
            else if (a < b)
            {
                MessageBox.Show("Your Money Is Not Enough", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMoney.Clear();
                txtMoney.Focus();
            }
            else
            {
                using (DataClassesDataContext db = new DataClassesDataContext())
                {
                    Headerorder h = db.Headerorders.Where(s => s.OrderID == Class.orderid).FirstOrDefault();
                    h.Payment = "Cash";
                    db.SubmitChanges();
                    MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel ?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void txtMoney_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (int)e.KeyChar != (int)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}
