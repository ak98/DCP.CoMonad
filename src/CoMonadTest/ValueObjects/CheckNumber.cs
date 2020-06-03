#nullable enable       
using System.Collections.Generic;
using System.Linq;
using CoMonad;

namespace CoMonadTest
{

        //# Value obects below 


        sealed class CheckNumber : ValueObject
        {
            private readonly string _value;
            private CheckNumber(string checknum) => _value = checknum;
            public static Result<CheckNumber> Create(string? checknum)
            {
                if (checknum is null || string.IsNullOrEmpty(checknum))
                    return CheckNumberErr.CheckNumberEmpty;
                if (checknum.All(char.IsDigit))
                    return CheckNumberErr.CheckNumberInvalidCharacter;

                if (checknum.Length < 8 || checknum.Length > 12)
                    return CheckNumberErr.CheckNumberErrInvalidLegth;

                return Result.Ok(new CheckNumber(checknum));
            }

            protected override IEnumerable<object> GetEqualityComponents() => new[] { _value };

            public static implicit operator string(CheckNumber email) => email._value;
            public static explicit operator CheckNumber(string email) => Create(email).Value;
            public override string ToString() => _value;
        }
}
