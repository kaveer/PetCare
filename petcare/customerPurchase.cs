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
    public partial class customerPurchase : Form
    {
        int VariableCustomerId;
        int variableProductDetailId;
        int variableQuantity;

        public customerPurchase()
        {
            InitializeComponent();
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                checkUser();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void checkUser()
        {
            if (string.IsNullOrEmpty(txtRefNo.Text))
            {
                lblErrorMsg.Text = "Enter RefNo";
            }
            SqlConnection Conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
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
        }

        private void customerPurchase_Load(object sender, EventArgs e)
        {
            loadCategory();
            loadType();
        }

        private void loadCategory()
        {
            // display petname in combobox
            DataSet ds2;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True";
            conn.Open();
            SqlDataAdapter daSearch = new SqlDataAdapter("select * from productTable where productStatus = 'active'", conn);
            ds2 = new DataSet();
            daSearch.Fill(ds2, "daSearch");
            ddlCategory.ValueMember = "category";
            ddlCategory.DataSource = ds2.Tables["daSearch"];
            ddlCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlCategory.Enabled = true;

        }

        private void loadType()
        {
            // display petname in combobox
            DataSet ds2;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True";
            conn.Open();
            SqlDataAdapter daSearch = new SqlDataAdapter("select distinct(productType) from productDetailsTable where productDetailStatus = 'active'", conn);
            ds2 = new DataSet();
            daSearch.Fill(ds2, "daSearch");
            ddlType.ValueMember = "productType";
            ddlType.DataSource = ds2.Tables["daSearch"];
            ddlType.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlType.Enabled = true;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlWeight.SelectedIndex == -1)
                {
                    lblErrorSave.Text = "select weight";
                }
                else if (string.IsNullOrEmpty(txtQuantity.Text))
                {
                    lblErrorSave.Text = "Enter Quantity";
                }
                else if (string.IsNullOrEmpty(txtRefNo.Text))
                {
                    lblErrorSave.Text = "Enter refNo";
                }
                else
                {
                    checkUser();

                    SqlConnection Conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
                    SqlCommand Comm1 = new SqlCommand("select * from productTable as p inner join productDetailsTable as pd on pd.productId=p.productId  where category= @cat and productType = @type and Productweight = @wei and quantity >= 1", Conn);
                    Comm1.Parameters.AddWithValue("@cat", ddlCategory.SelectedValue.ToString());
                    Comm1.Parameters.AddWithValue("@type", ddlType.SelectedValue.ToString());
                    Comm1.Parameters.AddWithValue("@wei", ddlWeight.SelectedItem.ToString());
                    Conn.Open();
                    SqlDataReader DR1 = Comm1.ExecuteReader();
                    if (DR1.Read())
                    {
                      variableProductDetailId = (int)DR1.GetValue(4);
                      variableQuantity = (int)DR1.GetValue(8) - int.Parse(txtQuantity.Text);
                      purchaseProduct(VariableCustomerId, variableProductDetailId);
                      updateQuantity(variableQuantity, variableProductDetailId);
                      displayUsingGridView();
                        
                    }
                    else if (DR1.Read() == false)
                    {
                        MessageBox.Show("No products available");
                    }
                    Conn.Close();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }           
        }

        private void purchaseProduct(int customerId, int productId)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("insert into purchaseTable(customerId , productDetailId , purchaseDate , purchaseStatus ,quantityPurchase) values (@cusId ,@proId, @date,'active',@quan)", conn);
            cmd.Parameters.AddWithValue("@cusId", customerId);
            cmd.Parameters.AddWithValue("@proId", productId);
            cmd.Parameters.AddWithValue("@quan", int.Parse(txtQuantity.Text));
            cmd.Parameters.AddWithValue("@date", DateTime.Today.ToString("yyyy-MM-dd"));
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }

        private void updateQuantity(int quantity , int productId)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("update productDetailsTable set quantity = @quan where productDetailId = @proId", conn);
            cmd.Parameters.AddWithValue("@quan", quantity);
            cmd.Parameters.AddWithValue("@proId", productId);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void displayUsingGridView()
        {
            using (SqlConnection con = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True"))
            {
                con.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter("select brand , category , productType , Productweight , quantityPurchase , purchaseDate  from customerTable as c inner join purchaseTable as p on c.customerId = p.customerId inner join productDetailsTable as pd on p.productDetailId = pd.productDetailId inner join productTable as pt on pt.productId = pd.productId where purchaseDate = '" + DateTime.Today.ToString("yyyy-MM-dd") + "'", con))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            StaffHome home = new StaffHome();
            home.Show();
            this.Close();
        }
    }
}
