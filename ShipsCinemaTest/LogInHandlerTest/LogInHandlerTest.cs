namespace LogInHandlerTest;

[TestClass]
public class TestLoginHandler
{
    private static string FileName = LoginHandler.FileName;
    private static List<User> original_FileName = JSONMethods.ReadJSON<User>(FileName).ToList();
    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        // Clear the test JSON file
        File.WriteAllText(FileName, "[]");

        List<User> testUsers = new List<User>
        {
            new User(1, "John", "Doe", "john.doe@outlook.com", "password123", "0612345678"),
            new User(2, "Barld", "Boot", "barld.boot@gmail.com", "securepass", "0612345679", true)
        };
        JSONMethods.WriteToJSON(testUsers, FileName);

    }

    [ClassCleanup]
    public static void CleanupJSONFile()
    {
       JSONMethods.WriteToJSON(original_FileName, FileName); 
    }

    [TestMethod]
    public void TestReadFromJSON()
    {
        User user = LoginHandler.Users[0];
        User user_admin = LoginHandler.Users[1];

        Assert.AreEqual(1, user.Id);
        Assert.AreEqual("John", user.FirstName);
        Assert.AreEqual("Doe", user.LastName);
        Assert.AreEqual("john.doe@email.com", user.Email);
        Assert.AreEqual("password123", user.Password);
        Assert.AreEqual("0612345678", user.PhoneNumber);

        Assert.IsFalse(user.IsAdmin);

        Assert.IsTrue(user_admin.IsAdmin);
    }

    [TestMethod]
    public void TestValidateNameInput()
    {
        bool result = LoginHandler.ValidateNameInput("John");

        bool wrongResult = LoginHandler.ValidateNameInput("Po");
        bool nullResult = LoginHandler.ValidateNameInput(null);
        bool resultNumber = LoginHandler.ValidateNameInput("John5");

        Assert.IsTrue(result);

        Assert.IsFalse(wrongResult);
        Assert.IsFalse(nullResult);
        Assert.IsFalse(resultNumber);
    }

    [TestMethod]
    public void TestValidatePassword()
    {
        bool lowercaseMissing = LoginHandler.ValidatePassword("PASSWORD123");
        bool digitMissing = LoginHandler.ValidatePassword("PASSWORDS");
        bool UppercaseMissing = LoginHandler.ValidatePassword("password123");
        bool resultNull = LoginHandler.ValidatePassword(null);


        bool resultLong = LoginHandler.ValidatePassword("PASSwooooooooooooooooooooooooooooooooooord123");
        bool resultNormal = LoginHandler.ValidatePassword("RightPassWord123");

        Assert.IsFalse(lowercaseMissing);
        Assert.IsFalse(digitMissing);
        Assert.IsFalse(UppercaseMissing);
        Assert.IsNotNull(resultNull);

        Assert.IsTrue(resultLong);
        Assert.IsTrue(resultNormal);
    }

    [TestMethod]
    public void TestValidatePhoneNumber()
    {
        bool resultShort = LoginHandler.ValidatePhoneNumber("31063");
        bool resultLong = LoginHandler.ValidatePhoneNumber("31063432693733101");
        bool resultLetters = LoginHandler.ValidatePhoneNumber("31063ds21");
        bool resultNegative = LoginHandler.ValidatePhoneNumber("-067128249");
        bool resultSymbol = LoginHandler.ValidatePhoneNumber("+067128249");
        bool resultNull = LoginHandler.ValidatePhoneNumber(null);

        bool resultGood = LoginHandler.ValidatePhoneNumber("067128249");
        bool resultGood2 = LoginHandler.ValidatePhoneNumber("067128249142649");

        Assert.IsFalse(resultLong);
        Assert.IsFalse(resultShort);
        Assert.IsFalse(resultLetters);       
        Assert.IsFalse(resultNegative); 
        Assert.IsFalse(resultSymbol);
        Assert.IsFalse(resultNull);

        Assert.IsTrue(resultGood);
        Assert.IsTrue(resultGood2);
    }

    [TestMethod]
    public void TestValidateEmail()
    {
        bool missingAt = LoginHandler.ValidateEmail("Johnoutlook.com");
        bool resultNull = LoginHandler.ValidateEmail(null);
        bool missingDomain = LoginHandler.ValidateEmail("John@.com");
        bool missingDot = LoginHandler.ValidateEmail("John@outlookcom");
        bool missingTopDomain = LoginHandler.ValidateEmail("John@outlook.");
        bool missingUsername = LoginHandler.ValidateEmail("@outlook.com");

        bool onlyNumbers = LoginHandler.ValidateEmail("3983748@outlook.com");
        bool resultAllowed = LoginHandler.ValidateEmail("John@outlook.com");
        bool resultAllowed2 = LoginHandler.ValidateEmail("johndoe@gmail.com");

        Assert.IsFalse(missingAt);
        Assert.IsFalse(resultNull);
        Assert.IsFalse(missingDomain);
        Assert.IsFalse(missingDot);
        Assert.IsFalse(missingTopDomain);
        Assert.IsFalse(missingUsername);

        Assert.IsTrue(onlyNumbers);
        Assert.IsTrue(resultAllowed);
        Assert.IsTrue(resultAllowed2);
    }
}