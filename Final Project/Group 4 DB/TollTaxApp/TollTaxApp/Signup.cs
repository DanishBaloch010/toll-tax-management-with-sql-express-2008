using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
// thread class
using System.Threading;
using System.Data.Sql;
using System.Data.SqlClient;


namespace TollTaxApp
{
    public partial class SignupForm : Form
    {
        // globally declaring a thread.
        Thread th;
        public SignupForm()
        {
            InitializeComponent();
        }

        // GUI WORK : creating drop shadow
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

        // GUI WORK : close button image
        private void closeImg_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //

        // sql command for insertion
        SqlCommand cmd;
        SqlConnection con;
        // sign up button CLICK code
        private void signupBtn_Click(object sender, EventArgs e)
        {
            // handling any default entry or any null entry.
            // btw database will also dont let enter any null record.
            // because username is primary key.
            // and password is set to not null.
            if (usertxt.Text.Trim() == "Username" || passtxt.Text.Trim() == "Password" || string.IsNullOrEmpty(usertxt.Text.Trim()) || string.IsNullOrEmpty(passtxt.Text.Trim()))
            {
                DialogResult DR_defaultError = MessageBox.Show("Validation Erorr !\n\n1) *Fill all Fields.\n\n2) *Cannot have default Username and Password.", "ERROR!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (DR_defaultError == DialogResult.Retry)
                {
                    // close sign up page
                    this.Close();
                    //th.Abort();
                    // opening again signup page
                    th = new Thread(againOpenSignup);
                    th.SetApartmentState(ApartmentState.STA);
                    th.Start();
                    th.Join();
                }
                else if (DR_defaultError == DialogResult.Cancel)
                {
                    Application.Exit();
                }
            }

            else
            {
                try
                {
                    // creating and opening a sql connection to database.
                    con = new SqlConnection(@"Data Source=AlexanderAnjelo\sqlexpress;Initial Catalog=TaxAppDB;Integrated Security=True;Pooling=False");
                    con.Open();
                    // inserting sign up details
                    cmd = new SqlCommand("INSERT INTO UserLoginDetails(username,password) values(@userName,@password)", con);
                    cmd.Parameters.AddWithValue("@userName", usertxt.Text);
                    cmd.Parameters.AddWithValue("@password", passtxt.Text);
                    // to safely excecute sql command and return number of rows.
                    cmd.ExecuteNonQuery();
                    //closing connection
                    con.Close();

                    MessageBox.Show("New Account is Created !\nNow you Can Login.","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    this.Close();
                    th.Abort();
                    th = new Thread(redirectLoginForm);
                    // single threaded appartment.
                    th.SetApartmentState(ApartmentState.STA);
                    th.Start();

                }
                // catch for handling any return error from database.
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    
        // methods created for thread execution
        private void redirectLoginForm(object obj)
        {
            Application.Run(new LoginForm());
        }

        private void againOpenSignup(object obj)
        {
            Application.Run(new SignupForm());
        }

        // mouse hover methods (changing pictures on mouse hover)
        private void closeImg_MouseEnter(object sender, EventArgs e)
        {
            closeImg.Image = Image.FromFile(@"icons\\close.png");
        }

        private void closeImg_MouseLeave(object sender, EventArgs e)
        {
            closeImg.Image = Image.FromFile(@"icons\\close_black.png");
        }

        private void SignupForm_Load(object sender, EventArgs e)
        {

        }

    }
}
