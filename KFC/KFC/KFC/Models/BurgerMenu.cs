using System;
using System.Threading;
using KFC.Services;

namespace KFC.Models
{
    public class BurgerMenu
    {        
        private double price;
        public string Name { get; set; }
        public double Price
        {
            get => price;
            set
            {
                if (!IsOnSale)
                {
                    price = value;
                }
                else
                {
                    if (OnSalePercent != 0)
                    {
                        price = Price - (OnSalePercent / 100) * Price;
                    }
                    else price = value;
                }
            }
        }
        public bool IsOnSale { get; set; }
        public short OnSalePercent { get; set; }
        public string ImageSource { get; set; }
        public int BuyCount { get; set; }
        
        public int CreatorIndex { get; set; }

        public BurgerMenu(string name,double price, bool isOnSale,short onSalePercent, string imageSource, int buyCount, int creatorIndex )
        {
            Name = name;
            Price = price;
            IsOnSale = isOnSale;
            OnSalePercent = onSalePercent;
            ImageSource = imageSource;
            BuyCount = buyCount;
            CreatorIndex = creatorIndex;
        }

        public string GetPriceDescription
        {
            get
            {
                if (IsOnSale)
                {
                    return $"For only: {Price} lei\n\t-{OnSalePercent}%OFF for limited time\nLiked by more than {BuyCount} persons";
                }
                else
                {
                    return $"For only: {Price} lei\nLiked by more than {BuyCount} persons";
                }
            }
        }

        public string GetCreatorDescription => $"Created by {CurrentUser.CurrentAccount.Username}";
    }
}