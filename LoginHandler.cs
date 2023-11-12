using Newtonsoft.Json;
public class LoginHandler
{
    private static List<User> users;
    private static int lastUserId;
    public static User loggedInUser { get; private set; }

    public static User LogIn()
    {
        Console.CursorVisible = true;
        LoadUsers();
        bool login = true;

        while (login)
        {
            Console.CursorVisible = true;
            Console.Clear();

            DisplayAsciiArt.Header();
            AdHandler.DisplaySnacks();

            Console.WriteLine("Login to your account\n");

            Console.Write("E-mailadres: ");
            string inputted_email = Console.ReadLine();

            User user = users.FirstOrDefault(u => u.Email == inputted_email);

            if (user != null)
            {
                bool correctPassword = false;

                do
                {
                    Console.Write("Password: ");
                    string password = GetMaskedPassword();

                    if (user.Password == password)
                    {
                        loggedInUser = user;
                        correctPassword = true;
                        login = false;
                    }
                    else
                    {
                        Console.CursorVisible = false;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Incorrect password, press any key to try again");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                        DisplayAsciiArt.Header();
                        AdHandler.DisplaySnacks();
                        Console.WriteLine("Login to your account\n");
                        Console.Write("E-mailadres: ");
                        Console.WriteLine(inputted_email);
                    }
                } while (!correctPassword);
            }
            else
            {
                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("E-mailadres does not exist, press any key to continue");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        Console.CursorVisible = false;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Login successful, press any key to continue");
        Console.ResetColor();
        Console.ReadKey();

        return loggedInUser;
    }

    public static User Register()
    {
        LoadUsers();
        bool makeAccount = true;
        User newUser = null;

        while (makeAccount)
        {
            Console.Clear();
            DisplayAsciiArt.Header();
            AdHandler.DisplaySnacks();

            Console.WriteLine("Register new account\n");

            Console.CursorVisible = true;
            Console.Write("First Name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last Name: ");
            string lastName = Console.ReadLine();

            Console.Write("Phone Number: ");
            string phoneNumber;
            bool validPhoneNumber;
            PhoneNumberValidator phoneNumberValidator = new PhoneNumberValidator();

            do
            {
                Console.Clear();
                DisplayAsciiArt.Header();
                AdHandler.DisplaySnacks();

                Console.WriteLine("Register new account\n");
                Console.WriteLine($"First Name: {firstName}");
                Console.WriteLine($"Last Name: {lastName}");

                Console.Write("Phone Number: ");
                phoneNumber = Console.ReadLine();
                validPhoneNumber = phoneNumberValidator.IsValid(phoneNumber);

                if (!validPhoneNumber)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid phone number, please enter a valid number with 8 to 15 digits, press any key to continue");
                    Console.CursorVisible = false;
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.CursorVisible = true;
                }
            } while (!validPhoneNumber);

            string email;
            bool validEmail;
            EmailValidator emailValidator = new EmailValidator();

            do
            {
                Console.Clear();
                DisplayAsciiArt.Header();
                AdHandler.DisplaySnacks();

                Console.WriteLine("Register new account\n");
                Console.WriteLine($"First Name: {firstName}");
                Console.WriteLine($"Last Name: {lastName}");
                Console.WriteLine($"Phone Number: {phoneNumber}");

                Console.Write("Email: ");
                email = Console.ReadLine();
                validEmail = emailValidator.IsValid(email);

                if (!validEmail)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid email, please enter a valid email address with '@', press any key to continue");
                    Console.CursorVisible = false;
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.CursorVisible = true;
                }

            } while (!validEmail);

            string password;
            bool validPassword;
            PasswordValidator passwordValidator = new PasswordValidator();

            do
            {
                Console.Clear();
                DisplayAsciiArt.Header();
                AdHandler.DisplaySnacks();

                Console.WriteLine("Register new account\n");
                Console.WriteLine($"First Name: {firstName}");
                Console.WriteLine($"Last Name: {lastName}");
                Console.WriteLine($"Phone Number: {phoneNumber}");
                Console.WriteLine($"Email: {email}");

                Console.WriteLine("\nPassword requirements: \n-Minimum 6 characters\n-1 Uppercase letter\n-1 Lowercase letter\n-1 Digit");
                Console.Write("\nPassword: ");
                password = GetMaskedPassword();
                validPassword = passwordValidator.IsValid(password);

                if (!validPassword)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Password does not meet the requirements, press any key to try again");
                    Console.CursorVisible = false;
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.CursorVisible = true;
                }
            } while (!validPassword);

            string confirmPassword;
            do
            {
                Console.Write("Confirm Password: ");
                confirmPassword = GetMaskedPassword();

                if (confirmPassword != password)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Passwords do not match, press any key to try again");
                    Console.CursorVisible = false;
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.CursorVisible = true;
                }

            } while (confirmPassword != password);

            newUser = new User(++lastUserId, firstName, lastName, email, password, phoneNumber, false);
            users.Add(newUser);
            loggedInUser = newUser;
            SaveUsers();

            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Account {email}, has been created, press any key to continue");
            Console.ResetColor();
            Console.ReadKey(true);
            makeAccount = false;
        }
        return newUser;
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
        string json = JsonConvert.SerializeObject(users, Formatting.Indented);
        File.WriteAllText(filename, json);
    }

    private static string GetMaskedPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (char.IsLetterOrDigit(key.KeyChar) || char.IsSymbol(key.KeyChar) || char.IsPunctuation(key.KeyChar))
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

        Console.WriteLine();
        return password;
    }
}
