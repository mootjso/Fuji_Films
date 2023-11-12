public class PasswordValidator
{
    public bool IsValid(string password)
    {
        if (password.Length < 6 || password.Length > 256)
            return false;

        if (!HasUppercase(password) || !HasLowercase(password) || !HasDigits(password))
            return false;

        return true;
    }

    private bool HasUppercase(string password)
    {
        foreach (char letter in password)
            if (char.IsUpper(letter)) return true;

        return false;
    }

    private bool HasLowercase(string password)
    {
        foreach (char letter in password)
            if (char.IsLower(letter)) return true;

        return false;
    }

    private bool HasDigits(string password)
    {
        foreach (char letter in password)
            if (char.IsDigit(letter)) return true;

        return false;
    }

}
