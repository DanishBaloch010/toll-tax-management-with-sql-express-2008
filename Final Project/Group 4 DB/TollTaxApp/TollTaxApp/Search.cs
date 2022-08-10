using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Data.Sql;

namespace TollTaxApp
{
    public partial class Search : Form
    {
        public Search()
        {
            InitializeComponent();

            // when form load everything is hidden.
            nametxt.Visible = false;
            namelbl.Visible = false;
            phonenotxt.Visible = false;
            phonelbl.Visible = false;
            cnictxt.Visible = false;
            cniclbl.Visible = false;
            vehiclecategorytxt.Visible = false;
            vehiclecategorylbl.Visible = false;
            vehiclenumtxt.Visible = false;
            vehiclenamelbl.Visible = false;
            passengerstxt.Visible = false;
            passengerlbl.Visible = false;
            fromcitytxt.Visible = false;
            fromcitylbl.Visible = false;
            tocitytxt.Visible = false;
            tocitylbl.Visible = false;
            tolltaxtxt.Visible = false;
            tolltaxlbl.Visible = false;

            // the two text box in which user will enter id are disabled.
            // they will activate on choice of radio button
            didtxt.Enabled = false;
            vidtxt.Enabled = false;
        }
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        // connection for database
        SqlConnection conTaxAppDB;
        // an object and data table to work with view
        SqlDataAdapter daViewAllData;
        DataTable dtViewAllData;

        // these two strings are not used .
        // leave them , maybe baad me kaam ae.
        string driverid;
        string vehicleid;

