namespace LogInHandlerTest;

[TestClass]
public class UnitTest1
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
            new User(1, "John", "Doe", "john.doe@email.com", "password123", "0612345678"),
            new User(2, "Barld", "Boot", "barld.boot@email.com", "securepass", "0612345679", true),
            new User(3, "Bob", "Johnson", "bob.johnson@email.com", "pass123", "0612545678")
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
        bool wrong_result = LoginHandler.ValidateNameInput("Po");
        Assert.IsTrue(result);
        Assert.IsFalse(wrong_result);
    }

    [TestMethod]
    public void TestValidatePassword()
    {
        bool result_lowercase_missing = LoginHandler.ValidatePassword("PASSWORD123");
        bool result_digit_missing = LoginHandler.ValidatePassword("PASSWORDS");
        bool result_Uppercase_missing = LoginHandler.ValidatePassword("password123");
        bool result_null = LoginHandler.ValidatePassword("");


        bool result_long = LoginHandler.ValidatePassword("PASSwooooooooooooooooooooooooooooooooooord123");
        bool result_normal = LoginHandler.ValidatePassword("RightPassWord123");

        Assert.IsFalse(result_lowercase_missing);
        Assert.IsFalse(result_digit_missing);
        Assert.IsFalse(result_Uppercase_missing);
        Assert.IsNotNull(result_null);

        Assert.IsTrue(result_long);
        Assert.IsTrue(result_normal);
    }

    [TestMethod]
    public void TestValidatePhoneNumber()
    {
        bool result_short = LoginHandler.ValidatePhoneNumber("31063");
        bool result_long = LoginHandler.ValidatePhoneNumber("31063");
        bool result_letters = LoginHandler.ValidatePhoneNumber("31063");
        
    }
}