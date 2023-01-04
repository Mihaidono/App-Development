using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using KFC.Models;
using KFC.Services;
using Xamarin.Forms;

namespace KFC.ViewModels
{
    public class FlyoutMenuViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<FlyoutItemPage> FlyoutItems { get; set; }

        public FlyoutMenuViewModel()
        {

            FlyoutItems = new ObservableCollection<FlyoutItemPage>();
            if (CurrentUser.CurrentAccount.Permission)
            {
                FlyoutItems.Add(new FlyoutItemPage("Refresh Home", "homeIcon", new MainMenuPage())); 
                FlyoutItems.Add(new FlyoutItemPage("Contact", "contact", new ContactPage())); 
                FlyoutItems.Add(new FlyoutItemPage("Logout","logout",new MainPage()));
                FlyoutItems.Add(new FlyoutItemPage("My Burgers","",new MyBurgersPage())); 
            }
            else
            {
                FlyoutItems.Add(new FlyoutItemPage("Refresh Home", "homeIcon", new MainMenuPage())); 
                FlyoutItems.Add(new FlyoutItemPage("Contact", "contact", new ContactPage())); 
                FlyoutItems.Add(new FlyoutItemPage("Logout","logout",new MainPage()));
            }

            GoToProfileCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AccountInformationPage());
            });
        }
        
        private readonly FlyoutItemPage selectedMenu=null;
        
        public FlyoutItemPage SelectedMenu
        {
            get=>selectedMenu;
            set
            {
                if (selectedMenu!=value && value != null)
                {
                    switch (value.Title)
                    {
                        case "Refresh Home":
                            Application.Current.MainPage.Navigation.InsertPageBefore(new FlyoutPanePage(),
                                Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault());
                            Application.Current.MainPage.Navigation.PopAsync();
                            break;
                        case "Contact":
                            Application.Current.MainPage.Navigation.PushAsync(new ContactPage());
                            break;
                        case "Logout":
                            Application.Current.MainPage.Navigation.PushAsync(new MainPage());
                            break;
                        case "My Burgers":
                            Application.Current.MainPage.Navigation.PushAsync(new MyBurgersPage());
                            break;
                    }
                }
                
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMenu)));

            }
        }
        
        public ICommand GoToProfileCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}