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
    public partial class petTreatment : Form
    {
        int VariableCustomerId;
        int variablePetId;

        public petTreatment()
        {
            InitializeComponent();
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
              try
                {
                    if (string.IsNullOrEmpty(txtRefNo.Text))
                    {
                        lblErrorMsg.Text = "Enter RefNo";
                    }
                    SqlConnection Conn = new SqlConnection(@"Data Source=KAVEER-PC\MSSQL;Initial Catalog=petcare;Integrated Security=True");
                    SqlCommand Comm1 = new SqlCommand("select * from customerTable where refNo = @refno and customerStatus = 'active' ", Conn);
                    Comm1.Parameters.AddWithValue("@refno", txtRefNo.Text);
                    Conn.Open();
                    SqlDataReader DR1 = Comm1.ExecuteReader();
                    if (DR1.Read())
                    {
                        VariableCustomerId = (int)DR1.GetValue(0);
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


                  // display petname in combobox
                    DataSet ds2;
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = @"Data Source=KAVEER-PC\MSSQL;Initial Catalog=petcare;Integrated Security=True";
                    conn.Open();
                    SqlDataAdapter daSearch = new SqlDataAdapter("select petName from customerTable as c inner join petTable as p on p.customerId = c.customerId inner join breedTable as b on b.breedId = p.breedId inner join petTypeTable as pt on pt.petTypeId = b.petTypeId where c.customerId = '" + VariableCustomerId + "' and customerStatus ='active'", conn);
                    ds2 = new DataSet();
                    daSearch.Fill(ds2, "daSearch");
                    ddlPetName.ValueMember = "petName";
                    ddlPetName.DataSource = ds2.Tables["daSearch"];
                    ddlPetName.DropDownStyle = ComboBoxStyle.DropDown;
                    ddlPetName.Enabled = true;

                  //get pet deials according to petname combobox
                    getPetDetails(VariableCustomerId, ddlPetName.Text);
                } 
              catch (Exception ee)
              {
                  MessageBox.Show(ee.Message);
              }
        }

        private void getPetDetails(int cusId, string petName)
        {
            try
            {
                SqlConnection Conn = new SqlConnection(@"Data Source=KAVEER-PC\MSSQL;Initial Catalog=petcare;Integrated Security=True");
                SqlCommand Comm1 = new SqlCommand("select petId,gender,color,petDetails,breedName,petType from customerTable as c inner join petTable as p on p.customerId = c.customerId inner join breedTable as b on b.breedId = p.breedId inner join petTypeTable as pt on pt.petTypeId = b.petTypeId where c.customerId = "+VariableCustomerId+" and customerStatus ='active' and petName = '"+petName+"'", Conn);
                Conn.Open();
                SqlDataReader DR1 = Comm1.ExecuteReader();
                if (DR1.Read())
                {
                    variablePetId = (int)DR1.GetValue(0);
                    txtGender.Text = DR1.GetValue(1).ToString();
                    txtColor.Text = DR1.GetValue(2).ToString();
                    txtPetDetail.Text = DR1.GetValue(3).ToString();
                    txtBreed.Text = DR1.GetValue(4).ToString();
                    txtPetType.Text = DR1.GetValue(5).ToString();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void ddlPetName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                getPetDetails(VariableCustomerId, ddlPetName.Text);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtRefNo.Text))
                {
                    lblErrorSave.Text = "Enter refNo";
                }
                else if (string.IsNullOrEmpty(ddlPetName.Text))
                {
                    lblErrorSave.Text = "Select pet";
                }
                else if (string.IsNullOrEmpty(txtcost.Text))
                {
                    lblErrorSave.Text = "Enter cost";
                }
                else
                {
                    SqlConnection conn = new SqlConnection(@"Data Source=KAVEER-PC\MSSQL;Initial Catalog=petcare;Integrated Security=True");
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("insert into treatmentTable(petId,sickness,treatment,prescription,cost,sterilization,vaccination,euthanasis, treatmentDate) values(@petid ,@sick ,@treat,@prescrip,@cost,@steri,@vacci,@enthu,@date)", conn);
                    cmd.Parameters.AddWithValue("@petid", variablePetId);
                    cmd.Parameters.AddWithValue("@sick", txtSick.Text);
                    cmd.Parameters.AddWithValue("@treat", txtTreat.Text);
                    cmd.Parameters.AddWithValue("@prescrip", txtPrescrip.Text);
                    cmd.Parameters.AddWithValue("@cost", double.Parse(txtcost.Text));
                    cmd.Parameters.AddWithValue("@steri", ckSterilization.CheckState.ToString());
                    cmd.Parameters.AddWithValue("@vacci", ckVaccination.CheckState.ToString());
                    cmd.Parameters.AddWithValue("@enthu", ckEuthanasis.CheckState.ToString());
                    cmd.Parameters.AddWithValue("@date", DateTime.Today.ToString("yyyy-MM-dd"));
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("New record added");
                }
                
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void txtcost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            StaffHome home = new StaffHome();
            home.Show();
            this.Close();
        }

        private void petTreatment_Load(object sender, EventArgs e)
        {
            txtDate.Text =string.Format("{0:d/M/yyyy}" , DateTime.Today);
        }

       
    }
}
