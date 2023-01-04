using System;

namespace KFC.Models
{
    public class SupportTicket
    {
        public string TicketText { get; set; }
        public bool SolvedStatus { get; set; }
        
        public DateTime PostTime { get; set; }

        public SupportTicket(string ticketText)
        {
            TicketText = ticketText;
            SolvedStatus = false;
            PostTime = DateTime.Now;
        }
    }
}