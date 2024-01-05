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
    [DataRow("3983748@outlook.com")]
    [DataRow("John@outlook.com")]
    [DataRow("johndoe@gmail.com")]
    public void TestValidateEmailCorrect(string email)
    {
        bool result = ValidateEmail.IsValid(email);

        Assert.IsTrue(result);
    }
    
    [DataTestMethod]
    [DataRow("Johnoutlook.com")]
    [DataRow(null)]
    [DataRow("John@.com")]
    [DataRow("John@outlookcom")]
    [DataRow("John@outlook.")]
    [DataRow("@outlook.com")]
    public void TestValidateEmailIncorrect(string email)
    {
        bool result = ValidateEmail.IsValid(email);

        Assert.IsFalse(result);
    }
}
}