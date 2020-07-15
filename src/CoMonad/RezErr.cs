using System;
using System.Diagnostics;
using System.Net;

namespace CoMonad
{


    public sealed class RezErr : RezErrBase
    {
        public RezErr(string s) : base(s) { }
        public RezErr(Exception ex) : base(ex) { }
        public RezErr(string s, Exception ex) : base(s, ex) { }
        public RezErr(RezErr err) : base(err) { }
        public RezErr WithException(Exception ex) => new RezErr(this._value, ex);

        public static readonly RezErr None = new RezErr("");
        public static readonly RezErr Cancelled = new RezErr("Cancelled");
        public static readonly RezErr EmptyEnumerable = new RezErr("EmptyEnumerable");
        public static readonly RezErr OverThrow = new RezErr("OverThrow");
    }
}
