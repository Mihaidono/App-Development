using System;
using Xamarin.Forms;

namespace KFC.Models
{
    public class FlyoutItemPage
    {
        public string Title { get; set; }
        public string IconSource { get; set; }
        public Page TargetPage { get; set; }
        
        public FlyoutItemPage(string title, string iconSource, Page targetPage)
        {
            Title = title;
            IconSource = iconSource;
            TargetPage = targetPage;
        }
    }
}