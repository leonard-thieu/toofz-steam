using System;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    internal static class TaskExtensions
    {
        #region https://blogs.msdn.microsoft.com/pfxteam/2011/11/10/crafting-a-task-timeoutafter-method/

        public static Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            // Short-circuit #1: infinite timeout or task already completed
            if (task.IsCompleted || (timeout == Timeout.InfiniteTimeSpan))
            {
                // Either the task has already completed or timeout will never occur.
                // No proxy necessary.
                return task;
            }

            // tcs.Task will be returned as a proxy to the caller
            var tcs = new TaskCompletionSource<TResult>();

            // Short-circuit #2: zero timeout
            if (timeout == TimeSpan.Zero)
            {
                // We've already timed out.
                var ex = new TimeoutException();
                tcs.SetException(ex);

                return tcs.Task;
            }

            // Set up a timer to complete after the specified timeout period
            var timer = new Timer(state =>
            {
                // Recover your state information
                var myTcs = (TaskCompletionSource<TResult>)state;

                // Fault our proxy with a TimeoutException
                var ex = new TimeoutException();
                myTcs.TrySetException(ex);
            }, tcs, timeout, Timeout.InfiniteTimeSpan);

            // Wire up the logic for what happens when source task completes
            task.ContinueWith((antecedent, state) =>
            {
                // Recover our state data
                var tuple = (Tuple<Timer, TaskCompletionSource<TResult>>)state;

                // Cancel the Timer
                tuple.Item1.Dispose();

                // Marshal results to proxy
                MarshalTaskResults(antecedent, tuple.Item2);
            },
            Tuple.Create(timer, tcs),
            CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously,
            TaskScheduler.Default);

            return tcs.Task;
        }

        private static void MarshalTaskResults<TResult>(Task source, TaskCompletionSource<TResult> proxy)
        {
            switch (source.Status)
            {
                case TaskStatus.Faulted:
                    proxy.TrySetException(source.Exception);
                    break;
                case TaskStatus.Canceled:
                    proxy.TrySetCanceled();
                    break;
                case TaskStatus.RanToCompletion:
                    var castedSource = source as Task<TResult>;
                    proxy.TrySetResult(
                        castedSource == null ?
                            default : // source is a Task
                            castedSource.Result); // source is a Task<TResult>
                    break;
            }
        }

        #endregion
    }
}
