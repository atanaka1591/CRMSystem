using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aki_Tanaka_C969
{
    public partial class AddCustomer : Form
    {
        public AddCustomer()
        {
            InitializeComponent();
        }

        // Reference to Home Page required to make it visible again when this form is closed
        public Form RefToCustomerRecords { get; set; }

        private void buttonAddCust_Click(object sender, EventArgs e)
        {
            //Form validation
            if (textBoxCustName.Text == string.Empty
                || textBoxPhone.Text == string.Empty
                || textBoxAdd1.Text == string.Empty
                || comboBoxCity.Text == string.Empty
                || textBoxPostalCode.Text == string.Empty
                || comboBox1.Text == string.Empty )
            {
                MessageBox.Show("All required fields must be filled in.");
            }
            else
            {
                // connects to db and inserts new address entry into address table
                Cursor.Current = Cursors.WaitCursor;
                var context = new U05I3YDbContext();

                var queryCountry = context.countries.FirstOrDefault(c => c.country1 == comboBox1.Text);
                var queryCity = context.cities.FirstOrDefault(c => c.city1 == comboBoxCity.Text);
                var address = new address()
                {
                    address1 = textBoxAdd1.Text,
                    address2 = textBoxAdd2.Text,
                    cityId = queryCity.cityId,
                    postalCode = textBoxPostalCode.Text,
                    phone = textBoxPhone.Text,
                    createDate = DateTime.Now,
                    createdBy = Login.userName,
                    lastUpdate = DateTime.Now,
                    lastUpdateBy = Login.userName
                };
                context.addresses.Add(address);
                context.SaveChanges();

                //queries the same address1 and cityid to get back the addressId
                var queryAddressID = context.addresses.FirstOrDefault(c => c.address1 == textBoxAdd1.Text && c.cityId == queryCity.cityId);

                //inserts new customer entry into customer table
                var customer = new customer()
                {
                    customerName = textBoxCustName.Text,
                    addressId = queryAddressID.addressId,
                    active = true,
                    createDate = DateTime.Now,
                    createdBy = Login.userName,
                    lastUpdate = DateTime.Now,
                    lastUpdateBy = Login.userName
                };
                context.customers.Add(customer);
                context.SaveChanges();
                Cursor.Current = Cursors.Default;

                MessageBox.Show("Customer has been added.");

                //creates new customer records form and closes existing one to show latest data
                Form fr = new CustomerRecords();
                fr.Show();
                this.Close();
                this.RefToCustomerRecords.Close();

            }           
        }

        // Closes window and shows customer records 
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefToCustomerRecords.Show();
        }

    }
}
