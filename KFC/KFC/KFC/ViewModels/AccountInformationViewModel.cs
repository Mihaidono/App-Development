using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using KFC.Models;
using KFC.Services;
using Xamarin.Forms;

namespace KFC.ViewModels
{
    public class AccountInformationViewModel : INotifyPropertyChanged
    {
        public AccountInformationViewModel()
        {
            SetInitialStateData();
            
            ChangeEmailCommand = new Command(async () =>
            {
                var newProperty = await Application.Current.MainPage.DisplayPromptAsync("Email Change",
                    "What new email would you like to have?");
                if (newProperty != null && AccountDataBase.ListOfAccounts.All(account =>
                                            account.AccountMail != newProperty)
                                        && newProperty.Contains("@") && newProperty.Contains(".") &&
                                        !newProperty.EndsWith("."))
                {
                    EmailString = newProperty;
                    isEmailChanged = true;
                    SetTextColors();
                    SetSaveButtonVisibility();
                }
                else if (newProperty != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Not ok", "Try something else!", "Ok");
                }
            });

            ChangeNameCommand = new Command(async () =>
            {
                var newProperty = await Application.Current.MainPage.DisplayPromptAsync("Name Change",
                    "What new name would you like to have?");
                if (newProperty != null && newProperty!=CurrentUser.CurrentAccount.AccountUser.Name)
                {
                    isNameChanged = true;
                    NameString = newProperty;
                    SetTextColors();
                    SetSaveButtonVisibility();
                }
                else if (newProperty != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Not ok", "Try something else!", "Ok");
                }
            });

            ChangeSurnameCommand = new Command(async () =>
            {
                var newProperty = await Application.Current.MainPage.DisplayPromptAsync("Surname Change",
                    "What new surname would you like to have?");
                if (newProperty != null && newProperty!=CurrentUser.CurrentAccount.AccountUser.Surname)
                {
                    SurnameString = newProperty;
                    isSurnameChanged = true;
                    SetTextColors();
                    SetSaveButtonVisibility();
                }
                else if (newProperty != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Not ok", "Try something else!", "Ok");
                }
            });

            ChangeUsernameCommand = new Command(async () =>
            {
                var necessaryString = new Regex("[^A-Za-z0-9]");
                var newProperty = await Application.Current.MainPage.DisplayPromptAsync("Username Change",
                    "What new username would you like to have?");
                if (newProperty != null && AccountDataBase.ListOfAccounts.All(
                                            account => account.Username != newProperty)
                                        && !necessaryString.IsMatch(newProperty) && !(newProperty.Length < 8))
                {
                    UsernameString = newProperty;
                    isUsernameChanged = true;
                    SetTextColors();
                    SetSaveButtonVisibility();
                }
                else if (newProperty != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Not ok", "Try something else!", "Ok");
                }
            });

            ChangePasswordCommand = new Command(async () =>
            {
                var necessaryString = new Regex("[^A-Za-z0-9]");
                var newProperty = await Application.Current.MainPage.DisplayPromptAsync("Password Change",
                    "What new password would you like to have?");
                if (newProperty != null && !necessaryString.IsMatch(newProperty) && !(newProperty.Length < 6))
                {
                    PasswordString = newProperty;
                    isPasswordChanged = true;
                    SetTextColors();
                    SetSaveButtonVisibility();
                }
                else if (newProperty != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Not ok", "Try something else!", "Ok");
                }
            });

            SaveChangesButtonCommand = new Command(async () =>
            {
                var updatedUser = new AccountInfo(CurrentUser.CurrentAccount.AccountId,
                    UsernameString, PasswordString, EmailString, new User(NameString, SurnameString),CurrentUser.CurrentAccount.Permission);
                await AccountDataBase.UpdateAccount(updatedUser);
                await CurrentUser.UpdateCurrentUserDataFile(updatedUser);
                await CurrentUser.SetCurrentUserFromFile();
                SetInitialStateData();
            });
        }

