using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace petcare
{
    public partial class StaffHome : Form
    {
        public StaffHome()
        {
            InitializeComponent();
        }

        private void btnAddCustomerPet_Click(object sender, EventArgs e)
        {
            this.Hide();
            addCustomerPet addcus = new addCustomerPet();
            addcus.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            login log = new login();
            log.Show();
            this.Close();
        }

        private void btnPetTreatment_Click(object sender, EventArgs e)
        {
            petTreatment treat = new petTreatment();
            treat.Show();
            this.Close();
        }

        private void btnPurchase_Click(object sender, EventArgs e)
        {
            customerPurchase purchase = new customerPurchase();
            purchase.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            stockManagement stock = new stockManagement();
            stock.Show();
            this.Close();
        }

       
    }
}
