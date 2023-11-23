using Newtonsoft.Json;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class LoginHandler
{
    private const string FileName = "UserAccounts.json";
    private static List<User> Users;
    private static int lastUserId;
    public static User? loggedInUser { get; private set; } = null;

    static LoginHandler()
    {
        Users = JSONMethods.ReadJSON<User>(FileName).ToList();
        if (Users.Count >= 1)
            lastUserId = Users.Max(u => u.Id);
        else
            lastUserId = 0;
    }

    public static User? LogIn()
    {
        // Method returns null (if the user enters 'q' at any of the prompts) or a User object if login is successful
        string? emailInput = string.Empty;
        string? passwordInput = string.Empty;
        User? User = null;

        bool loggedIn = false;
        while (!loggedIn)
        {
            Console.Clear();
            DisplayAsciiArt.Header();
            AdHandler.DisplaySnacks();

            Console.WriteLine("Login to your account");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nType 'q' at any of the prompts to go back.");
            Console.ResetColor();

            Console.Write("E-mailadres: ");

            if (emailInput != string.Empty && User != null)
            {
                Console.Write(emailInput);
                Console.WriteLine();
            }
            else
            {
                Console.CursorVisible = true;
                Console.ForegroundColor = Program.InputColor;
                emailInput = Console.ReadLine();
                Console.ResetColor();
                Console.CursorVisible = false;
            }

            if (emailInput == "q")
                return null;

            User = Users.FirstOrDefault(u => u.Email == emailInput);

            if (User == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("E-mailadres does not exist, press any key to continue");
                Console.ResetColor();
                Console.ReadKey();
                continue;
            }
            else if (User != null)
            {
                Console.Write("Password: ");
                string password = GetMaskedPassword();

                if (password == "q")
                    return null;

                if (User.Password == password)
                {
                    loggedIn = true;
                }
                else
                {
                    Console.CursorVisible = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nIncorrect password, press any key to try again");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nLogin successful");
        Console.ResetColor();
        Thread.Sleep(800);

        return User;
    }

    public static User? Register()
    {
        User? newUser = null;
        string? firstName = string.Empty, lastName = string.Empty, phoneNumber = string.Empty, email = string.Empty, password = string.Empty;
        
        bool AccountCreated = false;
        while (!AccountCreated)
        {
            Console.Clear();
            DisplayAsciiArt.Header();
            AdHandler.DisplaySnacks();

            Console.WriteLine("Register new account");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nType 'q' at any of the prompts to go back.");
            Console.ResetColor();

            // First Name
            Console.Write("First Name: ");
            if (ValidateNameInput(firstName))
            {
                Console.ForegroundColor = Program.InputColor;
                Console.Write(firstName + "\n");
                Console.ResetColor();
            }
            else
            {
                Console.CursorVisible = true;
                Console.ForegroundColor = Program.InputColor;
                firstName = Console.ReadLine();
                Console.ResetColor();
                Console.CursorVisible = false;
                if (firstName == "q")
                    return null;
                if (!ValidateNameInput(firstName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nThe name '{firstName}' is invalid, please enter 3 or more letters.");
                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to continue");
                    Console.ReadKey();
                    firstName = string.Empty;
                    continue;
                }
            }
            // Last Name
            Console.Write("Last Name: ");
            if (ValidateNameInput(lastName))
            {
                Console.ForegroundColor = Program.InputColor;
                Console.Write(lastName + "\n");
                Console.ResetColor();
            }
            else
            {
                Console.CursorVisible = true;
                Console.ForegroundColor = Program.InputColor;
                lastName = Console.ReadLine();
                Console.ResetColor();
                Console.CursorVisible = false;
                if (lastName == "q")
                    return null;
                if (!ValidateNameInput(lastName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nThe name '{lastName}' is invalid, please enter 3 or more letters.");
                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to continue");
                    Console.ReadKey();
                    lastName = string.Empty;
                    continue;
                }
            }
            // Phone Number
            Console.Write("Phone Number: ");
            if (ValidatePhoneNumber(phoneNumber))
            {
                Console.ForegroundColor = Program.InputColor;
                Console.Write(phoneNumber + "\n");
                Console.ResetColor();
            }
            else
            {
                Console.CursorVisible = true;
                Console.ForegroundColor = Program.InputColor;
                phoneNumber = Console.ReadLine();
                Console.ResetColor();
                Console.CursorVisible = false;
                if (phoneNumber == "q")
                    return null;
                if (!ValidatePhoneNumber(phoneNumber))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nThe phone number '{phoneNumber}' is invalid, please enter between 8 and 15 digits, press any key to continue");
                    Console.ResetColor();
                    Console.ReadKey();
                    phoneNumber = string.Empty;
                    continue;
                }
            }
            // Email
            Console.Write("Email Address: ");
            if (ValidateEmail(email))
            {
                Console.ForegroundColor = Program.InputColor;
                Console.Write(email + "\n");
                Console.ResetColor();
            }
            else
            {
                Console.CursorVisible = true;
                Console.ForegroundColor = Program.InputColor;
                email = Console.ReadLine();
                Console.ResetColor();
                Console.CursorVisible = false;
                if (email == "q")
                    return null;
                if (!ValidateEmail(email))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n'{email}' is invalid, please enter a valid email e.g. 'example@email.com', press any key to continue");
                    Console.ResetColor();
                    Console.ReadKey();
                    email = string.Empty;
                    continue;
                }
            }
            // Password entry and validation
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Password requirements: \n- Minimum 6 characters\n- 1 Uppercase letter\n- 1 Lowercase letter\n- 1 Digit");
            Console.ResetColor();
            Console.Write("Password: ");
            if (ValidatePassword(password))
            {
                Console.ForegroundColor = Program.InputColor;
                Console.Write(password + "\n");
                Console.ResetColor();
            }
            else
            {
                Console.CursorVisible = true;
                Console.ForegroundColor = Program.InputColor;
                password = GetMaskedPassword();
                Console.ResetColor();
                Console.CursorVisible = false;
                if (password == "q")
                    return null;
                if (!ValidatePassword(password))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nThe password is invalid, press any key to continue");
                    Console.ResetColor();
                    Console.ReadKey();
                    password = string.Empty;
                    continue;
                }
                else
                {
                    Console.Write("Confirm password: ");
                    Console.CursorVisible = true;
                    Console.ForegroundColor = Program.InputColor;
                    string password2 = GetMaskedPassword();
                    Console.ResetColor();
                    Console.CursorVisible = false;
                    if (password2 == "q")
                        return null;
                    if (!(password == password2))
                    {
                        password = string.Empty;
                        password2 = string.Empty;
                        Console.ForegroundColor= ConsoleColor.Red;
                        Console.WriteLine("\nThe passwords do not match, press any key to try again.");
                        Console.ResetColor();
                        Console.ReadKey();
                        continue;
                    } 
                }
            }

            newUser = new User(++lastUserId, firstName!, lastName!, email!, password, phoneNumber!);
            Users.Add(newUser);
            JSONMethods.WriteToJSON(Users, FileName);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nAccount {email}, has been created, press any key to continue");
            Console.ResetColor();
            Console.ReadKey();
            AccountCreated = true;
        }

        return newUser;
    }

    public static bool ValidateNameInput(string? input)
    {
        if (input == null) 
            return false;
        
        string pattern = @"^[a-zA-Z]{3,}$";
        Regex regex = new Regex(pattern);

        if (regex.IsMatch(input))
            return true;

        return false;
    }

    public static bool ValidatePassword(string? password)
    {
        bool HasUpper = false, HasLower = false, HasDigits = false;

        if (password == null) 
            return false;
        
        if (password.Length < 6 || password.Length > 256)
            return false;

        foreach (char letter in password)
            if (char.IsUpper(letter))
                HasUpper = true;

        foreach (char letter in password)
            if (char.IsLower(letter))
                HasLower = true;

        foreach (char letter in password)
            if (char.IsDigit(letter))
                HasDigits = true;

        if (!HasUpper || !HasLower || !HasDigits)
            return false;

        return true;
    }

    public static bool ValidatePhoneNumber(string? phoneNumber)
    {
        if (phoneNumber == null)
            return false;
        
        return Regex.IsMatch(phoneNumber, @"^\d{8,15}$");
    }

    public static bool ValidateEmail(string? email)
    {
        if (email == null)
            return false;

        // [characters] '@' [characters] '.' [characters]
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(email);
    }

    private static string GetMaskedPassword()
    {
        Console.CursorVisible = true;
        Console.ForegroundColor = Program.InputColor;

        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (
                char.IsLetterOrDigit(key.KeyChar)
                || char.IsSymbol(key.KeyChar)
                || char.IsPunctuation(key.KeyChar)
            )
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.ResetColor();
        Console.CursorVisible = false;
        Console.WriteLine();
        return password;
    }
}
