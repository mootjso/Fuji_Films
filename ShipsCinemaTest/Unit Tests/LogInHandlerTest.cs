namespace LoginHandlerTest
{
    [TestClass]
    public class TestLoginHandler
    {        

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