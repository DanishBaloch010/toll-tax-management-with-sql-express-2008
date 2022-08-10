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
    public partial class Update : Form
    {

        public Update()
        {
            InitializeComponent();

            nametxt.Visible = false;
            namelbl.Visible = false;
            phonenotxt.Visible = false;
            phonelbl.Visible = false;
            cnictxt.Visible = false;
            cniclbl.Visible = false;
            vehiclecategorytxt.Visible = false;
            vehiclecategorylbl.Visible = false;
            vehiclenumtxt.Visible = false;
            vehiclenumlbl.Visible = false;
            passengerstxt.Visible = false;
            passengerlbl.Visible = false;
            fromcitytxt.Visible = false;
            fromcitylbl.Visible = false;
            tocitytxt.Visible = false;
            tocitylbl.Visible = false;
            tolltaxtxt.Visible = false;
            tolltaxlbl.Visible = false;
            updatebtn.Visible = false;

            vehiclecategorytxt.ReadOnly = true;
            fromcitytxt.ReadOnly = true;
            tocitytxt.ReadOnly = true;
            tolltaxtxt.ReadOnly = true;

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

        SqlConnection conTaxAppDB;
        SqlDataAdapter daViewAllData;
        DataTable dtViewAllData;

        string driverid;
        string vehicleid;
        private void showdatabtn_Click(object sender, EventArgs e)
        {
            if (didtxt.Enabled == false && vidtxt.Enabled == false)
            {
                MessageBox.Show("Please select a method to Update !","Error!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else if (didradiobtn.Checked)
            {
                // checked  hai to yani visible hai. 
                // agar visible hai and text box empty hai ya phir spaces dale we hai bus to..
                if (didtxt.Enabled == true && string.IsNullOrEmpty(didtxt.Text.Trim()))
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

                    // yeh wali cheez kar rahe hai similar kaam horaha bus data Adapter par.
                    // cmd wala sql command hai. jo insert kay lie use hoti hai
                    //cmd.Parameters.AddWithValue("@did", did);

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
                        vehiclenumlbl.Visible = false;
                        passengerstxt.Visible = false;
                        passengerlbl.Visible = false;
                        fromcitytxt.Visible = false;
                        fromcitylbl.Visible = false;
                        tocitytxt.Visible = false;
                        tocitylbl.Visible = false;
                        tolltaxtxt.Visible = false;
                        tolltaxlbl.Visible = false;

                        updatebtn.Visible = false;
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
                        vehiclenumlbl.Visible = true;
                        passengerstxt.Visible = true;
                        passengerlbl.Visible = true;
                        fromcitytxt.Visible = true;
                        fromcitylbl.Visible = true;
                        tocitytxt.Visible = true;
                        tocitylbl.Visible = true;
                        tolltaxtxt.Visible = true;
                        tolltaxlbl.Visible = true;

                        updatebtn.Visible = true;

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
                        vehiclenumlbl.Visible = false;
                        passengerstxt.Visible = false;
                        passengerlbl.Visible = false;
                        fromcitytxt.Visible = false;
                        fromcitylbl.Visible = false;
                        tocitytxt.Visible = false;
                        tocitylbl.Visible = false;
                        tolltaxtxt.Visible = false;
                        tolltaxlbl.Visible = false;
                        updatebtn.Visible = false;
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
                        vehiclenumlbl.Visible = true;
                        passengerstxt.Visible = true;
                        passengerlbl.Visible = true;
                        fromcitytxt.Visible = true;
                        fromcitylbl.Visible = true;
                        tocitytxt.Visible = true;
                        tocitylbl.Visible = true;
                        tolltaxtxt.Visible = true;
                        tolltaxlbl.Visible = true;
                        updatebtn.Visible = true;

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

        SqlCommand cmd;
        private void updatebtn_Click(object sender, EventArgs e)
        {
            // agar jo dabbe update karne kay lie hai agar woh khali hai to...
            if (string.IsNullOrEmpty(nametxt.Text.Trim()) || string.IsNullOrEmpty(phonenotxt.Text.Trim()) || string.IsNullOrEmpty(cnictxt.Text.Trim()) || string.IsNullOrEmpty(vehiclenumtxt.Text.Trim()) || string.IsNullOrEmpty(passengerstxt.Text.Trim()))
            {
                MessageBox.Show("Please Fill All Boxes!.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // agar khali nahi hai to...
            else
            {
                try
                {
                    conTaxAppDB = new SqlConnection(@"Data Source=AlexanderAnjelo\sqlexpress;Initial Catalog=TaxAppDB;Integrated Security=True;Pooling=False");
                    conTaxAppDB.Open();
                    cmd = new SqlCommand(@"UPDATE DriverData SET driver_name = @name,driver_phonenum = @phonenum,driver_cnic = @cnic where driver_id = @did", conTaxAppDB);
                    // driver_name ="danish" 
                    cmd.Parameters.AddWithValue("@name", nametxt.Text);
                    cmd.Parameters.AddWithValue("@phonenum", phonenotxt.Text);
                    cmd.Parameters.AddWithValue("@cnic", cnictxt.Text);
                    cmd.Parameters.AddWithValue("@did", driverid);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand(@"UPDATE VehicleData SET vnum=@vnum,vcat=@vcat,passengers=@passengers,toCity=@tcity,fromCity=@fcity where vid = @vid", conTaxAppDB);
                    cmd.Parameters.AddWithValue("@vnum", vehiclenumtxt.Text);
                    cmd.Parameters.AddWithValue("@vcat",vehiclecategorytxt.Text);
                    cmd.Parameters.AddWithValue("@passengers",passengerstxt.Text);
                    cmd.Parameters.AddWithValue("@tcity",tocitytxt.Text);
                    cmd.Parameters.AddWithValue("@fcity",fromcitytxt.Text);
                    cmd.Parameters.AddWithValue("@vid", vehicleid);
                    cmd.ExecuteNonQuery();
                    conTaxAppDB.Close();


                    nametxt.Text = string.Empty;
                    phonenotxt.Text = string.Empty;
                    cnictxt.Text = string.Empty;
                    vehiclecategorytxt.Text = string.Empty;
                    vehiclenumtxt.Text = string.Empty;
                    passengerstxt.Text = string.Empty;
                    fromcitytxt.Text = string.Empty;
                    tocitytxt.Text = string.Empty;
                    tolltaxtxt.Text = string.Empty;

                    updatebtn.Visible = false;

                    didtxt.Text = string.Empty;
                    vidtxt.Text = string.Empty;

                    MessageBox.Show("Data is Safely Updated !", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void Update_Load(object sender, EventArgs e)
        {

        }

        private void didradiobtn_CheckedChanged(object sender, EventArgs e)
        {
            didtxt.Text = string.Empty;
            vidtxt.Text = string.Empty;
            didtxt.Enabled = true;
            vidtxt.Enabled = false;

            nametxt.Text = string.Empty;
            phonenotxt.Text = string.Empty;
            cnictxt.Text = string.Empty;
            vehiclecategorytxt.Text = string.Empty;
            vehiclenumtxt.Text = string.Empty;
            passengerstxt.Text = string.Empty;
            fromcitytxt.Text = string.Empty;
            tocitytxt.Text = string.Empty;
            tolltaxtxt.Text = string.Empty;

            updatebtn.Visible = false;
        }

        private void vidradiobtn_CheckedChanged(object sender, EventArgs e)
        {
            didtxt.Text = string.Empty;
            vidtxt.Text = string.Empty;
            didtxt.Enabled = false;
            vidtxt.Enabled = true;

            nametxt.Text = string.Empty;
            phonenotxt.Text = string.Empty;
            cnictxt.Text = string.Empty;
            vehiclecategorytxt.Text = string.Empty;
            vehiclenumtxt.Text = string.Empty;
            passengerstxt.Text = string.Empty;
            fromcitytxt.Text = string.Empty;
            tocitytxt.Text = string.Empty;
            tolltaxtxt.Text = string.Empty;
            updatebtn.Visible = false;
        }
    }
}