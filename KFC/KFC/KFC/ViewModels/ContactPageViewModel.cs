using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Windows.Input;
using KFC.Models;
using KFC.Services;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace KFC.ViewModels
{
    public class ContactPageViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<SupportTicket> AllTickets { get; set; }

        public ContactPageViewModel()
        {

            AllTickets = new ObservableCollection<SupportTicket>(
                    TicketsDataBase.ListOfTickets[CurrentUser.CurrentAccount.AccountId]);

            SendTicketCommand = new Command(async () =>
            {
                if (EntryText != null)
                {
                    var newTicket = new SupportTicket(EntryText);
                    AllTickets.Add(newTicket);
                    await TicketsDataBase.AddTicket(newTicket);
                    await Application.Current.MainPage.DisplayAlert("Sending ticket", 
                        "Successfully sent the ticket", "Nice");
                    EntryText = "";
                }
            });

            EraseTicketCommand = new Command(() =>
            {
                EntryText = string.Empty;
            });

            DeleteCurrentTicketCommand = new Command(async () =>
            {
                if (CurrentItem != null)
                {
                    AllTickets.Remove(CurrentItem);
                    await TicketsDataBase.DeleteTicket(CurrentItem);
                    await Application.Current.MainPage.DisplayAlert("Deleting ticket", 
                        "Successfully deleted the ticket", "Nice");
                }
            });
        }

        public SupportTicket CurrentItem { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private string entryText;
        public string EntryText
        {
            get => entryText;
            set
            {
                entryText = value;
                
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EntryText)));

            }
        }
        
        public ICommand SendTicketCommand { get; set; }
        public ICommand EraseTicketCommand { get; set; }
        public ICommand DeleteCurrentTicketCommand { get; set; }
    }
}