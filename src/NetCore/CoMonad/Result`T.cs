#nullable enable
using System;
using System.Diagnostics;
using System.Threading.Tasks;
namespace CoMonad
{


    /// <summary>
    /// Monad - Result with Value OR Error
    /// The Golden rule to be followed is that Methods that return Result's CANNOT RETURN NULL as Result&lt;T&gtT; is a struct
    /// Futhermore no method that returns a Result&lt;T&gtT; is allowed throw errors. All errors must be captured and turned in to ResultError
    /// Type of append Identity is Result&lt;Unit&gt;>
    /// The Golden rule to be followed is that Methods that return Result's 
    /// 1. CANNOT RETURN NULL 
    /// 2. CANNOT THROW ERRORS
    /// 3. Ctor param isSuccess if false, param resulterror SHOULD NOT BE ResultErrorValue.None
    /// 4. Ctor param isSuccess if true, param resulterror SHOULD BE ResultErrorValue.None
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





        #region Just to Satisfy Monadic laws- DONT NEED THESE IN PRACTICE
        //### Two **Methods** of the Monoid Type
        //         A append(A a, A b)
        //         A identity;   // identity of Result<T> is Result<Unit>
        // Two Properties
        //1.        Identity(no-op value - does not do anything when used with the** append** method on the Type)
        //2.        **Associativity**: Grouping of append does not matter(Two types coming together to make another of the same type.)
        //
        //these methods are not needed because wea are nearly always using Funcs to append within the Bind/Map ops
        //Append is implemented and handles Identity
        //Identity  is Result<Unit> , or actions rather than funcs in arguements
        //Map is implemented
        //FlatMap is Bind in this class 
        //Pure (lift) is Result.OK(a), or resultError.FailAs<T>()
        //public static Result<T2> Append<T1, T2>(this Result<T1> result, Result<T2> result2)
        //        => result.IsFailure ? result.Error.FailAs<T2>() : result2;
        //public static Result<T1> Append<T1>(this Result<Unit> result, Result<T1> result2)
        //    => result.IsFailure ? result.Error.FailAs<T1>() : result2;
        //public static Result<T1> Append<T1>(this Result<T1> result, Result<Unit> result2)
        //    => result.IsFailure ? result : result2.IsFailure ? result2.Error.FailAs<T1>() : result;
        #endregion



  
    }
}
