using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logger
{
    internal class adminLogIn
    {
        private static List<Admin> admin;

        public static bool AdminLogger()
        {
            LoadAdminAccount();
            Console.WriteLine("Admin log in!");
            bool adminLogIn = false;
            while (true)
            {
                Console.WriteLine("Admin");

                Console.WriteLine("Password:");
                string password = Console.ReadLine();
                foreach (var account in admin)
                {
                    if (account.Password == password)
                    {
                        Console.WriteLine("Admin login succesvol!");
                        return adminLogIn = true;        
                    }
                    else
                    {
                        Console.WriteLine("Verkeerde Password!");
                    }
                }
            }
        }


        public static void LoadAdminAccount()
        {
            string jsonFilePath = @"C:\Users\mootj\OneDrive\Desktop\Informatica\Projecten B\Project\Tak-Mo\logger\adminAccount.json";

            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                admin = JsonConvert.DeserializeObject<List<Admin>>(json);
            }
            else
            {
                admin = new List<Admin>();
            }
        }
    }
}
