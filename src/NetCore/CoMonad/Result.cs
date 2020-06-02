#nullable enable
namespace CoMonad
{
    using System.Diagnostics;

    
    [DebuggerStepThrough]
    public static  class Result {
        //# ONE GOLDEN RULE --> Methods returning Result<T> CANNOT THROW ERRORS
        public static Result<T> Ok<T>(T value)
              => Result<T>.Ok(value);
    }

}
