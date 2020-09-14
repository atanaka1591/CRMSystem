using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aki_Tanaka_C969
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //test to query data in db
            /*
            var context = new U05I3YDbContext();
            var query =
                from c in context.users
                where c.userName == "test"
                select c;

            foreach (var user in query)
                Console.WriteLine(user.userName);
            Console.WriteLine("hello test");
            */


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}
