    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using static Gameshop_App_Seller.App;
    using Firebase.Database.Query;
    using Firebase.Auth;
    using Firebase.Database;
    using System.Collections.ObjectModel;
    using System.Reactive.Linq;
    using Firebase.Database.Streaming;
    





namespace Gameshop_App_Seller.Models
{
    public class Users
    {
        private static int NotificationIdCounter = 0;
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

        public string ShopProfile { get; set; }
        public string ShopCoverImg { get; set; }
        public string ShopName { get; set; }
        public string ShopContactNumber { get; set; }
        public string ShopMessengerLink { get; set; }



        public string isVerified { get; set; }
        public string Message { get; set; }
        public string ReporterEmail { get; set; }
        public string DeniedReason { get; set; }
        public string ProfilePicture { get; set; }



        public string StarReview { get; set; }
        public string comment { get; set; }
        public string itemShopImage { get; set; }
        public string EmailReviewer { get; set; }
        public string Date { get; set; }








        // properties of being purchase/buy confirmation
        public string soldName { get; set; }
        public string soldQuantity { get; set; }
        public string soldPrice { get; set; }
        public string soldTime { get; set; }
        public string soldDate { get; set; }
        public string Seller { get; set; }
        public string Buyer { get; set; }
        public string soldImageItem { get; set; }
        public string soldTranscationImage { get; set; }




        public async Task<bool> SoldProduct(
                                string img1,
                                string img2,
                                string soldname,
                                string soldquantity,
                                string soldprice,
                                string soldtime,
                                string solddate,
                                string soldtoseller,
                                string soldtobuyer)
        {

            // Construct the path to the user's account node
            var evaluateEmail = (await ClientUsers
              .Child("Purchase History")
              .OnceAsync<Users>())
              .FirstOrDefault(a => a.Object.MAIL == soldtoseller);

            var admin = new Users()
            {
                soldName = soldname,
                soldQuantity = soldquantity,
                soldPrice = soldprice,
                soldTime = soldtime,
                soldDate = solddate,
                Seller = soldtoseller,
                Buyer = soldtobuyer,
                soldImageItem = img1,
                soldTranscationImage = img2
            };
            await ClientUsers
                      .Child($"Purchase History")
                      .PostAsync(admin);

            return true;
        }



        public async Task<bool> PurchaseH(
                      string img1,
                      FileResult img2,
                     string soldname,
                     string soldquantity,
                     string soldprice,
                     string soldtime,
                     string solddate,
                     string soldtoseller,
                     string soldtobuyer)
        {

          
            var ImageTransaction = await UploadImage(await img2.OpenReadAsync(),
                                                "TransactionImage",
                                                img2.FileName);

            var ValidIDs = await SoldProduct(
                                         img1,
                                         ImageTransaction,
                                         soldname,
                                         soldquantity,
                                         soldprice,
                                         soldtime,
                                         solddate,
                                         soldtoseller,
                                         soldtobuyer);

            return true;
        }





        public async Task<bool> SellerReviews(string emailreviewer, string date, string rating, string RaterComment, string email, string itemImage)
        {
            var evaluateEmail = (await ClientUsers
                  .Child("Reviews")
                  .OnceAsync<Users>())
                  .FirstOrDefault(a => a.Object.MAIL == email);

            var admin = new Users()
            {
                StarReview = rating,
                comment = RaterComment,
                itemShopImage = itemImage,
                EmailReviewer = emailreviewer,
                Date = date,
                MAIL = email,

            };
            await ClientUsers
                      .Child($"Reviews")
                      .PostAsync(admin);
            return true;
        }
        public async Task<bool> UploadReview(
                       FileResult img1,
                       string shopRate,
                       string shopComment,
                       string shopEmail,
                       string emailReviewer,
                       string date)
        {

            var ImageReview = await UploadImage(await img1.OpenReadAsync(),
                                                 "ReviewImages",
                                                 img1.FileName);

            var ValidIDs = await SellerReviews(emailReviewer,
                                    date,
                                    shopRate,
                                    shopComment,
                                    shopEmail,
                                    ImageReview);

            return true;
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
                    .Child("Account")
                    .OnceAsync<Users>())
                    .FirstOrDefault(a => a.Object.MAIL == email && a.Object.PASSWORD == password);

