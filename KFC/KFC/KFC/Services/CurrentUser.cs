using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using KFC.Models;

namespace KFC.Services
{
    public static class CurrentUser
    {
        public static AccountInfo CurrentAccount { get; set; }
        private const string CurrentUserFileName = "/Users/roandrm/RiderProjects/KFC/current_user_info.json";

        public static async Task UpdateCurrentUserDataFile(AccountInfo newAccountInfo)
        {
            File.Delete(CurrentUserFileName);
            var createStream = File.Create(CurrentUserFileName);
            await JsonSerializer.SerializeAsync(createStream, newAccountInfo);
            createStream.Dispose();
                
        }

        public static async Task SetCurrentUserFromFile()
        {
            if (File.Exists(CurrentUserFileName))
            {
                var readStream = File.OpenRead(CurrentUserFileName);
                CurrentAccount = await JsonSerializer.DeserializeAsync<AccountInfo>(readStream);
                readStream.Dispose();
            }
        }
    }
}