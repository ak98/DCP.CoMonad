using System;
using CoMonad;

namespace CoMonadTest
{
    public sealed class EmailErr : RezErrBase
    {
        public EmailErr(string s) : base(s) { }
        public EmailErr(Exception ex) : base(ex) { }
        public EmailErr(string s, Exception ex) : base(s, ex) { }
        public EmailErr(RezErr err) : base(err) { }
        public RezErr WithException(Exception ex) => new RezErr(this._value, ex);

        public static readonly EmailErr EmailEmpty = new EmailErr("EmailEmpty");
        public static readonly EmailErr EmailInvalid = new EmailErr("EmailInvalid");
        public static readonly EmailErr EmailInvalidTooLong = new EmailErr("EmailInvalidTooLong");
        public static readonly EmailErr EmailNotVerifiable = new EmailErr("EmailNotVerifiable");


    }
}