                return evaluateEmail != null;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> UploadProfilePicture(Stream img, string imgProfile, string filename)
        {
            try
            {
                var image = await App.firebaseStorage
                    .Child($"Images/{imgProfile}/{filename}")
                    .PutAsync(img);
                return image;
            }
            catch (Exception ex)
            {
                return "false";
            }
        }

        public async Task<bool> UpdateUserData(
                                string ProfilePic,
                                string email,
                                string birthday,
                                string fName,
                                string lName,
                                string gender,
                                string hAddress,
                                string password)
        {
            try
            {
                // Construct the path to the user's account node
                var userAccountPath = "Account";

                var evaluateEmail = (await ClientUsers
                    .Child($"{userAccountPath}")
                    .OnceAsync<Users>())
                    .FirstOrDefault(a => a.Object.MAIL == email);

                if (evaluateEmail != null)
                {
                    // User exists, update user data
                    var existingUser = evaluateEmail.Object;

                    // Update only the fields that are provided
                    if (!string.IsNullOrEmpty(birthday))
                        existingUser.BIRTHDAY = birthday;

                    if (!string.IsNullOrEmpty(fName))
                        existingUser.FNAME = fName;

                    if (!string.IsNullOrEmpty(lName))
                        existingUser.LNAME = lName;

                    if (!string.IsNullOrEmpty(gender))
                        existingUser.GENDER = gender;

                    if (!string.IsNullOrEmpty(hAddress))
                        existingUser.Haddress = hAddress;

                    if (!string.IsNullOrEmpty(password))
                        existingUser.PASSWORD = password;

                    existingUser.ProfilePicture = ProfilePic;

                    // Ensure the Product property remains unchanged
                    //existingUser.Product = existingUser.Product;

                    // Use the user's account path as the child node
                    await ClientUsers
                        .Child($"{userAccountPath}/{App.key}")
                        .PatchAsync(existingUser);

                    return true;
                }
                else
                {
                    // User does not exist
                    return false;
                }
            }
            catch
            {
                // An error occurred
                return false;
            }
        }

        public async Task<bool> Update(FileResult ProfilePic,
                                           string email,
                                           string birthday,
                                           string fname,
                                           string lname,
                                           string address,
                                           string gender,
                                           string password)
        {
            var _mainimg = await UploadProfilePicture(await ProfilePic.OpenReadAsync(),
                                             "ProfilePictureUser",
                                             ProfilePic.FileName);



            var _main1 = await UpdateUserData(_mainimg, email, birthday, fname, lname, address, gender, password);

            return true;
        }

        //shop area


        public async Task<string> UploadProfilePicShop(Stream img, string imgProfile, string filename)
        {
            try
            {
                var image = await App.firebaseStorage
                    .Child($"Images/{imgProfile}/{filename}")
                    .PutAsync(img);
                return image;
            }
            catch (Exception ex)
            {
                return "false";
            }
        }


