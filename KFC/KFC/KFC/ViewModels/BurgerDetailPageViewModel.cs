using System.ComponentModel;
using System.Windows.Input;
using KFC.Services;
using Xamarin.Forms;

namespace KFC.ViewModels
{
    public class BurgerDetailPageViewModel : INotifyPropertyChanged
    {
        public BurgerDetailPageViewModel()
        {
            GetBurgerProperties();
            CheckBurgerVisibility();

            BuyBurgerButtonCommand = new Command(async () =>
            {
                BurgerMenuDataBase.CurrentBurger.BuyCount++;
                await BurgerMenuDataBase.UpdateBurgerMenu(BurgerMenuDataBase.CurrentBurger);
                BurgerDisplayString = BurgerMenuDataBase.CurrentBurger.GetPriceDescription;
                await BurgerMenuDataBase.InitializeBurgerMenuList();

                await Application.Current.MainPage.DisplayAlert("Purchase", "Burger has been purchased. Enjoy!", "Ok");

            });
        }

        public ICommand BuyBurgerButtonCommand { get; set; }

        private bool burgerVisibility;
        private string burgerName;
        private double burgerPrice;
        private short burgerSalePercent;
        private string burgerImageSource;
        private string burgerDisplayString;

        private void CheckBurgerVisibility()
        {
            BurgerVisibility = BurgerMenuDataBase.CurrentBurger.IsOnSale;
        }
        
        public bool BurgerVisibility
        {
            get => burgerVisibility;
            set
            {
                burgerVisibility = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(BurgerVisibility)));
            }
        }
        
        public string BurgerImageSource
        {
            get => burgerImageSource;
            set
            {
                burgerImageSource = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(BurgerImageSource)));
            }
        }
        public string BurgerName
        {
            get => burgerName;
            set
            {
                burgerName = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(BurgerName)));
            }
        }
        public double BurgerPrice
        {
            get => burgerPrice;
            set
            {
                burgerPrice = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(BurgerPrice)));
            }
        }
        public short BurgerSalePercent
        {
            get => burgerSalePercent;
            set
            {
                burgerSalePercent = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(BurgerSalePercent)));
            }
        }
        public string BurgerDisplayString
        {
            get => burgerDisplayString;
            set
            {
                burgerDisplayString = value;
                
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(BurgerDisplayString)));
            }
        }
        
        private void GetBurgerProperties()
        {
            BurgerName = BurgerMenuDataBase.CurrentBurger.Name;
            BurgerPrice = BurgerMenuDataBase.CurrentBurger.Price;
            BurgerSalePercent = BurgerMenuDataBase.CurrentBurger.OnSalePercent;
            BurgerImageSource = BurgerMenuDataBase.CurrentBurger.ImageSource;
            BurgerDisplayString = BurgerMenuDataBase.CurrentBurger.GetPriceDescription;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}