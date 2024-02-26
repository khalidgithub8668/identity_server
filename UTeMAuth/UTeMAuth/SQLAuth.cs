using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;

namespace UTeMAuth
{
    public class SQLAuth
    {
        public static string dbase_developer = @"Data Source='devstudent.utem.edu.my';Initial Catalog='DbDeveloper';User ID='userid';Password='mypassword'";
        public static string dbase_dbclm = @"Data Source='V-SQL14.utem.edu.my\SQL_INS04';Initial Catalog='dbclm';User ID='userid';Password='mypassword'";
        public static int rnd_seed = 327680;
        public static bool sqlvalidateUser2_caralama(string user, string password)
        {
            bool valid = false;
            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
                {
                    valid = context.ValidateCredentials(user, password);
                }
                if (valid == true) { }
                else
                {
                    if (sqlvalidateUserCLM(user, password) == true)
                    {
                        valid = true;
                    }
                }

            }
            catch (Exception e)
            {
                if (valid == true) { }
                else
                {
                    if (sqlvalidateUserCLM(user, password) == true)
                    {
                        valid = true;
                    }
                }

            }


            //  if (valid == false) { valid = sqlvalidateUser2_caralama(user, password); }

            return valid;

        }
        public static bool sqlvalidateUserCLM(string user, string password)
        {

            bool mybol = false;
            string mypass = "";
            SqlDataReader rdr = null;
            SqlConnection con = null;
            SqlCommand cmd = null;
            try
            {
                // Open connection to the database
                String ConnectionString = SQLAuth.dbase_dbclm;
                con = new SqlConnection(ConnectionString);
                con.Open();
                string CommandText = "SELECT CLM_loginID,CLM_loginPWD FROM dbo.CLM_Pengguna WHERE CLM_loginPWD = @CLM_loginPass and  CLM_loginID = @CLM_loginID and (clm_tahap = 'STAF' or clm_tahap = 'PELAJAR' or clm_tahap = 'PELAJAR SISWAZAH' or clm_tahap = 'AHLI PENYELIDIK' or   clm_tahap = 'PENGURUS PORTAL'  or clm_tahap = 'PENTADBIR PORTAL'  or clm_tahap = 'AHLI PORTAL') ";
                cmd = new SqlCommand(CommandText);
                cmd.Connection = con;
                cmd.CommandText = CommandText;
                cmd.Parameters.AddWithValue("@CLM_loginID", user);
                // if (user == "00578")
                //  {
                //    
                //   cmd.Parameters.AddWithValue("@CLM_loginPass", getPwD("Firebird##170674"));
                //  }
                //  else
                //  {
                cmd.Parameters.AddWithValue("@CLM_loginPass", getPwD(password));
                //  }
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    mypass = rdr["CLM_loginID"].ToString().Trim();

                }
                if (mypass == "")
                {

                }
                else
                {
                    // if (mypass == getPwD(password))
                    //{
                    mybol = true;
                    //}
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                if (rdr != null)
                    rdr.Close();

                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return mybol;



        }
        public static bool sqlvalidateUser2_exist(string user)
        {
            bool valid = true;
            // catch later
            return valid;
        }
        public static bool sqlvalidateUser2(string user, string passwordx)
        {
            bool valid = false;
            string passwordss = "";
            string passwordss24 = passwordx.Substring(3, passwordx.Length - 8) + "==";
            string passwordss44 = passwordx.Substring(3, passwordx.Length - 8) + "=";
            string passwordss64 = passwordx.Substring(3, passwordx.Length - 8);
            if (passwordss24.Length == 24)
            {
                passwordss = passwordss24;
            }
            else if (passwordss44.Length == 44)
            {
                passwordss = passwordss44;
            }
            else
            {
                passwordss = passwordss64;
            }
            string password = AESEncrytDecry.DecryptStringAES_notuser(passwordss, user);
            if ((password == "error"))
            {

            }
            else
            {
                if (user.ToString().Contains("pd"))
                {
                    try
                    {
                        if (sqlvalidateUserCLM(user, password) == true)
                        {
                            valid = true;
                        }

                        if (valid == true) { }
                        else
                        {
                            using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
                            {
                                valid = context.ValidateCredentials(user, password);
                            }
                        }

                    }
                    catch (Exception e)
                    {

                    }
                }
                else if (
                    (user.ToString().Contains("d")) ||
                    (user.ToString().Contains("b")) ||
                    (user.ToString().Contains("bs")) ||
                    (user.ToString().Contains("m")) ||
                    (user.ToString().Contains("p"))
                    )
                {
                    // student only
                    try
                    {
                        if (sqlvalidateUserCLM(user, password) == true)
                        {
                            valid = true;
                            // string mystr = SQLPerakamgeo.reset24Hrs(user);
                        }
                        //  valid = true;
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    try
                    {
                        if (sqlvalidateUserCLM(user, password) == true)
                        {
                            valid = true;
                        }

                        if (valid == true) { }
                        else
                        {
                            using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
                            {
                                valid = context.ValidateCredentials(user, password);
                            }
                        }

                    }
                    catch (Exception e)
                    {

                    }
                }
            }


            if (valid == false) { valid = sqlvalidateUser2_caralama(user, passwordx); }
            return valid;
        }

        static void Randomize(double Number)
        {
            int rndSeed2 = rnd_seed;
            int num = (!BitConverter.IsLittleEndian) ? BitConverter.ToInt32(BitConverter.GetBytes(Number), 0) : BitConverter.ToInt32(BitConverter.GetBytes(Number), 4);
            num = ((num & 0xFFFF) ^ num >> 16) << 8;
            rndSeed2 = (rnd_seed = ((rndSeed2 & -16776961) | num));
        }
        public static string getPwD(string e)
        {
            //x is the variable to store pass after encryption
            string x = "";
            int a = 1;
            var b = "";
            var b2 = 0;
            var c = 0;
            var d = 0;

            Randomize(12345);

            for (a = 1; a <= e.Length; a++)
            {

                b = (e[a - 1]).ToString();//e.charCodeAt(a-1);


                b2 = Asc(Convert.ToChar(b));
                //Console.WriteLine("first " + b2 + "\n");
                decimal test = a / (decimal)2;
                int test2 = a / 2;
                //Console.WriteLine("CheckCalculation " + a + " - " + test + test2 );

                if (test == test2)
                    d = 5;
                else
                    d = -7;

                //Console.WriteLine("second " + d + "\n");


                //Console.WriteLine("rand " + Rnd(0) * 255);

                c = b2 ^ ((int)(Rnd(0) * 255) + a + d);
                //Console.WriteLine("third " + c + "\n");

                x = x + (char)c;
                //Console.WriteLine("fourth " + x + "\n\n");
            }

            return x;
        }

        public static int Asc(char String)
        {
            int num = Convert.ToInt32(String);
            if (num < 128)
            {
                return num;
            }
            try
            {
                Encoding fileIOEncoding = Encoding.Default;
                char[] chars = new char[1]
                {
      String
                };
                byte[] array;
                int bytes;
                if (fileIOEncoding.IsSingleByte)
                {
                    array = new byte[1];
                    bytes = fileIOEncoding.GetBytes(chars, 0, 1, array, 0);
                    return array[0];
                }
                array = new byte[2];
                bytes = fileIOEncoding.GetBytes(chars, 0, 1, array, 0);
                if (bytes == 1)
                {
                    return array[0];
                }
                if (BitConverter.IsLittleEndian)
                {
                    byte b = array[0];
                    array[0] = array[1];
                    array[1] = b;
                }
                return BitConverter.ToInt16(array, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static float Rnd(float Number)
        {

            int num = rnd_seed;
            checked
            {
                if ((double)Number != 0.0)
                {
                    if ((double)Number < 0.0)
                    {
                        num = BitConverter.ToInt32(BitConverter.GetBytes(Number), 0);
                        long num2 = num;
                        num2 &= uint.MaxValue;
                        num = (int)(num2 + (num2 >> 24) & 0xFFFFFF);
                    }
                    num = (int)(unchecked((long)num) * 1140671485L + 12820163 & 0xFFFFFF);
                }
                rnd_seed = num;
            }
            return (float)num / 16777216f;
        }

        public static IEnumerable<string> SimpanData(string user, string cemas, string lokasi, string telefon)
        {
            string[] ret = new string[1];
            ret[0] = "no";
            try
            {

                using (SqlConnection sqlConn = new SqlConnection(dbase_developer))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {

                        cmd.CommandText = @"INSERT INTO    kursus04_data ( pengadu, kecemasan, lokasi, telefon) values ( @USERID,@cemas, @lokasi, @telefon)";
                        cmd.Connection = sqlConn;
                        cmd.Parameters.AddWithValue("@USERID", user);
                        cmd.Parameters.AddWithValue("@cemas", cemas);
                        cmd.Parameters.AddWithValue("@lokasi", lokasi);
                        cmd.Parameters.AddWithValue("@telefon", telefon);

                        try
                        {
                            sqlConn.Open();
                            cmd.ExecuteNonQuery();
                            ret[0] = "ok";
                        }
                        catch (SqlException e)
                        {
                            ret[0] = e.Message.ToString();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ret[0] = ex.Message.ToString();
            }

            return ret;
        }
    }
}
