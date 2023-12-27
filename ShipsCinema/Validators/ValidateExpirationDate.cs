public static class ValidateExpirationDate
{
    public static (bool validFormat, bool isExpired) IsExpirationDateValid(string expirationDate)
    {
        string[] dateParts;

        if (expirationDate.Contains("-"))
            dateParts = expirationDate.Split('-');
        else if (expirationDate.Contains("/"))
            dateParts = expirationDate.Split('/');
        else
            return (false, false);

        if (dateParts.Length == 2 && int.TryParse(dateParts[0], out int month) && int.TryParse(dateParts[1], out int year))
        {
            DateTime parsedExpirationDate = new DateTime(2000 + year, month, 1).AddMonths(1).AddDays(-1);
            DateTime currentDate = DateTime.Now;

            if (parsedExpirationDate > currentDate) // Correct format but expired
            {
                return (true, false);
            }
            else // Correct format, not expired
            {
                return (true, true);
            }
        }

        return (false, false);
    }
}