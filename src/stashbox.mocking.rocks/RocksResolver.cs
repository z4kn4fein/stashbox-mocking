using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Stashbox.Entity;
using System.Reflection;
using Stashbox.Infrastructure;
using Rocks;

namespace Stashbox.Mocking.Rocks
{
    internal class RocksResolver : ResolverBase
    {
        private static readonly MethodInfo MakeMethodInfo = typeof(Rock).GetMethod(nameof(Rock.Make), Type.EmptyTypes);

        public RocksResolver(ISet<Type> requestedTypes)
            : base(requestedTypes)
        { }

        public override Expression GetExpression(IContainerContext containerContext, TypeInformation typeInfo, ResolutionInfo resolutionInfo)
        {
            var method = MakeMethodInfo.MakeGenericMethod(typeInfo.Type);
            return Expression.Call(method);
        }
    }
}
