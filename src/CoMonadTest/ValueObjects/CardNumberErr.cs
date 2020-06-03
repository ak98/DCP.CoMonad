using CoMonad;

namespace CoMonadTest
{
    public sealed class CardNumberErr : RezErrBase
    {
        public CardNumberErr(string s) : base(s) { }

        public static readonly CardNumberErr CardNumberErrEmpty = new CardNumberErr("CardNumberErrEmpty");
        public static readonly CardNumberErr CardNumberErrInvalidCharacter = new CardNumberErr("CardNumberErrInvalidCharacter");
        public static readonly CardNumberErr CardNumberErrInvalidLegth = new CardNumberErr("CardNumberErrInvalidLegth");


    }
}
