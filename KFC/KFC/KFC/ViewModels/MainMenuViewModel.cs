using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using KFC.Models;
using KFC.Services;
using Xamarin.Forms;

namespace KFC.ViewModels
{
    public class MainMenuViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<BurgerMenu> MyBurgerList { get; set; }
        public ObservableCollection<BurgerMenu> MostWantedBurgers { get; set; }
        public ObservableCollection<BurgerMenu> OnSaleMenus { get; set; }

        public MainMenuViewModel()
        {
            MyBurgerList = new ObservableCollection<BurgerMenu>(BurgerMenuDataBase.ListOfBurgerMenus);

            MostWantedBurgers = GetMostWantedBurgers();

            OnSaleMenus = GetOnSaleBurgers();

            NavigateToTappedMenuFl = new Command(async () =>
            {
                BurgerMenuDataBase.CurrentBurger = CurrentTappedMenuFl;
                await Application.Current.MainPage.Navigation.PushAsync(new BurgerDetailPage());
            });
            
            NavigateToTappedMenuSl = new Command(async () =>
            {
                BurgerMenuDataBase.CurrentBurger = CurrentTappedMenuSl;
                await Application.Current.MainPage.Navigation.PushAsync(new BurgerDetailPage());
            });
        }

        private BurgerMenu currentTappedMenuFl;

        public BurgerMenu CurrentTappedMenuFl
        {
            get=>currentTappedMenuFl;
            set
            {
                currentTappedMenuFl = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(CurrentTappedMenuFl)));
            }
        }
        
        private BurgerMenu currentTappedMenuSl;

        public BurgerMenu CurrentTappedMenuSl
        {
            get=>currentTappedMenuSl;
            set
            {
                currentTappedMenuSl = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(CurrentTappedMenuSl)));
            }
        }

        private readonly BurgerMenu currentTappedMenuTl = null;

        public BurgerMenu CurrentTappedMenuTl
        {
            get=>currentTappedMenuTl;
            set
            {
                BurgerMenuDataBase.CurrentBurger = value;
                
                Application.Current.MainPage.Navigation.PushAsync(new BurgerDetailPage());

                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(CurrentTappedMenuTl)));
            }
        }
        public ICommand NavigateToTappedMenuFl { get; set; }
        public ICommand NavigateToTappedMenuSl { get; set; }
        private ObservableCollection<BurgerMenu> GetMostWantedBurgers()
        {
            var auxiliaryCollection = new ObservableCollection<BurgerMenu> 
                (MyBurgerList.OrderByDescending(burger=>burger.BuyCount).Take(5));
            return auxiliaryCollection;
        }

        private ObservableCollection<BurgerMenu> GetOnSaleBurgers()
        {
            var auxiliaryCollection = new ObservableCollection<BurgerMenu> 
                (MyBurgerList.Where(x=>x.IsOnSale));
            return auxiliaryCollection;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

