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
    public partial class FrmPayment : UserControl
    {
        public FrmPayment()
        {
            InitializeComponent();
        }

        private void ComboValue()
        {
            try
            {
                using (DataClassesDataContext db = new DataClassesDataContext())
                {
                    //cbxOrderid.DataSource = db.Headerorders.Select(s => s.OrderID);
                    
                    cbOrderID.DataSource = from u in db.Headerorders
                                           where u.Payment == null
                                           group u by u.OrderID into s
                                           select s.Key;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Total()
        {
            int total = 0;
            foreach (DataGridViewRow row in dg1.Rows)
            {
                total += Convert.ToInt32(row.Cells["Total"].Value.ToString());
                Class.orderid = cbOrderID.Text;
                Class.total = total.ToString();
            }
            lblTotal.Text = "Total : " + total.ToString();
        }

        private void DisplayData()
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                dg1.DataSource = from u in db.Detailorders
                                 join s in db.Msmenus
                                 on u.MenuID equals s.MenuID
                                 where u.OrderID == cbOrderID.SelectedItem.ToString() 
                                 && u.Status == "Deliver"
                                 select new
                                 {
                                     Menu = s.Name,
                                     Qty = u.Qty,
                                     u.DetailID,
                                     u.Status,
                                     Price = u.Price,
                                     Action = u.Status,
                                     Total = u.Price * u.Qty
                                 };
                dg1.Columns["Action"].Visible = false;
                dg1.Columns["Detailid"].Visible = false;
                dg1.Columns["Status"].Visible = false;
                Total();
            }
        }
        
        private void ClaerData()
        {
            lblTotal.Text = "Total : 0";
            dg1.Rows.Clear();
            cbPayment.SelectedItem = null;
            txtCard.Clear();
            cbBank.SelectedItem = null;
        }

        private void DisabledData()
        {
            cbPayment.Enabled = false;
            txtCard.Enabled = false;
            cbBank.Enabled = false;
        }

        private void FrmPayment_Load(object sender, EventArgs e)
        {
            ComboValue();
            DisabledData();
            this.Refresh();
        }

        private void cbOrderID_SelectedValueChanged(object sender, EventArgs e)
        {
            DisplayData();
            cbPayment.Enabled = true;
        }

        private void cbPayment_SelectedValueChanged(object sender, EventArgs e)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                if (dg1.Rows.Count == 0)
                {
                    MessageBox.Show("Data Cant Be Empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (cbPayment.SelectedItem.Equals("Cash"))
                    {
                        (new FrmAnmountCash()).Show();
                        DisabledData();
                        //this.Enabled = false;
                    }
                    else
                    {
                        txtCard.Enabled = true;
                        cbBank.Enabled = true;
                    }
                }

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbPayment.SelectedItem == null || txtCard.Text == "" || cbBank.SelectedItem == null)
                {
                    MessageBox.Show("Data Cant Be Empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbPayment.SelectedItem = null;
                    txtCard.Clear();
                    cbBank.SelectedItem = null;
                }
                else if (txtCard.Text.Length != 16)
                {
                    MessageBox.Show("Card Number Must 16 Digit", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    using (DataClassesDataContext db = new DataClassesDataContext())
                    {
                        Headerorder h = db.Headerorders.Where(s => s.OrderID == cbOrderID.SelectedItem.ToString()).FirstOrDefault();
                        h.Date = DateTime.Now;
                        h.Payment = cbPayment.SelectedItem.ToString();
                        h.Bank = cbBank.SelectedItem.ToString();
                        h.CardNumber = txtCard.Text;
                        db.SubmitChanges();
                        cbPayment.SelectedItem = null;
                        txtCard.Clear();
                        cbBank.SelectedItem = null;
                        dg1.DataSource = null;
                        lblTotal.Text = "Total : 0";
                        this.Refresh();
                        MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void txtCard_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (int)e.KeyChar != (int)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}
