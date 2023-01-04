using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using KFC.Models;

namespace KFC.Services
{
    public static class TicketsDataBase
    {
        public static Dictionary<int,List<SupportTicket>> ListOfTickets { get; set; }
        private const string TicketFileName = "/Users/roandrm/RiderProjects/KFC/list_of_tickets.json";
        
        public static async Task InitializeTicketList()
        {
            ListOfTickets = await GetAllTickets();
        }
        
        private static async Task<Dictionary<int ,List<SupportTicket>>> GetAllTickets()
        {
            Dictionary<int, List<SupportTicket>> auxList;
            FileStream openStream=null;
            try
            {
                openStream = File.OpenRead(TicketFileName);
            }
            catch (FileNotFoundException foundException)
            {
                Console.WriteLine(foundException);
            }
            if (openStream!=null)
                auxList= await JsonSerializer.DeserializeAsync<Dictionary<int ,List<SupportTicket>>>(openStream); 
            else auxList=new Dictionary<int ,List<SupportTicket>>(0);
            
            openStream?.Dispose();

            if (auxList != null && !auxList.ContainsKey(CurrentUser.CurrentAccount.AccountId))
            {
                auxList.Add(CurrentUser.CurrentAccount.AccountId,new List<SupportTicket>(0));
            }
            
            return auxList;
        }
        public static async Task AddTicket(SupportTicket newTicket)
        {
            File.Delete(TicketFileName);
            
            if (ListOfTickets.ContainsKey(CurrentUser.CurrentAccount.AccountId))
            {
                ListOfTickets[CurrentUser.CurrentAccount.AccountId].Add(newTicket);
            }
            else
            {
                var auxTicketList = new List<SupportTicket> { newTicket };
                ListOfTickets.Add(CurrentUser.CurrentAccount.AccountId,auxTicketList);
            }
            
            var writeStream = File.Create(TicketFileName);
            await JsonSerializer.SerializeAsync(writeStream, ListOfTickets);
            writeStream.Close();

        }

        public static async Task DeleteTicket(SupportTicket oldTicket)
        {
            File.Delete(TicketFileName);
            if (ListOfTickets != null && ListOfTickets.ContainsKey(CurrentUser.CurrentAccount.AccountId))
            { 
                ListOfTickets[CurrentUser.CurrentAccount.AccountId].Remove(oldTicket);
            }
            var writeStream = File.OpenWrite(TicketFileName);
            await JsonSerializer.SerializeAsync(writeStream, ListOfTickets);
            writeStream.Close();
            
        }
    }
}