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
    public partial class CustomerRecords : Form
    {
        public CustomerRecords()
          
        {
            InitializeComponent();
        }

        public class Customer
        {
            public int CustomerID { get; set; }
            public string Name { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string PostalCode { get; set; }
            public string Country { get; set; }
            public string Phone { get; set; }
        }

        public BindingList<Customer> allCustomers = new BindingList<Customer>();

        // Reference to Home Page required to make it visible again when this form is closed
        //public Form RefToHomePage { get; set; }

        public void RefreshCustomerData()
        {
            dataGridView1.Update();
            dataGridView1.Refresh();
        }

        private void CustomerRecords_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            var context = new U05I3YDbContext();
            var customerQuery =
                from c in context.customers
                where c.active == true
                select new
                {
                    CustomerID = c.customerId,
                    CustomerName = c.customerName,
                    CustomerAddress1 = c.address.address1,
                    CustomerAddress2 = c.address.address2,
                    CustomerCity = c.address.city.city1,
                    CustomerPostalCode = c.address.postalCode,
                    CustomerCountry = c.address.city.country.country1,
                    CustomerPhone = c.address.phone
                };

            foreach (var c in customerQuery)
            {
                allCustomers.Add(new Customer()
                {
                    CustomerID = c.CustomerID,
                    Name = c.CustomerName,
                    Address1 = c.CustomerAddress1,
                    Address2 = c.CustomerAddress2,
                    City = c.CustomerCity,
                    PostalCode = c.CustomerPostalCode,
                    Country = c.CustomerCountry,
                    Phone = c.CustomerPhone
                });
            }

            dataGridView1.DataSource = allCustomers;
            dataGridView1.Columns["CustomerID"].Visible = false;
            Cursor.Current = Cursors.Default;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Application.OpenForms.OfType<AddCustomer>().Any())
            {               
                var AddCustomerForm = new AddCustomer();
                AddCustomerForm.RefToCustomerRecords = this;
                AddCustomerForm.Show(this);
                this.Hide();              
            }
        }

        //Opens update customer form and passes on existing data
        private void button2_Click(object sender, EventArgs e)
        {
            if (!Application.OpenForms.OfType<UpdateCustomer>().Any())
            {
                var UpdateCustomerForm = new UpdateCustomer();
                UpdateCustomerForm.RefToCustomerRecords = this;
                UpdateCustomerForm.Show(this);
                this.Hide();

                var rowIndex = dataGridView1.CurrentCell.RowIndex;

                UpdateCustomerForm.PopulateCustomerData(
                    allCustomers[rowIndex].CustomerID,
                    allCustomers[rowIndex].Name,
                    allCustomers[rowIndex].Phone,
                    allCustomers[rowIndex].Address1,
                    allCustomers[rowIndex].Address2,
                    allCustomers[rowIndex].City,                 
                    allCustomers[rowIndex].PostalCode,
                    allCustomers[rowIndex].Country);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialog = new DialogResult();
            dialog = MessageBox.Show("Are you sure you want to delete this customer?", "Alert!", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                var rowIndex = dataGridView1.CurrentCell.RowIndex;
                var context = new U05I3YDbContext();

                var customerID = allCustomers[rowIndex].CustomerID;
                //LAMBDA EXPRESSION used below to query the database for the customer record based on customer id. Allows for shorter and easier to read code.
                var query = context.customers.FirstOrDefault(c => c.customerId == customerID); 
                if (query != null)
                {
                    context.customers.Remove(query);
                    context.SaveChanges();
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Customer has been deleted.");

                Form fr = new CustomerRecords();
                fr.Show();
                this.Close();
            }       
        }
    }
}
