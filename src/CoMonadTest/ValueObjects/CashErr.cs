using CoMonad;

namespace CoMonadTest
{
    public sealed class CashErr : RezErrBase
    {
        public CashErr(string s) : base(s) { }

        public static readonly CashErr CashErrNegative = new CashErr("CashErrNegative");
        public static readonly CashErr CardNumberErrInvalidCharacter = new CashErr("CardNumberErrInvalidCharacter");
        public static readonly CashErr CardNumberErrInvalidLegth = new CashErr("CardNumberErrInvalidLegth");


    }
}
