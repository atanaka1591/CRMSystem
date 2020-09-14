using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aki_Tanaka_C969
{
    class Login
    {
        public static int userId;
        public static string userName;

        //Checks if the username and password is valid and if so, retrieves and sets the user id and user name
        public static bool IsValidLogin(string username, string password)
        {
            var context = new U05I3YDbContext();
            var query =
                from c in context.users
                where c.userName == username && c.password == password
                select c;

            if (query.Any())
            {
                foreach (var user in query)
                {
                    userId = user.userId;
                    userName = user.userName;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
