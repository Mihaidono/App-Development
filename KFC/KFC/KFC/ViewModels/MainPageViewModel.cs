using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using KFC.Services;

namespace KFC.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            RegisterButtonCommand = new Command(async () =>
            {
                await AccountDataBase.InitializeAccountList();
                
                PassInput = string.Empty;
                UsernameInput = string.Empty;
                await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
            });

            LoginButtonCommand = new Command(async () =>
            {
                await AccountDataBase.InitializeAccountList();
                
                if (AccountDataBase.ListOfAccounts != null)
                {
                    CurrentUser.CurrentAccount = AccountDataBase.ListOfAccounts.SingleOrDefault(account =>
                            account.Username == UsernameInput && account.Password == PassInput);
                }
                
                PassInput = string.Empty;
                UsernameInput = string.Empty;
                
                    if (CurrentUser.CurrentAccount!=null)
                {
                    await TicketsDataBase.InitializeTicketList();
                    await BurgerMenuDataBase.InitializeBurgerMenuList();
                    await CurrentUser.UpdateCurrentUserDataFile(CurrentUser.CurrentAccount);
                    Application.Current.MainPage.Navigation.InsertPageBefore(new FlyoutPanePage(),
                        Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault());
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Don't know you", "Account does not exist. Username or Password might be typed wrong", "ok");
                }
            });

        }
        public event PropertyChangedEventHandler PropertyChanged;

        private string passInput;
        private string usernameInput;

        public string PassInput
        {
            get => passInput;
            set
            {
                passInput = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PassInput)));
            }
        }

        public string UsernameInput
        {
            get=>usernameInput;
            set
            {
                usernameInput = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UsernameInput)));
            }
        }

        public ICommand RegisterButtonCommand { get; set; }

        public ICommand LoginButtonCommand { get; set; }



    }
}