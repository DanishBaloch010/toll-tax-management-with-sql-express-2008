using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;


namespace TollTaxApp
{
    public partial class AllRecordForm : Form
    {
        public AllRecordForm()
        {
            InitializeComponent();

            AllDriverDataGrid.Columns.Add("A", "DID");
            AllDriverDataGrid.Columns.Add("B", "DriverName");
            AllDriverDataGrid.Columns.Add("C", "Phone Number");
            AllDriverDataGrid.Columns.Add("D", "DriverCNIC");
            AllDriverDataGrid.Columns.Add("E", "VID");
            AllDriverDataGrid.Columns.Add("F", "Vehicle Number");
            AllDriverDataGrid.Columns.Add("G", "Vehicle Category");
            AllDriverDataGrid.Columns.Add("H", "Passengers");
            AllDriverDataGrid.Columns.Add("I", "ToCity");
            AllDriverDataGrid.Columns.Add("J", "FromCity");
            AllDriverDataGrid.Columns.Add("K", "TollTax");

            AllDriverDataGrid.Columns[0].Width = 50;
            AllDriverDataGrid.Columns[1].Width = 110;
            AllDriverDataGrid.Columns[2].Width = 110;
            AllDriverDataGrid.Columns[3].Width = 110;
            AllDriverDataGrid.Columns[4].Width = 50;
            AllDriverDataGrid.Columns[5].Width = 100;
            AllDriverDataGrid.Columns[6].Width = 100;
            AllDriverDataGrid.Columns[7].Width = 80;
            AllDriverDataGrid.Columns[8].Width = 80;
            AllDriverDataGrid.Columns[9].Width = 80;
            AllDriverDataGrid.Columns[10].Width = 160;
        }

        private void closepicbox_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void closepicbox_MouseEnter(object sender, EventArgs e)
        {
            closepicbox.Image = Image.FromFile(@"icons\close.png");
        }

        private void closepicbox_MouseLeave(object sender, EventArgs e)
        {
            closepicbox.Image = Image.FromFile(@"icons\close_black.png");
        }

        SqlConnection conTaxAppDB;
        SqlDataAdapter daAllDataView;
        DataTable dtAllDataView;

        // all work of this form is in the form load event
        // only showing all the data in the database.
        // data is fetched and shown from a view.
        private void AllRecordForm_Load(object sender, EventArgs e)
        {
            AllDriverDataGrid.Rows.Clear();
            conTaxAppDB = new SqlConnection(@"Data Source=AlexanderAnjelo\sqlexpress;Initial Catalog=TaxAppDB;Integrated Security=True;Pooling=False");
            conTaxAppDB.Open();
            daAllDataView = new SqlDataAdapter(@"select * from allData",conTaxAppDB);
            dtAllDataView=new DataTable();
            daAllDataView.Fill(dtAllDataView);

            conTaxAppDB.Close();

            

            string[] array= new string[11];
            string str;
            for (int i = 0; i < dtAllDataView.Rows.Count; i++) // row
            {
                // is loop kay baad ek array me sara data agaya hai. ek row ka bus. 
                for (int j = 0; j < dtAllDataView.Columns.Count; j++) //column
                {
                    str = dtAllDataView.Rows[i][j].ToString();
                    array[j] = str;
                }

                AllDriverDataGrid.Rows.Add(array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9], array[10]);
            }

                int Count = dtAllDataView.Rows.Count;
                lblCount.Text = "Total Records: " + Count;
        }
    }
}
