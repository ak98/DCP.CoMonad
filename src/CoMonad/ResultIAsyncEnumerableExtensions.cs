#if NET45 
using Dasync.Collections;
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CoMonad
{
    [DebuggerStepThrough]
    public static class ResultIAsyncEnumerableExtensions
    {
        public static IAsyncEnumerable<Result<T2>> MapIAsync<T1, T2>(this Result<T1> rt1, Func<T1, IAsyncEnumerable<T2>> selector)
        {
            if (rt1.Error is { }) return new[] { rt1.Error.AsFail<T2>() }.ToAsyncEnumerable();
            var t1 = rt1.Value;
            IAsyncEnumerable<T2> iet2 = selector(rt1.Value);
            return iet2.Select(t2 => Result.Ok(t2));

        }
        //# Task<Result<T1>> ==> Func<Result<T1>, IAsyncEnumerable<Result<T2>>> ==> Task<IAsyncEnumerable<Result<T2>>>
        public static Task<IAsyncEnumerable<Result<T2>>> TaskResultAsync<T1, T2>(this Task<Result<T1>> result, Func<Result<T1>, IAsyncEnumerable<Result<T2>>> selector)
        {
            return result.ContinueWith(completedtask =>
            {
                if (completedtask.IsFaulted)
                {
                    return new[] { completedtask.Exception.AsFail<T2>() }.ToAsyncEnumerable();
                }
                else if (completedtask.IsCanceled)
                {
                    return new[] { RezErr.Cancelled.AsFail<T2>() }.ToAsyncEnumerable();
                }
                if (completedtask.Result.Error is { })
                {
                    return new[] { completedtask.Result.Error.AsFail<T2>() }.ToAsyncEnumerable();
                }
                return selector(completedtask.Result);
            });
        }
        //#  Result<T1> ==> Func<T1, IAsyncEnumerable<Result<T2>>> ==> IAsyncEnumerable<Result<T2>>
        public static IAsyncEnumerable<Result<T2>> BindIAsync<T1, T2>(this Result<T1> rt1, Func<T1, IAsyncEnumerable<Result<T2>>> selector)
        {
            if (rt1.Error is { }) return new[] { rt1.Error.AsFail<T2>() }.ToAsyncEnumerable();
            return selector(rt1.Value);
        }
        //# Task<Result<T1>> ==>  Func<T1, IAsyncEnumerable<Result<T2>>> ==> IAsyncEnumerable<Result<T2>>
        public static IAsyncEnumerable<Result<T2>> BindIAsync<T1, T2>(this Task<Result<T1>> result, Func<T1, IAsyncEnumerable<Result<T2>>> selector)
            => result.TaskResultAsync<T1, T2>(rt1 => rt1.BindIAsync(selector)).GetAwaiter().GetResult();
        //# IAsyncEnumerable<Result<T1>> =>  Func<T1, T2> => IAsyncEnumerable<Result<T2>>
        public static IAsyncEnumerable<Result<T2>> Map<T1, T2>(this IAsyncEnumerable<Result<T1>> rt1s, Func<T1, T2> select) 
            => rt1s.Select(rt1 => rt1.Map(select));

        //# IAsyncEnumerable<Result<T1>> =>   Func<T1, Result<T2>> => IAsyncEnumerable<Result<T2>>
        public static IAsyncEnumerable<Result<T2>> Bind<T1, T2>(this IAsyncEnumerable<Result<T1>> rt1s, Func<T1, Result<T2>> select) 
            => rt1s.Select(rt1 => rt1.Bind(select));
        //# IAsyncEnumerable<Result<T1>> =>   Func<T1, Task<T2>> => IAsyncEnumerable<Result<T2>>
        public static IAsyncEnumerable<Result<T2>> Map<T1, T2>(this IAsyncEnumerable<Result<T1>> rt1s, Func<T1, Task<T2>> select) 
            => rt1s.Select(rt1 => (rt1.MapAsync(select)).GetAwaiter().GetResult());

        //#   IAsyncEnumerable<Result<T1>> =>  Func<T1, IAsyncEnumerable<Result<T2>>> => IAsyncEnumerable<Result<T2>>
        public static IAsyncEnumerable<Result<T2>> BindManyIAsync<T1, T2>(this IAsyncEnumerable<Result<T1>> rt1s, Func<T1, IAsyncEnumerable<Result<T2>>> selectMany)
            => rt1s.SelectMany(rt1 => rt1.BindIAsync(selectMany));
        ////--------------------- MapIAsync HAS THREE POSSIBLE COMBINATORIALS that make sense as building blocks-----------------------------------

        //        [Obsolete(" error handling is undefined")]
        //        public static IAsyncEnumerable<Result<T2>> MapManyIAsync<T1, T2>(this IAsyncEnumerable<Result<T1>> rt1s, Func<T1, IAsyncEnumerable<T2>> selectMany)
        //        {
        //#pragma warning disable CS0618 // Type or member is obsolete
        //            return rt1s.SelectMany(rt1 => rt1.MapIAsync(selectMany));
        //#pragma warning restore CS0618 // Type or member is obsolete
        //        }

        //#   IAsyncEnumerable<Result<T1>> =>  Func<T1, ValueTask<Result<T2>>> => IAsyncEnumerable<Result<T2>>
        public static IAsyncEnumerable<Result<T2>> BindIAsync<T1, T2>(this IAsyncEnumerable<Result<T1>> rt1s, Func<T1, ValueTask<Result<T2>>> selectAsync) 
            => BindIAsync(rt1s, t1 => selectAsync(t1).AsTask());
        //# ValueTask<Result<T1>> ==> Func<Result<T1>, IAsyncEnumerable<Result<T2>>> ==> ValueTask<IAsyncEnumerable<Result<T2>>>
        public static ValueTask<IAsyncEnumerable<Result<T2>>> TaskResultAsync<T1, T2>(this ValueTask<Result<T1>> result, Func<Result<T1>, IAsyncEnumerable<Result<T2>>> selector)
        {
            if (result.IsCompletedSuccessfully) return new ValueTask<IAsyncEnumerable<Result<T2>>>(selector(result.Result));
            return new ValueTask<IAsyncEnumerable<Result<T2>>>(result.AsTask().TaskResultAsync(rt1 => selector(rt1)));

        }
#if !NET45
        //#   IAsyncEnumerable<Result<T1>> =>  Func<T1, Task<Result<T2>> => IAsyncEnumerable<Result<T2>>
        public static IAsyncEnumerable<Result<T2>> BindIAsync<T1, T2>(this IAsyncEnumerable<Result<T1>> rt1s, Func<T1, Task<Result<T2>>> selectAsync) 
            => rt1s.Select(rt1 => (rt1.BindAsync(selectAsync)).GetAwaiter().GetResult());
#else
        public static IAsyncEnumerable<Result<T2>> BindIAsync<T1, T2>(this IAsyncEnumerable<Result<T1>> rt1s, Func<T1, Task<Result<T2>>> converter)
        {
            return new AsyncEnumerable<Result<T2>>(async yielder =>
            {
                try
                {
                    await rt1s.ForEachAsync(async t1 =>
                    {
                        if (t1.Error is { })
                            await yielder.ReturnAsync(t1.Error.AsFail<T2>());
                        else
                        {
                            var rt2 = await converter(t1.Value);
                            await yielder.ReturnAsync(rt2);
                        }


                    });
                }
                catch (AsyncEnumerationCanceledException)
                {

                }
            });
        }
#endif
    }
}
