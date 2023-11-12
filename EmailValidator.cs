public class EmailValidator
{
    public bool IsValid(string email)
    {
        // Check if the email contains "@" symbol.
        return email.Contains("@");
    }
}