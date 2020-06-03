#nullable enable
namespace CoMonad
{
    using System;
    using System.Diagnostics;

    
    [DebuggerStepThrough]
    public static  class Result {
        //# ONE GOLDEN RULE --> Methods returning Result<T> CANNOT THROW ERRORS
        public static Result<T> Ok<T>(T value)
              => Result<T>.Ok(value);

        public static Result<T1> Try<T1>(Func<T1> func)
       => func.TryResult();
        public static Result<T2> Try<T1, T2>(Func<T1, T2> func, T1 t1)
                => func.TryResult(t1);
        public static Result<T3> Try<T1, T2, T3>(Func<T1, T2, T3> func, T1 t1, T2 t2)
                => func.TryResult(t1, t2);
        public static Result<T4> Try<T1, T2, T3, T4>(Func<T1, T2, T3, T4> func, T1 t1, T2 t2, T3 t3)
                => func.TryResult(t1, t2, t3);
    }

}
