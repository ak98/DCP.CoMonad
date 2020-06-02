using CoMonad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Dasync.Collections
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class ForEachAsyncExtensions
    {

        public static async Task ForEachAsync<T>(this IAsyncEnumerable<Result<T>> enumerable, Action<T> action, Action<RezErrBase> onerror, CancellationToken cancellationToken = default)
        {
            var enumerator = enumerable.GetAsyncEnumerator(cancellationToken);
            try
            {
                while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    if (enumerator.Current.Error is null)
                    {
                        action(enumerator.Current.Value);
                    }
                    else
                    {
                        onerror(enumerator.Current.Error);
                    }
                }
            }
            catch (ForEachAsyncBreakException)
            {
            }
            finally
            {
                await enumerator.DisposeAsync().ConfigureAwait(false);
            }
        }

        public static async Task ForEachAsync<T>(this IAsyncEnumerator<Result<T>> enumerator, Action<T> action, Action<RezErrBase> onerror)
        {
            try
            {
                while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    if (enumerator.Current.Error is null)
                    {
                        action(enumerator.Current.Value);
                    }
                    else
                    {
                        onerror(enumerator.Current.Error);
                    }
                }
            }
            catch (ForEachAsyncBreakException)
            {
            }
            finally
            {
                await enumerator.DisposeAsync().ConfigureAwait(false);
            }
        }

        public static async Task ForEachAsync<T>(this IAsyncEnumerable<Result<T>> enumerable, Action<T, long> action, Action<RezErrBase, long> onerror, CancellationToken cancellationToken = default)
        {
            var enumerator = enumerable.GetAsyncEnumerator(cancellationToken);
            try
            {
                long index = 0;

                while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    if (enumerator.Current.Error is null)
                    {
                        action(enumerator.Current.Value, index);
                    }
                    else
                    {
                        onerror(enumerator.Current.Error, index);
                    }

                    index++;
                }
            }
            catch (ForEachAsyncBreakException)
            {
            }
            finally
            {
                await enumerator.DisposeAsync().ConfigureAwait(false);
            }
        }

        public static async Task ForEachAsync<T>(this IAsyncEnumerator<Result<T>> enumerator, Action<T, long> action, Action<RezErrBase, long> onerror)
        {
            try
            {
                long index = 0;

                while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    if (enumerator.Current.Error is null)
                    {
                        action(enumerator.Current.Value, index);
                    }
                    else
                    {
                        onerror(enumerator.Current.Error, index);
                    }

                    index++;
                }
            }
            catch (ForEachAsyncBreakException)
            {
            }
            finally
            {
                await enumerator.DisposeAsync().ConfigureAwait(false);
            }
        }

        public static async Task ForEachAsync<T>(this IAsyncEnumerable<Result<T>> enumerable, Func<T, Task> action, Action<RezErrBase> onerror, CancellationToken cancellationToken = default)
        {
            var enumerator = enumerable.GetAsyncEnumerator(cancellationToken);
            try
            {
                while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    if (enumerator.Current.Error is null)
                    {
                        await action(enumerator.Current.Value).ConfigureAwait(false);
                    }
                    else
                    {
                        onerror(enumerator.Current.Error);
                    }
                }
            }
            catch (ForEachAsyncBreakException)
            {
            }
            finally
            {
                await enumerator.DisposeAsync().ConfigureAwait(false);
            }
        }

        public static async Task ForEachAsync<T>(this IAsyncEnumerator<Result<T>> enumerator, Func<T, Task> action, Action<RezErrBase> onerror)
        {
            try
            {
                while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    if (enumerator.Current.Error is null)
                    {
                        await action(enumerator.Current.Value).ConfigureAwait(false);
                    }
                    else
                    {
                        onerror(enumerator.Current.Error);
                    }
                }
            }
            catch (ForEachAsyncBreakException)
            {
            }
            finally
            {
                await enumerator.DisposeAsync().ConfigureAwait(false);
            }
        }

        public static async Task ForEachAsync<T>(this IAsyncEnumerable<Result<T>> enumerable, Func<T, long, Task> action, Action<RezErrBase, long> onerror, CancellationToken cancellationToken = default)
        {
            var enumerator = enumerable.GetAsyncEnumerator(cancellationToken);
            try
            {
                long index = 0;

                while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    if (enumerator.Current.Error is null)
                    {
                        await action(enumerator.Current.Value, index).ConfigureAwait(false);
                    }
                    else
                    {
                        onerror(enumerator.Current.Error, index);
                    }
                    index++;
                }
            }
            catch (ForEachAsyncBreakException)
            {
            }
            finally
            {
                await enumerator.DisposeAsync().ConfigureAwait(false);
            }
        }

        public static async Task ForEachAsync<T>(this IAsyncEnumerator<Result<T>> enumerator, Func<T, long, Task> action, Action<RezErrBase, long> onerror)
        {
            try
            {
                long index = 0;

                while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    if (enumerator.Current.Error is null)
                    {
                        await action(enumerator.Current.Value, index).ConfigureAwait(false);
                    }
                    else
                    {
                        onerror(enumerator.Current.Error, index);
                    }
                    index++;
                }
            }
            catch (ForEachAsyncBreakException)
            {
            }
            finally
            {
                await enumerator.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}
