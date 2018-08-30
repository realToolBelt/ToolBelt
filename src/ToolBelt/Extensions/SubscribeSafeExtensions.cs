using Splat;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ToolBelt.Extensions
{
    public static class SubscribeSafeExtensions
    {
#if DEBUG
        public static IDisposable SubscribeSafe<T>(
            this IObservable<T> @this,
            [CallerMemberName]string callerMemberName = null,
            [CallerFilePath]string callerFilePath = null,
            [CallerLineNumber]int callerLineNumber = 0)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            return @this
                .Subscribe(
                    _ => { },
                    ex =>
                    {
                        LogHost.Default.Error(
                            $"An exception went unhandled. Caller member name: '{callerMemberName}', caller file path: '{callerFilePath}', caller line number: {callerLineNumber}.\r\n\t{ex}");

                        Debugger.Break();
                    });
        }

        public static IDisposable SubscribeSafe<T>(
            this IObservable<T> @this,
            Action<T> onNext,
            [CallerMemberName]string callerMemberName = null,
            [CallerFilePath]string callerFilePath = null,
            [CallerLineNumber]int callerLineNumber = 0)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            if (onNext == null)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            return @this
                .Subscribe(
                    onNext,
                    ex =>
                    {
                        LogHost.Default.Error(
                            $"An exception went unhandled. Caller member name: '{callerMemberName}', caller file path: '{callerFilePath}', caller line number: {callerLineNumber}.\r\n\t{ex}");

                        Debugger.Break();
                    });
        }
#else
        public static IDisposable SubscribeSafe<T>(this IObservable<T> @this) =>
            @this.Subscribe();

        public static IDisposable SubscribeSafe<T>(this IObservable<T> @this, Action<T> onNext) =>
            @this.Subscribe(onNext);
#endif
    }
}
