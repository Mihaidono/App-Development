using System;
using System.Text.Json.Serialization;

namespace KFC.Models
{
    public class AccountInfo
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string AccountMail { get; set; }
        public User AccountUser { get; set; }
        public bool Permission { get; set; }
        public int AccountId { get; set; }
        
        [JsonConstructor] public AccountInfo(int accountId,string username,string password,string accountMail,User accountUser,bool permission)
        {
            AccountId = accountId;
            Username = username;
            Password = password;
            AccountMail = accountMail;
            AccountUser = accountUser;
            Permission = permission;
        }
    }
}