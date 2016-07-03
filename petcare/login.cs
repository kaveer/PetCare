using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace petcare
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPwd.Text))
            {
                MessageBox.Show("Please provide UserName and Password");
                return;
            }
            try
            {
                checkAdmin();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void checkAdmin()
        {
            //Create SqlConnection
            SqlConnection con = new SqlConnection(@"Data Source=KAVEER-PC\MSSQL;Initial Catalog=petcare;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("select * from userTable where userStatus = 'active' and userType = 'admin' and username = @username and userPassword = @password ", con);
            cmd.Parameters.AddWithValue("@username", txtUsername.Text);
            cmd.Parameters.AddWithValue("@password", MD5Hash(txtPwd.Text));
            con.Open();
            SqlDataAdapter adapt = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapt.Fill(ds);
            con.Close();
            int count = ds.Tables[0].Rows.Count;
            //If count is equal to 1, than show frmMain form
            if (count == 1)
            {
                MessageBox.Show("Login Successful!");
                AdminHome admin = new AdminHome();
                this.Hide();
                admin.Show();
            }
            else
            {
                checkStaff();
            }
        }

        public void checkStaff()
        {
            //Create SqlConnection
            SqlConnection con = new SqlConnection(@"Data Source=KAVEER-PC\MSSQL;Initial Catalog=petcare;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("select * from userTable where userStatus = 'active' and userType = 'staff' and username = @username and userPassword = @password ", con);
            cmd.Parameters.AddWithValue("@username", txtUsername.Text);
            cmd.Parameters.AddWithValue("@password", MD5Hash(txtPwd.Text));
            con.Open();
            SqlDataAdapter adapt = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapt.Fill(ds);
            con.Close();
            int count = ds.Tables[0].Rows.Count;
            //If count is equal to 1, than show frmMain form
            if (count == 1)
            {
                MessageBox.Show("Login Successful!");
                StaffHome staff = new StaffHome();
                this.Hide();
                staff.Show();
            }
            else
            {
                MessageBox.Show("Login Failed");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
     } 
 }

