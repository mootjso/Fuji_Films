using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace logger
{
    internal class logIn
    {
        private static List<Account> accounts;

        public static void Start()
        {
            LoadAccount();
            Console.WriteLine("Welkom bij Ships Cinema!\n1. Log in\n2. Maak een account\n");
            string accountChoice = Console.ReadLine();
            switch (accountChoice)
            {
                case "1":
                    bool login = true;
                    while (login)
                    {
                        Console.WriteLine("Gebruikersnaam:");
                        string username = Console.ReadLine();

                        Console.WriteLine("Password:");
                        string password = Console.ReadLine();

                        bool accountExists = false;
                        bool passwordCorrect = false;

                        foreach (var account in accounts)
                        {
                            if (account.Username == username)
                            {
                                accountExists = true;

                                if (account.Password == password)
                                {
                                    passwordCorrect = true;
                                    login = false;
                                }
                            }
                        }

                        if (accountExists)
                        {
                            if (passwordCorrect)
                            {
                                Console.WriteLine("Login succesvol!");
                            }
                            else
                            {
                                Console.WriteLine("Verkeerde Password!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Account bestaat niet!");
                        }
                    }
                    break;
                case "2":
                    bool makeAccount = true;
                    while (makeAccount)
                    {
                        Console.WriteLine("Gebruikersnaam:");
                        string username = Console.ReadLine();

                        bool usernameExists = accounts.Any(account => account.Username == username);

                        if (usernameExists)
                        {
                            Console.WriteLine("Account met dezelfde gebruikersnaam bestaat al!");
                        }
                        else
                        {
                            Console.WriteLine("Password:");
                            string password = Console.ReadLine();

                            Console.WriteLine("E-mail:");
                            string email = Console.ReadLine();

                            Console.WriteLine("Phone Number:");
                            string number = Console.ReadLine();

                            var newAccount = new Account(username, password, email, number);
                            accounts.Add(newAccount);
                            Console.WriteLine($"Account {username}, is gemaakt!");

                            SaveAccounts();
                            makeAccount = false;
                        }
                    }
                    break;
                case "admin login":
                    bool AdminLogInSucess = adminLogIn.AdminLogger();
                    break;
                default:
                    Console.WriteLine("Error 404\nKies een valide optie!");
                    break;
            }
        }


        public static void LoadAccount()
        {
            string jsonFilePath = @"C:\Users\mootj\OneDrive\Desktop\Informatica\Projecten B\Project\Tak-Mo\logger\accounts.json";

            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                accounts = JsonConvert.DeserializeObject<List<Account>>(json);
            }
            else
            {
                accounts = new List<Account>();
            }
        }


        public static void SaveAccounts()
        {
            string jsonFilePath = @"C:\Users\mootj\OneDrive\Desktop\Informatica\Projecten B\Project\Tak-Mo\logger\accounts.json";
            // Ik moest newtonsoft.json gebruiken ik weet niet precies wrm
            string json = JsonConvert.SerializeObject(accounts, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(jsonFilePath, json);
        }
    }

}
