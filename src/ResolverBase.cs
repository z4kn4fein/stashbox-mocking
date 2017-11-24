using Stashbox.Entity;
using Stashbox.Infrastructure;
using Stashbox.Infrastructure.Resolution;
using Stashbox.Resolution;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Stashbox.Mocking
{
    internal class ResolverBase : Resolver
    {
        private readonly ISet<Type> requestedTypes;

        public ResolverBase(ISet<Type> requestedTypes)
        {
            this.requestedTypes = requestedTypes;
        }

        public override Expression GetExpression(IContainerContext containerContext, TypeInformation typeInfo, ResolutionContext resolutionInfo)
        {
            throw new NotImplementedException();
        }

        public override bool CanUseForResolution(IContainerContext containerContext, TypeInformation typeInfo, ResolutionContext resolutionInfo) =>
            !this.requestedTypes.Contains(typeInfo.Type) && typeInfo.Type.CanMock();
    }
}
