using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using static Gameshop_M_App.App;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace Gameshop_M_App.Models
{
    public class Users
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Birthday { get; set; }
        public string Gender { get; set; }





        public async Task<bool> AddUsers(string fname, string lname, string mail, string pword, string Bday ,string gender )
        {
            try
            {
                var evaluateEmail = (await players.Child("Users")
                    .OnceAsync<Users>())
                    .FirstOrDefault(a => a.Object.Email == mail);

                if (evaluateEmail == null)
                {
                    var admin = new Users()
                    {
                        FirstName = fname,
                        LastName = lname,
                        Email = mail,
                        Password = pword,
                        Birthday = Bday,
                        Gender = gender
                        
                    };
                    await players
                        .Child("Users")
                        .PostAsync(admin);
                    players.Dispose();
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
        public async Task<bool> UsersLogin(string email, string Pass)
        {
            try
            {
                var evaluateEmail = (await players.Child("Users")
                                  .OnceAsync<Users>())
                                  .FirstOrDefault
                                  (a => a.Object.Email == email &&
                                   a.Object.Password == Pass);
                return evaluateEmail != null;
            }
            catch
            {
                return false;
            }

        }
    }
}
