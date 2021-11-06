using System;
using EmployeesApp.Validation;
using Xunit;

namespace EmployeesApp.Tests.Validation
{
    public class AccountNumberValidationTests
    {
        public AccountNumberValidation _validation;

        public AccountNumberValidationTests()
        {
            _validation = new AccountNumberValidation();
        }

        [Fact]
        //Naming COnvention => MethodWeTest_StateUnderTest_ExpectedBehavior
        public void isValid_ValidAccountNumber_returnsTrue ()
        {
            Assert.True(_validation.validateAccountNumber("234-4694609047-61"));
        }

        [Theory]
        [InlineData("234-6946090471-1")]
        [InlineData("234-694609047-61")]
        [InlineData("23-6946090471-61")]
        public void isValid_ValidAccountNumberWrong_returnsFalse(string acctNo)
        {
            Assert.False(_validation.validateAccountNumber(acctNo));
        }

        [Theory]
        [InlineData("234-694609047-61")]
        [InlineData("234-69460904711-61")]
        public void isValid_AccountNumberMiddlePartWrong_returnsFalse(string acctNo)
        {
            Assert.False(_validation.validateAccountNumber(acctNo));
        }

        [Theory]
        [InlineData("1234-694609047-61")]
        [InlineData("34-69460904711-61")]
        public void isValid_AccountNumberFirstPartWrong_returnsFalse(string acctNo)
        {
            Assert.False(_validation.validateAccountNumber(acctNo));
        }
        
        [Theory]
        [InlineData("1234-694609047-610")]
        [InlineData("34-69460904711-6")]
        public void isValid_AccountNumberLastPartWrong_returnsFalse(string acctNo)
        {
            Assert.False(_validation.validateAccountNumber(acctNo));
        }
        
        [Theory]
        [InlineData("124-6946090471+60")]
        [InlineData("124+6946090471-60")]
        [InlineData("124+6946090471+60")]       
        public void isValid_InvalidDelimiters_ThrowsArgumentException(string acctNo)
        {
            Assert.Throws<ArgumentException>(() => _validation.validateAccountNumber(acctNo));
        }
    }
}
