using Newtonsoft.Json;
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
            Console.Write("E-mailadres: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            bool userLogIn = false;
            bool accountExists = false;

            foreach (var user in users)
            {
                if (user.Email == username)
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
            Console.Write("First Name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last Name: ");
            string lastName = Console.ReadLine();

            Console.Write("Phone Number: ");
            string phoneNumber = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();


            var newUser = new User(++lastUserId, firstName, lastName, email, password, phoneNumber);
            users.Add(newUser);
            loggedInUser = newUser;
            SaveUsers();

            Console.WriteLine($"Account {email}, has been created!");
            makeAccount = false;
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