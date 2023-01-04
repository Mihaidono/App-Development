using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KFC
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactPage : ContentPage
    {
        public ContactPage()
        {
            InitializeComponent();
            NavigationPage page = Application.Current.MainPage as NavigationPage;
            page.BarBackgroundColor = Color.DarkOrange;
            page.BarTextColor=Color.White;
        }
    }
}