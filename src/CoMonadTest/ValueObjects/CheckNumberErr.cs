using CoMonad;

namespace CoMonadTest
{
    public sealed class CheckNumberErr : RezErrBase
    {
        public CheckNumberErr(string s) : base(s) { }

        public static readonly CheckNumberErr CheckNumberEmpty = new CheckNumberErr("CheckNumberEmpty");
        public static readonly CheckNumberErr CheckNumberInvalidCharacter = new CheckNumberErr("CheckNumberInvalidCharacter");
        public static readonly CheckNumberErr CheckNumberErrInvalidLegth = new CheckNumberErr("CheckNumberErrInvalidLegth");


    }
}
