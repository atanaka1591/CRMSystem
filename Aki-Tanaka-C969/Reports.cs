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
    public partial class Reports : Form
    {
        public Reports()
        {
            InitializeComponent();
        }
           
        public class Report1
        {
            public string Month { get; set; }
            public string AppointmentType { get; set; }
        }
        public class Report2
        {
            public string Consultant { get; set; }
            public string Customer { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
        }
        public class Report3
        {
            public string Customer { get; set; }
            public int AppointmentCount { get; set; }
        }

        public BindingList<Report1> report1 = new BindingList<Report1>();
        public BindingList<Report2> report2 = new BindingList<Report2>();
        public BindingList<Report3> report3 = new BindingList<Report3>();

        //Generate Report 1
        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            var context = new U05I3YDbContext();
            var queryReport1 = context.appointments
            .Where(c => c.userId == Login.userId)
            .Where(c => c.start.Year == DateTime.Now.Year)
            .OrderBy(c => c.start)
            .ToList();
            
            foreach (var c in queryReport1)
            {
                report1.Add(new Report1()
                {
                    Month = (c.start + Calendar.currentOffset).ToString("MMMM"),
                    AppointmentType = c.type
                });                                            
            }

            dataGridView1.DataSource = report1;
            Cursor.Current = Cursors.Default;
        }

        //Generate Report 2
        private void button2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            var context = new U05I3YDbContext();
            var queryReport2 = context.appointments
            .Where(c => c.start.Year == DateTime.Now.Year)
            .OrderBy(c => c.user.userName)
            .OrderBy(c => c.start)
            .ToList();

            foreach (var c in queryReport2)
            {
                report2.Add(new Report2()
                {
                    Consultant = c.user.userName,
                    Customer = c.customer.customerName,
                    Start = (c.start + Calendar.currentOffset),
                    End = (c.end + Calendar.currentOffset)
                });
            }

            dataGridView1.DataSource = report2;
            Cursor.Current = Cursors.Default;
        }

        //Generate Report 3
        private void button3_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            var context = new U05I3YDbContext();

            //query to get list of all appointments for this year
            var queryReport3 = context.appointments
            .Where(c => c.start.Year == DateTime.Now.Year)
            .OrderBy(c => c.customer.customerName)
            .ToList();

            //query to get list of all existing distinct customers
            var queryCustomers = context.customers
                .OrderBy(c => c.customerName)
                .Distinct().ToList();

            //logic to get count of appointments per customer
            for (var i = 0; i < queryCustomers.Count; i++)
            {
                int appointmentCount = 0;
                foreach (var c in queryReport3)
                {                   
                    if (c.customer.customerName == queryCustomers[i].customerName)
                    {
                        appointmentCount += 1;
                    }                   
                }
                report3.Add(new Report3()
                {
                    Customer = queryCustomers[i].customerName,
                    AppointmentCount = appointmentCount
                });
            }

            dataGridView1.DataSource = report3; 
            Cursor.Current = Cursors.Default;
        }
    }
}
