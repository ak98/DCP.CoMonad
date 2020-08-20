#nullable enable
using CoMonad;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CoMonadTest
{
    [TestClass]
    public class Contact_Modelling_Scott_Wlashin
    {
        [TestMethod]
        public void Test_Contact_Example()
        {
            //https://youtu.be/PLFl95c-IiU?t=2628
            Result<Contact> contact = EmailAddress
                .Create("test@test.com")
                .Map(e => new Contact("test",e));
        }
    }
    class Contact
    {
        public Contact(string name, DUnion<EmailAddress, PostalContactAddress> primary, DUnion<EmailAddress, PostalContactAddress>? secondaryContact=null) {//Nullability is the "NEW" optional provided u are comfortable with C# 8 nullable options
            Name = name;
            PrimaryContact = primary;
            SecondaryContact = secondaryContact;
        }
        public string Name { get;  }
        public DUnion<EmailAddress, PostalContactAddress> PrimaryContact { get; }
        public DUnion<EmailAddress, PostalContactAddress>? SecondaryContact { get;  }
    }

    public class PostalContactAddress { }
}
