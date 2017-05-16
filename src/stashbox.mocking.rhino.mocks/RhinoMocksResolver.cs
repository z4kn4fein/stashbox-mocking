using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Rhino.Mocks;
using Stashbox.Entity;
using Stashbox.Infrastructure;

namespace Stashbox.Mocking.Rhino.Mocks
{
    internal class RhinoMocksResolver : ResolverBase
    {
        public RhinoMocksResolver(ISet<Type> requestedTypes)
            : base(requestedTypes)
        { }

        public override Expression GetExpression(IContainerContext containerContext, TypeInformation typeInfo, ResolutionInfo resolutionInfo) =>
            Expression.Constant(MockRepository.GenerateStub(typeInfo.Type));
    }
}
