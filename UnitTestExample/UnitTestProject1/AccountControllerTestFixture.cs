using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestExample.Controllers;

namespace UnitTestProject1
{
    public class AccountControllerTestFixture
    {
        [
 Test,
 TestCase("abcd1234", false),
 TestCase("irf@uni-corvinus", false),
 TestCase("irf.uni-corvinus.hu", false),
 TestCase("irf@uni-corvinus.hu", true)
]
        public void TestValidateEmail(string email, bool expectedResult)
        {
            var accountController = new AccountController();
            var actualResult = accountController.ValidateEmail(email);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [
Test,
TestCase("Blabla", false),
TestCase("BLABLA89", false),
TestCase("kisbetus48", false),
TestCase("az", true),
            TestCase("Valami12843", true)
]
        public void TestValidatePassword(string password, bool expectedResult)
        {
            var accountController = new AccountController();
            var actualResult = accountController.ValidatePassword(password);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
