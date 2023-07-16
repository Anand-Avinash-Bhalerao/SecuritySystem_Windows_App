using Firebase.Auth;
using Firebase.Database;
using MVVMEssentials.ViewModels;
using SecuritySystem.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SecuritySystem.ViewModels
{
    public class RegisterAdminViewModel : ViewModelBase
    {
        private string email;
        public string Email
        {
            get 
            {
                return email; 
            }
            set 
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string locationType;
        public string LocationType
        {
            get
            {
                return locationType;
            }
            set
            {
                locationType = value;
                OnPropertyChanged(nameof(LocationType));
            }
        }

        private string location;
        public string Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        private string phoneNo;
        public string PhoneNo
        {
            get
            {
                return phoneNo;
            }
            set
            {
                phoneNo = value;
                OnPropertyChanged(nameof(PhoneNo));
            }
        }


        public ICommand RegisterCommand { get; }

        public ICommand NavigateLoginCommand { get; }

        public RegisterAdminViewModel(FirebaseAuthClient client, FirebaseClient dataClient)
        {
            RegisterCommand = new RegisterAdminCommand(this, client,dataClient);
        }

    }
}
