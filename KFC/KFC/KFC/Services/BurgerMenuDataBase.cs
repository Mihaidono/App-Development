using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using KFC.Models;

namespace KFC.Services
{
    public static class BurgerMenuDataBase
    {
        public static List<BurgerMenu> ListOfBurgerMenus { get; set; }
        public static List<BurgerMenu> UserBurgerMenus { get; set; }
        public static BurgerMenu CurrentBurger { get; set; }
        
        private const string BurgerMenuFileName = "/Users/roandrm/RiderProjects/KFC/list_of_burger_menus.json";

        public static async Task InitializeBurgerMenuList()
        {
            //await initBurgers();
            ListOfBurgerMenus = await GetBurgerMenuList();
            UserBurgerMenus = ListOfBurgerMenus
                .Where(burger => burger.CreatorIndex == CurrentUser.CurrentAccount.AccountId).ToList();
        }

        private static async Task<List<BurgerMenu>> GetBurgerMenuList()
        {
            List<BurgerMenu> auxList;
            FileStream openStream = null;
            try
            {
                openStream = File.OpenRead(BurgerMenuFileName);
            }
            catch (FileNotFoundException foundException)
            {
                Console.WriteLine(foundException);
            }

            if (openStream != null)
                auxList = await JsonSerializer.DeserializeAsync<List<BurgerMenu>>(openStream);
            else auxList = new List<BurgerMenu>(0);

            openStream?.Dispose();

            return auxList;
        }

        public static async Task AddBurgerMenu(BurgerMenu newBurgerMenu)
        {
            File.Delete(BurgerMenuFileName);
            ListOfBurgerMenus.Add(newBurgerMenu);

            var createStream = File.Create(BurgerMenuFileName);
            await JsonSerializer.SerializeAsync(createStream, ListOfBurgerMenus);

            createStream.Dispose();
        }

        public static async Task DeleteBurgerMenu(BurgerMenu oldBurgerMenu)
        {
            File.Delete(BurgerMenuFileName);
            ListOfBurgerMenus.Remove(oldBurgerMenu);

            var writeStream = File.OpenWrite(BurgerMenuFileName);
            await JsonSerializer.SerializeAsync(writeStream, ListOfBurgerMenus);
            writeStream.Dispose();
        }

        public static async Task UpdateBurgerMenu(BurgerMenu updatedBurgerMenu)
        {
            File.Delete(BurgerMenuFileName);
            var index = ListOfBurgerMenus.IndexOf(
                ListOfBurgerMenus.FirstOrDefault(burger => burger.Name == updatedBurgerMenu.Name));
            ListOfBurgerMenus[index] = updatedBurgerMenu;

            var writeStream = File.OpenWrite(BurgerMenuFileName);
            await JsonSerializer.SerializeAsync(writeStream, ListOfBurgerMenus);
            writeStream.Dispose();
        }

        /*private static async Task initBurgers()
        {
            var burgList = new List<BurgerMenu>
            {
                new BurgerMenu("MecChicken", 6.5, false, 0, "burg1", 0, 4),
                new BurgerMenu("McMeal", 4, true, 10, "burg2", 0, 4),
                new BurgerMenu("KFCBurga", 6, false, 0, "hamburger", 0, 4),
                new BurgerMenu("Sandvis", 3, false, 0, "burg2", 0, 4),
                new BurgerMenu("MayoMuch", 2.5, true, 34, "burg2", 0, 4),
                new BurgerMenu("Booster", 9, false, 0, "burg2", 0, 4),
                new BurgerMenu("Donzel", 12, false, 0, "burg1", 0, 4),
                new BurgerMenu("Caracal", 13, false, 0, "burg2", 0, 4),
                new BurgerMenu("TarguOcna", 5, true, 45, "burg1", 0, 4),
                new BurgerMenu("DinPloiesti", 6.8, false, 0, "burg2", 0, 4),
                new BurgerMenu("Dalas", 8.99, false, 0, "burg1", 0, 4),
                new BurgerMenu("Bigboi", 3, true, 33, "burg1", 0, 4),
                new BurgerMenu("Fatso", 3, false, 0, "burg2", 0, 4),
            };

            var writeStream = File.OpenWrite(BurgerMenuFileName);
            await JsonSerializer.SerializeAsync(writeStream, burgList);
            writeStream.Dispose();
        }*/
    }
}