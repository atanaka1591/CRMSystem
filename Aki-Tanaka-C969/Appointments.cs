using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aki_Tanaka_C969
{
    public partial class Appointments : Form
    {
        public Appointments()
        {
            InitializeComponent();
        }

        // Reference to Homepage required to close it when refreshing
        public Form RefToHomePage { get; set; }

        DateTime todayDateUTC = Calendar.todayDate - Calendar.currentOffset;

        List<customer> allCustomers = new List<customer>();

        public BindingList<Appointment> allAppointments = new BindingList<Appointment>();

        public class Appointment
        {
            public int AppointmentID { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public string Customer { get; set; }
            public string Type { get; set; }
            public string Location { get; set; }
        }


        private void Appointments_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            var context = new U05I3YDbContext();

            //query for list of all current customers - used to pass into the Create Appointment form
            var customerQuery =
                from c in context.customers
                where c.active == true
                orderby c.customerName
                select new
                {
                    CustomerName = c.customerName
                };
            foreach (var c in customerQuery)
            {
                allCustomers.Add(new customer()
                {
                    customerName = c.CustomerName
                });
            }

            //query for list of all future appointments
            var appointmentQuery =
                from c in context.appointments
                where c.start >= todayDateUTC
                orderby c.start
                select new
                {
                    Start = c.start,
                    End = c.end,
                    Customer = c.customer.customerName,
                    Type = c.type,
                    Location = c.location,
                    AppointmentID = c.appointmentId
                };

            foreach (var c in appointmentQuery)
            {
                allAppointments.Add(new Appointment()
                {
                    AppointmentID = c.AppointmentID,
                    Start = c.Start + Calendar.currentOffset,
                    End = c.End + Calendar.currentOffset,
                    Customer = c.Customer,
                    Type = c.Type,
                    Location = c.Location
                });
            }

            dataGridView1.DataSource = allAppointments;
            dataGridView1.Columns["AppointmentID"].Visible = false;
            Cursor.Current = Cursors.Default;
        }

        //opens Create Appointment form and passes in current list of available customers for the customer comboBox
        private void button1_Click(object sender, EventArgs e)
        {
            if (!Application.OpenForms.OfType<AppointmentsCreate>().Any())
            {
                var AppointmentsCreateForm = new AppointmentsCreate(allCustomers);
                AppointmentsCreateForm.RefToAppointments = this;
                AppointmentsCreateForm.Show(this);
                this.Hide();
            }
        }

        //opens Update Appointment form and passes in the data of the appointment that was selected
        private void button2_Click(object sender, EventArgs e)
        {
                if (!Application.OpenForms.OfType<AppointmentUpdate>().Any())
                {
                    if (dataGridView1.CurrentCell != null)
                    {
                        var rowIndex = dataGridView1.CurrentCell.RowIndex;

                        var AppointmentUpdateForm = new AppointmentUpdate(allCustomers, allAppointments, rowIndex);
                        AppointmentUpdateForm.RefToAppointments = this;
                        AppointmentUpdateForm.Show(this);
                        this.Hide();
                    }
                }               
        }
                

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialog = new DialogResult();
            dialog = MessageBox.Show("Are you sure you want to delete this appointment?", "Alert!", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                if (dataGridView1.CurrentCell != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    var rowIndex = dataGridView1.CurrentCell.RowIndex;
                    var context = new U05I3YDbContext();

                    var appointmentID = allAppointments[rowIndex].AppointmentID;
                    var query = context.appointments.FirstOrDefault(c => c.appointmentId == appointmentID);
                    if (query != null)
                    {
                        context.appointments.Remove(query);
                        context.SaveChanges();
                    }
                    Cursor.Current = Cursors.Default;

                    MessageBox.Show("Appointment has been deleted.");

                    Form fr = new Appointments();
                    fr.Show();
                    this.Close();
                }               
            }
        }

        // Closes window and shows refreshed homepage
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DateTime dt = DateTime.Now;
            TimeSpan UTCOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
            DateTime dtOffset = dt - UTCOffset;

            HomePage.RefreshPage(dt, dtOffset, UTCOffset);
        }
    }
}
