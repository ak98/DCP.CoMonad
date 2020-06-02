#if !NETSTANDARD
using Dasync.Collections;
#endif
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace CoMonad
{
    [DebuggerStepThrough]
    public static class ResultValueTaskExtensions
    {

        //# ValueTask<Result<T1>> ==> Func<Result<T1>, Result<T2>> ==> ValueTask<Result<T2>>
        public static ValueTask<Result<T2>> ValueTaskResultAsync<T1, T2>(this ValueTask<Result<T1>> result, Func<Result<T1>, Result<T2>> selector)
        {
            if (result.IsCompletedSuccessfully) return new ValueTask<Result<T2>>(selector(result.Result));
            return new ValueTask<Result<T2>>(result.AsTask().TaskResultAsync( selector));
        }
        //# ValueTask<Result<T1>> ==> Func<Result<T1>, ValueTask<Result<T2>>> ==> ValueTask<Result<T2>>
  
        public static ValueTask<Result<T2>> ValueTaskResultAsync<T1, T2>(this ValueTask<Result<T1>> result, Func<Result<T1>, ValueTask<Result<T2>>> selector)
        {
            if (result.IsCompletedSuccessfully) return selector(result.Result);
            return new ValueTask<Result<T2>>(result.AsTask().TaskResultAsync( rt1 => selector(rt1).AsTask()));
        }
        //# ValueTask<Result<T1>> ==> Func<Result<T1>, IAsyncEnumerable<Result<T2>>> ==> ValueTask<IAsyncEnumerable<Result<T2>>>
        public static ValueTask<IAsyncEnumerable<Result<T2>>> TaskResultAsync<T1, T2>(this ValueTask<Result<T1>> result, Func<Result<T1>, IAsyncEnumerable<Result<T2>>> selector)
        {
            if (result.IsCompletedSuccessfully) return new ValueTask<IAsyncEnumerable<Result<T2>>>(selector(result.Result));
            return new ValueTask<IAsyncEnumerable<Result<T2>>>(result.AsTask().TaskResultAsync(rt1 => selector(rt1)));

        }

        //# ValueTask<Result<T1>> ==>  Func<T1, T2> ==> ValueTask<Result<T2>>
        public static ValueTask<Result<T2>> MapAsync<T1, T2>(this ValueTask<Result<T1>> result, Func<T1, T2> resultSelector)
             => new ValueTask<Result<T2>>(result.AsTask().TaskResultAsync<T1, T2>(rt1 => rt1.Map(resultSelector)));
        //# ValueTask<Result<T1>> ==>  Func<T1, ValueTask<T2>> ==> ValueTask<Result<T2>>
        public static ValueTask<Result<T2>> MapAsync<T1, T2>(this ValueTask<Result<T1>> result, Func<T1, ValueTask<T2>> resultSelector)
              => result.ValueTaskResultAsync<T1, T2>(rt1 => rt1.MapAsync(resultSelector));
        //# ValueTask<Result<T1>> ==>  Func<T1, Result<T2>> ==> ValueTask<Result<T2>>
        public static ValueTask<Result<T2>> BindAsync<T1, T2>(this ValueTask<Result<T1>> result, Func<T1, Result<T2>> resultSelector)
                => new ValueTask<Result<T2>>(result.AsTask().TaskResultAsync<T1, T2>(rt1 => rt1.Bind(resultSelector)));
        //# ValueTask<Result<T1>> ==>  Func<T1, ValueTask<Result<T2>>> ==> ValueTask<Result<T2>>
        public static ValueTask<Result<T2>> BindAsync<T1, T2>(this ValueTask<Result<T1>> result, Func<T1, ValueTask<Result<T2>>> resultSelector)
                => result.ValueTaskResultAsync<T1, T2>(rt1 => rt1.BindAsync(resultSelector));




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

        //# Result<T1> ==> Func<T1, Result<T2>> ==> Result<(T1, T2)>
        public static Result<(T1, T2)> Combine<T1, T2>(in this Result<T1> r1, Func<T1, Result<T2>> r2func)
            => r1.Error ?? r1.Combine(r2func(r1.Value));
        //#  Result<T1> ==> Func<T1, ValueTask<Result<T2>>> ==> ValueTask<Result<T2>>
        public static ValueTask<Result<T2>> BindAsync<T1, T2>(in this Result<T1> result, Func<T1, ValueTask<Result<T2>>> resultSelector)
        {
            if (result.Error is { }) return new ValueTask<Result<T2>>(result.Error);
            return resultSelector(result.Value);
        }
    }
}
