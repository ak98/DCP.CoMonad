#nullable enable
using System;
using System.Diagnostics;
using System.Threading.Tasks;
namespace CoMonad
{


    /// <summary>
    /// Monad - Result with Value OR Error
    /// The Golden rule to be followed is that Methods that return Result's CANNOT THROW 
    /// All errors must be captured and turned in to Result.Error
    /// </summary>
    [DebuggerStepThrough]
    public struct Result<T>
    {
        public static Result<T> Fail(RezErrBase rez)
            => new Result<T>(rez);
        public static Result<T> Fail(Exception ex)
                     => new Result<T>(new RezErr(ex));
        internal static Result<T> Ok(T value)
              => new Result<T>(value);

        public readonly RezErrBase? Error;
        private readonly T _value;

        private Result(T value)
        {
            Error = null;
            _value = value;
        }
        private Result(RezErrBase errorBase)
        {

            Error = errorBase;
            _value = default(T)!;
        }
        //# Magic - simply return the RezErrBase from method where return type  Result<T> is expected
        public static implicit operator Result<T>(RezErrBase value)
            => value.AsFail<T>();

        public T Value
           =>  (Error is null) 
                ? _value
                : throw new InvalidOperationException("Error should be null when Result<T>.Value is accessed. Error was " + Error.ToString()); 
        public override string? ToString()
         => (Error is null )
            ? _value?.ToString() 
            : Error.ToString();
  
    }
}
