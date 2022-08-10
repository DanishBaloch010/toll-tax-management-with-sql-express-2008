using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Data.SqlClient;
using System.Data.Sql;
namespace TollTaxApp
{
    public partial class AppMainPage : Form
    {
        // default constuctor of main page
        // constructor is called first before the form load method.
        public AppMainPage()
        {
            InitializeComponent();
            //creating a grid
            DriverDataGrid.Columns.Add("A", "DID");
            DriverDataGrid.Columns.Add("B", "DriverName");
            DriverDataGrid.Columns.Add("C", "Phone Number");
            DriverDataGrid.Columns.Add("D", "DriverCNIC");
            DriverDataGrid.Columns.Add("E","VID");
            DriverDataGrid.Columns.Add("F", "Vehicle Number");
            DriverDataGrid.Columns.Add("G", "Vehicle Category");
            DriverDataGrid.Columns.Add("H", "Passengers");
            DriverDataGrid.Columns.Add("I", "FromCity");
            DriverDataGrid.Columns.Add("J", "ToCity");
            DriverDataGrid.Columns.Add("K", "TollTax");
            
            DriverDataGrid.Columns[0].Width = 50;
            DriverDataGrid.Columns[1].Width = 100;
            DriverDataGrid.Columns[2].Width = 100;
            DriverDataGrid.Columns[3].Width = 110;
            DriverDataGrid.Columns[4].Width = 50;
            DriverDataGrid.Columns[5].Width = 100;
            DriverDataGrid.Columns[6].Width = 100;
            DriverDataGrid.Columns[7].Width = 100;
            DriverDataGrid.Columns[8].Width = 80;
            DriverDataGrid.Columns[9].Width = 80;
            DriverDataGrid.Columns[10].Width = 150;
        
            //GUI WORK : a method for creating drop down buttons.
            customdesign();

        }

        // when main page loads these button are hidden 
        private void customdesign()
        {
            SubmenumoreOptions.Visible = false;
        }
        // hiding buttons if visible.
        private void hideSubMenu()
        {
            if (SubmenumoreOptions.Visible == true)
                SubmenumoreOptions.Visible = false;
        }
        // showing buttons if hidden
        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else {
                subMenu.Visible = false;
            }
        }
        // these button are hidden and shown on moreOptions button click
        private void optionbtn_Click(object sender, EventArgs e)
        {
            showSubMenu(SubmenumoreOptions);
        }

        // vehicle category text box checking
        private void vehciletxt_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // key pressed
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                // if whats in text box is in databse.
                // we are fetching the data from database and storing it in ArrVehicleCat(a string array collection).
                // than print vehicle category and text in their respective places. 

