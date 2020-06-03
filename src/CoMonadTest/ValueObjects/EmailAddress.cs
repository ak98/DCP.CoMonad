using System.Collections.Generic;
using CoMonad;
using System.Text.RegularExpressions;

namespace CoMonadTest
{
    sealed class EmailAddress : ValueObject
    {
        private readonly string _value;
        private EmailAddress(string email) => _value = email;
        private static Regex simpleEmailAddressPattern => new Regex(@"^[A-Z0-9][A-Z0-9._%+-]{0,63}@(?:[A-Z0-9-]{1,63}\.){1,8}[A-Z]{2,63}$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static Result<EmailAddress> Create(string? email)
        {
            if (email is null || string.IsNullOrEmpty(email))
                return EmailErr.EmailEmpty;
            if (email.IndexOf('@') <= 0)
                return EmailErr.EmailInvalid;
            if (email.Length > 256)
                return EmailErr.EmailInvalidTooLong;
            if (!simpleEmailAddressPattern.IsMatch(email))
                return EmailErr.EmailInvalid;
            return Result.Ok(new EmailAddress(email));
        }

        protected override IEnumerable<object> GetEqualityComponents() => new[] { _value };

        public static implicit operator string(EmailAddress email) => email._value;
        public static explicit operator EmailAddress(string email) => Create(email).Value;
        public override string ToString() => _value;
    }
}
