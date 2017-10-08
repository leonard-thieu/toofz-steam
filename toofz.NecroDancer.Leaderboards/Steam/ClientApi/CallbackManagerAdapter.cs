using System;
using System.Diagnostics.CodeAnalysis;
using SteamKit2;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    [ExcludeFromCodeCoverage]
    sealed class CallbackManagerAdapter : ICallbackManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackManagerAdapter"/> class.
        /// </summary>
        public CallbackManagerAdapter()
        {
            var steamClient = new SteamClient();
            CallbackManager = new CallbackManager(steamClient);
            SteamClient = new SteamClientAdapter(steamClient, this);
        }

        public CallbackManager CallbackManager { get; }
        public ISteamClient SteamClient { get; }

        /// <summary>
        /// Blocks the current thread to run all queued callbacks. If no callback is queued,
        /// the method will block for the given timeout.
        /// </summary>
        /// <param name="timeout">The length of time to block.</param>
        public void RunWaitAllCallbacks(TimeSpan timeout)
        {
            CallbackManager.RunWaitAllCallbacks(timeout);
        }

        /// <summary>
        /// Blocks the current thread to run a single queued callback. If no callback is 
        /// queued, the method will block until one is posted.
        /// </summary>
        public void RunWaitCallbacks()
        {
            CallbackManager.RunWaitCallbacks();
        }

        /// <summary>
        /// Registers the provided <see cref="Action{TCallback}"/> to receive callbacks of type <typeparamref name="TCallback"/>.
        /// </summary>
        /// <typeparam name="TCallback"></typeparam>
        /// <param name="callbackFunc">
        /// The function to invoke with the callback.
        /// </param>
        /// <returns>
        /// An <see cref="IDisposable"/>. Disposing of the return value will unsubscribe the <paramref name="callbackFunc"/>.
        /// </returns>
        public IDisposable Subscribe<TCallback>(Action<TCallback> callbackFunc) where TCallback : class, ICallbackMsg
        {
            return CallbackManager.Subscribe(callbackFunc);
        }
    }
}
