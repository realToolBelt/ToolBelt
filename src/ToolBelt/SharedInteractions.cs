using ReactiveUI;
using System;
using System.Reactive;

namespace ToolBelt
{
    /// <summary>
    /// Static class containing interactions that are shared across many views.
    /// </summary>
    public static class SharedInteractions
    {
        /// <summary>
        /// Gets the interaction used to handle errors.
        /// </summary>
        public static Interaction<Exception, Unit> Error { get; } = new Interaction<Exception, Unit>();
    }
}
