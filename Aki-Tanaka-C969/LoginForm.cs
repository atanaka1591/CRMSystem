using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aki_Tanaka_C969
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            label4.Visible = false;
            label5.Visible = false;

            //Gets the labels in the language of the user's region setting and assigns them to the form
            var loginLabels = new List<String>();
            loginLabels = LanguageSupport.GetLoginLabels();
            label3.Text = loginLabels[0];
            label1.Text = loginLabels[1];
            label2.Text = loginLabels[2];
            loginButton.Text = loginLabels[3];
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            //Gets the login message labels in the language of the user's region setting and assigns them to the form
            var loginMessageLabels = new List<String>();
            loginMessageLabels = LanguageSupport.GetLoginMessageLabels();
            label5.Text = loginMessageLabels[0];
            label4.Text = loginMessageLabels[1];

            label5.Visible = false;
            Cursor.Current = Cursors.WaitCursor;

            //Validates the username and password
            if (Login.IsValidLogin(textBox1.Text, textBox2.Text))
            {
                var HomePageForm = new HomePage();
                HomePageForm.RefToLoginForm = this;
                HomePageForm.Show(this);
                this.Hide();
            }
            else
            {
                label5.Visible = true;
            }
            Cursor.Current = Cursors.Default;
        }
    }
}
