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
    public partial class LoginForm : Form
    {
        // globally declaring a thread 
        Thread th;

        public LoginForm()
        {
            InitializeComponent();
        }

        // GUI WORK : creating drop shadow on login form 
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
        //

        // GUI WORK : close image code on login form 
        private void closeImg_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        // GUI WORK : changing close image picture with mouse enter an mouse leave events
        private void closeImg_MouseEnter(object sender, EventArgs e)
        {
            closeImg.Image = Image.FromFile(@"icons\\close.png");
        }

        private void closeImg_MouseLeave(object sender, EventArgs e)
        {
            closeImg.Image = Image.FromFile(@"icons\\close_black.png");
        }

        // GUI WORK : closing login form and opening signup form in new thread
        private void signupBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            th = new Thread(opensignupform);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            th.Join();
        }

        // GUI WORK : methods created for each special case to be used in threads
        private void opensignupform(object obj)
        {
            Application.Run(new SignupForm());
        }

        private void openloginform(object obj)
        {
            Application.Run(new LoginForm());
        }

        private void opendriverdata(object obj)
        {
            Application.Run(new AppMainPage());
        }
        //

        
        // checking username and password both
        SqlConnection con;
        SqlDataAdapter da;
        DataTable dt;
        // login button click
        private void loginBtn_Click(object sender, EventArgs e)
        {
            // SQL WORK : checking username and password both
            try
            {
                // making and opening a connection
                con = new SqlConnection(@"Data Source=AlexanderAnjelo\sqlexpress;Initial Catalog=TaxAppDB;Integrated Security=True;Pooling=False");
                con.Open();
                // fetching username and pass from database.
                da = new SqlDataAdapter("select *from UserLoginDetails where username = @uname  and password = @pass",con);
                da.SelectCommand.Parameters.AddWithValue("@uname",usertxt.Text.Trim());
                da.SelectCommand.Parameters.AddWithValue("@pass", passtxt.Text.Trim());
                // filling the fetched data in a table.
                dt = new DataTable();
                da.Fill(dt);
                con.Close();

                // username is primary key in table.
                // if the login details are in database than there must be a one record(one row) in table.
                // if not than there is no row in table.

                if (dt.Rows.Count == 1)
                {
                    //close login page
                    this.Close();
                    // open main page
                    th = new Thread(opendriverdata);
                    th.SetApartmentState(ApartmentState.STA);
                    th.Start();
                    th.Join();
                }
                else
                {
                    MessageBox.Show("Username or Password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // close login page
                    this.Close();
                    // open login form again
                    th = new Thread(openloginform);
                    th.SetApartmentState(ApartmentState.STA);
                    th.Start();
                }                
            }
            // a catch Exception for handling any return error from database.
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

   
    }
}
    