        private void searchbtn_Click(object sender, EventArgs e)
        {
            //  agar dono id wale text box enable nahi hai.
            // yani abhi tak radio button koi select nahi hoa wa
            // to usko handle yaha pe kiya hai.
            if (didtxt.Enabled == false && vidtxt.Enabled == false)
            {
                MessageBox.Show("Please Select a Method for Search.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // agar radio button checked hai to.
            // iske andar bhi khuch conditions handle ki hai takke program atak na jae. 
            else if (didradiobtn.Checked)
            {
                // checked  hai to yani visible hai. 
                // agar visible hai and text box empty hai ya phir spaces dale we hai bus to..
                if (didtxt.Visible == true && string.IsNullOrEmpty(didtxt.Text) || didtxt.Text.Trim() == "")
                {
                    MessageBox.Show("Please Insert DID for Search.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    didtxt.Text = string.Empty;
                    vidtxt.Text = string.Empty;
                }
                // agar esa nahi hai to database me entered values to check karo.
                else
                {

                    conTaxAppDB = new SqlConnection(@"Data Source=AlexanderAnjelo\sqlexpress;Initial Catalog=TaxAppDB;Integrated Security=True;Pooling=False");
                    conTaxAppDB.Open();
                    daViewAllData = new SqlDataAdapter(@"select *from allData where driver_id = @did", conTaxAppDB);
                    //string me dala text box wali cheez ko 
                    string dids = didtxt.Text.Trim();
                    // phir int me convert kiya kyunke databse me id int hai. 
                    int did = Int32.Parse(dids);
                    daViewAllData.SelectCommand.Parameters.AddWithValue("@did", did);
                    conTaxAppDB.Close();

                    // ab data agaya hai. ab baat yeh hai kay har waqt ek row aegi ya koi row nahi aegi.
                    dtViewAllData = new DataTable();
                    daViewAllData.Fill(dtViewAllData);
                    // agar ek bhi row nahi ayi hai to..
                    if (dtViewAllData.Rows.Count == 0)
                    {
                        nametxt.Visible = false;
                        namelbl.Visible = false;
                        phonenotxt.Visible = false;
                        phonelbl.Visible = false;
                        cnictxt.Visible = false;
                        cniclbl.Visible = false;
                        vehiclecategorytxt.Visible = false;
                        vehiclecategorylbl.Visible = false;
                        vehiclenumtxt.Visible = false;
                        vehiclenamelbl.Visible = false;
                        passengerstxt.Visible = false;
                        passengerlbl.Visible = false;
                        fromcitytxt.Visible = false;
                        fromcitylbl.Visible = false;
                        tocitytxt.Visible = false;
                        tocitylbl.Visible = false;
                        tolltaxtxt.Visible = false;
                        tolltaxlbl.Visible = false;
                        MessageBox.Show("Invalid DID.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                    // nahi to ek to row ayi hi hogi.
                    else
                    {
                        nametxt.Visible = true;
                        namelbl.Visible = true;
                        phonenotxt.Visible = true;
                        phonelbl.Visible = true;
                        cnictxt.Visible = true;
                        cniclbl.Visible = true;
                        vehiclecategorytxt.Visible = true;
                        vehiclecategorylbl.Visible = true;
                        vehiclenumtxt.Visible = true;
                        vehiclenamelbl.Visible = true;
                        passengerstxt.Visible = true;
                        passengerlbl.Visible = true;
                        fromcitytxt.Visible = true;
                        fromcitylbl.Visible = true;
                        tocitytxt.Visible = true;
                        tocitylbl.Visible = true;
                        tolltaxtxt.Visible = true;
                        tolltaxlbl.Visible = true;

                        // ek hi row hai to uske sare columns kay data ko respective txt boxes me daal denge. 
                        // yaha pe loop ki zaroorat nahi hai wese. 
                        for (int i = 0; i < dtViewAllData.Rows.Count; i++)
                        {
                            driverid = dtViewAllData.Rows[i]["driver_id"].ToString();
                            nametxt.Text = dtViewAllData.Rows[i]["driver_name"].ToString();
                            phonenotxt.Text = dtViewAllData.Rows[i]["driver_phonenum"].ToString();
                            cnictxt.Text = dtViewAllData.Rows[i]["driver_cnic"].ToString();

                            vehicleid = dtViewAllData.Rows[i]["vid"].ToString();
                            vehiclenumtxt.Text = dtViewAllData.Rows[i]["vnum"].ToString();
                            vehiclecategorytxt.Text = dtViewAllData.Rows[i]["vcat"].ToString();
                            passengerstxt.Text = dtViewAllData.Rows[i]["passengers"].ToString();
                            tocitytxt.Text = dtViewAllData.Rows[i]["toCity"].ToString();
                            fromcitytxt.Text = dtViewAllData.Rows[i]["fromCity"].ToString();
                            tolltaxtxt.Text = dtViewAllData.Rows[i]["tax"].ToString();
                        }
                    }
                }
            } 

            // ab isi tarah vid pe conditions check ki hai and data fetch kiya hai.
            else if (vidradiobtn.Checked)
            {
                if (vidtxt.Visible == true && string.IsNullOrEmpty(vidtxt.Text) || vidtxt.Text.Trim() == "")
                {
                    MessageBox.Show("Please Insert VID for Search.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    didtxt.Text = string.Empty;
                    vidtxt.Text = string.Empty;
                }
                else
                {
                    conTaxAppDB = new SqlConnection(@"Data Source=AlexanderAnjelo\sqlexpress;Initial Catalog=TaxAppDB;Integrated Security=True;Pooling=False");
                    conTaxAppDB.Open();
                    daViewAllData = new SqlDataAdapter(@"select *from allData where vid = @vid", conTaxAppDB);
                    string vids = vidtxt.Text.Trim();
                    int vid = Int32.Parse(vids);
                    daViewAllData.SelectCommand.Parameters.AddWithValue("@vid", vid);
                    conTaxAppDB.Close();

                    dtViewAllData = new DataTable();
                    daViewAllData.Fill(dtViewAllData);

                    if (dtViewAllData.Rows.Count == 0)
                    {
                        nametxt.Visible = false;
                        namelbl.Visible = false;
                        phonenotxt.Visible = false;
                        phonelbl.Visible = false;
                        cnictxt.Visible = false;
                        cniclbl.Visible = false;
                        vehiclecategorytxt.Visible = false;
                        vehiclecategorylbl.Visible = false;
                        vehiclenumtxt.Visible = false;
                        vehiclenamelbl.Visible = false;
                        passengerstxt.Visible = false;
                        passengerlbl.Visible = false;
                        fromcitytxt.Visible = false;
                        fromcitylbl.Visible = false;
                        tocitytxt.Visible = false;
                        tocitylbl.Visible = false;
                        tolltaxtxt.Visible = false;
                        tolltaxlbl.Visible = false;
                        MessageBox.Show("Invalid VID.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        nametxt.Visible = true;
                        namelbl.Visible = true;
                        phonenotxt.Visible = true;
                        phonelbl.Visible = true;
                        cnictxt.Visible = true;
                        cniclbl.Visible = true;
                        vehiclecategorytxt.Visible = true;
                        vehiclecategorylbl.Visible = true;
                        vehiclenumtxt.Visible = true;
                        vehiclenamelbl.Visible = true;
                        passengerstxt.Visible = true;
                        passengerlbl.Visible = true;
                        fromcitytxt.Visible = true;
                        fromcitylbl.Visible = true;
                        tocitytxt.Visible = true;
                        tocitylbl.Visible = true;
                        tolltaxtxt.Visible = true;
                        tolltaxlbl.Visible = true;

                        for (int i = 0; i < dtViewAllData.Rows.Count; i++)
                        {
                            driverid = dtViewAllData.Rows[i]["driver_id"].ToString();
                            nametxt.Text = dtViewAllData.Rows[i]["driver_name"].ToString();
                            phonenotxt.Text = dtViewAllData.Rows[i]["driver_phonenum"].ToString();
                            cnictxt.Text = dtViewAllData.Rows[i]["driver_cnic"].ToString();

                            vehicleid = dtViewAllData.Rows[i]["vid"].ToString();
                            vehiclenumtxt.Text = dtViewAllData.Rows[i]["vnum"].ToString();
                            vehiclecategorytxt.Text = dtViewAllData.Rows[i]["vcat"].ToString();
                            passengerstxt.Text = dtViewAllData.Rows[i]["passengers"].ToString();
                            tocitytxt.Text = dtViewAllData.Rows[i]["toCity"].ToString();
                            fromcitytxt.Text = dtViewAllData.Rows[i]["fromCity"].ToString();
                            tolltaxtxt.Text = dtViewAllData.Rows[i]["tax"].ToString();
                        }
                    }
                }
            } 
        }

        private void Search_Load(object sender, EventArgs e)
        {

        }
        private void didradiobtn_CheckedChanged(object sender, EventArgs e)
        {
            // if did is checked than show did text box.
            didtxt.Enabled = true;
            didtxt.Visible = true;
            vidtxt.Visible = false;
        }

        private void vidradiobtn_CheckedChanged(object sender, EventArgs e)
        {
            // if vid is checked than show vid text box.
            vidtxt.Enabled = true;
            vidtxt.Visible = true;
            didtxt.Visible = false;
        }

    }
}
