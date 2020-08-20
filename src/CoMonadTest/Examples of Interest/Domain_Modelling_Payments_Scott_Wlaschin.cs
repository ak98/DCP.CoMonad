#nullable enable       
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System;
using CoMonad;
namespace CoMonadTest
{
    [TestClass]
    public partial class Domain_Modelling_Payments_Scott_Wlaschin
    {
        [TestMethod]
        public void Payment_Model()  //# Scott Wlaschin Domain modelling https://youtu.be/PLFl95c-IiU?t=1169
        {
            Result<Payment> payment = Cash.Create(42)
                .Map(c => ( DUnion<Cash, Cheque, DUnion<MasterCard, Visacard>>)c)
                .Combine(pm => PaymentAmount.Create(42))
                .Map(tup => new Payment() { Amount = tup.Item2, Method = tup.Item1, Currency = Currency.USD });
        }
        //# Model classes
        class MasterCard
        {
            public CardNumber CardNumber { get; set; }
        }
        class Visacard
        {
            public CardNumber CardNumber { get; set; }
        }
        class Cheque
        {
            public CheckNumber CheckNumber { get; set; }
        }


        class Payment
        {
            public PaymentAmount Amount { get; set; }
            public Currency Currency { get; set; }
            public DUnion<Cash, Cheque, DUnion<MasterCard, Visacard>> Method { get; set; }
        }
    }
}
