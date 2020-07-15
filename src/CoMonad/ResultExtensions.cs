
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace CoMonad
{
    [DebuggerStepThrough]
    public static class ResultExtensions
    {
        //? Map 4
        //# Result<T1> ==> Func<T1, T2> ==> Result<T2>
        public static Result<T2> Map<T1, T2>(in this Result<T1> rt1, Func<T1, T2> func)
            => rt1.Error ?? func.TryResult(rt1.Value);

        //# Result<T1> ==> Func<T1, Task<T2>> ==> Task<Result<T2>>
        public static Task<Result<T2>> MapAsync<T1, T2>(in this Result<T1> rt1, Func<T1, Task<T2>> func)
            => (rt1.Error is { }) ? Task.FromResult(rt1.Error.AsFail<T2>()) :
                func(rt1.Value).ContinueWith(completedtask =>
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
        //# Task<Result<T1>> ==> Func<T1, T2> ==> Task<Result<T2>>
        public static Task<Result<T2>> MapAsync<T1, T2>(this Task<Result<T1>> taskrt1, Func<T1, T2> func)
                => taskrt1.TaskResultAsync<T1, T2>(rt1 => rt1.Map(func));
        //# Task<Result<T1>> ==> Func<T1, Task<T2>> ==> Task<Result<T2>>
        public static Task<Result<T2>> MapAsync<T1, T2>(this Task<Result<T1>> taskrt1, Func<T1, Task<T2>> func)
             => taskrt1.TaskResultAsync<T1, T2>(rt1 => rt1.MapAsync(func));
        //? Bind 4
        //# Result<T1> ==> Func<T1, Result<T2>> ==> Result<T2>
        public static Result<T2> Bind<T1, T2>(in this Result<T1> t1, Func<T1, Result<T2>> resultSelector)
            => t1.Error ?? resultSelector(t1.Value);
        //# Result<T1> ==> Func<T1, Task<Result<T2>>> ==> Task<Result<T2>>
        public static Task<Result<T2>> BindAsync<T1, T2>(in this Result<T1> result, Func<T1, Task<Result<T2>>> resultSelector)
        => (result.Error is { }) ? Task.FromResult(result.Error.AsFail<T2>())
                        : resultSelector(result.Value)
                            .ContinueWith(completedtask =>
                            {
                                if (completedtask.IsFaulted)
                                {
                                    return completedtask.Exception.AsFail<T2>();
                                }
                                else if (completedtask.IsCanceled)
                                {
                                    return RezErr.Cancelled.AsFail<T2>();
                                }
                                return completedtask.Result;
                            });

        //# Task<Result<T1>> ==> Func<T1, Result<T2>> ==> Task<Result<T2>>
        public static Task<Result<T2>> BindAsync<T1, T2>(this Task<Result<T1>> result, Func<T1, Result<T2>> resultSelector)
                => result.TaskResultAsync<T1, T2>(rt1 => rt1.Bind(resultSelector));
        //# Task<Result<T1>> ==> Func<T1, Task<Result<T2>>> ==> Task<Result<T2>>
        public static Task<Result<T2>> BindAsync<T1, T2>(this Task<Result<T1>> result, Func<T1, Task<Result<T2>>> resultSelector)
                => result.TaskResultAsync<T1, T2>(rt1 => rt1.BindAsync(resultSelector));




        //?  Combine 5
        //# Result<T1> ==> Result<T2> ==> Result<(T1, T2)>
        public static Result<(T1, T2)> Combine<T1, T2>(in this Result<T1> p1, Result<T2> p2)
               => (p1.Error ?? p2.Error) ?? Result.Ok(ValueTuple.Create(p1.Value, p2.Value));
        //# Result<T1> ==> Func<T1, Result<T2>> ==> Result<(T1, T2)>
        public static Result<(T1, T2)> Combine<T1, T2>(in this Result<T1> r1, Func<T1, Result<T2>> r2func)
                => r1.Error ?? r1.Combine(r2func(r1.Value));

        //# Task<Result<T1>> ==> Func<T1, Result<T2>> ==> Task<Result<(T1, T2)>>
        public static Task<Result<(T1, T2)>> CombineAsync<T1, T2>(this Result<T1> t1, Func<T1, Task<Result<T2>>> task)
        {

            if (t1.Error is { }) return Task.FromResult(t1.Error.AsFail<(T1, T2)>());
            return task(t1.Value).ContinueWith(completedtask =>
            {
                if (completedtask.IsFaulted)
                {
                    return completedtask.Exception.AsFail<(T1, T2)>();
                }
                else if (completedtask.IsCanceled)
                {
                    return RezErr.Cancelled.AsFail<(T1, T2)>();
                }

                return t1.Combine(completedtask.Result);
            });
        }
        //# Result<T1> ==> Func<T1, Task<Result<T2>>> ==> Task<Result<(T1, T2)>>
        public static Task<Result<(T1, T2)>> CombineAsync<T1, T2>(this Task<Result<T1>> result, Func<T1, Result<T2>> resultSelector)
                 => result.TaskResultAsync(rt1 => rt1.Combine(resultSelector));
        //# Task<Result<T1>> ==> Func<T1, Task<Result<T2>>> ==> Task<Result<(T1, T2)>>
        public static Task<Result<(T1, T2)>> CombineAsync<T1, T2>(this Task<Result<T1>> result, Func<T1, Task<Result<T2>>> resultSelector)
              => result.TaskResultAsync(rt1 => rt1.CombineAsync(resultSelector));


        //? IfThen 4
        //# Result<T1> ==>  Func<T1, bool> ==> Func<T1, Result<T1>> ==> Result<T2>
        public static Result<T1> IfThen<T1>(this Result<T1> rt1, Func<T1, bool> checker, Func<T1, Result<T1>> alternate)
        {
            return rt1.Combine<T1, bool>(t1 => checker.TryResult(t1)).Bind(tup => !tup.Item2 ? rt1 : alternate(tup.Item1));
        }
        //# Result<T1> ==>  Func<T1, bool> ==> Func<T1, Task<Result<T1>>> ==> Task<Result<T1>>
        public static Task<Result<T1>> IfThenAsync<T1>(this Result<T1> rt1, Func<T1, bool> checker, Func<T1, Task<Result<T1>>> alternate)
            => rt1.Combine(t1 => checker.TryResult(t1)).BindAsync(tup => !tup.Item2 ? Task.FromResult(rt1) : alternate(tup.Item1));
        //# Task<Result<T1>> ==>  Func<T1, bool> ==> Func<T1, Result<T1>> ==> Task<Result<T1>>
        public static Task<Result<T1>> IfThenAsync<T1>(this Task<Result<T1>> result, Func<T1, bool> checker, Func<T1, Result<T1>> alternate)
            => result.TaskResultAsync(rt => rt.IfThen(checker, alternate));
        //# Task<Result<T1>> ==>  Func<T1, bool> ==> Func<T1, Task<Result<T1>>> ==> Task<Result<T1>>
        public static Task<Result<T1>> IfThenAsync<T1>(this Task<Result<T1>> result, Func<T1, bool> checker, Func<T1, Task<Result<T1>>> alternate)
            => result.TaskResultAsync(rt => rt.IfThenAsync(checker, alternate));

        //? Tee 2
        //# Result<T1> ==> Action<T1> ==> Result<T1>
        public static Result<T1> Tee<T1>(in this Result<T1> t1, Action<T1> action)
        {

            try
            {
                if (t1.Error is null)
                {

                    action(t1.Value);
                }
                return t1;
            }
            catch (Exception ex)
            {
                return ex.AsFail<T1>();
            }
        }
        //# Task<Result<T1>> ==> Action<T1> ==>  Task<Result<T1>>
        public static Task<Result<T1>> TeeAsync<T1>(this Task<Result<T1>> result, Action<T1> action)
        {
            return result.ContinueWith(completedtask =>
            {
                if (completedtask.IsFaulted)
                {
                    return completedtask.Exception.AsFail<T1>();
                }
                else if (completedtask.IsCanceled)
                {
                    return RezErr.Cancelled.AsFail<T1>();
                }
                if (completedtask.Result.Error is { })
                {
                    return completedtask.Result.Error.AsFail<T1>();
                }
                return completedtask.Result.Tee(action);
            });
        }
        //? TeeError
        //# Result<T1> ==> Action<RezErrBase> ==> Result<T1>
        public static Result<T1> TeeError<T1>(in this Result<T1> t1, Action<RezErrBase> action)
        {
            if (t1.Error is { })
            {
                try
                {
                    action(t1.Error);
                }
                catch (Exception ex)
                {
                    return ex.AsFail<T1>();
                }
            }
            return t1;
        }

        //? RevertError
        //# Result<T1> ==> RezErrBase ==> Func<T1> ==> Result<T1>
        //# (Result<T1>, RezErrBase, Func<T1>) ==> Result<T1>
        public static Result<T1> RevertError<T1>(in this Result<T1> t1, RezErrBase failure, Func<T1> fix)
                => (t1.Error?.Equals(failure) == true) ? fix.TryResult() : t1;
        //# Result<T1> ==> RezErrBase ==>  Func<Result<T1>> ==> Result<T1>
        public static Result<T1> RevertError<T1>(in this Result<T1> t1, RezErrBase failure, Func<Result<T1>> fix)
               => (t1.Error?.Equals(failure) == true) ? fix() : t1;
        //# Task<Result<T1>> ==> RezErrBase ==> Func<T1> ==>  Task<Result<T1>>
        public static Task<Result<T1>> RevertErrorAsync<T1>(this Task<Result<T1>> result, RezErrBase failure, Func<T1> fix)
         => result.TaskResultAsync(rt1 => rt1.RevertError(failure, fix));
        //# Task<Result<T1>> ==> RezErrBase ==> Func<Result<T1>> ==> Task<Result<T1>>
        public static Task<Result<T1>> RevertErrorAsync<T1>(this Task<Result<T1>> result, RezErrBase failure, Func<Result<T1>> fix)
            => result.TaskResultAsync(rt1 => rt1.RevertError(failure, fix));

        //? Is Logic
        //#  Result<bool> ==> bool
        public static bool IsTrue(in this Result<bool> b) => b.Error is null && b.Value;
        //#  Result<bool> ==> bool
        public static bool IsFalse(in this Result<bool> b) => b.Error is null && !b.Value;
        //#  Result<bool> ==> Result<bool>  ==> bool
        public static bool IsBothTrue(in this Result<bool> b, Result<bool> c) => IsTrue(b) && IsTrue(c);
        //#  Result<bool> ==> Result<bool>  ==> bool
        public static bool IsEitherTrue(in this Result<bool> b, Result<bool> c) => IsTrue(b) || IsTrue(c);
        //# Task<Result<bool>> ==> bool
        public static bool IsTrue(this Task<Result<bool>> tb)
        {
            if (tb.IsCompleted) return tb.Result.IsTrue();
            var rb = tb.RunSync();
            return rb.IsTrue();
        }
        //# Task<Result<bool>> ==> Func<T1, Result<bool>> ==> bool
        public static bool IsTrue<T1>(this Task<Result<T1>> tb, Func<T1, Result<bool>> selector)
        {
            if (tb.IsCompleted) return tb.Result.Bind(selector).IsTrue();
            Result<T1> rb = tb.RunSync();
            return rb.Bind(selector).IsTrue();
        }


        //? Property Setter eg rt1.BindSet(t1=>GetValueToSetOnT1Property(t1) ,(t1,t2)=>t1.Property=t2)
        //# Result<T1> ==> Func<T1, Result<T2>> ==> Action<T1, T2> ==> Result<T1> 
        public static Result<T1> BindSet<T1, T2>(this Result<T1> t1, Func<T1, Result<T2>> resultSelector, Action<T1, T2> setter)
        {
            return t1.Error ?? resultSelector(t1.Value).Tee(x => setter(t1.Value, x)).Bind(_ => t1);
        }

        //# Result<T1> ==> string ==> Result<T1>
        public static Result<T1> Embelish<T1>(in this Result<T1> result, string extraErrorInfo)
           => (result.Error is { })
             ? new RezEmbelishedError(result.Error, extraErrorInfo)
             : result;





        //? Implementation details - mot monadic functions
        //# Task<Result<T1>> ==> Func<Result<T1>, Result<T2>> ==> Task<Result<T2>>
        /// <summary>
        /// Implementation - dont call directly
        /// </summary>
        public static Task<Result<T2>> TaskResultAsync<T1, T2>(this Task<Result<T1>> result, Func<Result<T1>, Result<T2>> selector)
        {
            return result.ContinueWith(completedtask =>
            {
                if (completedtask.IsFaulted)
                {
                    return completedtask.Exception.AsFail<T2>();
                }
                else if (completedtask.IsCanceled)
                {
                    return RezErr.Cancelled.AsFail<T2>();
                }
                if (completedtask.Result.Error is { })
                {
                    return completedtask.Result.Error.AsFail<T2>();
                }
                return selector(completedtask.Result);
            });
        }

        //# Task<Result<T1>> ==> Func<Result<T1>, Task<Result<T2>>> ==>  Task<Result<T2>>
        /// <summary>
        /// Implementation - dont call directly
        /// </summary>
        public static Task<Result<T2>> TaskResultAsync<T1, T2>(this Task<Result<T1>> task, Func<Result<T1>, Task<Result<T2>>> next)
        {
            var tcs = new TaskCompletionSource<Result<T2>>();
            task.ContinueWith(completedTask =>
            {
                if (completedTask.IsFaulted)
                {
                    tcs.TrySetResult(completedTask.Exception.AsFail<T2>());
                }
                else if (completedTask.IsCanceled)
                {
                    tcs.TrySetResult(RezErr.Cancelled.AsFail<T2>());
                }
                else
                {
                    try
                    {
                        next(completedTask.Result).ContinueWith(completedtask2 =>
                        {
                            tcs.TrySetFromResultTask(completedtask2);
                        });
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetResult(exc.AsFail<T2>());
                    }
                }
            });
            return tcs.Task;
        }
    }
}
