namespace Stashbox.Mocking
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Represents some helper methods to determine the arguments of a constructor.
    /// </summary>
    public static class StashArg
    {
        /// <summary>
        /// Represents an 'any instance of this type' argument.
        /// </summary>
        /// <typeparam name="TType">The type of the argument.</typeparam>
        /// <returns>The type of the argument.</returns>
        public static Type Any<TType>()
        {
            var type = typeof(TType);
            if (!type.CanMock())
                throw new NonMockableTypeException(type);

            return type;
        }
    }
}

namespace System
{
    /// <summary>
    /// Represents an exception which will be thrown when a non mockable type is requested.
    /// </summary>
    public class NonMockableTypeException : Exception
    {
        /// <summary>
        /// Constructs a <see cref="NonMockableTypeException"/>.
        /// </summary>
        /// <param name="type">The non mockable type.</param>
        public NonMockableTypeException(Type type)
            : base($"Could not create mock from the given type: {type.FullName}")
        { }
    }
}