        private void SetInitialStateData()
        {
            SetUserProperties();
            SetIsDataChangedFalse();
            SetSaveButtonVisibility();
            SetTextColors();
        }
        private void SetIsDataChangedFalse()
        {
            isPasswordChanged = false;
            isEmailChanged = false;
            isNameChanged = false;
            isSurnameChanged = false;
            isUsernameChanged = false;
            SaveButtonVisibility = false;
        }
        private void SetSaveButtonVisibility()
        {
            if (isPasswordChanged || isEmailChanged || isNameChanged || isUsernameChanged || isSurnameChanged)
            {
                SaveButtonVisibility = true;
            } else SaveButtonVisibility = false;
        }

        private void SetTextColors()
        {
            UsernameTextColor = isUsernameChanged ? Color.White : Color.Black;
            PasswordTextColor = isPasswordChanged ? Color.White : Color.Black;
            EmailTextColor = isEmailChanged ? Color.White : Color.Black;
            NameTextColor = isNameChanged ?  Color.White : Color.Black;
            SurnameTextColor = isSurnameChanged ? Color.White : Color.Black;
        }

        private void SetUserProperties()
        {
            NameString = CurrentUser.CurrentAccount.AccountUser.Name;
            SurnameString = CurrentUser.CurrentAccount.AccountUser.Surname;
            EmailString = CurrentUser.CurrentAccount.AccountMail;
            UsernameString = CurrentUser.CurrentAccount.Username;
            PasswordString = CurrentUser.CurrentAccount.Password;
        }
        public ICommand SaveChangesButtonCommand { get; set; }
        public ICommand ChangeUsernameCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }
        public ICommand ChangeEmailCommand { get; set; }
        public ICommand ChangeNameCommand { get; set; }
        public ICommand ChangeSurnameCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private string nameString;
        private string surnameString;
        private string emailString;
        private string usernameString;
        private string passwordString;

        private bool saveButtonVisibility;

        public bool SaveButtonVisibility
        {
            get => saveButtonVisibility;
            set
            {
                saveButtonVisibility = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(SaveButtonVisibility)));
            }
        }
        
        private Color usernameTextColor;
        public Color UsernameTextColor
        {
            get => usernameTextColor;
            set
            {
                usernameTextColor = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(UsernameTextColor)));
            }
        }
        private bool isUsernameChanged;
        
        private Color passwordTextColor;
        public Color PasswordTextColor
        {
            get => passwordTextColor;
            set
            {
                passwordTextColor = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(PasswordTextColor)));
            }
        }
        private bool isPasswordChanged;

        private Color emailTextColor;
        public Color EmailTextColor
        {
            get => emailTextColor;
            set
            {
                emailTextColor = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(EmailTextColor)));
            }
        }
        private bool isEmailChanged;

        private Color nameTextColor;
        public Color NameTextColor
        {
            get => nameTextColor;
            set
            {
                nameTextColor = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(NameTextColor)));
            }
        }
        private bool isNameChanged;

        private Color surnameTextColor;
        public Color SurnameTextColor
        {
            get => surnameTextColor;
            set
            {
                surnameTextColor = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(SurnameTextColor)));
            }
        }

        private bool isSurnameChanged;
        
        public string NameString
        {
            get => nameString;
            set
            {
                nameString = value;
                isNameChanged = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NameString)));
            }
        }
        public string SurnameString
        {
            get => surnameString;
            set
            {
                surnameString = value;
                isSurnameChanged = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SurnameString)));
            }
        }
        public string EmailString
        {
            get => emailString;
            set
            {
                emailString = value;
                isEmailChanged = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EmailString)));
            }
        }

        public string UsernameString
        {
            get => usernameString;
            set
            {
                usernameString = value;
                isUsernameChanged = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UsernameString)));
            }
        }

        public string PasswordString
        {
            get => passwordString;
            set
            {
                passwordString = value;
                isPasswordChanged = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PasswordString)));
            }
        }
    }
}