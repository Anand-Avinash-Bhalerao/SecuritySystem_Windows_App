﻿using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using MVVMEssentials.Commands;
using SecuritySystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SecuritySystem.Commands
{
    internal class RegisterAdminCommand : AsyncCommandBase
    {

        private readonly RegisterAdminViewModel viewModel;
        private readonly FirebaseAuthClient client;
        private readonly FirebaseClient dataClient;
        public RegisterAdminCommand(RegisterAdminViewModel viewModel, FirebaseAuthClient client, FirebaseClient dataClient)
        {
            this.viewModel = viewModel;
            this.client = client;
            this.dataClient = dataClient;
        }

        private async Task<int> getLastUsedCodeAsync()
        {
            // This should be added in an API call in the future.
            int code = (int) await dataClient.Child("DeviceDatabase").Child("Values").Child("Code").OnceSingleAsync<int?>();
            return code;
        }

        private int getNewNumber(int code)
        {
            int randomOffset = new Random().Next(1, 100);
            code += randomOffset;
            return code;
        }

        private async Task addUserInDatabase(string uid, Object userObject)
        {

            //replace with api call as well
            dataClient.Child("DeviceDatabase").Child("Devices").Child(uid).PutAsync(userObject);
        }

        private async Task updateLastDeviceCode(int newCode)
        {
            dataClient.Child("DeviceDatabase").Child("Values").Child("Code").PutAsync(newCode);
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            string name = viewModel.Name;
            string email = viewModel.Email;
            string password = viewModel.Password; 
            string locationType = viewModel.LocationType;
            string location = viewModel.Location;
            string phoneNo = viewModel.PhoneNo;

            // Need to add validation checks for these fields.
            try
            {

                //Made it await because we want to pause the execution till a new user is created. Nahi toh user account create nahi hua and database me write ho sakta faltu ka.
                var authResult = await client.CreateUserWithEmailAndPasswordAsync(email, password);


                if(authResult.User != null)
                {
                    string uid = authResult.User.Uid;

                    //We need to assign a code to this acoount that the user will use to connect to this acc.
                    //That code is generated by reading the last used value for code generation and adding a random value to that and updating the last used value.

                    int oldCode = await getLastUsedCodeAsync();
                    int newCode = getNewNumber(oldCode);

                    //use this new code for the current device.

                    var userObject = new
                    {
                        Code = newCode,
                        DeviceName = name,
                        Email = email,
                        LocationType = locationType,
                        location = location,
                        phoneNo = phoneNo
                    };

                    //update the old used code in the database with the current code. For futher accounts.
                    await updateLastDeviceCode(newCode);

                    //add the data in the devicedetabase.
                    await addUserInDatabase(uid, userObject);

                    MessageBox.Show("User creation was successfull","Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("User creation failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        
    }
}
