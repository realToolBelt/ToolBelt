using System;

namespace ToolBelt.Services
{
    /// <summary>
    /// A provider that can be used to get the current date and time.
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Gets an object whose value is the current local date and time.
        /// </summary>
        DateTime Now { get; }
    }

    /// <summary>
    /// Extension methods for the <see cref="IDateTimeProvider" />.
    /// </summary>
    public static class DateTimeProviderExtensions
    {
        /// <summary>
        /// Returns an object that is set to today's date, with the time component set to 00:00:00.
        /// </summary>
        /// <param name="provider">The provider to use to get today's date.</param>
        /// <returns>An object that is set to today's date, with the time component set to 00:00:00</returns>
        public static DateTime Today(this IDateTimeProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            return provider.Now.Date;
        }
    }

    /// <summary>
    /// A <see cref="IDateTimeProvider" /> that uses the system date and time information.
    /// </summary>
    public class SystemDateTimeProvider : IDateTimeProvider
    {
        /// <summary>
        /// Gets an object whose value is the current local date and time.
        /// </summary>
        public DateTime Now => DateTime.Now;
    }
}
