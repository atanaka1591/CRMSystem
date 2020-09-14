using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aki_Tanaka_C969
{
    public partial class AppointmentsCreate : Form
    {
        public AppointmentsCreate(List<customer> customerList)
        {
            InitializeComponent();
            foreach (var c in customerList)
            {
                comboBox1.Items.Add(c.customerName);
            }
        }

        // Reference to Appointments required to make it visible again when this form is closed
        public Form RefToAppointments { get; set; }

        //adds new appointment into db
        private void button1_Click(object sender, EventArgs e)
        {
            //Form validation
            if (comboBox1.Text == string.Empty
                || dateTimePicker1.Text == string.Empty
                || dateTimePicker2.Text == string.Empty
                || textBox2.Text == string.Empty
                || textBox3.Text == string.Empty)
            {
                MessageBox.Show("All required fields must be filled in.");
            }
            //Checks that appointment time is between business hours
            else if (dateTimePicker1.Value.TimeOfDay < Calendar.businessStart || dateTimePicker2.Value.TimeOfDay > Calendar.businessEnd)
            {
                MessageBox.Show("Appointment time cannot be outside of business hours.");
            }
            else
            {            
                // connects to db and inserts new appointment entry into appointment table
                Cursor.Current = Cursors.WaitCursor;
                var context = new U05I3YDbContext();

                //query needed to find the customer id of the customer that is chosen
                var queryCustomer = context.customers.FirstOrDefault(c => c.customerName == comboBox1.Text);

                //query all existing appointment times in order to check if selected time overlaps
                var appointmentTimesQuery = 
                from c in context.appointments
                //where c.start >= todayDateUTC
                orderby c.start
                select new
                {
                    Start = c.start,
                    End = c.end,
                };

                bool timeSlotOpen = true;

                //checks if there is already an existing appointment during selected time
                foreach (var a in appointmentTimesQuery)
                {
                    if ((dateTimePicker1.Value - Calendar.currentOffset >= a.Start && dateTimePicker1.Value - Calendar.currentOffset < a.End) || (dateTimePicker2.Value - Calendar.currentOffset > a.Start && dateTimePicker2.Value - Calendar.currentOffset <= a.End))
                    {
                        if (timeSlotOpen == true)
                        {
                            MessageBox.Show("You already have an appointment during this time.");
                        }                     
                        timeSlotOpen = false;
                    }
                }

                if (timeSlotOpen == true)
                {
                    var appointment = new appointment()
                    {
                        customerId = queryCustomer.customerId,
                        userId = Login.userId,
                        title = "not needed",
                        description = "not needed",
                        location = textBox3.Text,
                        contact = "not needed",
                        type = textBox2.Text,
                        url = "not needed",
                        start = dateTimePicker1.Value - Calendar.currentOffset,
                        end = dateTimePicker2.Value - Calendar.currentOffset,
                        createDate = DateTime.Now - Calendar.currentOffset,
                        createdBy = Login.userName,
                        lastUpdate = DateTime.Now - Calendar.currentOffset,
                        lastUpdateBy = Login.userName
                    };
                    context.appointments.Add(appointment);
                    context.SaveChanges();
                    Cursor.Current = Cursors.Default;

                    MessageBox.Show("Appointment has been created.");

                    //creates new appointments form and closes existing one to show latest data
                    Form fr = new Appointments();
                    fr.Show();
                    this.Close();
                    this.RefToAppointments.Close();
                }             
            }          
        }

        // Closes window and shows appointments form 
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefToAppointments.Show();
        }
    }
}
