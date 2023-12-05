public static class ValidatePassword
{
    public static bool IsValid(string password)
    {
        bool HasUpper = false, HasLower = false, HasDigits = false;

        if (password == null)
            return false;

        if (password.Length < 6 || password.Length > 256)
            return false;

        foreach (char letter in password)
        {
            if (char.IsUpper(letter))
                HasUpper = true;
            else if (char.IsLower(letter))
                HasLower = true;
            else if (char.IsDigit(letter))
                HasDigits = true;
        }

        return HasUpper && HasLower && HasDigits;
    }
}
