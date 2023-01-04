using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using KFC.Models;
using KFC.Services;
using Xamarin.Forms;

namespace KFC.ViewModels
{
    public class MyBurgersPageViewModel: INotifyPropertyChanged
    {
        private CancellationTokenSource clToken = new CancellationTokenSource();
        public ObservableCollection<BurgerMenu> MyBurgerList { get; set; }

        private ObservableCollection<BurgerMenu> searchResults;
        public ObservableCollection<BurgerMenu> SearchResults
        {
            get=>searchResults;
            set
            {
                searchResults = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(SearchResults)));
            }
        }

        public MyBurgersPageViewModel()
        {
            SearchListVisibility = false;
            MyBurgerList = new ObservableCollection<BurgerMenu>(BurgerMenuDataBase.UserBurgerMenus);
            SearchResults = new ObservableCollection<BurgerMenu>(BurgerMenuDataBase.ListOfBurgerMenus);
            
            PerformSearchCommand = new Command(() =>
            {
                SearchListVisibility = true;
                
                Interlocked.Exchange(ref this.clToken, new CancellationTokenSource()).Cancel();
                Task.Delay(TimeSpan.FromMilliseconds(700), this.clToken.Token)
                    .ContinueWith(
                        delegate
                        {
                            SearchResults = new ObservableCollection<BurgerMenu>
                                (BurgerMenuDataBase.ListOfBurgerMenus.Where(burger => 
                                    burger.Name.ToLower().Contains(SearchBarText.ToLower())).ToList());
                        },
                        CancellationToken.None,
                        TaskContinuationOptions.OnlyOnRanToCompletion,
                        TaskScheduler.FromCurrentSynchronizationContext());
            });

            NavigateToDetailPage= new Command(() =>
            {
                if (CurrentTappedBurgerHigh == null)
                {
                    return;
                }
                BurgerMenuDataBase.CurrentBurger = CurrentTappedBurgerHigh;
                Application.Current.MainPage.Navigation.PushAsync(new BurgerDetailPage());
            });

            AddBurgerCommandButton = new Command( async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new CreateBurgerPage());
            });
            
            DeleteBurgerCommandButton = new Command( () =>
            {
                
            });
        }
        
        public ICommand PerformSearchCommand { get; set; }
        public ICommand AddBurgerCommandButton { get; set; }
        public ICommand DeleteBurgerCommandButton { get; set; }

        private bool _searchListVisibility;
        public bool SearchListVisibility
        {
            get => _searchListVisibility;
            set
            {
                _searchListVisibility = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(SearchListVisibility)));
            }
        }
        
        private string searchBarText;

        public string SearchBarText
        {
            get=>searchBarText;
            set
            {
                if (searchBarText != value)
                {
                    searchBarText = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(SearchBarText)));
                }
            }
        }

        private BurgerMenu currentTappedBurgerLow;
        public BurgerMenu CurrentTappedBurgerLow
        {
            get => currentTappedBurgerLow;
            set
            {
                currentTappedBurgerLow = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(CurrentTappedBurgerLow)));
            }
        }
        
        private BurgerMenu currentTappedBurgerHigh;
        public BurgerMenu CurrentTappedBurgerHigh
        {
            get => currentTappedBurgerHigh;
            set
            {
                currentTappedBurgerHigh = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(CurrentTappedBurgerHigh)));
            }
        }
        
        public ICommand NavigateToDetailPage { get; set; }

        private readonly BurgerMenu selectedBurgerMenu=null;
        public BurgerMenu SelectedBurgerMenu
        {
            get => selectedBurgerMenu;
            set
            {
                if (value != null)
                {
                    BurgerMenuDataBase.CurrentBurger = value;
                    Application.Current.MainPage.Navigation.PushAsync(new BurgerDetailPage());
                }
                
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(SelectedBurgerMenu)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}