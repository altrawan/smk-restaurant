using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;

namespace SMK_Restaurant
{
    public partial class FrmReport : UserControl
    {
        public FrmReport()
        {
            InitializeComponent();
        }

        private void DisplayData()
        {
            SqlConnection con = new SqlConnection("Data Source=ACER-NOTEBOOK;Initial Catalog=LKSN2018;Integrated Security=True;"); 
            con.Open();
            DateTime fromDate, toDate;
            DateTime.TryParse(dateTimePicker1.Text, out fromDate);
            DateTime.TryParse(dateTimePicker2.Text, out toDate);
            SqlDataAdapter adapt = new SqlDataAdapter("Select FORMAT(Date,'MMM') AS M, FORMAT(SUM([Income]), '0,,') As I from Msincome WHERE Date BETWEEN @From AND @To GROUP BY FORMAT(Date,'MMM')", con);
            DataSet ds = new DataSet();
            adapt.SelectCommand.Parameters.Add("@From", SqlDbType.DateTime).Value = fromDate;
            adapt.SelectCommand.Parameters.Add("@To", SqlDbType.DateTime).Value = toDate;
            adapt.Fill(ds, "MsIncome");
            dg1.DataSource = ds.Tables["MsIncome"];
            dg1.Columns[0].HeaderText = "Month";
            dg1.Columns[1].HeaderText = "Income";
            con.Close();
        }

        //fillChart method  
        private void fillChart()
        {
            chart1.Titles.RemoveAt(0);
            SqlConnection con = new SqlConnection("Data Source=ACER-NOTEBOOK;Initial Catalog=LKSN2018;Integrated Security=True;");
            DataSet ds = new DataSet();
            con.Open();
            DateTime fromDate, toDate;
            DateTime.TryParse(dateTimePicker1.Text, out fromDate);
            DateTime.TryParse(dateTimePicker2.Text, out toDate);
            SqlDataAdapter adapt = new SqlDataAdapter("Select FORMAT(Date,'MMM') AS M, FORMAT(SUM([Income]), '0,,') As I from Msincome WHERE Date BETWEEN @From AND @To GROUP BY FORMAT(Date,'MMM')", con);
            adapt.SelectCommand.Parameters.Add("@From", SqlDbType.DateTime).Value = fromDate;
            adapt.SelectCommand.Parameters.Add("@To", SqlDbType.DateTime).Value = toDate;
            adapt.Fill(ds);
            chart1.DataSource = ds;
            //set the member of the chart data source used to data bind to the X-values of the series  
            chart1.Series["Income"].XValueMember = "M";
            //set the member columns of the chart data source used to data bind to the X-values of the series  
            chart1.Series["Income"].YValueMembers = "I";
            //chart1.Series["Income"].Points[0].Label = "5";
            //chart1.Series["Income"].Points[1].Label = "1";
            chart1.Titles.Add("Income in Milion");
            con.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            DateTime a = dateTimePicker1.Value;
            DateTime b = dateTimePicker2.Value;
            if (a == b)
            {
                MessageBox.Show("Tanggal dari dan sampai tidak boleh sama", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (dateTimePicker1.Value > DateTime.Now || dateTimePicker2.Value > DateTime.Now)
            {
                MessageBox.Show("Tanggal tidak boleh melebihi tanggal sekarang", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DisplayData();
                fillChart();
            }
        }

        private void btnIncome_Click(object sender, EventArgs e)
        {
            (new FrmIncome()).Show();
        }
    }
}
