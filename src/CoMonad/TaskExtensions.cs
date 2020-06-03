using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

 namespace CoMonad
{
    public static class TaskExtensions
    {
        private static readonly TaskFactory _myTaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);
        public static TResult RunSync<TResult, P>(this Func<P, Task<TResult>> func, P p)
        {
            return func(p).RunSync();
        }
        /// <summary>
        /// Locks the Synchronisation context. Enables not writing duplicate Sync Methods
        /// </summary>
        public static TResult RunSync<TResult>(this Func<Task<TResult>> func)
        {
            var cultureUi = CultureInfo.CurrentUICulture;
            var culture = CultureInfo.CurrentCulture;
            return _myTaskFactory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;
                return func();
            }).Unwrap().GetAwaiter().GetResult();
        }


        /// <summary>
        /// Locks the Synchronisation context. Enables not writing duplicate Sync Methods
        /// </summary>
        public static TResult RunSync<TResult>(this Task<TResult> task)
        {
            var cultureUi = CultureInfo.CurrentUICulture;
            var culture = CultureInfo.CurrentCulture;
            return _myTaskFactory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;
                return task;
            }).Unwrap().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Locks the Synchronisation context. Enables not writing duplicate Sync Methods
        /// </summary>
        public static void RunSync(this Func<Task> func)
        {
            var cultureUi = CultureInfo.CurrentUICulture;
            var culture = CultureInfo.CurrentCulture;
            _myTaskFactory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;
                return func();
            }).Unwrap().GetAwaiter().GetResult();
        }
    }
}
