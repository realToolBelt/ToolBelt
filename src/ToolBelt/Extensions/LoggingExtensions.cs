using Splat;
using System.Globalization;
using ToolBelt.Services;

namespace ToolBelt.Extensions
{
    public static class LoggingExtensions
    {
        public static PerformanceBlock Perf(this ILogger @this, string message)
        {
            if (LogLevel.Debug < @this.Level)
            {
                return PerformanceBlock.Empty;
            }

            return new PerformanceBlock(@this, message);
        }

        public static PerformanceBlock Perf(this ILogger @this, string format, params object[] args)
        {
            if (LogLevel.Debug < @this.Level)
            {
                return PerformanceBlock.Empty;
            }

            var message = string.Format(CultureInfo.InvariantCulture, format, args);
            return new PerformanceBlock(@this, message);
        }

        public static PerformanceBlock Perf<T0>(this ILogger @this, string format, T0 arg0)
        {
            if (LogLevel.Debug < @this.Level)
            {
                return PerformanceBlock.Empty;
            }

            var message = string.Format(CultureInfo.InvariantCulture, format, arg0);
            return new PerformanceBlock(@this, message);
        }

        public static PerformanceBlock Perf<T0, T1>(this ILogger @this, string format, T0 arg0, T1 arg1)
        {
            if (LogLevel.Debug < @this.Level)
            {
                return PerformanceBlock.Empty;
            }

            var message = string.Format(CultureInfo.InvariantCulture, format, arg0, arg1);
            return new PerformanceBlock(@this, message);
        }

        public static PerformanceBlock Perf<T0, T1, T2>(this ILogger @this, string format, T0 arg0, T1 arg1, T2 arg2)
        {
            if (LogLevel.Debug < @this.Level)
            {
                return PerformanceBlock.Empty;
            }

            var message = string.Format(CultureInfo.InvariantCulture, format, arg0, arg1, arg2);
            return new PerformanceBlock(@this, message);
        }

        public static PerformanceBlock Perf<T0, T1, T2, T3>(this ILogger @this, string format, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            if (LogLevel.Debug < @this.Level)
            {
                return PerformanceBlock.Empty;
            }

            var message = string.Format(CultureInfo.InvariantCulture, format, arg0, arg1, arg2, arg3);
            return new PerformanceBlock(@this, message);
        }

        public static PerformanceBlock Perf<T0, T1, T2, T3, T4>(this ILogger @this, string format, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (LogLevel.Debug < @this.Level)
            {
                return PerformanceBlock.Empty;
            }

            var message = string.Format(CultureInfo.InvariantCulture, format, arg0, arg1, arg2, arg3, arg4);
            return new PerformanceBlock(@this, message);
        }

        public static PerformanceBlock Perf<T0, T1, T2, T3, T4, T5>(this ILogger @this, string format, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (LogLevel.Debug < @this.Level)
            {
                return PerformanceBlock.Empty;
            }

            var message = string.Format(CultureInfo.InvariantCulture, format, arg0, arg1, arg2, arg3, arg4, arg5);
            return new PerformanceBlock(@this, message);
        }

        public static PerformanceBlock Perf<T0, T1, T2, T3, T4, T5, T6>(this ILogger @this, string format, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (LogLevel.Debug < @this.Level)
            {
                return PerformanceBlock.Empty;
            }

            var message = string.Format(CultureInfo.InvariantCulture, format, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            return new PerformanceBlock(@this, message);
        }

        public static PerformanceBlock Perf<T0, T1, T2, T3, T4, T5, T6, T7>(this ILogger @this, string format, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (LogLevel.Debug < @this.Level)
            {
                return PerformanceBlock.Empty;
            }

            var message = string.Format(CultureInfo.InvariantCulture, format, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            return new PerformanceBlock(@this, message);
        }

        public static PerformanceBlock Perf<T0, T1, T2, T3, T4, T5, T6, T7, T8>(this ILogger @this, string format, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (LogLevel.Debug < @this.Level)
            {
                return PerformanceBlock.Empty;
            }

            var message = string.Format(CultureInfo.InvariantCulture, format, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            return new PerformanceBlock(@this, message);
        }

        public static PerformanceBlock Perf<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this ILogger @this, string format, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (LogLevel.Debug < @this.Level)
            {
                return PerformanceBlock.Empty;
            }

            var message = string.Format(CultureInfo.InvariantCulture, format, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            return new PerformanceBlock(@this, message);
        }
    }
}
