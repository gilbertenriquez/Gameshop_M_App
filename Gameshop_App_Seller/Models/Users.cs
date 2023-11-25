using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Gameshop_App_Seller.App;
using Firebase.Database.Query;
using Firebase.Auth;
using Firebase.Database;
using System.Collections.ObjectModel;
using System.Threading.Tasks;


namespace Gameshop_App_Seller.Models
{
    public class Users
    {
        public string webAPIKey = "AIzaSyDkunRqHTm1yzzAy59rU_1m9GSxOZkzpoA";
        FirebaseAuthProvider authProvider;
        public string FNAME { get; set; }
        public string LNAME { get; set; }
        public string MAIL { get; set; }
        public string PASSWORD { get; set; }
        public string BIRTHDAY { get; set; }
        public string GENDER { get; set; }
        public string Haddress { get; set; }

        public Users()
        {
            authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIKey));
        }



        public async Task<bool> Register(string email, string fname, string lastname, string address, string birthday, string gender, string password)
        {

            var token = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password, fname);
            if (!string.IsNullOrEmpty(token.FirebaseToken))
            {
                return true;
            }
            return false;
        }
        public async Task<string> SignIn(string email, string password)
        {
            var token = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
            if (!string.IsNullOrEmpty(token.FirebaseToken))
            {
                return token.FirebaseToken;
            }
            return "";
        }

        public async Task<bool> ResetPassword(string email)
        {
            await authProvider.SendPasswordResetEmailAsync(email);
            return true;
        }

        public async Task<string> ChangePassword(string token, string password)
        {
            var auth = await authProvider.ChangeUserPassword(token, password);
            return auth.FirebaseToken;
        }


        public async Task<bool> addusers(string name, string lname, string email, string password, string address, string birthday, string gender)
        {
            try
            {
                var employeeAddress = (await ClientUsers.Child("Employee")
                    .OnceAsync<Users>())
                    .FirstOrDefault(a => a.Object.MAIL == email);

                if (employeeAddress == null)
                {
                    var user = new Users()
                    {
                        MAIL = email,
                        FNAME = name,
                        LNAME = lname,
                        PASSWORD = password,
                        Haddress = address,
                        BIRTHDAY = birthday,
                        GENDER = gender

                    };
                    await ClientUsers
                        .Child("Users")
                        .PostAsync(user);
                    ClientUsers.Dispose();
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        //public async Task<bool> userLogin(string email, string Pass)
        //{
        //    try
        //    {
        //        var evaluateEmail = (await users.Child("Users")
        //                          .OnceAsync<Users>())
        //                          .FirstOrDefault
        //                          (a => a.Object.MAIL == email &&
        //                           a.Object.PASSWORD == Pass);
        //        return evaluateEmail != null;
        //    }
        //    catch
        //    {
        //        return false;
        //    }

        //}

    }
}
