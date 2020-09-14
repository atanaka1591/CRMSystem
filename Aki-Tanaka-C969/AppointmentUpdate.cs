using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Aki_Tanaka_C969.Appointments;

namespace Aki_Tanaka_C969
{
    public partial class AppointmentUpdate : Form
    {
        public int appointmentID;

        public AppointmentUpdate(List<customer> customerList, BindingList<Appointment> allAppointments, int rowIndex)
        {
            InitializeComponent();
            foreach (var c in customerList)
            {
                comboBox1.Items.Add(c.customerName);
            }

            comboBox1.Text = allAppointments[rowIndex].Customer;
            dateTimePicker1.Value = allAppointments[rowIndex].Start;
            dateTimePicker2.Value = allAppointments[rowIndex].End;
            textBox1.Text = allAppointments[rowIndex].Type;
            textBox2.Text = allAppointments[rowIndex].Location;
            appointmentID = allAppointments[rowIndex].AppointmentID;
        }

        // Reference to Appointments required to make it visible again when this form is closed
        public Form RefToAppointments { get; set; }

        //Updates appointment in database with new data
        private void button1_Click(object sender, EventArgs e)
        {
            //Form validation
            if (comboBox1.Text == string.Empty
                || dateTimePicker1.Text == string.Empty
                || dateTimePicker2.Text == string.Empty
                || textBox1.Text == string.Empty
                || textBox2.Text == string.Empty)
            {
                MessageBox.Show("All required fields must be filled in.");
            }
            else if (dateTimePicker1.Value.TimeOfDay < Calendar.businessStart || dateTimePicker2.Value.TimeOfDay > Calendar.businessEnd)
            {
                MessageBox.Show("Appointment time cannot be outside of business hours.");
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                var context = new U05I3YDbContext();

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
                    //query needed to find the customer id of the customer that was selected. this needs to be passed into appointments table for the update
                    var queryCustomer = context.customers.FirstOrDefault(c => c.customerName == comboBox1.Text);

                    //query needed to find the chosen appointment based on its appointmentId
                    var queryAppointment = context.appointments.FirstOrDefault(c => c.appointmentId == appointmentID);

                    queryAppointment.customerId = queryCustomer.customerId;
                    queryAppointment.start = dateTimePicker1.Value - Calendar.currentOffset;
                    queryAppointment.end = dateTimePicker2.Value - Calendar.currentOffset;
                    queryAppointment.type = textBox1.Text;
                    queryAppointment.location = textBox2.Text;

                    context.SaveChanges();
                    Cursor.Current = Cursors.Default;

                    MessageBox.Show("Appointment has been updated.");

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
