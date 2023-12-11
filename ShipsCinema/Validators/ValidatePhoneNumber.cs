using System.Text.RegularExpressions;

public static class ValidatePhoneNumber
{
    public static bool IsValid(string phoneNumber)
    {
        if (phoneNumber == null)
            return false;

        return Regex.IsMatch(phoneNumber, @"^\d{8,15}$");
    }
}
