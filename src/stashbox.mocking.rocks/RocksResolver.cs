using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Stashbox.Entity;
using System.Reflection;
using Stashbox.Infrastructure;
using Stashbox.Infrastructure.Resolution;
using Rocks;

namespace Stashbox.Mocking.Rocks
{
    internal class RocksResolver : Resolver
    {
        private readonly ISet<Type> requestedTypes;
        private static readonly MethodInfo MakeMethodInfo = typeof(Rock).GetMethod(nameof(Rock.Make), Type.EmptyTypes);

        public RocksResolver(ISet<Type> requestedTypes)
        {
            this.requestedTypes = requestedTypes;
        }

        public override Expression GetExpression(IContainerContext containerContext, TypeInformation typeInfo, ResolutionInfo resolutionInfo)
        {
            var method = MakeMethodInfo.MakeGenericMethod(typeInfo.Type);
            return Expression.Call(method);
        }

        public override bool CanUseForResolution(IContainerContext containerContext, TypeInformation typeInfo, ResolutionInfo resolutionInfo) =>
            !this.requestedTypes.Contains(typeInfo.Type) && typeInfo.Type.CanMock();
    }
}
