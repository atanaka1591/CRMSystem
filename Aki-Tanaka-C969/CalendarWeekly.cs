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
    public partial class CalendarWeekly : Form
    {
        public CalendarWeekly()
        {
            InitializeComponent();
        }

        // Reference to Calendar required to make it visible again when this form is closed
        public Form RefToCalendar { get; set; }

       

        private void CalendarWeekly_Load(object sender, EventArgs e)
        {
           
            var textboxesWeekly = new List<TextBox>
            {
                textBox8, textBox9, textBox10, textBox11, textBox12, textBox13, textBox14
            };

            var richTextboxesWeekly = new List<RichTextBox>
            {
                richTextBox1, richTextBox2, richTextBox3, richTextBox4, richTextBox5, richTextBox6, richTextBox7
            };



            //adds days into textboxes on weekly calendar and makes today's box yellow
            for (int i = 0; i < 7; i++)
            {
                textboxesWeekly[i].Text = Calendar.monday.AddDays(i).ToString("MM/dd");
                if (Calendar.monday.AddDays(i) == Calendar.todayDateOnly)
                {
                    textboxesWeekly[i].BackColor = Color.Yellow;
                }
            }

            //displays available appointments in each rich textbox 
            for (int i = 0; i < 7; i++)
            {
                foreach (var a in Calendar.appointmentsThisWeek)
                {
                    //adjusts time based on users timezone and daylight savings and checks if it lands on the calendar day
                    if (a.start + Calendar.currentOffset >= Calendar.monday.AddDays(i) && a.start + Calendar.currentOffset < Calendar.monday.AddDays(i + 1))
                    {
                        richTextboxesWeekly[i].Text += $"{(a.start + Calendar.currentOffset).ToString("hh:mm tt")} - {a.type} with {a.customer} @ {a.location}\n";
                    }                  
                }
            }
        }
    
        // Closes window and shows monthly calendar 
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefToCalendar.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.RefToCalendar.Show();
            this.Close();
        }
    }
}
