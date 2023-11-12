public class EmailValidator
{
    public bool IsValid(string email)
    {
        if (email.Length < 4 || !email.Contains("@"))
            return false;
        return true;
    }
}