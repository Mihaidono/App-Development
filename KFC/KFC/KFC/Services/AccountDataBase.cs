using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using KFC.Models;

namespace KFC.Services
{
    public class AccountDataBase
    { 
        public static List<AccountInfo> ListOfAccounts { get; set; }
        private const string AccountFileName = "/Users/roandrm/RiderProjects/KFC/list_of_accounts.json";
        public static async Task InitializeAccountList()
        {
            ListOfAccounts = await GetAccountList();
        }
        
        private static async Task<List<AccountInfo>> GetAccountList()
        {
            List<AccountInfo> auxList;
            FileStream openStream=null;
            try
            {
                openStream = File.OpenRead(AccountFileName);
            }
            catch (FileNotFoundException foundException)
            {
                Console.WriteLine(foundException);
            }
            if (openStream!=null)
                auxList= await JsonSerializer.DeserializeAsync<List<AccountInfo>>(openStream); 
            else auxList=new List<AccountInfo>(0);
            
            openStream?.Dispose();
            
            return auxList;
        }

        public static async Task AddAccount(AccountInfo newAccount)
        {
            File.Delete(AccountFileName);
            ListOfAccounts.Add(newAccount);

            var createStream = File.Create(AccountFileName);
            await JsonSerializer.SerializeAsync(createStream, ListOfAccounts);
            
            createStream.Dispose();
        }

        public static async Task DeleteAccount(AccountInfo oldAccount)
        {
            File.Delete(AccountFileName);
            ListOfAccounts.Remove(oldAccount);

            var writeStream = File.OpenWrite(AccountFileName);
            await JsonSerializer.SerializeAsync(writeStream, ListOfAccounts);
            writeStream.Dispose();
        }

        public static async Task UpdateAccount(AccountInfo updatedAccount)
        {
            File.Delete(AccountFileName);
            var index = ListOfAccounts.IndexOf(CurrentUser.CurrentAccount);
            if (index != -1)
            {
                ListOfAccounts[index] = updatedAccount;
            }
            
            var writeStream = File.OpenWrite(AccountFileName);
            await JsonSerializer.SerializeAsync(writeStream, ListOfAccounts);
            writeStream.Dispose();
        }
    }
}