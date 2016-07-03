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
    public partial class RegisterUser : Form
    {
       
        public RegisterUser()
        {
            
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                lblErrorMsg.Text = "Enter username";
            }
            else if (checkUsername() == 1)
            {
                lblErrorMsg.Text = "username already exists";
            }
            else if (string.IsNullOrEmpty(txtPassword.Text))
            {
                lblErrorMsg.Text = "Enter a password";
            }
            else if (string.IsNullOrEmpty(txtName.Text))
            {
                lblErrorMsg.Text = "Enter name";
            }
            else if (string.IsNullOrEmpty(txtSurname.Text))
            {
                lblErrorMsg.Text = "Enter Surname";
            }
            else if (string.IsNullOrEmpty(ddlGender.Text))
            {
                lblErrorMsg.Text = "Select gender";
            }
            else if (string.IsNullOrEmpty(txtJobTitle.Text))
            {
                lblErrorMsg.Text = "Enter Job Title";
            }
            else
            {
                try
                {
                    SqlConnection conn = new SqlConnection(@"Data Source=KAVEER-PC\MSSQL;Initial Catalog=petcare;Integrated Security=True");
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO userTable(username,userPassword,name,surname,dob,gender,userStatus,jobTitle,userType) VALUES(@user,@pwd,@name,@surname,@dob,@gender,'active',@jobTitle,'staff')", conn);
                    cmd.Parameters.AddWithValue("@user", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@pwd", MD5Hash(txtPassword.Text));
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@surname", txtSurname.Text);
                    cmd.Parameters.AddWithValue("@dob", dpDob.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@gender", ddlGender.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@jobTitle", txtJobTitle.Text);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("New staff added");
                    this.Close();
                    AdminHome home = new AdminHome();
                    home.Show();
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }
            
        }

        public int checkUsername()
        {
            //Create SqlConnection
            SqlConnection con = new SqlConnection(@"Data Source=KAVEER-PC\MSSQL;Initial Catalog=petcare;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("select * from userTable where username = @username ", con);
            cmd.Parameters.AddWithValue("@username", txtUsername.Text);
            con.Open();
            SqlDataAdapter adapt = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapt.Fill(ds);
            con.Close();
            int count = ds.Tables[0].Rows.Count;
            //If count is equal to 1, than show frmMain form
            if (count == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
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
    }
}
