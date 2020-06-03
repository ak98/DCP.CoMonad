using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace CoMonadTest
{


    [TestClass]
    public class ValueObjectExample
    {

        [TestMethod]
        public void CreateEmails()
        {
            var email = EmailAddress.Create("test@test.com.au");
            Assert.IsTrue(email.Error is null, "email.Error is null :: Failed true assertion");

            var emailfail = EmailAddress.Create("test @test.com");
            Assert.IsTrue(emailfail.Error is{ }, "email.Error should not be null :: Failed true assertion");

            var verifiedemail = VerifiedEmailAddress.Create(email.Value);
            Assert.IsTrue(verifiedemail.Error is null, "verifiedemail.Error is null :: Failed true assertion  "+ verifiedemail.Error?.ToString());
        }






    }
}
