public class UserAccountsHandler
{
    public const int MainAdminId = 1;

    // Extra check added in this method to ensure the rights of the main admin do not get revoked
    public static void ChangeUserAdminRights(User user) => user.IsAdmin = user.Id == MainAdminId ? true : !user.IsAdmin;

    public static User? SelectUserFromList(bool alsoMainAdmin = false)
    {
        string header = "  Rights".PadRight(8) + " | " + "First Name".PadRight(15) + " | " + "Last Name".PadRight(15) + " | " + "Email".PadRight(22) + " | " + "Phonenumber"
                + "\n--------------------------------------------------------------------------------------";
        List<User> userObjects = userObjects = LoginHandler.Users;
        if (!alsoMainAdmin)
        {
            userObjects = LoginHandler.Users
            .Where(u => u.Id != MainAdminId) // Skip the Main Admin account
            .ToList();
        }

        List<string> userStrings = new();
        userObjects = userObjects
                .OrderByDescending(u => u.IsAdmin)
                .ThenBy(u => u.Id)
                .ToList();
        userStrings = userObjects.Select(u => u.ToString()).ToList();
        userStrings.Add("Back");

        Console.Clear();
        DisplayAsciiArt.AdminHeader();
        int index = Menu.Start($"Set Admin Rights\n\nSelect a user to change the Admin rights:\n\n{header}", userStrings, true);
        if (index == userStrings.Count || index == userStrings.Count - 1)
            return null;

        return userObjects[index];
    }
}
