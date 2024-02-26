using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTeMAuth
{
    public static class Auth_Sql
    {
        public static  bool ValidateUserUser2(string username, string password)
        {
            if (SQLAuth.sqlvalidateUser2(username, password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ValidateUserUser1(string myclient)
        {
            if (SQLAuth.sqlvalidateUser2_exist(myclient))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
