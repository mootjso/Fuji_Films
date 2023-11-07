using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class LoginHandler
{
    private static List<User> users;
    private static int lastUserId;

    public static void LogIn()
    {
        Console.CursorVisible = true;
        LoadUsers();
        bool login = true;
        while (login)
        {
            Console.Write("E-mailadres (type 'back' to return to the menu): ");
            StringBuilder username = new StringBuilder();
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Backspace && username.Length > 0)
                {
                    username.Length -= 1; // Remove last character
                    Console.Write("\b \b"); // Erase character from display
                }
                else if (key.Key != ConsoleKey.Enter)
                {
                    username.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            } while (key.Key != ConsoleKey.Enter);

            string usernameString = username.ToString();

            if (usernameString.ToLower() == "back")
            {
                login = false;
                break;
            }

            Console.Write("\nPassword: ");
            string password = Console.ReadLine();

            bool userLogIn = false;
            bool accountExists = false;

            foreach (var user in users)
            {
                if (user.Email == usernameString)
                {
                    accountExists = true;

                    if (user.Password == password)
                    {
                        userLogIn = true;
                        loggedInUser = user;
                        login = false;
                    }
                }
            }

            if (accountExists)
            {
                if (userLogIn)
                {
                    Console.WriteLine("Login successful!");
                }
                else
                {
                    Console.WriteLine("Incorrect password!");
                }
            }
            else
            {
                Console.WriteLine("Account does not exist!");
            }
        }
    }

    public static void Register()
    {
        Console.CursorVisible = true;
        LoadUsers();
        bool makeAccount = true;
        while (makeAccount)
        {
            Console.Write("First Name (type 'back' to return to the menu): ");
            StringBuilder firstName = new StringBuilder();
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Backspace && firstName.Length > 0)
                {
                    firstName.Length -= 1; // Remove last character
                    Console.Write("\b \b"); // Erase character from display
                }
                else if (key.Key != ConsoleKey.Enter)
                {
                    firstName.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            } while (key.Key != ConsoleKey.Enter);

            string firstNameString = firstName.ToString();

            if (firstNameString.ToLower() == "back")
            {
                makeAccount = false;
                break;
            }

            // Continue with other prompts (Last Name, Phone Number, Email, Password)
            // ...
        }
    }

    public static void LoadUsers()
    {

        string filename = "UserAccounts.json";

        if (File.Exists(filename))
        {
            string json = File.ReadAllText(filename);
            users = JsonConvert.DeserializeObject<List<User>>(json);
            lastUserId = users.Max(u => u.Id);
        }
        else
        {
            users = new List<User>();
            lastUserId = 0;
        }
    }

    public static void SaveUsers()
    {
        string filename = "UserAccounts.json";
        string json = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(filename, json);
    }

    public static User loggedInUser { get; private set; }
}
