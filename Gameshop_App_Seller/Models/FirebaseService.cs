using Firebase.Database;
using Firebase.Database.Query;
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
                .Child("Account")
                .OnceAsync<Users>();

            var userWithKey = users.FirstOrDefault(u => u.Object.MAIL == email);

            return userWithKey?.Key;
        }


        public async Task<string> GetUserKeyByEmailinReview(string email)
        {
            var users = await ClientUsers
                .Child("Reviews")
                .OnceAsync<Users>();

            var userWithKey = users.FirstOrDefault(u => u.Object.MAIL == email);

            return userWithKey?.Key;
        }



        public async Task<string> GetUserKeyByEmailinDenied(string email)
        {
            var users = await ClientUsers
                .Child("Denied Applications")
                .OnceAsync<Users>();

            var userWithKey = users.FirstOrDefault(u => u.Object.MAIL == email);

            return userWithKey?.Key;
        }


        public void SetUserKey(string userKey)
        {
            UserKey = userKey;
        }


        // Inside FirebaseService class
        public async Task<bool> IsUserEmailInRequestAsync(string email)
        {
            try
            {
                var request = await ClientUsers
                    .Child($"Request")
                    .OnceAsync<Users>(); // Adjust this according to your database structure

                // Check if any item in Request has the specified email
                return request.Any(item => item.Object.MAIL == email);
            }
            catch (Exception ex)
            {
                // Log or display any exceptions for debugging
                Console.WriteLine($"Error checking email in Request: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> IsUserEmailInVerifiedAsync(string email)
        {
            try
            {
                var request = await ClientUsers
                    .Child($"Verified")
                    .OnceAsync<Users>(); // Adjust this according to your database structure

                // Check if any item in Request has the specified email
                return request.Any(item => item.Object.MAIL == email);
            }
            catch (Exception ex)
            {
                // Log or display any exceptions for debugging
                Console.WriteLine($"Error checking email in Request: {ex.Message}");
                return false;
            }
        }


        public async Task<string> GetUserKeyByDetails(string email)
        {
            var users = await ClientUsers
                .Child("Account")
                .OnceAsync<Users>();

            var userWithKey = users.FirstOrDefault(u => u.Object.MAIL == email);

            return userWithKey?.Key;
        }


    }
}
