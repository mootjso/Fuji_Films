public class PhoneNumberValidator
{
    public bool IsValid(string phoneNumber)
    {
        // Check if the phone number contains only digits and has a length between 8 and 15.
        return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\d{8,15}$");
    }
}