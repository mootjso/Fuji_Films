using System.Text.RegularExpressions;

public static class ValidateNameInput
{
    public static bool IsValid(string input)
    {
        if (input == null)
            return false;

        string pattern = @"^[a-zA-Z]{3,}$";
        Regex regex = new Regex(pattern);

        return regex.IsMatch(input);
    }
}
