using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameshop_App_Seller.Models
{
    public class FirebaseService
    {
        private FirebaseClient ClientUsers { get; }
        public static string UserKey { get; set; }

        public FirebaseService()
        {
            // Initialize FirebaseClient with your configuration
            ClientUsers = new FirebaseClient("https://gameshopdb-f4df3-default-rtdb.asia-southeast1.firebasedatabase.app/");
        }

        public async Task<string> GetUserKeyByEmail(string email)
        {
            var users = await ClientUsers
                .Child("Users/Account")
                .OnceAsync<Users>();

            var userWithKey = users.FirstOrDefault(u => u.Object.MAIL == email);

            return userWithKey?.Key;
        }
        public void SetUserKey(string userKey)
        {
            UserKey = userKey;
        }
    }
}