                // the user is restricted to only choose vehicle category which is in database.
                if (ArrVehicleCat.Contains(vehcilecattxt.Text))
                {
                    // split whats in text box.
                    // create a string array and store the splitted data.
                    string[] A = vehcilecattxt.Text.Split('#');
                    // print vehicle category and tax in their respective places.
                    vehcilecattxt.Text = A[0];
                    tolltaxlbl.Text = A[1]; 
                    // focus on next text box.
                    vehiclenumtxt.Focus();
                }
                // if user entered(a vehicle category) something else which is not in database than.
                else
                {
                    // show error.
                    MessageBox.Show("Select Valid Category!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // empty vehicle text box and tax label.
                    vehcilecattxt.Text = string.Empty;
                    tolltaxlbl.Text = string.Empty;
                    // focus on vehicle category text box.
                    vehcilecattxt.Focus();
                }

            }
        }
        
        SqlConnection conTaxAppDB;
        //vehiclecat
        SqlDataAdapter daVehicleCat;
        DataTable dtVehicleCat;
        //fromcity
        SqlDataAdapter dafcity;
        DataTable dtfcity;
        //tocity
        SqlDataAdapter datcity;
        DataTable dttcity;
        // for getting driver id 
        SqlDataAdapter daDriverID;
        DataTable dtDriverID;
        // getting vehicle id
        SqlDataAdapter daVehicleID;
        DataTable dtVehicleID;

        private void DriverData_Load(object sender, EventArgs e)
        {
            conTaxAppDB = new SqlConnection(@"Data Source=AlexanderAnjelo\sqlexpress;Initial Catalog=TaxAppDB;Integrated Security=True;Pooling=False");
            conTaxAppDB.Open();
            // getting vehicle category.
            daVehicleCat = new SqlDataAdapter("select *from vehicleCategory", conTaxAppDB);
            // getting from cities.
            dafcity = new SqlDataAdapter("select *from fromCity", conTaxAppDB);
            // getting to cities.
            datcity = new SqlDataAdapter("select *from toCity", conTaxAppDB);

            //data table for vehicle cat
            dtVehicleCat = new DataTable();
            // data table for from cities
            dtfcity = new DataTable();
            //data table for to cities
            dttcity = new DataTable();

            // filling all the data adapters into respective data tables
            dafcity.Fill(dtfcity);
            datcity.Fill(dttcity);
            daVehicleCat.Fill(dtVehicleCat);
            // closing connection
            conTaxAppDB.Close();

            // getting next driver id.
            getDriverID();
            // getting next vehicle ID
            getVehicleID();
            // now running all three auto completion fucntions.
            getFromCity();
            getToCity();
            getVehicleCat();
        }

        // function for auto completion of text in three boxes
        // arrVehicleCat is used in two fucntions(keyPreviewVehicleCat and getVehicleCat) that's why it is global.
        AutoCompleteStringCollection ArrVehicleCat = new AutoCompleteStringCollection();
        private void getVehicleCat()
        {
            string vcat = "";
            string tax = "";
            string concat = "";
            // concatinating and filling and the data from table into auto completion string colletion(ArrVehicleCat)
            for (int i = 0; i < dtVehicleCat.Rows.Count; i++)
            {
                vcat = dtVehicleCat.Rows[i]["vcat"].ToString();
                tax = dtVehicleCat.Rows[i]["tax"].ToString();
                concat = vcat+"#"+tax ;

                ArrVehicleCat.Add(concat);
            }
            // now creating a custom auto-complete mode on vehicle category text box.
            vehcilecattxt.AutoCompleteCustomSource = ArrVehicleCat;
            vehcilecattxt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            vehcilecattxt.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        // ArrFromCity is used in two fucntions(keyPreviewFromCity and getFromCity) that's why it is global.
        AutoCompleteStringCollection ArrFromCity = new AutoCompleteStringCollection();
        private void getFromCity()
        {
            // doing the same thing just not concatinating anything.
            string fromcity = "";
            for (int i = 0; i < dtfcity.Rows.Count; i++)
            {
                fromcity = dtfcity.Rows[i]["fcity"].ToString();
                ArrFromCity.Add(fromcity);
            }

            // creating a custom auto-complete mode on FROM city text box
            FromCitytxt.AutoCompleteCustomSource = ArrFromCity;
            vehcilecattxt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            vehcilecattxt.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        // ArrToCity is used in two fucntions(keyPreviewToCity and getToCity) that's why it is global.
        AutoCompleteStringCollection ArrToCity = new AutoCompleteStringCollection();
        private void getToCity()
        {
            // doing the same thing just not concatinating anything.
            string tocity = "";
            for (int i = 0; i < dttcity.Rows.Count; i++)
            {
                tocity = dttcity.Rows[i]["tcity"].ToString();
                ArrToCity.Add(tocity);
            }

            // now creating a custom auto-complete mode on TO city text box
            ToCItytxt.AutoCompleteCustomSource = ArrToCity;
            vehcilecattxt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            vehcilecattxt.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        // getting next driver id function
        // this function is used at form load and save_data button click
        private void getDriverID()
        {
            conTaxAppDB.Open();

            //getting last updated driver ID value.
            // driver id is primary key and auto_increment .
            // starting from 100 with increment of 1.
            daDriverID = new SqlDataAdapter(@"select IDENT_CURRENT('DriverData') as ID", conTaxAppDB);

            dtDriverID = new DataTable();
            daDriverID.Fill(dtDriverID);

            conTaxAppDB.Close();

            string driverID = "";
            int convertedDriverID = 0;
          
            driverID = dtDriverID.Rows[0]["ID"].ToString();
            // converting string to integer
            convertedDriverID = Int32.Parse(driverID);
            // increminting +1 in id 
            convertedDriverID++;
            
            // printing next DriverID in label of GUI
            driveridlbl.Text = convertedDriverID.ToString();
        }

        // getting next vehicle id function
        // this function is used at form load and save_data button click
        private void getVehicleID()
        {
            conTaxAppDB.Open();
            daVehicleID = new SqlDataAdapter(@"select IDENT_CURRENT('VehicleData') as ID", conTaxAppDB);

            dtVehicleID = new DataTable();
            daVehicleID.Fill(dtVehicleID);

            string vehicleID = "";
            int convertedVehicleID = 0;
            
            vehicleID = dtVehicleID.Rows[0]["ID"].ToString();
            convertedVehicleID = Int32.Parse(vehicleID);
            convertedVehicleID++;
            
            // printing next DriverID in label of GUI
            vidlbl.Text = convertedVehicleID.ToString();

        }

        // data insertion work
        // data is inserted into three tables.
        // driver , vehicle and there junction table.
        SqlCommand cmdDriverData;
        SqlCommand cmdVehicleData;
        SqlCommand cmdJunction;
        SqlDataAdapter daDriverData;
        SqlDataAdapter daVehicleData;
        DataTable dtDriverData;
        DataTable dtVehicleData;
        
        private void savedataBtn_Click(object sender, EventArgs e)
        {
            // if data entered int any box in empty.
            if (string.IsNullOrEmpty(dnametxt.Text.Trim()) || string.IsNullOrEmpty(dphonetxt.Text.Trim()) || string.IsNullOrEmpty(dcnictxt.Text.Trim()) || string.IsNullOrEmpty(vehcilecattxt.Text.Trim()) || string.IsNullOrEmpty(vehiclenumtxt.Text.Trim()) || string.IsNullOrEmpty(FromCitytxt.Text.Trim()) || string.IsNullOrEmpty(ToCItytxt.Text.Trim()) || string.IsNullOrEmpty(passengertxt.Text.Trim()))
            {
                MessageBox.Show("Insert Proper Data !!", "ERORR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    conTaxAppDB = new SqlConnection(@"Data Source=AlexanderAnjelo\sqlexpress;Initial Catalog=TaxAppDB;Integrated Security=True;Pooling=False");
                    conTaxAppDB.Open();

                    // parametrized string
                    //insert in driver table.
                    cmdDriverData = new SqlCommand("INSERT INTO DriverData(driver_name,driver_phonenum,driver_cnic) values(@driverName,@driverPhoneNum,@driverCNIC)", conTaxAppDB);
                    cmdDriverData.Parameters.AddWithValue("@driverName", dnametxt.Text);
                    cmdDriverData.Parameters.AddWithValue("@driverPhoneNum", dphonetxt.Text);
                    cmdDriverData.Parameters.AddWithValue("@driverCNIC", dcnictxt.Text);
                    cmdDriverData.ExecuteNonQuery();

                    //insert in vehicle table.
                    cmdVehicleData = new SqlCommand("INSERT INTO VehicleData(vnum,vcat,passengers,toCity,fromCity) values(@vNUM,@vCat,@passengers,@toCity,@fromCity)", conTaxAppDB);
                    cmdVehicleData.Parameters.AddWithValue("@vNUM", vehiclenumtxt.Text);
                    cmdVehicleData.Parameters.AddWithValue("@vCat", vehcilecattxt.Text);
                    cmdVehicleData.Parameters.AddWithValue("@passengers", passengertxt.Text);
                    cmdVehicleData.Parameters.AddWithValue("@toCity", ToCItytxt.Text);
                    cmdVehicleData.Parameters.AddWithValue("@fromCity", FromCitytxt.Text);
                    cmdVehicleData.ExecuteNonQuery();

                    // get current Driver and Vehicle id.
                    daDriverData = new SqlDataAdapter(@"select IDENT_CURRENT('DriverData') as driverID", conTaxAppDB);
                    daVehicleData = new SqlDataAdapter(@"select IDENT_CURRENT('VehicleData') as vehicleID", conTaxAppDB);

                    dtDriverData = new DataTable();
                    dtVehicleData = new DataTable();

                    // sql command line pe jesa output ata hai.
                    // wesa yaha se sochan shuru karo.
                    daDriverData.Fill(dtDriverData);
                    daVehicleData.Fill(dtVehicleData);

                    string did = "";
                    string vid = "";
                   
                    did = dtDriverData.Rows[0]["driverID"].ToString();

                    vid = dtVehicleData.Rows[0]["vehicleID"].ToString();

                    // parsing both the id into integer
                    int didINT = Int32.Parse(did);
                    int vidINT = Int32.Parse(vid);

                    // inserting these id's into junction table.
                    // in junction table did and vid and composite primary key.
                    cmdJunction = new SqlCommand("Insert into DriverVehicleJunction(did,vid) values (@did,@vid)", conTaxAppDB);
                    cmdJunction.Parameters.AddWithValue("@did", didINT);
                    cmdJunction.Parameters.AddWithValue("@vid", vidINT);
                    cmdJunction.ExecuteNonQuery();

                    conTaxAppDB.Close();
                   
                    // woh jo main page me grid pe data dikh raha hai insert hote we bus yeh woh hai ek line. 
                    DriverDataGrid.Rows.Add(driveridlbl.Text, dnametxt.Text, dphonetxt.Text, dcnictxt.Text,vidlbl.Text ,vehiclenumtxt.Text,vehcilecattxt.Text, passengertxt.Text, ToCItytxt.Text,FromCitytxt.Text, tolltaxlbl.Text);

                    // data insert hogaya and ab data dikh bhi raha hai grid me.
                    // ab sare text boxes khali kardo.
                    dnametxt.Text = string.Empty;
                    dphonetxt.Text = string.Empty;
                    dcnictxt.Text = string.Empty;
                    vehcilecattxt.Text = string.Empty;
                    vehiclenumtxt.Text = string.Empty;
                    passengertxt.Text = string.Empty;
                    FromCitytxt.Text = string.Empty;
                    ToCItytxt.Text = string.Empty;

                    tolltaxlbl.Text = "No Data";

                    // next driver id nikalo
                    getDriverID();
                    getVehicleID();

                    // and driver name wala text box pe phir se focus kardo.
                    dnametxt.Focus();

                }
                // database kay koi bhi error ko handle karne kay lie ek catch exception.
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        

        Search searchform;
        // search button form opening 
        private void searchbtn_Click(object sender, EventArgs e)
        {
            // if there is no search form open
            if (searchform == null)
            {
                // create a new search form
                searchform = new Search();
                // open that form.
                searchform.FormClosed += new FormClosedEventHandler(searchform_FormClosed);
                // make this form as top level form.means only this form will work now until it is closed.
                searchform.TopLevel = true;
                // show this form in dialog state.
                searchform.ShowDialog();
            }
            // if open than activate it.
            else
            {
                searchform.Activate();
            }
        }

        void searchform_FormClosed(object sender, FormClosedEventArgs e)
        {
            searchform = null;
        }

        
        // update button form opening 
        Update updateform;
        private void updatebtn_Click(object sender, EventArgs e)
        {
            if (updateform == null)
            {
                updateform = new Update();
                updateform.FormClosed += new FormClosedEventHandler(updateform_FormClosed);
                updateform.TopLevel = true;
                updateform.ShowDialog();
            }

            else
            {
                updateform.Activate();
            }

        }
        
        void updateform_FormClosed(object sender, FormClosedEventArgs e)
        {
            updateform = null;
        }

        // exit button mouse hover code. changing image
        private void exitbtn_MouseEnter(object sender, EventArgs e)
        {
            exitbtn.BackgroundImage= Image.FromFile(@"icons\exit_red.png");
        }

        private void exitbtn_MouseLeave(object sender, EventArgs e)
        {
            exitbtn.BackgroundImage = Image.FromFile(@"icons\exit.png");
        }

        // close application on exit button click.
        private void exitbtn_Click(object sender, EventArgs e)
        {
            DialogResult DR = MessageBox.Show("Are you sure you want to exit ?","EXIT",MessageBoxButtons.YesNo,MessageBoxIcon.Stop);
            if(DR==DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        AllRecordForm allrecordform;
        // view all records button form opening 
        private void viewRecordsbtn_Click(object sender, EventArgs e)
        {
            if (allrecordform == null)
            {
                allrecordform = new AllRecordForm();
                allrecordform.FormClosed += new FormClosedEventHandler(allrecordform_FormClosed);
                allrecordform.TopLevel = true;
                allrecordform.ShowDialog();
            }
            else
            {
                allrecordform.Activate();
            }
        }

        void allrecordform_FormClosed(object sender, FormClosedEventArgs e)
        {
            allrecordform = null;
        }

        // Search_more button form opening 
        SearchMore searchMore;
        private void searchMore_Click(object sender, EventArgs e)
        {
            if (searchMore == null)
            {
                searchMore = new SearchMore();
                searchMore.FormClosed += new FormClosedEventHandler(searchMore_FormClosed);
                searchMore.TopLevel = true;
                searchMore.ShowDialog();
            }
            else
            {
                searchMore.Activate();
            }
        }
        void searchMore_FormClosed(object sender, FormClosedEventArgs e)
        {
            searchMore = null;
        }

        // add_data button form opening 
        addData addDataForm;
        private void addDatabtn_Click(object sender, EventArgs e)
        {
            if (addDataForm == null)
            {
                addDataForm = new addData();
                addDataForm.FormClosed += new FormClosedEventHandler(addDataForm_FormClosed);
                addDataForm.TopLevel = true;
                addDataForm.ShowDialog();
            }
            else
            {
                addDataForm.Activate();
            }
        }

        void addDataForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            addDataForm = null;
        }

        // checking if FROM CITY selected is in the database
        private void FromCitytxt_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (ArrFromCity.Contains(FromCitytxt.Text.Trim()))
                {
                    /// do nothing
                }
                else
                {
                    MessageBox.Show("Select Valid From City !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FromCitytxt.Text = string.Empty;
                    FromCitytxt.Focus();
                }

            }
        }

        // checking if TO CITY selected is in the database
        private void ToCItytxt_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (ArrToCity.Contains(ToCItytxt.Text.Trim()))
                {
                   // do nothing
                }
                else
                {
                    MessageBox.Show("Select Valid To City !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ToCItytxt.Text = string.Empty;
                    ToCItytxt.Focus();
                }
            }
        }


  
    }
}
