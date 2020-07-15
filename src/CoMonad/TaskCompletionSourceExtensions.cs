using CoMonad;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace CoMonad
{
    [DebuggerStepThrough]
    public static class TaskCompletionSourceExtensions
    {
        public static bool TrySetFromTask<TResult>(this TaskCompletionSource<TResult> resultSetter, Task task)
        {
            switch (task.Status)
            {
                case TaskStatus.RanToCompletion: return resultSetter.TrySetResult(default(TResult)!);
                case TaskStatus.Faulted: return resultSetter.TrySetException(task.Exception.InnerExceptions);
                case TaskStatus.Canceled: return resultSetter.TrySetCanceled();
                default: throw new InvalidOperationException("The task was not completed.");
            }
        }
        /// <summary>Attempts to transfer the result of a Task to the TaskCompletionSource.</summary>
        public static bool TrySetFromTask<TResult>(this TaskCompletionSource<TResult> resultSetter, Task<TResult> task)
        {
            switch (task.Status)
            {
                case TaskStatus.RanToCompletion: return resultSetter.TrySetResult(task.Result);
                case TaskStatus.Faulted: return resultSetter.TrySetException(task.Exception.InnerExceptions);
                case TaskStatus.Canceled: return resultSetter.TrySetCanceled();
                default: throw new InvalidOperationException("The task was not completed.");
            }
        }
        public static bool TrySetFromResultTask<T2>(this TaskCompletionSource<Result<T2>> resultSetter, Task<Result<T2>> task)
        {
            switch (task.Status)
            {
                case TaskStatus.RanToCompletion: return resultSetter.TrySetResult(task.Result);
                case TaskStatus.Faulted: return resultSetter.TrySetResult(task.Exception.AsFail<T2>());
                case TaskStatus.Canceled: return resultSetter.TrySetResult(RezErr.Cancelled.AsFail<T2>());
                default: throw new InvalidOperationException("The task was not completed.");
            }
        }
    }
}
