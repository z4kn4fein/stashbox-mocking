using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Rhino.Mocks;
using Stashbox.Entity;
using Stashbox.Infrastructure;
using Stashbox.Infrastructure.Resolution;

namespace Stashbox.Mocking.Rhino.Mocks
{
    internal class RhinoMocksResolver : Resolver
    {
        private readonly ISet<Type> requestedTypes;

        public RhinoMocksResolver(ISet<Type> requestedTypes)
        {
            this.requestedTypes = requestedTypes;
        }

        public override Expression GetExpression(IContainerContext containerContext, TypeInformation typeInfo, ResolutionInfo resolutionInfo) =>
            Expression.Constant(MockRepository.GenerateStub(typeInfo.Type));

        public override bool CanUseForResolution(IContainerContext containerContext, TypeInformation typeInfo, ResolutionInfo resolutionInfo) =>
            !this.requestedTypes.Contains(typeInfo.Type) && typeInfo.Type.CanMock();
    }
}
