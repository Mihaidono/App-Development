using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KFC
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            var page = Application.Current.MainPage as NavigationPage;
            page.BarBackgroundColor = Color.DarkOrange;
            page.BarTextColor=Color.White;
        }
    }
}