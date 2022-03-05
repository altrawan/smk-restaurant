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
    public partial class FrmViewOrder : UserControl
    {
        public FrmViewOrder()
        {
            InitializeComponent();
        }

        private void ComboValue()
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                //cbxOrderid.DataSource = db.Headerorders.Select(s => s.OrderID);
                cbOrderID.DataSource = from u in db.Detailorders
                                        where u.Status != "Deliver"
                                        group u by u.OrderID into s
                                        select s.Key;
            }
        }
        private void DisplayData()
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                dg1.DataSource = from u in db.Detailorders
                                 join s in db.Msmenus
                                 on u.MenuID equals s.MenuID
                                 where u.OrderID == cbOrderID.SelectedItem.ToString()
                                 select new
                                 {
                                     Menu = s.Name,
                                     u.Qty,
                                     u.DetailID,
                                     u.Status,
                                     s.Price,
                                 };
                dg1.Columns["Status"].Visible = false;
                dg1.Columns["Price"].Visible = false;
                dg1.Columns["Detailid"].Visible = false;
                dg1.Columns["Action"].DisplayIndex = 2;
                foreach (DataGridViewRow row in dg1.Rows)
                {
                    row.Cells["Action"].Value = row.Cells["Status"].Value;
                }
                isRows = true;
            }
        }
        private bool isRows;

        private void FrmViewOrder_Load(object sender, EventArgs e)
        {
            ComboValue();
            this.Refresh();
        }

        private void cbOrderID_SelectedValueChanged(object sender, EventArgs e)
        {
            DisplayData();
        }

        private void dg1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isRows)
            {
                DataGridViewRow row = dg1.Rows[e.RowIndex];
                using (DataClassesDataContext db = new DataClassesDataContext())
                {
                    int id = Convert.ToInt32(row.Cells["Detailid"].Value.ToString());
                    Detailorder d = db.Detailorders.Where(s => s.DetailID == id).FirstOrDefault();
                    d.Status = row.Cells["Action"].Value.ToString();
                    db.SubmitChanges();
                }
            }
        }
    }
}
