namespace LoginHandlerTest
{
    [TestClass]
    public class TestLoginHandler
    {
        private static string _fileName = LoginHandler.FileName;
        private static List<User> _originalFileName = JSONMethods.ReadJSON<User>(_fileName).ToList();

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
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

        [TestMethod]
        public void TestValidateNameInput()
        {
            bool result = ValidateNameInput.IsValid("John");

            bool wrongResult = ValidateNameInput.IsValid("Po");
            bool nullResult = ValidateNameInput.IsValid(null);
            bool resultNumber = ValidateNameInput.IsValid("John5");

            Assert.IsTrue(result);

            Assert.IsFalse(wrongResult);
            Assert.IsFalse(nullResult);
            Assert.IsFalse(resultNumber);
        }

        [TestMethod]
        public void TestValidatePassword()
        {
            bool lowercaseMissing = ValidatePassword.IsValid("PASSWORD123");
            bool digitMissing = ValidatePassword.IsValid("PASSWORDS");
            bool UppercaseMissing = ValidatePassword.IsValid("password123");
            bool resultNull = ValidatePassword.IsValid(null);

            bool resultLong = ValidatePassword.IsValid("PASSwooooooooooooooooooooooooooooooooooord123");
            bool resultNormal = ValidatePassword.IsValid("RightPassWord123");

            Assert.IsFalse(lowercaseMissing);
            Assert.IsFalse(digitMissing);
            Assert.IsFalse(UppercaseMissing);
            Assert.IsFalse(resultNull);

            Assert.IsTrue(resultLong);
            Assert.IsTrue(resultNormal);
        }

        [TestMethod]
        public void TestValidatePhoneNumber()
        {
            bool resultShort = ValidatePhoneNumber.IsValid("31063");
            bool resultLong = ValidatePhoneNumber.IsValid("31063432693733101");
            bool resultLetters = ValidatePhoneNumber.IsValid("31063ds21");
            bool resultNegative = ValidatePhoneNumber.IsValid("-067128249");
            bool resultSymbol = ValidatePhoneNumber.IsValid("+067128249");
            bool resultNull = ValidatePhoneNumber.IsValid(null);

            bool resultGood = ValidatePhoneNumber.IsValid("067128249");
            bool resultGood2 = ValidatePhoneNumber.IsValid("067128249142649");

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
            bool missingAt = ValidateEmail.IsValid("Johnoutlook.com");
            bool resultNull = ValidateEmail.IsValid(null);
            bool missingDomain = ValidateEmail.IsValid("John@.com");
            bool missingDot = ValidateEmail.IsValid("John@outlookcom");
            bool missingTopDomain = ValidateEmail.IsValid("John@outlook.");
            bool missingUsername = ValidateEmail.IsValid("@outlook.com");

            bool onlyNumbers = ValidateEmail.IsValid("3983748@outlook.com");
            bool resultAllowed = ValidateEmail.IsValid("John@outlook.com");
            bool resultAllowed2 = ValidateEmail.IsValid("johndoe@gmail.com");

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
}
