namespace LoginHandlerTest
{
    [TestClass]
    public class TestLoginHandler
    {
        private static string _fileName = LoginHandler.FileName;
        private static List<User> _originalFileName = JSONMethods.ReadJSON<User>(_fileName).ToList();
        [TestInitialize]
    public void TestInitialize()
    {
        // Clear the test JSON file
        File.WriteAllText(_fileName, "[]");

        List<User> testUsers = new List<User>
        {
            new User(1, "John", "Doe", "john.doe@outlook.com", "password123", "0612345678"),
            new User(2, "Barld", "Boot", "barld.boot@gmail.com", "securepass", "0612345679", true)
        };
        JSONMethods.WriteToJSON(testUsers, _fileName);
    }

        [ClassCleanup]
        public static void CleanupJSONFile()
        {
            JSONMethods.WriteToJSON(_originalFileName, _fileName);
        }

        [TestMethod]
        public void TestReadFromJSON()
        {
            User user = LoginHandler.Users[0];
            User user_admin = LoginHandler.Users[1];

            Assert.AreEqual(1, user.Id);
            Assert.AreEqual("John", user.FirstName);
            Assert.AreEqual("Doe", user.LastName);
            Assert.AreEqual("john.doe@outlook.com", user.Email);
            Assert.AreEqual("password123", user.Password);
            Assert.AreEqual("0612345678", user.PhoneNumber);

            Assert.IsFalse(user.IsAdmin);

            Assert.IsTrue(user_admin.IsAdmin);
        }

    [DataTestMethod]
    [DataRow("John")]
    [DataRow("Doe")]
    [DataRow("Sam")]
    [DataRow("Obama")]
    public void TestValidateNameInputCorrect(string name)
    {
        bool result = ValidateNameInput.IsValid(name);

        Assert.IsTrue(result);
    }
    
    [DataTestMethod]
    [DataRow("Po")]
    [DataRow(null)]
    [DataRow("John5")]
    public void TestValidateNameInputIncorrect(string name)
    {
        bool result = ValidateNameInput.IsValid(name);

        Assert.IsFalse(result);
    }

    [DataTestMethod]
    [DataRow("PASSwooooooooooooooooooooooooooooooooooord123")]
    [DataRow("RightPassWord123")]
    public void TestValidatePasswordCorrect(string password)
    {
        bool result = ValidatePassword.IsValid(password);

        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow("PASSWORD123")]
    [DataRow("PASSWORDS")]
    [DataRow("password123")]
    [DataRow(null)]
    public void TestValidatePasswordincorrect(string password)
    {
        bool result = ValidatePassword.IsValid(password);

        Assert.IsFalse(result);
    }

    [DataTestMethod]
    [DataRow("067128249")]
    [DataRow("067128249142649")]
    public void TestValidatePhoneNumberCorrect(string phonenumber)
    {
        bool result = ValidatePhoneNumber.IsValid(phonenumber);

        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow("31063")]
    [DataRow("31063432693733101")]
    [DataRow("31063ds21")]
    [DataRow("-067128249")]
    [DataRow("+067128249")]
    [DataRow(null)]
    public void TestValidatePhoneNumberIncorrect(string phonenumber)
    {
        bool result = ValidatePhoneNumber.IsValid(phonenumber);

        Assert.IsFalse(result);
    }

    [DataTestMethod]
    [DataRow("3983748@outlook.com", true)]
    [DataRow("John@outlook.com", true)]
    [DataRow("johndoe@gmail.com", true)]
    public void TestValidateEmailCorrect(string email, bool expectedOutput)
    {
        bool result = ValidateEmail.IsValid(email);

        Assert.AreEqual(expectedOutput, result);
    }
    
    [DataTestMethod]
    [DataRow("Johnoutlook.com", false)]
    [DataRow(null, false)]
    [DataRow("John@.com", false)]
    [DataRow("John@outlookcom", false)]
    [DataRow("John@outlook.", false)]
    [DataRow("@outlook.com", false)]
    public void TestValidateEmailIncorrect(string email, bool expectedOutput)
    {
        bool result = ValidateEmail.IsValid(email);

        Assert.AreEqual(expectedOutput, result);
    }
}
}