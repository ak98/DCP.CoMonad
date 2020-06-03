#nullable enable       
using System.Collections.Generic;
using System.Linq;
using CoMonad;

namespace CoMonadTest
{

        sealed class CardNumber : ValueObject
        {
            private readonly string _value;
            private CardNumber(string checknum) => _value = checknum;
            public static Result<CardNumber> Create(string? checknum)
            {
                if (checknum is null || string.IsNullOrEmpty(checknum))
                    return CardNumberErr.CardNumberErrEmpty;
                if (checknum.All(char.IsDigit))
                    return CardNumberErr.CardNumberErrInvalidCharacter;

                if (checknum.Length != 10)
                    return CardNumberErr.CardNumberErrInvalidLegth;

                return Result.Ok(new CardNumber(checknum));
            }

            protected override IEnumerable<object> GetEqualityComponents() => new[] { _value };

            public static implicit operator string(CardNumber email) => email._value;
            public static explicit operator CardNumber(string email) => Create(email).Value;
            public override string ToString() => _value;
        }
}
