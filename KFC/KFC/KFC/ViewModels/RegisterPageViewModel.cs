using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using KFC.Models;
using KFC.Services;
using Xamarin.Forms;

namespace KFC.ViewModels
{
    internal enum RegisterErrors
    {
        Username, Password, Email, Empty, Ok
    }
    public class RegisterPageViewModel : INotifyPropertyChanged
    { 
        public RegisterPageViewModel()
        {
            RegisterAccountButtonCommand = new Command(async () =>
            {
                await AccountDataBase.InitializeAccountList();
                var errorArray= new[] {CheckEmail(),CheckPassword(),CheckUsername(),CheckCompletedFields()};
                var errorReport = string.Empty;
                var okCount = 0;
                if (CheckCompletedFields() == RegisterErrors.Ok)
                {
                    foreach (var error in errorArray)
                    {
                        switch (error)
                        {
                            case RegisterErrors.Email:
                                Email = string.Empty;
                                errorReport += "Email must:\n\t-Look like ex@mail.domain\n\t-Not be used more than once)\n";
                                break;
                            case RegisterErrors.Password:
                                PassRegister = string.Empty;
                                errorReport += "Password must:\n\t-Contain letters\n\t-Contain numbers\n\t-Be over 6 characters\n";
                                break;
                            case RegisterErrors.Username:
                                UsernameRegister = string.Empty;
                                errorReport += "Username must:\n\t-Not be taken\n\t-Contain letters\n\t-Contain numbers\n\t-Be over 8 characters\n";
                                break;
                            case RegisterErrors.Ok:
                                okCount++;
                                break;
                        }
                        if (okCount == errorArray.Length)
                        {
                            var newAccount = 
                                new AccountInfo(UsernameRegister.GetHashCode(),
                                    UsernameRegister,PassRegister,Email,new User(Name,Surname),IsChefChecked);
                            
                            await AccountDataBase.AddAccount(newAccount);
                            
                            Name = string.Empty;
                            Surname = string.Empty;
                            UsernameRegister = string.Empty;
                            PassRegister = string.Empty;
                            Email = string.Empty;
                            
                            await Application.Current.MainPage.Navigation.PopAsync();
                        }
                    }

                    if (okCount != errorArray.Length)
                    {
                        await Application.Current.MainPage.DisplayAlert("Something is not ok", 
                            errorReport, "Ok");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Complete the form", 
                        "Please complete the fields first", "Ok");
                }
            });

            ClearNameButtonCommand = new Command(() =>
            {
                Name = string.Empty;
            });

            ClearSurnameButtonCommand = new Command(() =>
            {
                Surname = string.Empty;
            });

            ClearEmailButtonCommand = new Command(() =>
            {
                Email = string.Empty;
            });

            ClearUsernameButtonCommand = new Command(() =>
            {
                UsernameRegister = string.Empty;
            });

            ClearPasswordButtonCommand = new Command(() =>
            {
                PassRegister = string.Empty;
            });

            UsernameRegisterPlaceholder = "";
            PassRegisterPlaceholder = "";
            EmailPlaceholder = "random@email.com";
            
        }
        
        private bool isChefChecked=false;
        public bool IsChefChecked
        {
            get => isChefChecked;
            set
            {
                isChefChecked = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(IsChefChecked)));
            }
        }
        private bool isCustomerChecked=true;
        public bool IsCustomerChecked
        {
            get => isCustomerChecked;
            set
            {
                isCustomerChecked = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(IsCustomerChecked)));
            }
        }

        private RegisterErrors CheckCompletedFields()
        {
            if (Name == null || Surname == null || UsernameRegister == null || PassRegister == null || Email == null)
            {
                return RegisterErrors.Empty;
            }
            
            if (Name == "" || Surname == "" || UsernameRegister == "" || PassRegister == "" || Email == "")
            {
                return RegisterErrors.Empty;
            }

            return RegisterErrors.Ok;
        }
        private RegisterErrors CheckEmail()
        {
            if (Email == null) return RegisterErrors.Empty;

            if (!Email.Contains("@") || !Email.Contains(".") || Email.EndsWith("."))
            {
                return RegisterErrors.Email;
            }

            return AccountDataBase.ListOfAccounts.Any(user => user.AccountMail == Email) ? RegisterErrors.Email : RegisterErrors.Ok;
        }

        private RegisterErrors CheckUsername()
        {
            if (UsernameRegister == null) return RegisterErrors.Empty;
            
            if (AccountDataBase.ListOfAccounts.Any(account => account.Username == UsernameRegister))
            {
                return RegisterErrors.Username;
            }
            
            var necessaryString = new Regex("[^A-Za-z0-9]");
            if (necessaryString.IsMatch(UsernameRegister) || UsernameRegister.Length<8)
            {
                return RegisterErrors.Username;
            }
            
            return RegisterErrors.Ok;
        }
        
        private RegisterErrors CheckPassword()
        {
            if (PassRegister == null) return RegisterErrors.Empty;
            
            var necessaryString = new Regex("[^A-Za-z0-9]");
            if (necessaryString.IsMatch(PassRegister) || PassRegister.Length<6)
            {
                return RegisterErrors.Password;
            }

            return RegisterErrors.Ok;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand RegisterAccountButtonCommand { get; set; }
        public ICommand ClearNameButtonCommand { get; set; }
        public ICommand ClearSurnameButtonCommand { get; set; }
        public ICommand ClearEmailButtonCommand { get; set; }
        public ICommand ClearUsernameButtonCommand { get; set; }
        public ICommand ClearPasswordButtonCommand { get; set; }

        private string name;
        private string surname;
        private string email;
        private string usernameRegister;
        private string passRegister;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public string Surname
        {
            get => surname;
            set
            {
                surname = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Surname)));
            }
        }

        public string Email
        {
            get => email;
            set
            {
                email = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
            }
        }

        public string UsernameRegister
        {
            get => usernameRegister;
            set
            {
                usernameRegister = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UsernameRegister)));
            }
        }

        public string PassRegister
        {
            get => passRegister;
            set
            {
                passRegister = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PassRegister)));
            }
        }
        
        private string emailPlaceholder;
        private string usernameRegisterPlaceholder;
        private string passRegisterPlaceholder;
        public string EmailPlaceholder
        {
            get => emailPlaceholder;
            set
            {
                emailPlaceholder = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EmailPlaceholder)));
            }
        }

        public string UsernameRegisterPlaceholder
        {
            get => usernameRegisterPlaceholder;
            set
            {
                usernameRegisterPlaceholder = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UsernameRegisterPlaceholder)));
            }
        }

        public string PassRegisterPlaceholder
        {
            get => passRegisterPlaceholder;
            set
            {
                passRegisterPlaceholder = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PassRegisterPlaceholder)));
            }
        }
    }
}

