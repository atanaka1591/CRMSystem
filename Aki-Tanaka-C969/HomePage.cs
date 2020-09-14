using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aki_Tanaka_C969
{
    public partial class HomePage : Form
    {
        public HomePage()
        {
            InitializeComponent();

            //Displays dynamic homepage based on username and user's date/time adjusted to their timezone
            label1.Text = $"Welcome, {Login.userName}!";

            DateTime dt = DateTime.Now;
            DayOfWeek dayOfWeek = dt.DayOfWeek;
            string dtFormatted = dt.ToString("MMMM dd, yyyy");
            label2.Text = $"{dayOfWeek}, {dtFormatted}.";

            TimeZone localZone = TimeZone.CurrentTimeZone;
            TimeSpan UTCOffset = localZone.GetUtcOffset(dt);
            DateTime dtOffset = dt - UTCOffset;
            label4.Text = RefreshPage(dt, dtOffset, UTCOffset);

            //Logs user name and time of login to text file
            using (StreamWriter sw = File.AppendText(@"c:\temp\Log.txt"))
            {
                sw.WriteLine($"{Login.userName} logged in at {DateTime.Now - Calendar.currentOffset} UTC");
            }
        }

        // Reference to Login Screen required to make it visible again when this form is closed
        public Form RefToLoginForm { get; set; }

        // Log out confirmation when close out button is pressed
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog = new DialogResult();
            dialog = MessageBox.Show("Are you sure you want to log out?", "Alert!", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                this.RefToLoginForm.Show();
            }
            else if (dialog == DialogResult.No)
                e.Cancel = true;
        }

        //Opens Customer Records form
        private void customerRecordsButton_Click(object sender, EventArgs e)
        {
            var CustomerRecordsForm = new CustomerRecords();
            //CustomerRecordsForm.RefToHomePage = this;
            CustomerRecordsForm.Show(this);
            //this.Hide();
        }

        //Opens calendar
        private void calendarButton_Click(object sender, EventArgs e)
        {
            var CalendarForm = new Calendar();
            CalendarForm.Show(this);
        }

        //Opens Appointments form
        private void appointmentsButton_Click(object sender, EventArgs e)
        {
            var AppointmentsForm = new Appointments();
            AppointmentsForm.Show(this);
        }

        //Opens Reports form
        private void reportsButton_Click(object sender, EventArgs e)
        {
            var ReportsForm = new Reports();
            ReportsForm.Show(this);
        }

        //Returns the user's next upcoming appointment information 
        public static string RefreshPage(DateTime dt, DateTime dtOffset, TimeSpan UTCOffset)
        {
            var context = new U05I3YDbContext();

            //query the database for the next appointment in the future from current date time
            var appointmentQuery =
                (from d in context.appointments
                 where d.userId == Login.userId && d.start >= dtOffset
                 orderby d.start
                 select d).FirstOrDefault();

            //displays message regarding next upcoming appointment
            if (appointmentQuery == null)
            {
                return "You have no upcoming appointments scheduled at this time.";
            }
            else if (dt < (appointmentQuery.start + UTCOffset))
            {
                //if the next upcoming appointment is within 15 minutes from current date time, display reminder message
                if (((appointmentQuery.start + Calendar.currentOffset) - dt).Duration().TotalMinutes <= 15)
                {
                    MessageBox.Show("Reminder! You have an appointment scheduled in the next 15 minutes.");
                }

                var convertedAppTime = appointmentQuery.start + UTCOffset;
                return $"Your next appointment is with \n{appointmentQuery.customer.customerName} on {convertedAppTime.ToString("MM/dd/yyyy")} \nat {convertedAppTime.ToString("hh:mm tt")}";
            }
            else
            {
                return "You have no upcoming appointments scheduled at this time.";
            }
        }

    }
}
