using System.Text.RegularExpressions;

public static class ValidateCreditCardNumber
{
    public static bool IsValid(string creditCardNumber)
    {
        string pattern = @"^\d{4}[-/]\d{4}[-/]\d{4}[-/]\d{4}$";
        Regex regex = new Regex(pattern);

        return regex.IsMatch(creditCardNumber);
    }
}