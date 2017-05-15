namespace System.Reflection
{
    internal static class TypeExtensions
    {
#if !NET40
        public static bool CanMock(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsAbstract || typeInfo.IsInterface || typeInfo.IsClass && !typeInfo.IsSealed;
        }
#endif

#if NET40
        public static bool CanMock(this Type type) =>
            type.IsAbstract || type.IsInterface || type.IsClass && !type.IsSealed;
#endif
    }

}
