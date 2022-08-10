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
    public partial class addData : Form
    {

        public addData()
        {
            InitializeComponent();

            toCitytxt.Enabled = false;
            fromCitytxt.Enabled = false;
            vcatxt.Enabled = false;
            taxtxt.Enabled = false;

            toCityradiobtn.Checked = false;
            fromCityradiobtn.Checked = false;
            vcattaxradiobtn.Checked = false;

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
            DialogResult dr = MessageBox.Show("For Changes, Restart App Now or Later ?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
            {
                // restart
                Application.Restart();
            }
            else
            {
                this.Close();
            }
        }

        // radio buttons
        private void fromCityradiobtn_CheckedChanged(object sender, EventArgs e)
        {
            fromCitytxt.Text = string.Empty;
            toCitytxt.Text = string.Empty;
            vcatxt.Text = string.Empty;
            taxtxt.Text = string.Empty;

            fromCitytxt.Enabled = true;
            toCitytxt.Enabled = false;
            vcatxt.Enabled = false;
            taxtxt.Enabled = false;

            addfcitybtn.Visible = true;
            addtcitybtn.Visible = false;
            addvcattaxbtn.Visible = false;

        }

        private void toCityradiobtn_CheckedChanged(object sender, EventArgs e)
        {
            fromCitytxt.Text = string.Empty;
            toCitytxt.Text = string.Empty;
            vcatxt.Text = string.Empty;
            taxtxt.Text = string.Empty;

            fromCitytxt.Enabled = false;
            toCitytxt.Enabled = true;
            vcatxt.Enabled = false;
            taxtxt.Enabled = false;

            addfcitybtn.Visible = false;
            addtcitybtn.Visible = true;
            addvcattaxbtn.Visible = false;

        }

        private void vcattaxradiobtn_CheckedChanged(object sender, EventArgs e)
        {
            fromCitytxt.Text = string.Empty;
            toCitytxt.Text = string.Empty;
            vcatxt.Text = string.Empty;
            taxtxt.Text = string.Empty;

            fromCitytxt.Enabled = false;
            toCitytxt.Enabled = false;
            vcatxt.Enabled = true;
            taxtxt.Enabled = true;

            addfcitybtn.Visible = false;
            addtcitybtn.Visible = false;
            addvcattaxbtn.Visible = true;

        }

        // insertion of data
        SqlConnection conTaxAppDB;
        SqlCommand cmd;

        private void addfcitybtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(fromCitytxt.Text.Trim()))
            {
                fromCitytxt.Text=string.Empty;
                MessageBox.Show("Please Insert a From City !", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    conTaxAppDB = new SqlConnection(@"Data Source=AlexanderAnjelo\sqlexpress;Initial Catalog=TaxAppDB;Integrated Security=True;Pooling=False");
                    conTaxAppDB.Open();
                    cmd = new SqlCommand(@"Insert into fromCity(fcity) values(@fcity)", conTaxAppDB);
                    cmd.Parameters.AddWithValue("@fcity", fromCitytxt.Text.Trim());
                    cmd.ExecuteNonQuery();
                    conTaxAppDB.Close();

                    fromCitytxt.Text = string.Empty;
                    MessageBox.Show("Data is Safely Saved.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void addtcitybtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(toCitytxt.Text.Trim()))
            {
                toCitytxt.Text = string.Empty;
                MessageBox.Show("Please Insert a To City !", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    conTaxAppDB = new SqlConnection(@"Data Source=AlexanderAnjelo\sqlexpress;Initial Catalog=TaxAppDB;Integrated Security=True;Pooling=False");
                    conTaxAppDB.Open();
                    cmd = new SqlCommand(@"Insert into toCity(tcity) values(@tcity)", conTaxAppDB);
                    cmd.Parameters.AddWithValue("@tcity", toCitytxt.Text.Trim());
                    cmd.ExecuteNonQuery();
                    conTaxAppDB.Close();

                    toCitytxt.Text = string.Empty;
                    MessageBox.Show("Data is Safely Saved.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void addvcattaxbtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(vcatxt.Text.Trim()) || string.IsNullOrEmpty(taxtxt.Text.Trim()))
            {
                vcatxt.Text = string.Empty;
                taxtxt.Text = string.Empty;
                MessageBox.Show("Please Insert Vehicle Category & Tax !", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {

                conTaxAppDB = new SqlConnection(@"Data Source=AlexanderAnjelo\sqlexpress;Initial Catalog=TaxAppDB;Integrated Security=True;Pooling=False");
                conTaxAppDB.Open();
                cmd = new SqlCommand(@"Insert into vehicleCategory(vcat,tax) values(@vcat,@tax)", conTaxAppDB);
                cmd.Parameters.AddWithValue("@vcat", vcatxt.Text.Trim());
                cmd.Parameters.AddWithValue("@tax", taxtxt.Text.Trim());
                cmd.ExecuteNonQuery();
                conTaxAppDB.Close();

                vcatxt.Text = string.Empty;
                taxtxt.Text = string.Empty;
                MessageBox.Show("Data is Safely Saved.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addData_Load(object sender, EventArgs e)
        {

        }

    }
}