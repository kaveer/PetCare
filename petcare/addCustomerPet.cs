using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace petcare
{
    public partial class addCustomerPet : Form
    {
        int VariableCustomerId;
        int VariablePetTypeId;
        int VariableBreedId;
       
        
        public addCustomerPet()
        {
            InitializeComponent();
        }

        private void addCustomerPet_Load(object sender, EventArgs e)
        {
            txtRefNo.Text = RandomString(4);
            getPetType();
            
            SqlConnection Conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
            SqlCommand Comm1 = new SqlCommand("select * from petTypeTable where petType = @pet and petTypeStatus = 'active'", Conn);
            Comm1.Parameters.AddWithValue("@pet", ddlPet.Text.ToLower());
            Conn.Open();
            SqlDataReader DR1 = Comm1.ExecuteReader();
            if (DR1.Read())
            {
                VariablePetTypeId = (int)DR1.GetValue(0);
                
            }

            DataSet ds2;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True";
            conn.Open();
            SqlDataAdapter daSearch = new SqlDataAdapter("select * from breedTable where petTypeId = '" +  VariablePetTypeId  +"' and breedStatus = 'active'", conn);
            ds2 = new DataSet();
            daSearch.Fill(ds2, "daSearch");
            ddlBreed.ValueMember = "breedName";
            ddlBreed.DataSource = ds2.Tables["daSearch"];
            ddlBreed.DropDownStyle = ComboBoxStyle.DropDown;
            ddlBreed.Enabled = true;
        }

        private static Random random = new Random((int)DateTime.Now.Ticks);
        private string RandomString(int Size)
        {
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < Size; i++)
            {
                ch = input[random.Next(0, input.Length)];
                builder.Append(ch);
            }
            return builder.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRefNo.Text))
            {
                lblErrorMsg.Text = "Enter ref no";
            }
            else if (string.IsNullOrEmpty(txtName.Text))
            {
                lblErrorMsg.Text = "Enter Name";
            }
            else if (string.IsNullOrEmpty(txtSurname.Text))
            {
                lblErrorMsg.Text = "Enter Surname";
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(txtAddress.Text))
                    {
                        txtAddress.Text = "Unknown";
                    }
                    if (string.IsNullOrEmpty(txtTelNo.Text))
                    {
                        txtTelNo.Text = "Unknown";
                    }
                    SqlConnection Conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
                    SqlCommand Comm1 = new SqlCommand("select * from customerTable where refNo = @refno and customerStatus = 'active' ", Conn);
                    Comm1.Parameters.AddWithValue("@refno", txtRefNo.Text);
                    Conn.Open();
                    SqlDataReader DR1 = Comm1.ExecuteReader();
                    if (DR1.Read())
                    {
                        VariableCustomerId =(int)DR1.GetValue(0);
                        txtName.Text = DR1.GetValue(2).ToString();
                        txtSurname.Text = DR1.GetValue(3).ToString();
                        txtAddress.Text = DR1.GetValue(4).ToString();
                        txtTelNo.Text = DR1.GetValue(5).ToString();
                        lblErrorMsg.Text = "";                      
                        if (string.IsNullOrEmpty(ddlPet.Text))
                        {
                            lblErrorSave.Text = "Select pet";
                        }
                        else if (string.IsNullOrEmpty(ddlBreed.Text))
                        {
                            lblErrorSave.Text = "Select breed";
                        }
                        else if (string.IsNullOrEmpty(txtPetName.Text))
                        {
                            lblErrorSave.Text = "Enter pet name";
                        }
                        else if (string.IsNullOrEmpty(ddlGender.Text))
                        {
                            lblErrorSave.Text = "Select gender";
                        }
                        else if (string.IsNullOrEmpty(txtColor.Text))
                        {
                            txtColor.Text = "unknown";
                        }
                        else if (string.IsNullOrEmpty(txtDetails.Text))
                        {
                            txtDetails.Text = "unknown";
                        }
                        else
                        {                        
                            checkPetType(ddlPet.Text);                         
                            checkBreed(ddlBreed.Text);
                            setPetDetail();
                            getPetType();
                        }
                        MessageBox.Show("New record added");
                    }
                    else if (DR1.Read() == false)
                    {
                        SqlConnection conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("insert into customerTable(refNo, customerName, customerSurname, customerAddress , customerTellNo , customerStatus) values(@ref,@name,@surname,@address,@telno,'active')", conn);
                        cmd.Parameters.AddWithValue("@ref", txtRefNo.Text);
                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@surname", txtSurname.Text);
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@telno", txtTelNo.Text);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        VariableCustomerId = getCustomerId();
                        lblErrorMsg.Text = "";
                        if (string.IsNullOrEmpty(ddlPet.Text))
                        {
                            lblErrorSave.Text = "Select pet";
                        }
                        else if (string.IsNullOrEmpty(ddlBreed.Text))
                        {
                            lblErrorSave.Text = "Select breed";
                        }
                        else if (string.IsNullOrEmpty(txtPetName.Text))
                        {
                            lblErrorSave.Text = "Enter pet name";
                        }
                        else if (string.IsNullOrEmpty(ddlGender.Text))
                        {
                            lblErrorSave.Text = "Select gender";
                        }
                        else if (string.IsNullOrEmpty(txtColor.Text))
                        {
                            txtColor.Text = "unknown";
                        }
                        else if (string.IsNullOrEmpty(txtDetails.Text))
                        {
                            txtDetails.Text = "unknown";
                        }
                        else
                        {                           
                            checkPetType(ddlPet.Text);                           
                            checkBreed(ddlBreed.Text);
                            setPetDetail();
                            getPetType();
                        }
                        MessageBox.Show("New record added");
                       
                    }
                    Conn.Close();                  
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        }

        private int getCustomerId()
        {
            int customerId = 0;
            SqlConnection Conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
            SqlCommand Comm1 = new SqlCommand("select * from customerTable where refNo = @refno and customerStatus = 'active' ", Conn);
            Comm1.Parameters.AddWithValue("@refno", txtRefNo.Text);
            Conn.Open();
            SqlDataReader DR1 = Comm1.ExecuteReader();
            if (DR1.Read())
            {
                customerId = (int)DR1.GetValue(0);
            }
            Conn.Close();
            return customerId;
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            SqlConnection Conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
            SqlCommand Comm1 = new SqlCommand("select * from customerTable where refNo = @refno and customerStatus = 'active' ", Conn);
            Comm1.Parameters.AddWithValue("@refno", txtRefNo.Text);
            Conn.Open();
            SqlDataReader DR1 = Comm1.ExecuteReader();
            if (DR1.Read())
            {
                txtName.Text = DR1.GetValue(2).ToString();
                txtSurname.Text = DR1.GetValue(3).ToString();
                txtAddress.Text = DR1.GetValue(4).ToString();
                txtTelNo.Text = DR1.GetValue(5).ToString();
            }
            else if (DR1.Read() == false)
            {
                MessageBox.Show("No customers with this RefNo");
            }
            Conn.Close();
        }

        private void checkPetType(string pettype)
        {
            SqlConnection Conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
            SqlCommand Comm1 = new SqlCommand("select * from petTypeTable where petType = @pet and petTypeStatus = 'active'", Conn);
            Comm1.Parameters.AddWithValue("@pet", pettype.ToLower());
            Conn.Open();
            SqlDataReader DR1 = Comm1.ExecuteReader();
            if (DR1.Read())
            {
                VariablePetTypeId = (int)DR1.GetValue(0);
                
            }
            else if (DR1.Read() == false)
            {
                setPetType(ddlPet.Text);
            }
            Conn.Close();
        } 

        private void setPetType(string pet)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("insert into petTypeTable(petType, petTypeStatus) values(@petType,'active')", conn);
            cmd.Parameters.AddWithValue("@petType", pet.ToLower());
            cmd.ExecuteNonQuery();
            conn.Close();

            SqlConnection Connn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
            SqlCommand Comm1n = new SqlCommand("select * from petTypeTable where petType = @pet and petTypeStatus = 'active'", Connn);
            Comm1n.Parameters.AddWithValue("@pet", pet.ToLower());
            Connn.Open();
            SqlDataReader DR1n = Comm1n.ExecuteReader();
            if (DR1n.Read())
            {
                VariablePetTypeId = (int)DR1n.GetValue(0);

            }
        }

        private void checkBreed(string breedName)
        {
            SqlConnection Conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
            SqlCommand Comm1 = new SqlCommand("select * from breedTable where breedName = @breed and breedStatus = 'active'", Conn);
            Comm1.Parameters.AddWithValue("@breed", breedName.ToLower());
            Conn.Open();
            SqlDataReader DR1 = Comm1.ExecuteReader();
            if (DR1.Read())
            {
                VariableBreedId = (int)DR1.GetValue(0);

            }
            else if (DR1.Read() == false)
            {
                setBreed(ddlBreed.Text, VariablePetTypeId);
            }
            Conn.Close();
        }

        private void setBreed(string breed , int petType)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("insert into breedTable(breedName, petTypeId ,breedStatus) values(@breed,@typeId ,'active')", conn);
            cmd.Parameters.AddWithValue("@breed", breed.ToLower());
            cmd.Parameters.AddWithValue("@typeId", petType);
            cmd.ExecuteNonQuery();
            conn.Close();

            //get breed Id
            SqlConnection Connn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
            SqlCommand Comm1n = new SqlCommand("select * from breedTable where breedName = @breed and breedStatus = 'active'", Connn);
            Comm1n.Parameters.AddWithValue("@breed", breed.ToLower());
            Connn.Open();
            SqlDataReader DR1n = Comm1n.ExecuteReader();
            if (DR1n.Read())
            {
                VariableBreedId = (int)DR1n.GetValue(0);

            }
        }

        private void getPetType()
        {
            DataSet ds2;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True";
            conn.Open();
            SqlDataAdapter daSearch = new SqlDataAdapter("select * from petTypeTable where petTypeStatus='active'", conn);
            ds2 = new DataSet();
            daSearch.Fill(ds2, "daSearch");
            ddlPet.ValueMember = "petType";
            ddlPet.DataSource = ds2.Tables["daSearch"];
            ddlPet.DropDownStyle = ComboBoxStyle.DropDown;
            ddlPet.Enabled = true;
        }

        private void ddlPet_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection Conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
            SqlCommand Comm1 = new SqlCommand("select * from petTypeTable where petType = @pet and petTypeStatus = 'active'", Conn);
            Comm1.Parameters.AddWithValue("@pet", ddlPet.Text.ToLower());
            Conn.Open();
            SqlDataReader DR1 = Comm1.ExecuteReader();
            if (DR1.Read())
            {
                VariablePetTypeId = (int)DR1.GetValue(0);
               
            }

            DataSet ds2;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True";
            conn.Open();
            SqlDataAdapter daSearch = new SqlDataAdapter("select * from breedTable where petTypeId = '" + VariablePetTypeId + "' and breedStatus = 'active'", conn);
            ds2 = new DataSet();
            daSearch.Fill(ds2, "daSearch");
            ddlBreed.ValueMember = "breedName";
            ddlBreed.DataSource = ds2.Tables["daSearch"];
            ddlBreed.DropDownStyle = ComboBoxStyle.DropDown;
            ddlBreed.Enabled = true;

            
        }

        private void setPetDetail()
        {
            SqlConnection conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("insert into petTable(customerId,breedId,petName,gender,color,petDetails,petStatus) values(@cusid,@breedid,@name,@gender,@color,@details,'active')", conn);
            cmd.Parameters.AddWithValue("@cusid", VariableCustomerId);
            cmd.Parameters.AddWithValue("@breedid", VariableBreedId);
            cmd.Parameters.AddWithValue("@name", txtPetName.Text);
            cmd.Parameters.AddWithValue("@gender", ddlGender.Text);
            cmd.Parameters.AddWithValue("@color", txtColor.Text);
            cmd.Parameters.AddWithValue("@details", txtDetails.Text);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            StaffHome home = new StaffHome();
            home.Show();
            this.Close();
        }

       
    }
}
