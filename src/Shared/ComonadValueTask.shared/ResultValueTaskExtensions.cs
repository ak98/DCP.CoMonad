
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
//.NET Framework 2.0   --> NET20
//.NET Framework 3.5   --> NET35
//.NET Framework 4.0   --> NET40
//.NET Framework 4.5   --> NET45
//.NET Framework 4.5.1 --> NET451
//.NET Framework 4.5.2 --> NET452
//.NET Framework 4.6   --> NET46
//.NET Framework 4.6.1 --> NET461
//.NET Framework 4.6.2 --> NET462
//.NET Framework 4.7   --> NET47
//.NET Framework 4.7.1 --> NET471
//.NET Framework 4.7.2 --> NET472

//.NET Standard 1.0    --> NETSTANDARD1_0
//.NET Standard 1.1    --> NETSTANDARD1_1
//.NET Standard 1.2    --> NETSTANDARD1_2
//.NET Standard 1.3    --> NETSTANDARD1_3
//.NET Standard 1.4    --> NETSTANDARD1_4
//.NET Standard 1.5    --> NETSTANDARD1_5
//.NET Standard 1.6    --> NETSTANDARD1_6
//.NET Standard 2.0    --> NETSTANDARD2_0

//.Net Core 1.0        --> NETCOREAPP1_0
//.Net Core 1.1        --> NETCOREAPP1_1
//.Net Core 2.0        --> NETCOREAPP2_0
//.Net Core 2.1        --> NETCOREAPP2_1
namespace CoMonad
{
    [DebuggerStepThrough]
    public static class ResultValueTaskExtensions
    {
        //# ValueTask<Result<T1>> ==>  Func<T1, T2> ==> ValueTask<Result<T2>>
        public static ValueTask<Result<T2>> MapAsync<T1, T2>(this ValueTask<Result<T1>> result, Func<T1, T2> resultSelector)
             => new ValueTask<Result<T2>>(result.AsTask().TaskResultAsync<T1, T2>(rt1 => rt1.Map(resultSelector)));
        //# ValueTask<Result<T1>> ==>  Func<T1, ValueTask<T2>> ==> ValueTask<Result<T2>>
        public static ValueTask<Result<T2>> MapAsync<T1, T2>(this ValueTask<Result<T1>> result, Func<T1, ValueTask<T2>> resultSelector)
              => result.ValueTaskResultAsync<T1, T2>(rt1 => rt1.MapAsync(resultSelector));

        //# Result<T1> ==> Func<T1, ValueTask<T2>> ==> ValueTask<Result<T2>>
        public static ValueTask<Result<T2>> MapAsync<T1, T2>(in this Result<T1> result, Func<T1, ValueTask<T2>> resultSelector)
        {
            if (result.Error is { }) return new ValueTask<Result<T2>>(result.Error);
            ValueTask<T2> vt = resultSelector(result.Value);
            if (vt.IsCompletedSuccessfully) return new ValueTask<Result<T2>>(Result.Ok(vt.Result));
            Task<Result<T2>> xx = vt.AsTask().ContinueWith(completedtask =>
            {
                if (completedtask.IsFaulted)
                {
                    return completedtask.Exception.AsFail<T2>();
                }
                else if (completedtask.IsCanceled)
                {
                    return RezErr.Cancelled.AsFail<T2>();
                }
                return Result.Ok(completedtask.Result);
            });
            return new ValueTask<Result<T2>>(xx);
        }

        //#  Result<T1> ==> Func<T1, ValueTask<Result<T2>>> ==> ValueTask<Result<T2>>
        public static ValueTask<Result<T2>> BindAsync<T1, T2>(in this Result<T1> result, Func<T1, ValueTask<Result<T2>>> resultSelector)
        {
            if (result.Error is { }) return new ValueTask<Result<T2>>(result.Error);
            return resultSelector(result.Value);
        }
        //# ValueTask<Result<T1>> ==>  Func<T1, Result<T2>> ==> ValueTask<Result<T2>>
        public static ValueTask<Result<T2>> BindAsync<T1, T2>(this ValueTask<Result<T1>> result, Func<T1, Result<T2>> resultSelector)
                => new ValueTask<Result<T2>>(result.AsTask().TaskResultAsync<T1, T2>(rt1 => rt1.Bind(resultSelector)));

        //# ValueTask<Result<T1>> ==>  Func<T1, ValueTask<Result<T2>>> ==> ValueTask<Result<T2>>
        public static ValueTask<Result<T2>> BindAsync<T1, T2>(this ValueTask<Result<T1>> result, Func<T1, ValueTask<Result<T2>>> resultSelector)
                => result.ValueTaskResultAsync<T1, T2>(rt1 => rt1.BindAsync(resultSelector));

        //# ValueTask<Result<T1>> ==> Func<Result<T1>, Result<T2>> ==> ValueTask<Result<T2>>
        public static ValueTask<Result<T2>> ValueTaskResultAsync<T1, T2>(this ValueTask<Result<T1>> result, Func<Result<T1>, Result<T2>> selector)
        {
            if (result.IsCompletedSuccessfully) return new ValueTask<Result<T2>>(selector(result.Result));
            return new ValueTask<Result<T2>>(result.AsTask().TaskResultAsync(selector));
        }
        //# ValueTask<Result<T1>> ==> Func<Result<T1>, ValueTask<Result<T2>>> ==> ValueTask<Result<T2>>
        public static ValueTask<Result<T2>> ValueTaskResultAsync<T1, T2>(this ValueTask<Result<T1>> result, Func<Result<T1>, ValueTask<Result<T2>>> selector)
        {
            if (result.IsCompletedSuccessfully) return selector(result.Result);
            return new ValueTask<Result<T2>>(result.AsTask().TaskResultAsync(rt1 => selector(rt1).AsTask()));
        }

    }
}
