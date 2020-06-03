using System.Collections.Generic;
using CoMonad;

namespace CoMonadTest
{
    sealed class VerifiedEmailAddress : ValueObject
    {
        private readonly string _value;

        private VerifiedEmailAddress(EmailAddress email)
        {
            _value = email;
        }
        public static Result<VerifiedEmailAddress> Create(EmailAddress email)
        {
            return !HashServiceVerify(email) ? EmailErr.EmailNotVerifiable : Result.Ok(new VerifiedEmailAddress(email));

            static bool HashServiceVerify(EmailAddress email)
            {
                return ((string)email)?.EndsWith(".com.au") ?? false;
            }
        }



        protected override IEnumerable<object> GetEqualityComponents() => new[] { _value };

        public static implicit operator string(VerifiedEmailAddress email) => email._value;
        public override string ToString() => _value;
    }
}
