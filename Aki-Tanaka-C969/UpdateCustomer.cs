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
    public partial class UpdateCustomer : Form
    {
        public UpdateCustomer()
        {
            InitializeComponent();
        }

        // Reference to Customer Records required to make it visible again when this form is closed
        public Form RefToCustomerRecords { get; set; }

        //customer ID of the customer that is being updated
        int updatingCustID;

        //method to populate existing customer data into fields
        public void PopulateCustomerData(int customerID, string customerName, string phone, string add1, string add2, string city, string postalCode, string country)
        {
            updatingCustID = customerID;
            textBoxCustName.Text = customerName;
            textBoxPhone.Text = phone;
            textBoxAdd1.Text = add1;
            textBoxAdd2.Text = add2;
            comboBoxCity.Text = city;
            textBoxPostalCode.Text = postalCode;
            comboBox1.Text = country;            
        }

        //updates customer data into database
        private void buttonUpdateCust_Click(object sender, EventArgs e)
        {
            //Form validation
            if (textBoxCustName.Text == string.Empty
                || textBoxPhone.Text == string.Empty
                || textBoxAdd1.Text == string.Empty
                || comboBoxCity.Text == string.Empty
                || textBoxPostalCode.Text == string.Empty
                || comboBox1.Text == string.Empty)
            {
                MessageBox.Show("All required fields must be filled in.");
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                var context = new U05I3YDbContext();

                //need to query the city and country data first to know which cityid and countryid to update with
                //LAMBDA EXPRESSIONS used as it allows for shorter and easier to read code. You can see the difference compared to the linq query below.
                var queryCountry = context.countries.FirstOrDefault(c => c.country1 == comboBox1.Text);
                var queryCity = context.cities.FirstOrDefault(c => c.city1 == comboBoxCity.Text);

                //example below of linq query shows how much messier it looks compared to lambda expressions
                var customerQuery =
                    (from c in context.customers
                     where c.customerId == updatingCustID
                     select c).FirstOrDefault();

                customerQuery.customerName = textBoxCustName.Text;
                customerQuery.address.phone = textBoxPhone.Text;
                customerQuery.address.address1 = textBoxAdd1.Text;
                customerQuery.address.address2 = textBoxAdd2.Text;
                //customerQuery.address.city.city1 = queryCity.city1;
                customerQuery.address.cityId = queryCity.cityId;
                customerQuery.address.postalCode = textBoxPostalCode.Text;
                //customerQuery.address.city.country.country1 = queryCountry.country1;
                //customerQuery.address.city.countryId = queryCountry.countryId;

                context.SaveChanges();
                Cursor.Current = Cursors.Default;

                MessageBox.Show("Customer has been updated.");

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