        public async Task<bool> UpdateUserShop(
                                string ShopProfilePicture,
                                string ShopCoverPicture,
                                string Shopname,
                                string Contactnumber,
                                string Messengerlink)
        {
            try
            {
                // Construct the path to the user's account node
                var userAccountPath = "Account";

                var evaluateEmail = (await ClientUsers
                    .Child($"{userAccountPath}")
                    .OnceAsync<Users>())
                    .FirstOrDefault(a => a.Object.MAIL == email);

                if (evaluateEmail != null)
                {

                    var existingUser = evaluateEmail.Object;

                    existingUser.ShopProfile = ShopProfilePicture;
                    existingUser.ShopCoverImg = ShopCoverPicture;
                    existingUser.ShopName = Shopname;
                    existingUser.ShopContactNumber = Contactnumber;
                    existingUser.ShopMessengerLink = Messengerlink;

                    await ClientUsers
                        .Child($"{userAccountPath}/{App.key}")
                        .PatchAsync(existingUser);

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



        public async Task<bool> UpdateShop(FileResult ShopProfilePic,
                                           FileResult ShopCover,
                                           string Shopname,
                                           string ShopContactNumber,
                                           string messengerLink)
        {
            var ShopProfile = await UploadProfilePicture(await ShopProfilePic.OpenReadAsync(),
                                             "ProfilePictureShop",
                                             ShopProfilePic.FileName);

            var ShopCoverPic = await UploadProfilePicture(await ShopCover.OpenReadAsync(),
                                            "CoverPictureShop",
                                            ShopProfilePic.FileName);



            var ShopUpdates = await UpdateUserShop(ShopProfile, ShopCoverPic, Shopname, ShopContactNumber, messengerLink);

            return true;
        }


        //shop area

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
        public async Task<RegistrationResult> AddUser(string name, string lname, string email, string password, string address, string birthday, string gender)
        {
            try
            {
                var user = (await ClientUsers.Child("Account")
                    .OnceAsync<Users>())
                    .FirstOrDefault(a => a.Object.MAIL == email);

                if (user == null)
                {
                    var newUser = new Users()
                    {
                        MAIL = email,
                        FNAME = name,
                        LNAME = lname,
                        PASSWORD = password,
                        Haddress = address,
                        BIRTHDAY = birthday,
                        GENDER = gender
                    };

                    await ClientUsers.Child("Account").PostAsync(newUser);
                    ClientUsers.Dispose();
                    return RegistrationResult.Success;
                }
                else
                {
                    return RegistrationResult.EmailExists;
                }
            }
            catch (Exception)
            {
                return RegistrationResult.Error;
            }
        }

        public enum RegistrationResult
        {
            Success,
            EmailExists,
            Error
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
                var userAccountPath = $"Account/{App.key}";

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
                var userSnapshot = (await ClientUsers.Child($"Account/{App.key}/Product").OnceAsync<Users>())
                    .FirstOrDefault(a => a.Object.MAIL == mail);

                if (userSnapshot == null)
                    return null;

                image1 = userSnapshot.Object.image1;
                image2 = userSnapshot.Object.image2;
                image3 = userSnapshot.Object.image3;
                image4 = userSnapshot.Object.image4;
                image5 = userSnapshot.Object.image5;
                image6 = userSnapshot.Object.image6;
                Imagae_1_link = userSnapshot.Object.Imagae_1_link;
                ProductName = userSnapshot.Object.ProductName;
                ProductDesc = userSnapshot.Object.ProductDesc;
                ProductPrice = userSnapshot.Object.ProductPrice;
                ProductQuantity = userSnapshot.Object.ProductQuantity;

                return userSnapshot?.Key;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserKey: {ex.Message}");
                return null;
            }
        }


        public async Task<Users> GetUserDataByEmailAsync(string email)
        {
            try
            {
                var userSnapshots = await ClientUsers
                    .Child("Account")
                    .Child("Product")
                    .OnceAsync<Users>();

                foreach (var snapshot in userSnapshots)
                {
                    Console.WriteLine($"Snapshot Key: {snapshot.Key}, Email: {snapshot.Object.MAIL}");
                }
                var userData = userSnapshots
                    .Select(snapshot => snapshot.Object)
                    .FirstOrDefault(user => user.MAIL.Equals(email, StringComparison.OrdinalIgnoreCase));
                return userData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserDataByEmailAsync: {ex.Message}");
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
                    .Child($"Account/{userKey}/Product")
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
                .Child("Account")
                .OnceAsync<object>();

            return userKeysSnapshot.Select(u => u.Key).ToList();
        }


        //not yet
        public async Task<List<Users>> GetAllUsers(string userKey)
        {
            try
            {
                return (await ClientUsers
                    .Child($"Account/{userKey}")
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
                    .Child($"Account/{userKey}")
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
                    .Child($"Account/{userKey}/Product")
                    .OnceAsync<Users>();

                var productList = products.Select(p =>
                {
                    var user = p.Object;
                    user.ProductPath = $"Account/{userKey}/Product/{p.Key}";
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

        //update user item


        public async Task<bool> UpdateProductData(
                                 string MainImageItem,
                                 string ITEM1,
                                 string ITEM2,
                                 string ITEM3,
                                 string ITEM4,
                                 string ITEM5,
                                 string ITEM6,
                                 string productname,
                                 string productDescript,
                                 string productprice,
                                 string productquantity,
                                 string productpath)
        {
            try
            {
                // Construct the path to the product node
                var userProductPath = $"{productpath}";

                Console.WriteLine($"Updating product at path: {userProductPath}");

                // Retrieve the product using the known key directly
                var evaluateProduct = await ClientUsers
                    .Child(userProductPath)
                    .OnceSingleAsync<Users>();

                if (evaluateProduct != null)
                {
                    // Product exists, update product data
                    Console.WriteLine($"Existing product: {evaluateProduct.ProductName}");

                    // Update only the fields that are provided
                    if (!string.IsNullOrEmpty(productDescript))
                        evaluateProduct.ProductDesc = productDescript;

                    if (!string.IsNullOrEmpty(productname))
                        evaluateProduct.ProductName = productname;

                    if (!string.IsNullOrEmpty(productprice))
                        evaluateProduct.ProductPrice = productprice;

                    if (!string.IsNullOrEmpty(productquantity))
                        evaluateProduct.ProductQuantity = productquantity;

                    // Update image links
                    evaluateProduct.Imagae_1_link = MainImageItem;
                    evaluateProduct.image1 = ITEM1;
                    evaluateProduct.image2 = ITEM2;
                    evaluateProduct.image3 = ITEM3;
                    evaluateProduct.image4 = ITEM4;
                    evaluateProduct.image5 = ITEM5;
                    evaluateProduct.image6 = ITEM6;

                    // Use the product's path as the child node
                    await ClientUsers
                        .Child(userProductPath)
                        .PatchAsync(evaluateProduct);

                    Console.WriteLine("Product updated successfully!");
                    return true;
                }
                else
                {
                    // Product does not exist
                    Console.WriteLine("Product does not exist.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // An error occurred
                Console.WriteLine($"Error updating product data: {ex.Message}");
                return false;
            }
        }






        public async Task<bool> UpdateProductUser(
                         FileResult maninimg,
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
                         string productpath) // Add productpath parameter
        {
            try
            {
                var MainImageItem = await UploadImage(await maninimg.OpenReadAsync(),
                                                     "ProductImg",
                                                     maninimg.FileName);

                var ITEM1 = await UploadImage(await img1.OpenReadAsync(),
                                              "ProductImg",
                                              img1.FileName);
                var ITEM2 = await UploadImage(await img2.OpenReadAsync(),
                                              "ProductImg",
                                              img2.FileName);
                var ITEM3 = await UploadImage(await img3.OpenReadAsync(),
                                              "ProductImg",
                                              img3.FileName);
                var ITEM4 = await UploadImage(await img4.OpenReadAsync(),
                                              "ProductImg",
                                              img4.FileName);
                var ITEM5 = await UploadImage(await img5.OpenReadAsync(),
                                              "ProductImg",
                                              img5.FileName);
                var ITEM6 = await UploadImage(await img6.OpenReadAsync(),
                                              "ProductImg",
                                              img6.FileName);

                // Pass the productpath parameter to UpdateProductData
                var isUpdated = await UpdateProductData(
                                      MainImageItem,
                                      ITEM1,
                                      ITEM2,
                                      ITEM3,
                                      ITEM4,
                                      ITEM5,
                                      ITEM6,
                                      productname,
                                      productDescript,
                                      productprice,
                                      productquantity,
                                      productpath);

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in UpdateProductUser: {ex.Message}");
                return false;
            }
        }



        //not yet


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
                                    string isVerified)
        {
            try
            {
                // Construct the path to the user's account node
                var userAccountPath = $"Request";

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
                        .Child($"{userAccountPath}/{App.key}")
                        .PutAsync(admin);

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



        public async Task<bool> ReportedProduct(string images, string productName, string productPrice, string email, string reportedreason, string reporterEmails)
        {
            var evaluateEmail = (await ClientUsers
                   .Child("Reported")
                   .OnceAsync<Users>())
                   .FirstOrDefault(a => a.Object.MAIL == email);

            var admin = new Users()
            {
                image1 = images,
                ProductName = productName,
                ProductPrice = productPrice,
                Message = reportedreason,
                MAIL = email,
                ReporterEmail = reporterEmails

            };
            await ClientUsers
                      .Child($"Reported")
                      .PostAsync(admin);
            return true;
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


        public async Task<ObservableCollection<Users>> GetDeniedApplicationsListAsync()
        {
            try
            {
                // Assuming DeniedApplication is a class representing denied applications
                var deniedApplications = await ClientUsers
                    .Child("Denied Applications")
                    .OnceAsync<Users>();

                var deniedApplicationsList = deniedApplications
                    .Select(item => item.Object)
                    .ToList();

                return new ObservableCollection<Users>(deniedApplicationsList);
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed
                Console.WriteLine($"Error getting denied applications list: {ex.Message}");
                return new ObservableCollection<Users>();
            }
        }


        public async Task<ObservableCollection<Users>> GetReviewListAsync()
        {
            try
            {
                // Assuming DeniedApplication is a class representing denied applications
                var deniedApplications = await ClientUsers
                    .Child("Reviews")
                    .OnceAsync<Users>();

                var deniedApplicationsList = deniedApplications
                    .Select(item => item.Object)
                    .ToList();

                return new ObservableCollection<Users>(deniedApplicationsList);
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed
                Console.WriteLine($"Error getting denied applications list: {ex.Message}");
                return new ObservableCollection<Users>();
            }
        }


        public async Task<ObservableCollection<Users>> GetPurchaseListAsync()
        {
            try
            {
                // Assuming DeniedApplication is a class representing denied applications
                var deniedApplications = await ClientUsers
                    .Child("Purchase History")
                    .OnceAsync<Users>();

                var deniedApplicationsList = deniedApplications
                    .Select(item => item.Object)
                    .ToList();

                return new ObservableCollection<Users>(deniedApplicationsList);
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed
                Console.WriteLine($"Error getting denied applications list: {ex.Message}");
                return new ObservableCollection<Users>();
            }
        }






        public async Task<ObservableCollection<Users>> GetUserProductListsAsync(string searchQuery = null)
        {
            var userKeys = await GetUserKeysAsync();

            var userProductList = new ObservableCollection<Users>();

            foreach (var userKey in userKeys)
            {
                var userData = await ClientUsers
                    .Child($"Account/{userKey}/Product")
                    .OnceAsync<Users>();

                foreach (var firebaseObject in userData)
                {
                    var user = firebaseObject.Object;

                    // If a search query is provided and the product doesn't match, skip it
                    if (!string.IsNullOrWhiteSpace(searchQuery) &&
                        !user.ProductName.Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }

                    userProductList.Add(user);
                }
            }

            return userProductList;
        }




        //public async Task<string> ReceiveMessage(string receiverEmail)
        //{
        //    try
        //    {
        //        // Assuming your structure is like this: Users/Messages
        //        var user = (await ClientUsers
        //            .Child($"Users/Messages")
        //            .OnceAsync<Users>())
        //            .FirstOrDefault(a => a.Object.MAIL == receiverEmail);

        //        // Check if the user exists and has a message
        //        if (user != null && !string.IsNullOrEmpty(user.Object.Message))
        //        {
        //            return user.Object.Message;
        //        }

        //        return "No messages"; // Return a default message if there are no messages
        //    }
        //    catch
        //    {
        //        return "Error fetching messages"; // Handle errors appropriately
        //    }
        //}

        //private void ShowNotification(string title, string description)
        //{
        //    var notification = new NotificationRequest
        //    {
        //        NotificationId = NotificationIdCounter,
        //        Title = title,
        //        Description = description,
        //        ReturningData = "optional data",
        //        Schedule = new NotificationRequestSchedule { NotifyTime = DateTime.Now.AddSeconds(5) } // Adjust timing as needed
        //    };

        //    NotificationCenter.Current.Show(notification);
        //}

    }
}
