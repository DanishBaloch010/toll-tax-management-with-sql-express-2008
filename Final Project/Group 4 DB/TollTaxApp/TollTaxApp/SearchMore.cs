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
    public partial class SearchMore : Form
    {
        public SearchMore()
        {
            InitializeComponent();

        }
        SqlConnection conTaxAppDB;
        SqlDataAdapter daAllData;
        DataTable dtAllData;

   
        // creating drop shadow
        private const int cs_shadow = 0x00020000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = cs_shadow;
                return cp;
            }
        }

        // departure
        SqlDataAdapter dafcity;
        DataTable dtfcity;

        //arrival
        SqlDataAdapter datcity;
        DataTable dttcity;

        // vcat 
        SqlDataAdapter daVehicleCat;
        DataTable dtVehicleCat;

        AutoCompleteStringCollection ArrFromCity = new AutoCompleteStringCollection();
        AutoCompleteStringCollection ArrToCity = new AutoCompleteStringCollection();
        AutoCompleteStringCollection ArrVehicleCat = new AutoCompleteStringCollection();

        // auto completion method. used on load event.
        private void getFromCity()
        {
            conTaxAppDB = new SqlConnection(@"Data Source=AlexanderAnjelo\sqlexpress;Initial Catalog=TaxAppDB;Integrated Security=True;Pooling=False");
            conTaxAppDB.Open();
            //dafcity = new SqlDataAdapter(@"select sum(passengers) as totalPassengersDeparted where fromCity in (select fromCity from allData where fromCity = @fcity) ", conTaxAppDB);
            //dafcity.SelectCommand.Parameters.AddWithValue("@fcity",departedtxt.Text.ToString().Trim());
            dafcity = new SqlDataAdapter(@"select *FROM fromCity",conTaxAppDB);
            conTaxAppDB.Close();

            dtfcity = new DataTable();
            dafcity.Fill(dtfcity);

            
            string fromcity = "";
            for (int i = 0; i < dtfcity.Rows.Count; i++)
            {
                fromcity = dtfcity.Rows[i]["fcity"].ToString();
                ArrFromCity.Add(fromcity);
            }

            departedtxt.AutoCompleteCustomSource = ArrFromCity;
            departedtxt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            departedtxt.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        // auto completion method. used on load event.
        private void getToCity()
        {
            conTaxAppDB = new SqlConnection(@"Data Source=AlexanderAnjelo\sqlexpress;Initial Catalog=TaxAppDB;Integrated Security=True;Pooling=False");
            conTaxAppDB.Open();
            //dafcity = new SqlDataAdapter(@"select sum(passengers) as totalPassengersDeparted where fromCity in (select fromCity from allData where fromCity = @fcity) ", conTaxAppDB);
            //dafcity.SelectCommand.Parameters.AddWithValue("@fcity",departedtxt.Text.ToString().Trim());
            datcity = new SqlDataAdapter("select *FROM toCity", conTaxAppDB);
            conTaxAppDB.Close();

            dttcity = new DataTable();
            datcity.Fill(dttcity);

          
            string toCity = "";
            for (int i = 0; i < dttcity.Rows.Count; i++)
            {
                toCity = dttcity.Rows[i]["tcity"].ToString();
                ArrToCity.Add(toCity);
            }

            arrivedtxt.AutoCompleteCustomSource = ArrToCity;
            arrivedtxt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            arrivedtxt.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        // auto completion method. used on load event.
        private void getVehicleCat()
        {
            conTaxAppDB = new SqlConnection(@"Data Source=AlexanderAnjelo\sqlexpress;Initial Catalog=TaxAppDB;Integrated Security=True;Pooling=False");
            conTaxAppDB.Open();
            daVehicleCat = new SqlDataAdapter(@"select vcat from vehicleCategory", conTaxAppDB);
            conTaxAppDB.Close();
            dtVehicleCat = new DataTable();
            daVehicleCat.Fill(dtVehicleCat);

            string vcat ="";
            for (int i = 0; i < dtVehicleCat.Rows.Count; i++)
            {
                vcat = dtVehicleCat.Rows[i]["vcat"].ToString();
                ArrVehicleCat.Add(vcat);
            }
            vcattaxttxt.AutoCompleteCustomSource = ArrVehicleCat;
            vcattaxttxt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            vcattaxttxt.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        //calculating total passengers and total tax collection method. used on load event.
        private void getPassengers_Tax()
        {

            conTaxAppDB = new SqlConnection(@"Data Source=AlexanderAnjelo\sqlexpress;Initial Catalog=TaxAppDB;Integrated Security=True;Pooling=False");
            conTaxAppDB.Open();
            daAllData = new SqlDataAdapter(@"select sum(passengers) as TotalTravel , sum(tax) as TotalTax from allData", conTaxAppDB);
            conTaxAppDB.Close();

            // new data table banaya. 
            dtAllData = new DataTable();
            // us data table ko fill kardia. 
            daAllData.Fill(dtAllData);

            totalTravellbl.Text = dtAllData.Rows[0]["TotalTravel"].ToString();
            totaltaxlbl.Text = dtAllData.Rows[0]["TotalTax"].ToString();
        }

        private void closepicbox_MouseEnter(object sender, EventArgs e)
        {
            closepicbox.Image = Image.FromFile(@"icons\close.png");
        }

        private void closepicbox_MouseLeave(object sender, EventArgs e)
        {
            closepicbox.Image = Image.FromFile(@"icons\close_black.png");
        }

        private void closepicbox_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void SearchMore_Load(object sender, EventArgs e)
        {
            // this fucntion is getting total number of passenger and total tax collection.s
            // than printing the calculated data on form.
            getPassengers_Tax();

            // these three are custom auto completion methods.
            // used for checking if entered data is in the database or not. 
            // otherwise the calulation will not be done.
            getFromCity();
            getToCity();
            getVehicleCat();
        }

    
        // checking if selected city is in database.
        // if in database than calculating the total passengers arrived into that city.
        // using sub query.
        private void arrivedtxt_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (ArrToCity.Contains(arrivedtxt.Text))
                {
                    conTaxAppDB = new SqlConnection(@"Data Source=AlexanderAnjelo\sqlexpress;Initial Catalog=TaxAppDB;Integrated Security=True;Pooling=False");
                    conTaxAppDB.Open();
                     
                    daAllData = new SqlDataAdapter(@"select sum(passengers) as TotalArrived from allData where toCity in (select toCity from allData where toCity=@tcity)", conTaxAppDB);
                    daAllData.SelectCommand.Parameters.AddWithValue("@tcity", arrivedtxt.Text.Trim());
                    conTaxAppDB.Close();
                    dtAllData = new DataTable();
                    daAllData.Fill(dtAllData);
                    arrivedlbl.Text = dtAllData.Rows[0]["TotalArrived"].ToString();
                }
                else
                {
                    MessageBox.Show("Select Valid City!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    arrivedtxt.Text = string.Empty;
                    arrivedlbl.Text = "No Data"; 
                }
            }
        }

        // checking if selected city is in database.
        // if in database than calculating the total passengers departed from that city.
        // using sub query.
        private void departedtxt_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (ArrFromCity.Contains(departedtxt.Text))
                {
                    conTaxAppDB = new SqlConnection(@"Data Source=AlexanderAnjelo\sqlexpress;Initial Catalog=TaxAppDB;Integrated Security=True;Pooling=False");
                    conTaxAppDB.Open();
                    daAllData = new SqlDataAdapter(@"select sum(passengers) as TotalDeparted from allData where fromCity in (select fromCity from allData where fromCity=@fcity)", conTaxAppDB);
                    daAllData.SelectCommand.Parameters.AddWithValue("@fcity", departedtxt.Text.Trim());
                    conTaxAppDB.Close();
                    dtAllData = new DataTable();
                    daAllData.Fill(dtAllData);
                    departedlbl.Text = dtAllData.Rows[0]["TotalDeparted"].ToString();
                }
                else
                {
                    MessageBox.Show("Select Valid City!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    departedtxt.Text = string.Empty;
                    departedlbl.Text = "No Data";
                }
            }
        }

        SqlDataAdapter davcattax;
        DataTable dtvcattax;

        // checking if selected vehicle category is in database.
        // if in database than calculating the total tax collection of that category.
        // using sub query.
        private void vcattaxttxt_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (ArrVehicleCat.Contains(vcattaxttxt.Text.Trim()))
                {
                    conTaxAppDB = new SqlConnection(@"Data Source=AlexanderAnjelo\sqlexpress;Initial Catalog=TaxAppDB;Integrated Security=True;Pooling=False");
                    conTaxAppDB.Open();
                    davcattax = new SqlDataAdapter(@"select sum(tax) as TotalTax from allData where vcat in (select vcat from allData where vcat=@vcat)", conTaxAppDB);
                    davcattax.SelectCommand.Parameters.AddWithValue("@vcat", vcattaxttxt.Text.Trim());
                    conTaxAppDB.Close();
                    dtvcattax = new DataTable();
                    davcattax.Fill(dtvcattax);

                    string totalTax = dtvcattax.Rows[0]["TotalTax"].ToString();

                    if (string.IsNullOrEmpty(totalTax))
                    {
                        MessageBox.Show("No Tax record of this vehicle Category!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        vcattaxlbl.Text = "0";
                        vcattaxttxt.Focus();
                        
                    }
                    else
                    {
                        vcattaxlbl.Text = totalTax;
                    }
                }
                else
                {
                    MessageBox.Show("Select Valid Vehicle Category!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    vcattaxttxt.Text = string.Empty;
                    vcattaxlbl.Text = "No Data";
                }
            }
            
        }
    }
}