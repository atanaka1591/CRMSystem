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
    public partial class Calendar : Form
    {
        public Calendar()
        {
            InitializeComponent();
        }

        public class Appointment
        {
            public string customer { get; set; }
            public DateTime start { get; set; }
            public DateTime end { get; set; }
            public string location { get; set; }
            public string type { get; set; }
        }

        public List<Appointment> appointmentsThisMonth = new List<Appointment>();
        public static List<Appointment> appointmentsThisWeek = new List<Appointment>();

        public int daysOffset;

        //Business hours
        public static TimeSpan businessStart = new TimeSpan(8, 0, 0);
        public static TimeSpan businessEnd = new TimeSpan(18, 0, 0);
        //public static string businessStart = "00:08:00";
        //public static string businessEnd = "00:18:00";

        //current date and conversions for monthly calendar
        public static DateTime todayDate = DateTime.Now;
        public string todayMonth = todayDate.ToString("MMMM");
        public static TimeZone localZone = TimeZone.CurrentTimeZone;
        public static TimeSpan currentOffset = localZone.GetUtcOffset(todayDate);

        public static string todayMonthNum = todayDate.ToString("MM");
        public int todayMonthInt = int.Parse(todayMonthNum);
        public static string todayYear = todayDate.ToString("yyyy");
        public int todayYearInt = int.Parse(todayYear);
        public int todayDayInt = todayDate.Day;

        //date and conersions for weekly calendar
        public static int todayDayOfWeek = (int)Calendar.todayDate.DayOfWeek;
        //public int daysOffSet = Calendar.getDaysOffSet(todayDayOfWeek);
        //int todayMonth = Calendar.todayDate.Month;
        public static DateTime todayDateOnly = Calendar.todayDate.Date;
        public static DateTime monday;
        public static DateTime nextMonday;

        //gets the offset of days depending on the day of week it is - used to calculate which cell on the calendar to begin on
        public static int getDaysOffSet(int dayOfWeek)
        {
            if (dayOfWeek == 1)
            {
                return 0;
            }
            else if (dayOfWeek == 2)
            {
                return 1;
            }
            else if (dayOfWeek == 3)
            {
                return 2;
            }
            else if (dayOfWeek == 4)
            {
                return 3;
            }
            else if (dayOfWeek == 5)
            {
                return 4;
            }
            else if (dayOfWeek == 6)
            {
                return 5;
            }
            else if (dayOfWeek == 0)
            {
                return 6;
            }
            else
            {
                return 0;
            }
        }

        private void Calendar_Load(object sender, EventArgs e)
        {
            //updates label with current month and year
            label1.Text = $"{todayMonth} {todayYear}";

            //determines the first day of current month and its day of the week
            DateTime firstOfTheMonth = new DateTime(todayYearInt, todayMonthInt, 1);
            int dayOfTheFirst = (int)firstOfTheMonth.DayOfWeek;

            //determines number of days in current month
            int lastDayofMonth;
            switch (todayMonthInt)
            {
                case 1:
                    lastDayofMonth = 31;
                    break;
                case 2:
                    lastDayofMonth = 28;
                    break;
                case 3:
                    lastDayofMonth = 31;
                    break;
                case 4:
                    lastDayofMonth = 30;
                    break;
                case 5:
                    lastDayofMonth = 31;
                    break;
                case 6:
                    lastDayofMonth = 30;
                    break;
                case 7:
                    lastDayofMonth = 31;
                    break;
                case 8:
                    lastDayofMonth = 31;
                    break;
                case 9:
                    lastDayofMonth = 30;
                    break;
                case 10:
                    lastDayofMonth = 31;
                    break;
                case 11:
                    lastDayofMonth = 30;
                    break;
                case 12:
                    lastDayofMonth = 31;
                    break;
                default:
                    lastDayofMonth = 30;
                    break;
            }

            //determines the first day of the next month from today
            DateTime firstOfNextMonth;
            if (todayMonthInt == 12)
            {
                firstOfNextMonth = new DateTime(todayYearInt + 1, 1, 1);
            }
            else
            {
                firstOfNextMonth = new DateTime(todayYearInt, todayMonthInt + 1, 1);
            }
            
            //creates list of textbox controls so we can iterate over them
            var textboxes = new List<TextBox>
            {
                textBox8, textBox9, textBox10, textBox11, textBox12, textBox13, textBox14,
                textBox15, textBox16, textBox17, textBox18, textBox19, textBox20, textBox21,
                textBox22, textBox23, textBox24, textBox25, textBox26, textBox27, textBox28,
                textBox29, textBox30, textBox31, textBox32, textBox33, textBox34, textBox35,
                textBox36, textBox37, textBox38, textBox39, textBox40, textBox41, textBox42
            };

            //method for adding days into textboxes on calendar and makes today's box yellow
            void addDayOnCalendar(int lastDay, int increment)
            {
                for (var i = 0; i < lastDay; i++)
                {
                    textboxes[i + increment].Text = (i + 1).ToString();
                    if (i + 1 == todayDayInt)
                    {
                        textboxes[i + increment].BackColor = Color.Yellow;
                    }
                }
            }
          
            //determines which textbox to start on depending on what day of the week the first lands on          
            var daysOffSet = getDaysOffSet(dayOfTheFirst);
            addDayOnCalendar(lastDayofMonth, daysOffSet);


            //weekly calendar 
            //gets the offset of days depending on the day of week it is - used to calculate which cell on the calendar to begin on
            int offset = Calendar.getDaysOffSet(todayDayOfWeek);

            //determines which day is Monday for this weekly calendar           
            for (int i = 0; i < 7; i++)
            {
                if (offset == i)
                {
                    monday = todayDateOnly.AddDays(-i);
                    nextMonday = todayDateOnly.AddDays(7 - i);
                }
            }

            /*if (dayOfTheFirst == 1)
            {
                daysOffset = 0;
                addDayOnCalendar(lastDayofMonth, daysOffset);
            }
            else if (dayOfTheFirst == 2)
            {
                daysOffset = 1;
                addDayOnCalendar(lastDayofMonth, daysOffset);
            }
            else if (dayOfTheFirst == 3)
            {
                daysOffset = 2;
                addDayOnCalendar(lastDayofMonth, daysOffset);
            }
            else if (dayOfTheFirst == 4)
            {
                daysOffset = 3;
                addDayOnCalendar(lastDayofMonth, daysOffset);
            }
            else if (dayOfTheFirst == 5)
            {
                daysOffset = 4;
                addDayOnCalendar(lastDayofMonth, daysOffset);
            }
            else if (dayOfTheFirst == 6)
            {
                daysOffset = 5;
                addDayOnCalendar(lastDayofMonth, daysOffset);
            }
            else if (dayOfTheFirst == 7)
            {
                daysOffset = 6;
                addDayOnCalendar(lastDayofMonth, daysOffset);
            }*/


            //connects to db and gets all appointments for the current month           
            Cursor.Current = Cursors.WaitCursor;
            var context2 = new U05I3YDbContext();
            var queryForMonth =
                from c in context2.appointments
                where c.start >= firstOfTheMonth && c.start < firstOfNextMonth
                orderby c.start
                select new
                {
                    custName = c.customer.customerName,
                    appStart = c.start,
                    appEnd = c.end,
                    appLoc = c.location,
                    appType = c.type
                };

            var queryForWeek =
                from c in context2.appointments
                where c.start >= monday && c.start < nextMonday
                orderby c.start
                select new
                {
                    custName = c.customer.customerName,
                    appStart = c.start,
                    appEnd = c.end,
                    appLoc = c.location,
                    appType = c.type
                };

            foreach (var appointment in queryForMonth)
            {
                appointmentsThisMonth.Add(new Appointment()
                {
                    customer = appointment.custName,
                    start = appointment.appStart,
                    end = appointment.appEnd,
                    location = appointment.appLoc,
                    type = appointment.appType
                });
            }
            foreach (var appointment in queryForWeek)
            {
                appointmentsThisWeek.Add(new Appointment()
                {
                    customer = appointment.custName,
                    start = appointment.appStart,
                    end = appointment.appEnd,
                    location = appointment.appLoc,
                    type = appointment.appType
                });
            }

            //creates list of rich textbox controls so we can iterate over them
            var richtextboxes = new List<RichTextBox>
            {
                richTextBox1, richTextBox2, richTextBox3, richTextBox4, richTextBox5, richTextBox6, richTextBox7,
                richTextBox8, richTextBox9, richTextBox10, richTextBox11, richTextBox12, richTextBox13, richTextBox14,
                richTextBox15, richTextBox16, richTextBox17, richTextBox18, richTextBox19, richTextBox20, richTextBox21,
                richTextBox22, richTextBox23, richTextBox24, richTextBox25, richTextBox26, richTextBox27, richTextBox28,
                richTextBox29, richTextBox30, richTextBox31, richTextBox32, richTextBox33, richTextBox34, richTextBox35
            };

            //displays available appointments on each rich textbox 
            for (int i = 0; i < lastDayofMonth; i++)
            {
                foreach (var a in appointmentsThisMonth)
                {
                    //adjusts time based on users timezone and daylight savings and checks if it lands on the calendar day
                    if (a.start + currentOffset >= firstOfTheMonth.AddDays(i) && a.start + currentOffset < firstOfTheMonth.AddDays(i + 1))
                    {
                        richtextboxes[i + daysOffSet].Text += $"{(a.start + currentOffset).ToString("hh:mm tt")} - {a.type} with {a.customer} @ {a.location}\n";
                    }

                    /*//checks if daylight savings, and if so adds 1 hour - not needed as UTCoffset automatically includes daylight savings adjustment
                    if (localZone.IsDaylightSavingTime(todayDate))
                    {
                        //adjusts time based on users timezone and daylight savings and checks if it lands on the calendar day
                        if (a.start.AddHours(1) + currentOffset >= firstOfTheMonth.AddDays(i) && a.start.AddHours(1) + currentOffset < firstOfTheMonth.AddDays(i + 1))
                        {
                            richtextboxes[i + daysOffSet].Text += $"{a.start.AddHours(1) + currentOffset} - {a.type} with {a.customer} @ {a.location}\n";
                        }
                    }
                    else
                    {
                        //adjusts time based on users timezone and checks if it lands on the calendar day
                        if (a.start + currentOffset >= firstOfTheMonth.AddDays(i) && a.start + currentOffset < firstOfTheMonth.AddDays(i + 1))
                        {
                            richtextboxes[i + daysOffSet].Text += $"{a.start + currentOffset} - {a.type} with {a.customer} @ {a.location}\n";
                        }
                    }*/
                }
            }
            Cursor.Current = Cursors.Default;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Application.OpenForms.OfType<CalendarWeekly>().Any())
            {
                var CalendarWeeklyForm = new CalendarWeekly();
                CalendarWeeklyForm.RefToCalendar = this;
                CalendarWeeklyForm.Show(this);
                this.Hide();
            }
        }
    }
}
