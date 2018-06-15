using Rhino.Mocks;
using Stashbox.Entity;
using Stashbox.Resolution;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Stashbox.Mocking.Rhino.Mocks
{
    internal class RhinoMocksResolver : ResolverBase
    {
        public RhinoMocksResolver(ISet<Type> requestedTypes)
            : base(requestedTypes)
        { }

        protected override Expression GetExpressionInternal(IContainerContext containerContext, TypeInformation typeInfo, ResolutionContext resolutionInfo) =>
            Expression.Constant(MockRepository.GenerateStub(typeInfo.Type));
    }
}
