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
    public partial class FrmIncome : Form
    {

        public FrmIncome()
        {
            InitializeComponent();
        }

        private void ClearData()
        {
            dateTimePicker1.Value = DateTime.Now;
            txtIncome.Text = "";
            dateTimePicker1.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > DateTime.Now)
            {
                MessageBox.Show("Tanggal tidak boleh melebihi tanggal sekarang", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ClearData();
            }
            else if (txtIncome.Text == "")
            {
                MessageBox.Show("Data Cant Be Empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearData();
            }
            else
            {
                using (DataClassesDataContext db = new DataClassesDataContext())
                {
                    db.Msincomes.InsertOnSubmit(new Msincome()
                    {
                        Date = DateTime.Parse(dateTimePicker1.Value.ToString("yyyy-MM-dd")),
                        Income = Convert.ToInt32(txtIncome.Text)
                    });
                    db.SubmitChanges();
                    ClearData();
                    MessageBox.Show("Successfully Added Data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void txtIncome_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (int)e.KeyChar != (int)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}
