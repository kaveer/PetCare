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
    public partial class stockManagement : Form
    {
        int variableCategoryId;
        int variableProductDetailId;

        public stockManagement()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try { 
                    if (string.IsNullOrEmpty(ddlCategory.Text))
                    {
                        lblErrorSave.Text = "Enter or select category";
                    }
                    else if (string.IsNullOrEmpty(ddlType.Text))
                    {
                        lblErrorSave.Text = "Enter or select type";
                    }
                    else if (ddlWeight.SelectedIndex == -1)
                    {
                        lblErrorSave.Text = "Select weight";
                    }
                    else if (string.IsNullOrEmpty(txtQuantity.Text))
                    {
                        lblErrorSave.Text = "Enter quantity";
                    }
                    else
                    {
                        SqlConnection Conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
                        SqlCommand Comm1 = new SqlCommand("select * from productTable where category = @category and productStatus = 'active'", Conn);
                        Comm1.Parameters.AddWithValue("@category", ddlCategory.Text.ToLower());
                        Conn.Open();
                        SqlDataReader DR1 = Comm1.ExecuteReader();
                        if (DR1.Read())
                        {
                            variableCategoryId = (int)DR1.GetValue(0);
                        }
                        else if (DR1.Read() == false)
                        {
                            variableCategoryId = setProduct(txtBrand.Text, ddlCategory.Text);
                        }
                        Conn.Close();

                        checkSetProductType();
                        displayUsingGridView();
                    }
                }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
           
        }

        private int setProduct(string brand, string category)
        {
            int proId = 0;
            SqlConnection conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("insert into productTable(brand , category , productStatus) values('"+brand+"' , '"+category.ToLower()+"' , 'active')", conn);
            cmd.ExecuteNonQuery();
            conn.Close();

            SqlConnection Connn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
            SqlCommand Comm1n = new SqlCommand("select * from productTable where category = @category", Connn);
            Comm1n.Parameters.AddWithValue("@category", ddlCategory.Text.ToLower());
            Connn.Open();
            SqlDataReader DR1 = Comm1n.ExecuteReader();
            if (DR1.Read())
            {
                proId = (int)DR1.GetValue(0);
            }
            return proId;
        }

        private void checkSetProductType()
        {
            SqlConnection Conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
            SqlCommand Comm1 = new SqlCommand("select * from productDetailsTable as pd inner join productTable as p on p.productId = pd.productId where productType = @type and Productweight = @weight and productDetailStatus = 'active' and category = '" + ddlCategory.Text.ToLower() + "'", Conn);
            Comm1.Parameters.AddWithValue("@type", ddlType.Text.ToLower());
            Comm1.Parameters.AddWithValue("@weight",ddlWeight.SelectedItem.ToString());
            Conn.Open();
            SqlDataReader DR1 = Comm1.ExecuteReader();
            if (DR1.Read())
            {
                variableProductDetailId = (int)DR1.GetValue(0);
                updateQuantity();
                MessageBox.Show("Product already exist. Quantity wil be updated ");
            }
            else if (DR1.Read() == false)
            {
                SqlConnection conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into productDetailsTable(productId ,productType, Productweight , quantity, productDetailStatus) values (@proId,@type,@wei,@quan,'active')", conn);
                cmd.Parameters.AddWithValue("@proId" , variableCategoryId);
                cmd.Parameters.AddWithValue("@type" , ddlType.Text.ToLower());
                cmd.Parameters.AddWithValue("@wei" , ddlWeight.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@quan" ,int.Parse(txtQuantity.Text));
                cmd.ExecuteNonQuery();
                conn.Close();              
            }
            Conn.Close();
        }

        private void updateQuantity()
        {
            SqlConnection conn = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("update productDetailsTable set quantity = @quan where productDetailId = @proId", conn);
            cmd.Parameters.AddWithValue("@proId", variableProductDetailId);
            cmd.Parameters.AddWithValue("@quan", int.Parse(txtQuantity.Text));
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void stockManagement_Load(object sender, EventArgs e)
        {
            loadCategory();
            loadType();
            displayUsingGridView();
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
            ddlCategory.DropDownStyle = ComboBoxStyle.DropDown;
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
            ddlType.DropDownStyle = ComboBoxStyle.DropDown;
            ddlType.Enabled = true;
           
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }

        private void displayUsingGridView()
        {
            using (SqlConnection con = new SqlConnection(@"Data Source=kaveer-pc\SQL12HOMEMASTER;Initial Catalog=petcare;Integrated Security=True"))
            {
                con.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter("select brand , category , productType as [type] , Productweight as [weight] , quantity from productTable as p inner join productDetailsTable as pd on pd.productId=p.productId  where productStatus = 'active' and productDetailStatus = 'active'", con))
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
