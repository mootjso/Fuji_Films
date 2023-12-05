using System.Text.RegularExpressions;

public static class ValidateEmail
{
    public static bool IsValid(string email)
    {
        if (email == null)
            return false;

        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(pattern);

        return regex.IsMatch(email);
    }
}
