﻿using System;
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


        public string Name { get; set; }
        public string Description { get; set; }
        public string Imagae_1_link { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public string image4 { get; set; }
        public string image5 { get; set; }
        public string image6 { get; set; }
        public string image7 { get; set; }

        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public string ProductPrice { get; set; }
        public string ProductPath { get; set; }
        public string ProductQuantity { get; set; }

        public string Message { get; set; }






        public Users()
        {
            authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIKey));
        }



        //public async Task<bool> Register(string email, string fname, string lastname, string address, string birthday, string gender, string password)
        //{

        //    var token = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password, fname);
        //    if (!string.IsNullOrEmpty(token.FirebaseToken))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public async Task<bool> AdminLogin(string email, string Pass)
        {
            try
            {
                var user = (await ClientUsers
             .Child($"Users/Account")
             .OnceAsync<Users>())
             .FirstOrDefault
             (a => a.Object.MAIL == email && a.Object.PASSWORD == Pass);

                return user != null;
            }
            catch
            {
                return false;
            }

        }

        //public async Task<bool> Login(string email, string Pass)
        //{
        //    try
        //    {
        //        var evaluateEmail = (await ClientUsers.Child("Users/Account")
        //                          .OnceAsync<Users>())
        //                          .FirstOrDefault
        //                          (a => a.Object.MAIL == email &&
        //                           a.Object.PASSWORD == Pass);

        //        if (evaluateEmail != null)
        //        {                   
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Exception during login: {ex}");
        //        return false;
        //    }
        //}


        //public async Task<string> SignIn(string email, string password)
        //{
        //    var token = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
        //    if (!string.IsNullOrEmpty(token.FirebaseToken))
        //    {
        //        return token.FirebaseToken;
        //    }
        //    return "";
        //}

        public async Task<bool> ResetPassword(string email)
        {
            await authProvider.SendPasswordResetEmailAsync(email);
            return true;
        }

        //public async Task<string> ChangePassword(string token, string password)
        //{
        //    var auth = await authProvider.ChangeUserPassword(token, password);
        //    return auth.FirebaseToken;
        //}


        public async Task<bool> addusers(string name, string lname, string email, string password, string address, string birthday, string gender)
        {
            try
            {
                var User = (await ClientUsers.Child("Users/Account")
                    .OnceAsync<Users>())
                    .FirstOrDefault(a => a.Object.MAIL == email);

                if (User == null)
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
                        .Child("Users/Account")
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

        public async Task<string> UploadImage(Stream img, string proname, string filename)
        {
            try
            {
                var image = await App.firebaseStorage
                    .Child($"Images/{proname}/{filename}")
                    .PutAsync(img);
                return image;
            }
            catch (Exception ex)
            {
                return "false";
            }
        }
        public async Task<bool> addDesc(string imglink,
                                 string img1,
                                 string img2,
                                 string img3,
                                 string img4,
                                 string img5,
                                 string img6,
                                 string ProdName,
                                 string ProdDesc,
                                 string ProdPrice,
                                 string ProdQuan)
        {


            try
            {
                // Construct the path to the user's account node
                var userAccountPath = $"Users/Account/{App.key}";

                var evaluateEmail = (await ClientUsers
                    .Child($"{userAccountPath}/Product")
                    .OnceAsync<Users>())
                    .FirstOrDefault(a => a.Object.Imagae_1_link == imglink);

                if (evaluateEmail == null)
                {
                    // Product does not exist, add it
                    var admin = new Users()
                    {
                        image1 = img1,
                        image2 = img2,
                        image3 = img3,
                        image4 = img4,
                        image5 = img5,
                        image6 = img6,
                        Imagae_1_link = imglink,
                        ProductName = ProdName,
                        ProductDesc = ProdDesc,
                        ProductPrice = ProdPrice,
                        ProductQuantity = ProdQuan
                    };

                    // Use the user's account path as the child node
                    await ClientUsers
                        .Child($"{userAccountPath}/Product")
                        .PostAsync(admin);

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

        public async Task<string> GetUserKey(string mail)
        {
            try
            {
                var getemploykey = (await ClientUsers.Child("Products").OnceAsync<Users>()).
                    FirstOrDefault(a => a.Object.MAIL == mail);
                if (getemploykey == null) return null;

                img1 = getemploykey.Object.image1;
                image2 = getemploykey.Object.image2;
                image3 = getemploykey.Object.image3;
                image4 = getemploykey.Object.image4;
                image5 = getemploykey.Object.image5;
                image6 = getemploykey.Object.image6;
                Imagae_1_link = getemploykey.Object.Imagae_1_link;
                ProductName = getemploykey.Object.ProductName;
                ProductDesc = getemploykey.Object.ProductDesc;
                ProductPrice = getemploykey.Object.ProductPrice;
                ProductQuantity = getemploykey.Object.ProductQuantity;


                return getemploykey?.Key;
            }
            catch (Exception ex)
            {
                return null;
            }

        }






        public async Task<List<Users>> GetAllUsers(string userKey)
        {
            try
            {
                return (await ClientUsers
                    .Child($"Users/Account/{userKey}")
                    .OnceAsync<Users>()).Select(item => new Users
                    {
                        FNAME = item.Object.FNAME,
                        LNAME = item.Object.LNAME,
                    }).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Firebase Exception: {ex}");
                return null;
            }
        }

        public async Task<ObservableCollection<Users>> GetUserProducts(string userKey)
        {
            try
            {
                var products = await ClientUsers
                    .Child($"Users/Account/{userKey}/Product")
                    .OnceAsync<Users>();

                var productList = products.Select(p =>
                {
                    var user = p.Object;
                    user.ProductPath = $"Users/Account/{userKey}/Product/{p.Key}";
                    return user;
                }).ToList();

                return new ObservableCollection<Users>(productList);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                return new ObservableCollection<Users>();
            }
        }




        public async Task<bool> Save(FileResult maninimg,
                                 FileResult img1,
                                 FileResult img2,
                                 FileResult img3,
                                 FileResult img4,
                                 FileResult img5,
                                 FileResult img6,
                                 string productname,
                                 string productDescript,
                                 string productprice,
                                 string productquantity)
        {
            var _mainimg = await UploadImage(await maninimg.OpenReadAsync(),
                                             "ProductImg",
                                             maninimg.FileName);

            var _mainimg1 = await UploadImage(await img1.OpenReadAsync(),
                                            "ProductImg",
                                            img1.FileName);
            var _mainimg2 = await UploadImage(await img2.OpenReadAsync(),
                                            "ProductImg",
                                            img2.FileName);
            var _mainimg3 = await UploadImage(await img3.OpenReadAsync(),
                                            "ProductImg",
                                            img3.FileName);
            var _mainimg4 = await UploadImage(await img4.OpenReadAsync(),
                                            "ProductImg",
                                            img4.FileName);
            var _mainimg5 = await UploadImage(await img5.OpenReadAsync(),
                                            "ProductImg",
                                            img5.FileName);
            var _mainimg6 = await UploadImage(await img6.OpenReadAsync(),
                                            "ProductImg",
                                            img6.FileName);

            var _main1 = await addDesc(_mainimg,
                                       _mainimg1,
                                       _mainimg2,
                                       _mainimg3,
                                       _mainimg4,
                                       _mainimg5,
                                       _mainimg6,
                                       productname,
                                       productDescript,
                                       productprice,
                                       productquantity);

            return true;
        }


        public async Task<bool> SendMessage(string senderEmail, string receiverEmail, string message)
        {
            try
            {
                // Assuming your structure is like this: Users/Messages
                var user = (await ClientUsers
                    .Child($"Users/Messages")
                    .OnceAsync<Users>())
                    .FirstOrDefault(a => a.Object.MAIL == senderEmail && a.Object.MAIL == receiverEmail);

                // Check if the user exists and the message is sent successfully
                if (user != null)
                {
                    // Assuming the Users class has properties like SenderEmail, ReceiverEmail, Message, etc.
                    user.Object.Message = message;

                    // Update the message in the database
                    await ClientUsers
                        .Child($"Users/Messages/{user.Key}")
                        .PutAsync(user.Object);

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> ReceiveMessage(string receiverEmail)
        {
            try
            {
                // Assuming your structure is like this: Users/Messages
                var user = (await ClientUsers
                    .Child($"Users/Messages")
                    .OnceAsync<Users>())
                    .FirstOrDefault(a => a.Object.MAIL == receiverEmail);

                // Check if the user exists and has a message
                if (user != null && !string.IsNullOrEmpty(user.Object.Message))
                {
                    return user.Object.Message;
                }

                return "No messages"; // Return a default message if there are no messages
            }
            catch
            {
                return "Error fetching messages"; // Handle errors appropriately
            }
        }



    }
}
