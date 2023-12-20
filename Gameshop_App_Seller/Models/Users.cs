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
    using Newtonsoft.Json;



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

        public string isVerified { get; set; }
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

        //login code
        public async Task<bool> AdminLogin(string email, string password)
        {
            try
            {
                var evaluateEmail = (await ClientUsers
                    .Child("Users/Account")
                    .OnceAsync<Users>())
                    .FirstOrDefault(a => a.Object.MAIL == email && a.Object.PASSWORD == password);

                return evaluateEmail != null;
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

        //not yet
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

        // add user
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


        //upload photos of product
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


        //adding product code
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
                                     string ProdQuan,
                                     string email)
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
                            ProductQuantity = ProdQuan,
                            MAIL = email
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

        //not yet
            public async Task<string> GetUserKey(string mail)
            {
                try
                {
                    var getuserkey = (await ClientUsers.Child($"Users/Account/{App.key}/Product").OnceAsync<Users>()).
                        FirstOrDefault(a => a.Object.MAIL == mail);
                    if (getuserkey == null) return null;

                    img1 = getuserkey.Object.image1;
                    image2 = getuserkey.Object.image2;
                    image3 = getuserkey.Object.image3;
                    image4 = getuserkey.Object.image4;
                    image5 = getuserkey.Object.image5;
                    image6 = getuserkey.Object.image6;
                    Imagae_1_link = getuserkey.Object.Imagae_1_link;
                    ProductName = getuserkey.Object.ProductName;
                    ProductDesc = getuserkey.Object.ProductDesc;
                    ProductPrice = getuserkey.Object.ProductPrice;
                    ProductQuantity = getuserkey.Object.ProductQuantity;
              


                    return getuserkey?.Key;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }



        //displaying all seller products
        public async Task<ObservableCollection<Users>> GetUserProductListsAsync()
        {
            var userKeys = await GetUserKeysAsync();

            var userProductList = new ObservableCollection<Users>();

            foreach (var userKey in userKeys)
            {
                var userData = await ClientUsers
                    .Child($"Users/Account/{userKey}/Product")
                    .OnceAsync<Users>();

                foreach (var firebaseObject in userData)
                {
                    userProductList.Add(firebaseObject.Object);
                }
            }

            return userProductList;
        }
        private async Task<List<string>> GetUserKeysAsync()
        {
            var userKeysSnapshot = await ClientUsers
                .Child("Users/Account")
                .OnceAsync<object>();

            return userKeysSnapshot.Select(u => u.Key).ToList();
        }


        //not yet
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


        public async Task<ObservableCollection<Users>> GetUsersinfoAsync(string userKey)
        {
            try
            {
                var userSnapshot = await ClientUsers
                    .Child($"Users/Account/{userKey}")
                    .OnceSingleAsync<Users>();

                if (userSnapshot != null)
                {
                    return new ObservableCollection<Users> { userSnapshot };
                }
                else
                {
                    // User not found
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                Console.WriteLine($"Exception in GetUsersinfoAsync: {ex.Message}");
                return null;
            }
        }



        //when product of seller is added it will display their own product in their seller page or home
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



        //upload seller products
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
                                     string productquantity,
                                     string mail)
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
                                           productquantity,
                                           mail);

                return true;
        }

        //not yet
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


        //path and upload a photo to validate
        public async Task<string> UploadValid(Stream img, string proname, string filename)
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

        //Valid ID submission
        public async Task<bool> addValidID(
                                     string img1,
                                     string img2,
                                     string img3,
                                     string img4,
                                     string email,
                                     string isVerified
                                     )
        {


            try
            {
                // Construct the path to the user's account node
                var userAccountPath = $"Users/Request";

                var evaluateEmail = (await ClientUsers
                    .Child($"{userAccountPath}")
                    .OnceAsync<Users>())
                    .FirstOrDefault(a => a.Object.image1 == img1);

                if (evaluateEmail == null)
                {
                    // Product does not exist, add it
                    var admin = new Users()
                    {
                        image1 = img1,
                        image2 = img2,
                        image3 = img3,
                        image4 = img4,
                        MAIL = email,
                        isVerified = isVerified
                    };

                    // Use the user's account path as the child node
                    await ClientUsers
                        .Child($"{userAccountPath}")
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

        //sending the application for becoming a seller
        public async Task<bool> SaveValid(
                        FileResult img1,
                        FileResult img2,
                        FileResult img3,
                        FileResult img4,
                        string email,
                        string isVerified)
        {

            var ValidImg1 = await UploadImage(await img1.OpenReadAsync(),
                                                 "ValidImg",
                                                 img1.FileName);

            var ValidImg2 = await UploadImage(await img2.OpenReadAsync(),
                                            "ValidImg",
                                            img2.FileName);
            var ValidImg3 = await UploadImage(await img3.OpenReadAsync(),
                                            "ValidImg",
                                            img3.FileName);
            var ValidImg4 = await UploadImage(await img4.OpenReadAsync(),
                                            "ValidImg",
                                            img4.FileName);


            var ValidIDs = await addValidID(ValidImg1,
                                    ValidImg2,
                                    ValidImg3,
                                    ValidImg4,
                                    email,
                                    isVerified);

            return true;
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
